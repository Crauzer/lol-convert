using System.Text.Json;
using LeagueToolkit.Meta;
using lol_convert.Meta;
using lol_convert.Packages;
using lol_convert.Services;
using lol_convert.Utils;
using Serilog;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Converters;

internal partial class CharacterConverter
{
    private AnimationGraph CreateAnimationGraph(
        string character,
        string skin,
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
                "Failed to find animation graph data link (character: {character}, skin: {skin})",
                character,
                skin
            );
            return new();
        }

        var animationGraph = MetaSerializer.Deserialize<MetaClass.AnimationGraphData>(
            this._metaEnvironment,
            binObjectContainer.Objects[animationGraphDataLink]
        );

        return new(animationGraph);
    }

    private void ProduceAnimationGraph(string character, string skin, AnimationGraph animationGraph)
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
}
