﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Animation;
using LeagueToolkit.Core.Mesh;
using LeagueToolkit.Core.Meta;
using LeagueToolkit.Core.Wad;
using LeagueToolkit.Hashing;
using LeagueToolkit.IO.SimpleSkinFile;
using LeagueToolkit.Meta;
using LeagueToolkit.Meta.Classes;
using LeagueToolkit.Toolkit.Gltf;
using Serilog;
using SharpGLTF.Schema2;
using Skeleton = LeagueToolkit.Core.Animation.RigResource;

namespace lol_convert;

public sealed class LeagueConverter
{
    public string OutputPath { get; private set; }
    public LeagueConverterOptions Options { get; private set; }

    private readonly WadHashtable _hashtable;
    private readonly MetaEnvironment _metaEnvironment;

    public LeagueConverter(
        string outputPath,
        WadHashtable hashtable,
        LeagueConverterOptions options
    )
    {
        this.OutputPath = outputPath;
        this._hashtable = hashtable;
        this._metaEnvironment = MetaEnvironment.Create(
            Assembly.Load("LeagueToolkit.Meta.Classes").ExportedTypes.Where(x => x.IsClass)
        );
        this.Options = options;
    }

    public LeaguePackage CreateLeaguePackage(string finalPath)
    {
        var championPackages = CreateChampionPackages(finalPath);
        List<string> championPackagePaths = this.Options.ConvertChampions switch
        {
            true => SaveChampionPackages(championPackages),
            false => []
        };

        return new() { ChampionPackagePaths = championPackagePaths };
    }

    public List<string> SaveChampionPackages(List<ChampionPackage> championPackages)
    {
        List<string> championPackagePaths = new(championPackages.Count);
        foreach (var championPackage in championPackages)
        {
            var championPackagePath = SaveChampionPackage(championPackage);
            championPackagePaths.Add(championPackagePath);
        }

        return championPackagePaths;
    }

    public string SaveChampionPackage(ChampionPackage championPackage)
    {
        var championName = championPackage.Name.ToLower();
        var championPackageDirectory = Path.Join(
            this.OutputPath,
            $"data/characters/{championName}"
        );
        var championPackagePath = Path.Join(championPackageDirectory, $"{championName}.json");

        Directory.CreateDirectory(championPackageDirectory);
        using var championPackageStream = File.Create(championPackagePath);
        JsonSerializer.Serialize(
            championPackageStream,
            championPackage,
            options: new()
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            }
        );

        return championPackagePath;
    }

    public List<ChampionPackage> CreateChampionPackages(string finalPath)
    {
        var championWadPaths = ConvertUtils.GlobChampionWads(finalPath).ToList();
        List<ChampionPackage> championPackages = new(championWadPaths.Count);
        foreach (string championWadPath in championWadPaths)
        {
            var championWadName = Path.GetFileName(championWadPath);
            var championName = championWadName.ToLower().Remove(championWadName.IndexOf('.'));

            WadFile wad = new(File.OpenRead(championWadPath));
            var chunkPaths = wad
                .Chunks.Keys.Select(x => this._hashtable.Resolve(x).ToLower())
                .ToList();

            var championPackage = CreateChampionPackage(championName, wad, chunkPaths);
            championPackages.Add(championPackage);
        }

        return championPackages;
    }

    private ChampionPackage CreateChampionPackage(
        string championName,
        WadFile wad,
        List<string> chunkPaths
    )
    {
        var skins = CreateChampionSkins(championName, wad, chunkPaths);

        return new() { Name = championName, Skins = skins };
    }

    private List<ChampionSkin> CreateChampionSkins(
        string championName,
        WadFile wad,
        List<string> chunkPaths
    )
    {
        var skinBinPaths = ConvertUtils.GlobChampionSkinBinPaths(championName, chunkPaths).ToList();
        List<ChampionSkin> skins = new(skinBinPaths.Count);
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

            if (this.Options.ConvertChampionSkins is false && skinName != "skin0")
            {
                Log.Verbose(
                    "Skipping champion skin package (championName: {championName}, skinName: {skinName})",
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

    private ChampionSkin CreateChampionSkin(
        string championName,
        string skinName,
        BinTree bin,
        WadFile wad
    )
    {
        string skinPropertiesObjectPath = $"characters/{championName}/skins/{skinName}";
        string meshAssetPath =
            $"assets/characters/{championName}/skins/{skinName}/{championName}.glb";
        string absoluteMeshAssetPath = Path.Join(this.OutputPath, meshAssetPath);

        ChampionSkin skin =
            new()
            {
                DisplayName = "TODO",
                SkinMeshPath = meshAssetPath,
                SkinScale = 1.0f
            };

        Directory.CreateDirectory(Path.GetDirectoryName(absoluteMeshAssetPath));

        var skinPropertiesObject = bin.Objects[Fnv1a.HashLower(skinPropertiesObjectPath)];
        var skinProperties = MetaSerializer.Deserialize<SkinCharacterDataProperties>(
            this._metaEnvironment,
            skinPropertiesObject
        );

        if (
            skinProperties.SkinMeshProperties?.Value
            is not SkinMeshDataProperties skinMeshProperties
        )
        {
            throw new NullReferenceException("SkinMeshProperties does not exist");
        }

        skin.SkinScale = skinMeshProperties.SkinScale;

        Skeleton rig = new(wad.LoadChunkDecompressed(skinMeshProperties.Skeleton).AsStream());
        SkinnedMesh simpleSkin = SkinnedMesh.ReadFromSimpleSkin(
            wad.LoadChunkDecompressed(skinMeshProperties.SimpleSkin).AsStream()
        );
        var animations = LoadAnimations(championName, skinName, wad);

        Log.Verbose("Saving glTf -> {meshAssetPath}", meshAssetPath);
        simpleSkin
            .ToGltf(rig, new List<(string, Stream)>(), animations)
            .Save(absoluteMeshAssetPath);

        return skin;
    }

    private List<(string, IAnimationAsset)> LoadAnimations(
        string championName,
        string skinName,
        WadFile wad
    )
    {
        var animationPaths = wad
            .Chunks.Keys.Select(x => this._hashtable.Resolve(x).ToLower())
            .Where(chunkPath =>
                chunkPath.StartsWith(
                    $"assets/characters/{championName}/skins/{skinName}/animations/",
                    StringComparison.OrdinalIgnoreCase
                )
            );

        List<(string, IAnimationAsset)> animations = new();
        foreach (var animationPath in animationPaths)
        {
            IAnimationAsset animationAsset = null;
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
}

public sealed class LeagueConverterOptions
{
    public bool ConvertChampions { get; set; } = true;
    public bool ConvertChampionSkins { get; set; } = true;
}
