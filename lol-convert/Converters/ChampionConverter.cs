﻿using System.Text.Json;
using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Animation;
using LeagueToolkit.Core.Mesh;
using LeagueToolkit.Core.Meta;
using LeagueToolkit.Core.Wad;
using LeagueToolkit.Hashing;
using LeagueToolkit.IO.SimpleSkinFile;
using LeagueToolkit.Meta;
using lol_convert.Meta;
using lol_convert.Packages;
using lol_convert.Services;
using lol_convert.Utils;
using lol_convert.Wad;
using Serilog;
using MetaClass = LeagueToolkit.Meta.Classes;
using Skeleton = LeagueToolkit.Core.Animation.RigResource;

namespace lol_convert.Converters;

internal class ChampionConverter
{
    private readonly WadHashtable _wadHashtable;
    private readonly MetaEnvironment _metaEnvironment;
    private readonly string _outputPath;

    public ChampionConverter(
        WadHashtable wadHashtable,
        MetaEnvironment metaEnvironment,
        string outputPath
    )
    {
        _wadHashtable = wadHashtable;
        _metaEnvironment = metaEnvironment;
        _outputPath = outputPath;
    }

    public List<string> CreateChampionPackages(string finalPath)
    {
        var championWadPaths = ConvertUtils.GlobChampionWads(finalPath).ToList();
        List<string> championPackagePaths = new(championWadPaths.Count);
        foreach (string championWadPath in championWadPaths)
        {
            var championWadName = Path.GetFileName(championWadPath);
            var championName = championWadName.ToLower().Remove(championWadName.IndexOf('.'));

            WadFile wad = new(File.OpenRead(championWadPath));
            var chunkPaths = wad
                .Chunks.Keys.Select(x => _wadHashtable.Resolve(x).ToLower())
                .ToList();

            var championPackage = CreateChampionPackage(championName, wad, chunkPaths);
            var championPackagePath = SaveChampionPackage(championPackage);

            championPackagePaths.Add(Path.GetRelativePath(_outputPath, championPackagePath));
        }

        return championPackagePaths;
    }

    private ChampionPackage CreateChampionPackage(
        string championName,
        WadFile wad,
        List<string> chunkPaths
    )
    {
        var skins = CreateChampionSkins(championName, wad, chunkPaths);
        foreach (var skin in skins)
        {
            try
            {
                SaveChampionSkinPackage(championName, skin);
            }
            catch (Exception e)
            {
                Log.Error(
                    e,
                    "Failed to save champion skin package (championName: {championName}, skinName: {skinName})",
                    championName,
                    skin.Name
                );
            }
        }

        return new() { Name = championName, SkinNames = skins.Select(x => x.Name).ToList() };
    }

    private void SaveChampionSkinPackage(string championName, ChampionSkinPackage skin)
    {
        Log.Information(
            "Saving champion skin package (championName: {championName}, skinName: {skinName})",
            championName,
            skin.Name
        );

        var championPackageDirectory = CreateChampionPackageDirectoryPath(championName);
        var skinDirectory = Path.Join(championPackageDirectory, "skins", skin.Name);
        var skinPackagePath = Path.Join(skinDirectory, $"{skin.Name}.json");

        Directory.CreateDirectory(skinDirectory);
        using var skinStream = File.Create(skinPackagePath);
        JsonSerializer.Serialize(skinStream, skin, JsonUtils.DefaultOptions);
    }

    public string SaveChampionPackage(ChampionPackage championPackage)
    {
        var championName = championPackage.Name.ToLower();
        var championPackageDirectory = CreateChampionPackageDirectoryPath(championName);
        var championPackagePath = Path.Join(championPackageDirectory, $"{championName}.json");

        Directory.CreateDirectory(championPackageDirectory);
        using var championPackageStream = File.Create(championPackagePath);
        JsonSerializer.Serialize(championPackageStream, championPackage, JsonUtils.DefaultOptions);

        return championPackagePath;
    }

    private List<ChampionSkinPackage> CreateChampionSkins(
        string championName,
        WadFile wad,
        List<string> chunkPaths
    )
    {
        var skinBinPaths = ConvertUtils.GlobChampionSkinBinPaths(championName, chunkPaths).ToList();
        List<ChampionSkinPackage> skins = new(skinBinPaths.Count);
        foreach (string skinBinPath in skinBinPaths)
        {
            string skinName = Path.GetFileNameWithoutExtension(skinBinPath);
            if (skinName == "root")
            {
                Log.Verbose(
                    "Skipping root champion skin package (championName: {championName})",
                    championName,
                    skinName
                );
                continue;
            }

            try
            {
                var skin = CreateChampionSkin(
                    championName,
                    skinName,
                    new(wad.LoadChunkDecompressed(skinBinPath).AsStream()),
                    wad
                );
                skins.Add(skin);
            }
            catch (Exception e)
            {
                Log.Error(
                    e,
                    "Failed to create champion skin package (championName: {championName}, skinName: {skinName})",
                    championName,
                    skinName
                );
            }
        }

        return skins;
    }

    private ChampionSkinPackage CreateChampionSkin(
        string championName,
        string skinName,
        BinTree bin,
        WadFile wad
    )
    {
        string skinPropertiesObjectPath = $"characters/{championName}/skins/{skinName}";
        string meshAssetPath =
            $"assets/characters/{championName}/skins/{skinName}/{championName}_{skinName}.glb";

        var binObjectContainer = BinObjectContainer.FromPropertyBin(bin, wad);
        var staticMaterials = CreateChampionSkinMaterials(binObjectContainer.Objects.Values);
        var vfxSystems = CreateVfxSystems(binObjectContainer.Objects.Values);
        var resourceResolver = CreateResourceResolver(
            binObjectContainer.Objects.Values,
            championName,
            skinName
        );

        ChampionSkinPackage skin =
            new()
            {
                Name = skinName,
                DisplayName = "TODO",
                SkinMeshPath = meshAssetPath,
                Materials = staticMaterials,
                VfxSystems = vfxSystems,
                ResourceResolver = resourceResolver
            };

        var skinProperties = MetaSerializer.Deserialize<MetaClass.SkinCharacterDataProperties>(
            MetaEnvironmentService.Environment,
            bin.Objects[Fnv1a.HashLower(skinPropertiesObjectPath)]
        );
        if (
            skinProperties.SkinMeshProperties?.Value
            is not MetaClass.SkinMeshDataProperties skinMeshProperties
        )
        {
            throw new NullReferenceException("SkinMeshProperties does not exist");
        }

        skin.SkinScale = skinMeshProperties.SkinScale;
        skin.Material =
            skinMeshProperties.Material != 0
                ? BinHashtableService.ResolveObjectLink(skinMeshProperties.Material)
                : null;
        skin.Texture = skinMeshProperties.Texture;
        skin.MaterialOverrides = CollectMaterialOverrides(skinMeshProperties);

        ProduceChampionSkinMesh(
            championName,
            skinName,
            meshAssetPath,
            skinProperties,
            binObjectContainer,
            wad
        );

        return skin;
    }

    private List<StaticMaterialPackage> CreateChampionSkinMaterials(
        IEnumerable<BinTreeObject> binObjects
    )
    {
        List<StaticMaterialPackage> materialPackages = [];

        var staticMaterialObjects = binObjects.Where(binObject =>
            binObject.ClassHash == Fnv1a.HashLower(nameof(MetaClass.StaticMaterialDef))
        );

        foreach (var staticMaterialObject in staticMaterialObjects)
        {
            MetaClass.StaticMaterialDef staticMaterialDef = null;
            try
            {
                staticMaterialDef = MetaSerializer.Deserialize<MetaClass.StaticMaterialDef>(
                    _metaEnvironment,
                    staticMaterialObject
                );
            }
            catch (Exception ex)
            {
                Log.Error(
                    ex,
                    "Failed to deserialize static material object (object: {object})",
                    BinHashtableService.TryResolveObjectLink(staticMaterialObject.PathHash)
                );
            }

            materialPackages.Add(new(staticMaterialDef));
        }

        return materialPackages;
    }

    public Dictionary<string, VfxSystem> CreateVfxSystems(IEnumerable<BinTreeObject> binObjects)
    {
        Dictionary<string, VfxSystem> vfxSystems = [];

        var vfxSystemObjects = binObjects.Where(binObject =>
            binObject.ClassHash == Fnv1a.HashLower(nameof(MetaClass.VfxSystemDefinitionData))
        );
        foreach (var vfxSystemObject in vfxSystemObjects)
        {
            MetaClass.VfxSystemDefinitionData vfxSystemDefinitionData = null;
            try
            {
                vfxSystemDefinitionData =
                    MetaSerializer.Deserialize<MetaClass.VfxSystemDefinitionData>(
                        _metaEnvironment,
                        vfxSystemObject
                    );
            }
            catch (Exception ex)
            {
                Log.Error(
                    ex,
                    "Failed to deserialize Vfx System object (object: {object})",
                    BinHashtableService.ResolveObjectLink(vfxSystemObject.PathHash)
                );
            }

            vfxSystems.Add(
                BinHashtableService.ResolveObjectLink(vfxSystemObject.PathHash),
                new(vfxSystemDefinitionData)
            );
        }

        return vfxSystems;
    }

    public Dictionary<string, string> CreateResourceResolver(
        IEnumerable<BinTreeObject> binObjects,
        string character,
        string skin
    )
    {
        var resourceResolvers = binObjects
            .Where(binObject =>
                binObject.ClassHash == Fnv1a.HashLower(nameof(MetaClass.ResourceResolver))
            )
            .ToArray();

        if (resourceResolvers.Length == 0)
        {
            Log.Warning(
                "No resource resolvers found (character: {character}, skin: {skin})",
                character,
                skin
            );
            return null;
        }

        if (resourceResolvers.Length > 1)
        {
            Log.Warning(
                "Multiple resource resolvers found (character: {character}, skin: {skin})",
                character,
                skin
            );
            return null;
        }

        var resourceResolver = MetaSerializer.Deserialize<MetaClass.ResourceResolver>(
            MetaEnvironmentService.Environment,
            resourceResolvers[0]
        );

        return resourceResolver
            .ResourceMap.Select(x => new KeyValuePair<string, string>(
                BinHashtableService.TryResolveHash(x.Key),
                BinHashtableService.TryResolveObjectLink(x.Value)
            ))
            .ToDictionary();
    }

    private static List<SkinMeshMaterialOverridePackage> CollectMaterialOverrides(
        MetaClass.SkinMeshDataProperties skinMeshDataProperties
    ) =>
        skinMeshDataProperties
            .MaterialOverride?.Select(materialOverride => new SkinMeshMaterialOverridePackage(
                materialOverride
            ))
            .ToList() ?? [];

    private void ProduceChampionSkinMesh(
        string championName,
        string skinName,
        string meshAssetPath,
        MetaClass.SkinCharacterDataProperties skinCharacterProperties,
        BinObjectContainer binObjectContainer,
        WadFile wad
    )
    {
        string absoluteMeshAssetPath = Path.Join(this._outputPath, meshAssetPath);
        Directory.CreateDirectory(Path.GetDirectoryName(absoluteMeshAssetPath));

        Skeleton rig =
            new(
                wad.LoadChunkDecompressed(skinCharacterProperties.SkinMeshProperties.Value.Skeleton)
                    .AsStream()
            );
        SkinnedMesh simpleSkin = SkinnedMesh.ReadFromSimpleSkin(
            wad.LoadChunkDecompressed(skinCharacterProperties.SkinMeshProperties.Value.SimpleSkin)
                .AsStream()
        );

        var animationGraphData = ResolveAnimationGraphData(
            championName,
            skinName,
            skinCharacterProperties,
            binObjectContainer
        );
        var animationPaths = CollectAtomicClipResourcePaths(animationGraphData);
        var animations = LoadAnimations(animationPaths, wad);

        Log.Verbose("Saving glTf -> {meshAssetPath}", meshAssetPath);
        simpleSkin
            .ToGltf(rig, new List<(string, Stream)>(), animations)
            .Save(absoluteMeshAssetPath);
    }

    private AnimationGraphPackage CreateAnimationGraph(
        string championName,
        string skinName,
        MetaClass.SkinCharacterDataProperties skinCharacterProperties,
        BinObjectContainer binObjectContainer
    )
    {
        if (
            skinCharacterProperties.SkinAnimationProperties?.Value?.AnimationGraphData
            is not MetaObjectLink animationGraphDataLink
        )
        {
            Log.Warning(
                "{championName} - {skinName} does not have an animation graph data link",
                championName,
                skinName
            );
            return new();
        }

        var animationGraph = MetaSerializer.Deserialize<MetaClass.AnimationGraphData>(
            this._metaEnvironment,
            binObjectContainer.Objects[animationGraphDataLink]
        );

        return new(animationGraph);
    }

    private static List<(string, IAnimationAsset)> LoadAnimations(
        IEnumerable<string> animationPaths,
        WadFile wad
    )
    {
        List<(string, IAnimationAsset)> animations = [];
        foreach (var animationPath in animationPaths)
        {
            IAnimationAsset animationAsset;
            try
            {
                animationAsset = AnimationAsset.Load(
                    wad.LoadChunkDecompressed(animationPath).AsStream()
                );
            }
            catch (Exception e)
            {
                Log.Error(
                    e,
                    "Failed to load animation asset (animationPath: {animationPath})",
                    animationPath
                );
                continue;
            }

            animations.Add((Path.GetFileNameWithoutExtension(animationPath), animationAsset));
        }

        return animations;
    }

    private MetaClass.AnimationGraphData ResolveAnimationGraphData(
        string championName,
        string skinName,
        MetaClass.SkinCharacterDataProperties skinCharacterProperties,
        BinObjectContainer binObjectContainer
    )
    {
        if (
            skinCharacterProperties.SkinAnimationProperties?.Value?.AnimationGraphData
            is not MetaObjectLink animationGraphDataLink
        )
        {
            return null;
        }

        return MetaSerializer.Deserialize<MetaClass.AnimationGraphData>(
            this._metaEnvironment,
            binObjectContainer.Objects[animationGraphDataLink]
        );
    }

    private static List<string> CollectAtomicClipResourcePaths(
        MetaClass.AnimationGraphData animationGraph
    )
    {
        List<string> paths = [];

        foreach (var clip in animationGraph.ClipDataMap.Values)
        {
            if (clip is not MetaClass.AtomicClipData atomicClip)
            {
                continue;
            }

            paths.Add(atomicClip.AnimationResourceData.Value.AnimationFilePath);
        }

        return paths;
    }

    private string CreateChampionPackageDirectoryPath(string championName)
    {
        return Path.Join(_outputPath, "data", "characters", championName);
    }
}
