using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Environment;
using LeagueToolkit.Core.Meta;
using LeagueToolkit.Core.Wad;
using LeagueToolkit.Hashing;
using LeagueToolkit.Meta;
using LeagueToolkit.Meta.Classes;
using lol_convert.Packages;
using Serilog;
using Meta = LeagueToolkit.Meta.Classes;

namespace lol_convert;

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
        this._wadHashtable = wadHashtable;
        this._metaEnvironment = metaEnvironment;
        this._outputPath = outputPath;
    }

    public List<string> CreateMapPackages(string finalPath)
    {
        Log.Information("Creating map packages...");

        var mapWadPaths = ConvertUtils.GlobMapWads(finalPath).ToArray();
        var mapPackages = new List<string>(mapWadPaths.Length);

        foreach (var mapWadPath in mapWadPaths)
        {
            using var wad = new WadFile(mapWadPath);

            string mapName = Path.GetFileName(mapWadPath).ToLower();
            var mapShippingBinTree = ResolveMapShippingBinTree(wad, mapName);

            var mapPackage = CreateMapPackage(mapName, mapShippingBinTree, wad);
        }

        return mapPackages;
    }

    public MapPackage CreateMapPackage(string mapName, BinTree shippingBinTree, WadFile wad)
    {
        Log.Information("Creating map package (mapName = {mapName})", mapName);

        var mapBinObject = shippingBinTree.Objects[Fnv1a.HashLower(nameof(Meta.Map))];
        var mapDefinition = MetaSerializer.Deserialize<Meta.Map>(
            this._metaEnvironment,
            mapBinObject
        );

        foreach (var mapSkinObjectLink in mapDefinition.MapSkins)
        {
            var mapSkinObject = shippingBinTree.Objects[Fnv1a.HashLower(nameof(Meta.MapSkin))];
            var mapSkinDefinition = MetaSerializer.Deserialize<Meta.MapSkin>(
                this._metaEnvironment,
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

            var mapSkinPackage = CreateMapSkinPackage(mapSkinDefinition, wad);
        }

        return new() { };
    }

    private MapSkinPackage CreateMapSkinPackage(Meta.MapSkin mapSkinDefinition, WadFile wad)
    {
        Log.Information(
            "Creating map skin package (mapSkinName = {mapSkinName})",
            mapSkinDefinition.Name
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

        return new ();
    }

    private List<StaticMaterialPackage> CollectStaticMaterialPackages(BinTree binTree)
    {
        var staticMaterialDefClassHash = Fnv1a.HashLower(nameof(Meta.StaticMaterialDef));
        return binTree
            .Objects.Values.Where(x => x.ClassHash == staticMaterialDefClassHash)
            .Select(x => new StaticMaterialPackage(
                MetaSerializer.Deserialize<Meta.StaticMaterialDef>(this._metaEnvironment, x)
            ))
            .ToList();
    }

    private BinTree ResolveMapShippingBinTree(WadFile wad, string mapName)
    {
        string mapShippingPath = $"data/maps/shipping/{mapName}/{mapName}.bin";

        using var mapShippingBinStream = wad.LoadChunkDecompressed(mapShippingPath).AsStream();
        return new(mapShippingBinStream);
    }
}
