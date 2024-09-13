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

internal partial class CharacterConverter
{
    private readonly WadHashtable _wadHashtable;
    private readonly MetaEnvironment _metaEnvironment;
    private readonly string _outputPath;

    public CharacterConverter(
        WadHashtable wadHashtable,
        MetaEnvironment metaEnvironment,
        string outputPath
    )
    {
        _wadHashtable = wadHashtable;
        _metaEnvironment = metaEnvironment;
        _outputPath = outputPath;
    }

    public Character CreateCharacterPackage(
        string characterName,
        WadFile wad,
        List<string> chunkPaths
    )
    {
        var skins = CreateCharacterSkins(characterName, wad, chunkPaths);
        foreach (var skin in skins)
        {
            try
            {
                SaveCharacterSkinPackage(characterName, skin);
            }
            catch (Exception e)
            {
                Log.Error(
                    e,
                    "Failed to save character skin package (characterName: {characterName}, skinName: {skinName})",
                    characterName,
                    skin.Name
                );
            }
        }

        return new() { Name = characterName, SkinNames = skins.Select(x => x.Name).ToList() };
    }

    private void SaveCharacterSkinPackage(string characterName, CharacterSkin skin)
    {
        Log.Information(
            "Saving character skin package (characterName: {characterName}, skinName: {skinName})",
            characterName,
            skin.Name
        );

        var skinDirectory = PathBuilder.CreateCharacterSkinDataDirectoryPath(
            characterName,
            skin.Name
        );
        var skinDataPath = PathBuilder.CreateCharacterSkinDataPath(characterName, skin.Name);

        Directory.CreateDirectory(Path.Combine(this._outputPath, skinDirectory));
        using var skinStream = File.Create(Path.Combine(this._outputPath, skinDataPath));
        JsonSerializer.Serialize(skinStream, skin, JsonUtils.DefaultOptions);
    }

    public string SaveCharacterPackage(Character character)
    {
        var characterName = character.Name.ToLower();
        var dataDirectoryPath = PathBuilder.CreateCharacterDataDirectoryPath(characterName);
        var dataPath = PathBuilder.CreateCharacterDataPath(characterName);

        Directory.CreateDirectory(Path.Combine(this._outputPath, dataDirectoryPath));
        using var championPackageStream = File.Create(Path.Combine(this._outputPath, dataPath));
        JsonSerializer.Serialize(championPackageStream, character, JsonUtils.DefaultOptions);

        return dataPath;
    }

    private List<CharacterSkin> CreateCharacterSkins(
        string characterName,
        WadFile wad,
        List<string> chunkPaths
    )
    {
        var skinBinPaths = ConvertUtils
            .GlobCharacterSkinBinPaths(characterName, chunkPaths)
            .ToList();
        List<CharacterSkin> skins = new(skinBinPaths.Count);
        foreach (string skinBinPath in skinBinPaths)
        {
            string skinName = Path.GetFileNameWithoutExtension(skinBinPath);
            if (skinName == "root")
            {
                Log.Verbose(
                    "Skipping root character skin package (characterName: {characterName})",
                    characterName,
                    skinName
                );
                continue;
            }

            try
            {
                var skin = CreateCharacterSkin(
                    characterName,
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
                    "Failed to create character skin package (characterName: {characterName}, skinName: {skinName})",
                    characterName,
                    skinName
                );
            }
        }

        return skins;
    }

    private CharacterSkin CreateCharacterSkin(
        string characterName,
        string skinName,
        BinTree bin,
        WadFile wad
    )
    {
        string meshAssetPath =
            $"assets/characters/{characterName}/skins/{skinName}/{characterName}_{skinName}.glb";

        var binObjectContainer = BinObjectContainer.FromPropertyBin(bin, wad);
        var staticMaterials = CreateCharacterSkinMaterials(binObjectContainer.Objects.Values);
        var vfxSystems = CreateVfxSystems(binObjectContainer.Objects.Values);
        var resourceResolver = CreateResourceResolver(
            binObjectContainer.Objects.Values,
            characterName,
            skinName
        );
        CharacterSkin skin =
            new()
            {
                Name = skinName,
                DisplayName = "TODO",
                SkinMeshPath = meshAssetPath,
                Materials = staticMaterials,
                VfxSystems = vfxSystems,
                ResourceResolver = resourceResolver
            };

        var skinProperties = ResolveSkinProperties(binObjectContainer, characterName, skinName);
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

        ProduceCharacterSkinMesh(
            characterName,
            skinName,
            meshAssetPath,
            skinProperties,
            binObjectContainer,
            wad
        );
        return skin;
    }

    private Dictionary<string, StaticMaterialPackage> CreateCharacterSkinMaterials(
        IEnumerable<BinTreeObject> binObjects
    )
    {
        Dictionary<string, StaticMaterialPackage> materials = [];

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
                continue;
            }

            materials.Add(
                BinHashtableService.ResolveObjectLink(staticMaterialObject.PathHash),
                new(staticMaterialDef)
            );
        }
        return materials;
    }

    private MetaClass.SkinCharacterDataProperties ResolveSkinProperties(
        BinObjectContainer bin,
        string characterName,
        string skinName
    )
    {
        string skinPropertiesObjectPath = $"characters/{characterName}/skins/{skinName}";

        var skinPropertiesObject = bin.Objects[Fnv1a.HashLower(skinPropertiesObjectPath)];
        return skinPropertiesObject.ClassHash switch
        {
            _
                when skinPropertiesObject.ClassHash
                    == Fnv1a.HashLower(nameof(MetaClass.SkinCharacterDataProperties))
                => MetaSerializer.Deserialize<MetaClass.SkinCharacterDataProperties>(
                    MetaEnvironmentService.Environment,
                    skinPropertiesObject
                ),
            _
                when skinPropertiesObject.ClassHash
                    == Fnv1a.HashLower(nameof(MetaClass.TftSkinCharacterDataProperties))
                => MetaSerializer.Deserialize<MetaClass.TftSkinCharacterDataProperties>(
                    MetaEnvironmentService.Environment,
                    skinPropertiesObject
                ),
            _
                => throw new InvalidOperationException(
                    $"Unknown skin properties object class: {skinPropertiesObject.ClassHash}"
                )
        };
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

    private void ProduceCharacterSkinMesh(
        string character,
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
            character,
            skinName,
            skinCharacterProperties,
            binObjectContainer
        );
        if (animationGraphData is null)
        {
            Log.Warning(
                "Failed to resolve animation graph data (character: {character}, skin: {skin})",
                character,
                skinName
            );
        }

        Log.Verbose("Saving glTf -> {meshAssetPath}", meshAssetPath);
        simpleSkin.ToGltf(rig, [], []).Save(absoluteMeshAssetPath);
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
        string characterName,
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
}