using System.Text.Json;
using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Animation;
using LeagueToolkit.Core.Wad;
using LeagueToolkit.Meta;
using lol_convert.Animation;
using lol_convert.Meta;
using lol_convert.Packages;
using lol_convert.Services;
using lol_convert.Utils;
using Serilog;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Converters;

internal partial class CharacterConverter
{
    private void ProduceAnimationGraph(
        string character,
        string skin,
        MetaClass.AnimationGraphData metaAnimationGraph,
        List<(string, IAnimationAsset)> animations
    )
    {
        DumpAnimations(character, skin, animations);

        try
        {
            SaveAnimationGraph(character, skin, new(metaAnimationGraph));
        }
        catch (Exception e)
        {
            Log.Error(
                e,
                "Failed to save animation graph (character: {character}, skin: {skin})",
                character,
                skin
            );
        }
    }

    private void SaveAnimationGraph(string character, string skin, AnimationGraph animationGraph)
    {
        string skinDataDirectoryPath = PathBuilder.GetCharacterSkinDataDirectory(character, skin);
        string animationGraphDataPath = PathBuilder.GetCharacterSkinAnimationGraphDataPath(
            character,
            skin
        );

        Directory.CreateDirectory(Path.Join(this._outputPath, skinDataDirectoryPath));
        using var stream = File.Create(Path.Join(this._outputPath, animationGraphDataPath));
        JsonSerializer.Serialize(stream, animationGraph, JsonUtils.DefaultOptions);
    }

    private void DumpAnimations(
        string character,
        string skin,
        List<(string, IAnimationAsset)> animations
    )
    {
        string animationsDirectoryPath = PathBuilder.GetCharacterSkinAnimationAssetsDirectory(
            character,
            skin
        );
        Directory.CreateDirectory(Path.Join(this._outputPath, animationsDirectoryPath));

        foreach (var (animationName, animationAsset) in animations)
        {
            try
            {
                AnimationDump.DumpAnimationAsset(
                    Path.Join(
                        this._outputPath,
                        PathBuilder.GetCharacterSkinAnimationAssetPath(
                            character,
                            skin,
                            animationName
                        )
                    ),
                    animationAsset
                );
            }
            catch (Exception e)
            {
                Log.Error(
                    e,
                    "Failed to dump animation asset (character: {character}, skin: {skin}, animation: {animationName})",
                    character,
                    skin,
                    animationName
                );
            }
        }
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

    private static List<string> CollectAtomicClipResourcePaths(
        MetaClass.AnimationGraphData animationGraph
    )
    {
        if (animationGraph.ClipDataMap is null)
        {
            return [];
        }

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

    private MetaClass.AnimationGraphData ResolveAnimationGraphData(
        MetaClass.SkinCharacterDataProperties skinCharacterProperties,
        BinObjectContainer binObjectContainer
    )
    {
        return skinCharacterProperties.SkinAnimationProperties?.Value?.AnimationGraphData switch
        {
            MetaObjectLink animationGraphDataLink
                => MetaSerializer.Deserialize<MetaClass.AnimationGraphData>(
                    this._metaEnvironment,
                    binObjectContainer.Objects[animationGraphDataLink]
                ),
            _ => null
        };
    }
}
