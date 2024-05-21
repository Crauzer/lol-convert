using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Environment;
using LeagueToolkit.Core.Meta;
using LeagueToolkit.Core.Wad;
using LeagueToolkit.Hashing;
using LeagueToolkit.Meta;
using LeagueToolkit.Meta.Classes;
using lol_convert.Packages;
using lol_convert.Services;
using lol_convert.Utils;
using lol_convert.Wad;
using Serilog;
using Meta = LeagueToolkit.Meta.Classes;

namespace lol_convert.Converters;

internal class MapConverter
{
    private readonly WadHashtable _wadHashtable;
    private readonly MetaEnvironment _metaEnvironment;
    private readonly string _outputPath;

    public MapConverter(
        WadHashtable wadHashtable,
        MetaEnvironment metaEnvironment,
        string outputPath
    )
    {
        _wadHashtable = wadHashtable;
        _metaEnvironment = metaEnvironment;
        _outputPath = outputPath;
    }

    public List<string> CreateMapPackages(string finalPath)
    {
        Log.Information("Creating map packages...");

        Directory.CreateDirectory(Path.Combine(_outputPath, "data", "maps"));

        var mapWadPaths = ConvertUtils.GlobMapWads(finalPath).ToArray();
        var mapPackages = new List<string>(mapWadPaths.Length);
        foreach (var mapWadPath in mapWadPaths)
        {
            using var wad = new WadFile(mapWadPath);

            string mapName = FsUtils.FileNameWithoutMultiExtension(mapWadPath).ToLower();
            var mapShippingBinTree = ResolveMapShippingBinTree(wad, mapName);

            try
            {
                var mapPackage = CreateMapPackage(mapName, mapShippingBinTree, wad);
                SaveMapPackage(mapPackage);
                mapPackages.Add(mapPackage.Name);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Failed to create map package (mapName = {0})", mapName);
            }
        }

        return mapPackages;
    }

    public MapPackage CreateMapPackage(string mapName, BinTree shippingBinTree, WadFile wad)
    {
        Log.Information("Creating map package (mapName = {mapName})", mapName);

        Directory.CreateDirectory(Path.Combine(_outputPath, "data", "maps", mapName));
        Directory.CreateDirectory(Path.Combine(_outputPath, "data", "maps", mapName, "skins"));

        var mapBinObject = shippingBinTree
            .Objects.FirstOrDefault(x => x.Value.ClassHash == Fnv1a.HashLower(nameof(Map)))
            .Value;
        var mapDefinition = MetaSerializer.Deserialize<Map>(
            _metaEnvironment,
            mapBinObject
        );

        List<string> mapSkinNames = [];
        foreach (var mapSkinObjectLink in mapDefinition.MapSkins)
        {
            var mapSkinObject = shippingBinTree.Objects[mapSkinObjectLink];
            var mapSkinDefinition = MetaSerializer.Deserialize<MapSkin>(
                _metaEnvironment,
                mapSkinObject
            );

            if (string.IsNullOrEmpty(mapSkinDefinition.MapContainerLink))
            {
                Log.Warning(
                    "{mapName} - {mapSkinName} has no map container link, skipping skin",
                    mapName,
                    mapSkinDefinition.Name
                );
                continue;
            }

            try
            {
                var mapSkinPackage = CreateMapSkinPackage(mapName, mapSkinDefinition, wad);
                SaveMapSkinPackage(mapName, mapSkinPackage);
                mapSkinNames.Add(mapSkinPackage.Name);
            }
            catch (Exception exception)
            {
                Log.Error(
                    exception,
                    "Failed to create map skin package (mapSkinName = {0})",
                    mapSkinDefinition.Name
                );
            }
        }

        return new() { Name = mapName.ToLower(), Skins = mapSkinNames };
    }

    private MapSkinPackage CreateMapSkinPackage(
        string mapName,
        MapSkin mapSkinDefinition,
        WadFile wad
    )
    {
        Log.Information(
            "Creating map skin package (mapSkinName = {mapSkinName})",
            mapSkinDefinition.Name
        );

        Directory.CreateDirectory(
            Path.Combine(
                _outputPath,
                "data",
                "maps",
                mapName,
                "skins",
                mapSkinDefinition.Name.ToLower()
            )
        );

        var mapSkinGeometryPath = $"data/{mapSkinDefinition.MapContainerLink}";
        var mapSkinEnvironmentAssetPath = $"{mapSkinGeometryPath}.mapgeo";
        var mapSkinMaterialsBinPath = $"{mapSkinGeometryPath}.materials.bin";

        using var environmentAssetStream = wad.LoadChunkDecompressed(mapSkinEnvironmentAssetPath)
            .AsStream();
        using var materialsBinStream = wad.LoadChunkDecompressed(mapSkinMaterialsBinPath)
            .AsStream();

        using var environmentAsset = new EnvironmentAsset(environmentAssetStream);
        var materialsBin = new BinTree(materialsBinStream);

        var mapContainer = ResolveMapContainer(materialsBin, mapSkinDefinition.MapContainerLink);

        var staticMaterialPackages = CollectStaticMaterialPackages(materialsBin)
            .Select(x => new KeyValuePair<string, StaticMaterialPackage>(x.Name, x))
            .ToDictionary();

        var chunks = CollectChunks(materialsBin);

        var container = new MapContainerPackage(mapContainer);

        return new()
        {
            Name = mapSkinDefinition.Name.ToLower(),
            Container = container,
            StaticMaterials = staticMaterialPackages,
            Chunks = chunks
        };
    }

    private MapContainer ResolveMapContainer(BinTree shippingBinTree, string mapContainerLink)
    {
        var mapContainerObject = shippingBinTree.Objects[Fnv1a.HashLower(mapContainerLink)];
        return MetaSerializer.Deserialize<MapContainer>(
            MetaEnvironmentService.Environment,
            mapContainerObject
        );
    }

    private IEnumerable<StaticMaterialPackage> CollectStaticMaterialPackages(BinTree materialsBin)
    {
        var staticMaterialDefClassHash = Fnv1a.HashLower(nameof(StaticMaterialDef));
        return materialsBin
            .Objects.Values.Where(x => x.ClassHash == staticMaterialDefClassHash)
            .Select(x => new StaticMaterialPackage(
                MetaSerializer.Deserialize<StaticMaterialDef>(
                    MetaEnvironmentService.Environment,
                    x
                )
            ));
    }

    private Dictionary<string, MapPlaceableContainerPackage> CollectChunks(BinTree materialsBin)
    {
        var mapPlaceableContainerClassHash = Fnv1a.HashLower(nameof(MapPlaceableContainer));
        return materialsBin
            .Objects.Values.Where(x => x.ClassHash == mapPlaceableContainerClassHash)
            .Select(treeObject => new KeyValuePair<string, MapPlaceableContainerPackage>(
                BinHashtableService.ResolveObjectHash(treeObject.PathHash),
                new(
                    MetaSerializer.Deserialize<MapPlaceableContainer>(
                        MetaEnvironmentService.Environment,
                        treeObject
                    )
                )
            ))
            .ToDictionary();
    }

    private BinTree ResolveMapShippingBinTree(WadFile wad, string mapName)
    {
        string mapShippingPath = $"data/maps/shipping/{mapName}/{mapName}.bin";

        using var mapShippingBinStream = wad.LoadChunkDecompressed(mapShippingPath).AsStream();
        return new(mapShippingBinStream);
    }

    private void SaveMapPackage(MapPackage map)
    {
        using var stream = File.Create(
            Path.Combine(_outputPath, "data", "maps", map.Name, $"{map.Name}.json")
        );
        JsonSerializer.Serialize(stream, map, JsonUtils.DefaultOptions);
    }

    private void SaveMapSkinPackage(string mapName, MapSkinPackage mapSkin)
    {
        using var stream = File.Create(
            Path.Combine(
                _outputPath,
                "data",
                "maps",
                mapName,
                "skins",
                mapSkin.Name,
                $"{mapSkin.Name}.json"
            )
        );
        JsonSerializer.Serialize(stream, mapSkin, JsonUtils.DefaultOptions);
    }
}
