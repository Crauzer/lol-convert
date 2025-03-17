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
using LeagueToolkit.IO.MapGeometryFile;
using LeagueToolkit.Meta;
using LeagueToolkit.Meta.Classes;
using lol_convert.Packages;
using lol_convert.Services;
using lol_convert.Utils;
using lol_convert.Wad;
using Serilog;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Converters;

internal class MapConverter
{
    private readonly WadHashtable _wadHashtable;
    private readonly MetaEnvironment _metaEnvironment;
    private readonly string _outputPath;

    public MapConverterSettings Settings { get; init; }
    private readonly CharacterConverter _characterConverter;

    public MapConverter(
        WadHashtable wadHashtable,
        MetaEnvironment metaEnvironment,
        string outputPath,
        MapConverterSettings settings
    )
    {
        _wadHashtable = wadHashtable;
        _metaEnvironment = metaEnvironment;
        _outputPath = outputPath;

        this.Settings = settings;

        _characterConverter = new(wadHashtable, metaEnvironment, outputPath);
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

            var mapPackage = CreateMapPackageFromLeague(mapName, finalPath);
            if (mapPackage is not null)
            {
                mapPackages.Add(mapPackage.Name);
            }
        }

        return mapPackages;
    }

    public MapPackage CreateMapPackageFromLeague(string mapName, string finalPath)
    {
        Log.Information("Creating map package (mapName = {mapName})", mapName);

        var mapWadPath = Path.Combine(finalPath, "maps", "shipping", $"{mapName}.wad.client");
        using var wad = new WadFile(mapWadPath);

        var mapShippingBinTree = ResolveMapShippingBinTree(wad, mapName);

        try
        {
            var mapPackage = CreateMapPackage(mapName, mapShippingBinTree, wad);
            SaveMapPackage(mapPackage);
            return mapPackage;
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to create map package (mapName = {mapName})", mapName);
            return null;
        }
    }

    public MapPackage CreateMapPackage(string mapName, BinTree shippingBinTree, WadFile wad)
    {
        Log.Information("Creating map package (mapName = {mapName})", mapName);

        Directory.CreateDirectory(Path.Combine(_outputPath, "data", "maps", mapName));
        Directory.CreateDirectory(Path.Combine(_outputPath, "data", "maps", mapName, "skins"));

        var mapBinObject = shippingBinTree
            .Objects.FirstOrDefault(x => x.Value.ClassHash == Fnv1a.HashLower(nameof(Map)))
            .Value;
        var mapDefinition = MetaSerializer.Deserialize<Map>(_metaEnvironment, mapBinObject);

        var chunkPaths = ConvertUtils.ResolveWadChunkPaths(wad, _wadHashtable).ToList();

        var linkedCharacters = this.Settings.ConvertLinkedCharacters
            ? CollectLinkedCharacterNames(shippingBinTree)
            : [];
        
        if(this.Settings.ConvertLinkedCharacters)
        {
            ConvertLinkedCharacters(linkedCharacters, wad, chunkPaths);
        }

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

        return new()
        {
            Name = mapName.ToLower(),
            Skins = mapSkinNames,
            LinkedCharacters = linkedCharacters
        };
    }

    private static List<string> CollectLinkedCharacterNames(BinTree shipping)
    {
        return shipping
            .Objects.Where(x => x.Value.ClassHash == Fnv1a.HashLower(nameof(MetaClass.Character)))
            .Select(x =>
                MetaSerializer.Deserialize<MetaClass.Character>(
                    MetaEnvironmentService.Environment,
                    x.Value
                )
            )
            .Select(x => x.Name)
            .ToList();
    }

    private void ConvertLinkedCharacters(
        List<string> characters,
        WadFile wad,
        List<string> chunkPaths
    )
    {
        Log.Information("Converting linked characters...");

        foreach (var characterName in characters)
        {
            try
            {
                var characterData = _characterConverter.CreateCharacterData(
                    characterName,
                    wad,
                    chunkPaths
                );
                _characterConverter.SaveCharacterData(characterData);
            }
            catch (Exception exception)
            {
                Log.Error(
                    exception,
                    "Failed to convert character ({characterName})",
                    characterName
                );
            }
        }
    }

    private MapSkinPackage CreateMapSkinPackage(
        string mapName,
        MapSkin mapSkinDefinition,
        WadFile wad
    )
    {
        string skinName = mapSkinDefinition.Name.ToLower();

        Log.Information("Creating map skin package (mapSkinName = {0})", skinName);
        Directory.CreateDirectory(
            Path.Join(
                this._outputPath,
                PathBuilder.GenerateMapSkinDataDirectoryPath(mapName, skinName)
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

        TrySaveMapAsset(mapName, skinName, environmentAsset, materialsBin);

        return new()
        {
            Name = mapSkinDefinition.Name.ToLower(),
            AssetPath = PathBuilder.GenerateMapSkinAssetPath(mapName, skinName),
            Container = new(mapContainer),
            VfxSystems = CollectVfxSystems(materialsBin),
            StaticMaterials = staticMaterialPackages,
            ShaderTextureOverrides = environmentAsset
                .ShaderTextureOverrides.Select(x => new MapShaderTextureOverridePackage(x))
                .ToList(),
            VisibilityControllers = CollectMapVisibilityControllers(materialsBin),
            Meshes = CreateMapMeshes(environmentAsset),
            Chunks = CollectChunks(materialsBin)
        };
    }

    private static MapContainer ResolveMapContainer(
        BinTree shippingBinTree,
        string mapContainerLink
    )
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
                MetaSerializer.Deserialize<StaticMaterialDef>(MetaEnvironmentService.Environment, x)
            ));
    }

    private Dictionary<string, VfxSystem> CollectVfxSystems(BinTree materialsBin)
    {
        var vfxSystemClassHash = Fnv1a.HashLower(nameof(MetaClass.VfxSystemDefinitionData));
        return materialsBin
            .Objects.Values.Where(x => x.ClassHash == vfxSystemClassHash)
            .Select(x => new VfxSystem(
                MetaSerializer.Deserialize<MetaClass.VfxSystemDefinitionData>(
                    MetaEnvironmentService.Environment,
                    x
                )
            ))
            .Select(x => new KeyValuePair<string, VfxSystem>(x.ParticlePath, x))
            .ToDictionary();
    }

    private Dictionary<string, MapPlaceableContainerPackage> CollectChunks(BinTree materialsBin)
    {
        var mapPlaceableContainerClassHash = Fnv1a.HashLower(nameof(MapPlaceableContainer));
        return materialsBin
            .Objects.Values.Where(x => x.ClassHash == mapPlaceableContainerClassHash)
            .Select(treeObject => new KeyValuePair<string, MapPlaceableContainerPackage>(
                BinHashtableService.ResolveObjectLink(treeObject.PathHash),
                new(
                    MetaSerializer.Deserialize<MapPlaceableContainer>(
                        MetaEnvironmentService.Environment,
                        treeObject
                    )
                )
            ))
            .ToDictionary();
    }

    private static Dictionary<string, MapMeshPackage> CreateMapMeshes(
        EnvironmentAsset environmentAsset
    )
    {
        Dictionary<string, MapMeshPackage> map = [];
        foreach (EnvironmentAssetMesh mesh in environmentAsset.Meshes)
        {
            map.Add(
                mesh.Name,
                new()
                {
                    LegacyVisibilityFlags = (byte)mesh.VisibilityFlags,
                    VisibilityController =
                        mesh.VisibilityControllerPathHash != 0
                            ? BinHashtableService.ResolveObjectLink(
                                mesh.VisibilityControllerPathHash
                            )
                            : null,
                    DisableBackfaceCulling = mesh.DisableBackfaceCulling,
                    QualityFilter = (byte)mesh.EnvironmentQualityFilter,
                    LayerTransitionBehavior = (byte)mesh.LayerTransitionBehavior,
                    RenderFlags = (ushort)mesh.RenderFlags,
                    StationaryLight = new(mesh.StationaryLight),
                    BakedLight = new(mesh.BakedLight),
                    TextureOverrides = mesh
                        .TextureOverrides.Select(x => new MapMeshTextureOverridePackage(x))
                        .ToList(),
                    BakedPaintScale = mesh.BakedPaintScale,
                    BakedPaintBias = mesh.BakedPaintBias
                }
            );
        }

        return map;
    }

    private static Dictionary<string, MapVisibilityControllerBase> CollectMapVisibilityControllers(
        BinTree materialsBin
    )
    {
        Dictionary<string, MapVisibilityControllerBase> map = [];
        foreach (var treeObject in materialsBin.Objects)
        {
            MapVisibilityControllerBase package = DeserializeMapVisibilityController(
                treeObject.Value
            ) switch
            {
                MetaClass.ChildMapVisibilityController childMapVisibilityController
                    => new ChildMapVisibilityControllerPackage(childMapVisibilityController),
                MetaClass.Class0x6b863734 class0x6b863734
                    => new LegacyMapVisibilityControllerPackage(class0x6b863734),
                MetaClass.Class0xec733fe2 class0xec733fe2
                    => new Parent2MapVisibilityControllerPackage(class0xec733fe2),
                MetaClass.Class0xe07edfa4 class0xe07edfa4
                    => new Parent1MapVisibilityControllerPackage(class0xe07edfa4),
                _ => null
            };

            if (package is not null)
            {
                map.Add(BinHashtableService.ResolveObjectLink(treeObject.Key), package);
            }
        }

        return map;
    }

    private static MetaClass.IMapVisibilityController DeserializeMapVisibilityController(
        BinTreeObject treeObject
    )
    {
        return treeObject.ClassHash switch
        {
            _
                when treeObject.ClassHash
                    == Fnv1a.HashLower(nameof(MetaClass.ChildMapVisibilityController))
                => MetaSerializer.Deserialize<MetaClass.ChildMapVisibilityController>(
                    MetaEnvironmentService.Environment,
                    treeObject
                ),
            0x6b863734
                => MetaSerializer.Deserialize<MetaClass.Class0x6b863734>(
                    MetaEnvironmentService.Environment,
                    treeObject
                ),
            0xe07edfa4
                => MetaSerializer.Deserialize<MetaClass.Class0xe07edfa4>(
                    MetaEnvironmentService.Environment,
                    treeObject
                ),
            0xec733fe2
                => MetaSerializer.Deserialize<MetaClass.Class0xec733fe2>(
                    MetaEnvironmentService.Environment,
                    treeObject
                ),
            _ => null
        };
    }

    private static BinTree ResolveMapShippingBinTree(WadFile wad, string mapName)
    {
        string mapShippingPath = $"data/maps/shipping/{mapName}/{mapName}.bin";

        using var mapShippingBinStream = wad.LoadChunkDecompressed(mapShippingPath).AsStream();
        return new(mapShippingBinStream);
    }

    private void TrySaveMapAsset(
        string mapName,
        string mapSkinName,
        EnvironmentAsset environmentAsset,
        BinTree materialsBin
    )
    {
        try
        {
            Log.Information(
                "Saving map asset (mapName: {0}, mapSkinName: {1})",
                mapName,
                mapSkinName
            );

            Directory.CreateDirectory(
                Path.Join(
                    this._outputPath,
                    PathBuilder.GenerateMapSkinAssetDirectoryPath(mapName, mapSkinName)
                )
            );

            environmentAsset
                .ToGltf(
                    materialsBin,
                    new()
                    {
                        MetaEnvironment = MetaEnvironmentService.Environment,
                        Settings = new()
                        {
                            LayerGroupingPolicy = MapGeometryGltfLayerGroupingPolicy.Ignore
                        }
                    }
                )
                .SaveGLB(
                    Path.Join(
                        this._outputPath,
                        PathBuilder.GenerateMapSkinAssetPath(mapName, mapSkinName)
                    )
                );
        }
        catch (Exception e)
        {
            Log.Error(
                e,
                "Failed to save map asset (mapName: {0}, mapSkinName: {1})",
                mapName,
                mapSkinName
            );
        }
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

public struct MapConverterSettings
{
    public bool ConvertLinkedCharacters { get; set; }
}
