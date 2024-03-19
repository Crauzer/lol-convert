using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
    private const string CHAMPIONS_PATH = "/Champions";

    public string OutputPath { get; private set; }

    private readonly WadHashtable _hashtable;
    private readonly MetaEnvironment _metaEnvironment;

    public LeagueConverter(string outputPath, WadHashtable hashtable)
    {
        this.OutputPath = outputPath;
        this._hashtable = hashtable;
        this._metaEnvironment = MetaEnvironment.Create(
            Assembly.Load("LeagueToolkit.Meta.Classes").ExportedTypes.Where(x => x.IsClass)
        );
    }

    public LeaguePackage CreateLeaguePackage(string finalPath)
    {
        var championPackages = CreateChampionPackages(finalPath);

        return new() {  };
    }

    public List<ChampionPackage> CreateChampionPackages(string finalPath)
    {
        var championWadPaths = Directory
            .EnumerateFiles(Path.Join(finalPath, CHAMPIONS_PATH))
            .Where(path => Regex.IsMatch(Path.GetFileName(path), $@"^[\w]+\.wad\.client"))
            .ToList();

        List<ChampionPackage> championPackages = new(championWadPaths.Count);
        foreach (string championWadPath in championWadPaths)
        {
            var championWadName = Path.GetFileName(championWadPath);
            var championName = championWadName.ToLower().Remove(championWadName.IndexOf('.'));
            WadFile wad = new(File.OpenRead(championWadPath));

            var championPackage = CreateChampionPackage(championName, wad);
            championPackages.Add(championPackage);
        }

        return championPackages;
    }

    private ChampionPackage CreateChampionPackage(string championName, WadFile wad)
    {
        string skinPrefixPath = $"data/characters/{championName}/skins/";

        var skinBinPaths = wad
            .Chunks.Keys.Select(x => this._hashtable.Resolve(x).ToLower())
            .Where(chunkPath =>
                chunkPath.StartsWith(skinPrefixPath, StringComparison.OrdinalIgnoreCase)
            )
            .ToList();

        List<ChampionSkin> skins = new(skinBinPaths.Count);
        foreach (string skinBinPath in skinBinPaths)
        {
            string skinName = Path.GetFileNameWithoutExtension(skinBinPath);

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

        return new() { Name = championName, Skins = skins };
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
        return wad
            .Chunks.Keys.Select(x => this._hashtable.Resolve(x).ToLower())
            .Where(chunkPath =>
                chunkPath.StartsWith(
                    $"assets/characters/{championName}/skins/{skinName}/animations/",
                    StringComparison.OrdinalIgnoreCase
                )
            )
            .Select(animationPath =>
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
                }

                return (Path.GetFileNameWithoutExtension(animationPath), animationAsset);
            })
            .Where(x => x.Item2 is not null)
            .ToList();
    }
}
