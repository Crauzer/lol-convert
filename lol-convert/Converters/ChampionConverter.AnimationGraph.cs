using LeagueToolkit.Meta;
using lol_convert.Meta;
using lol_convert.Packages;
using Serilog;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Converters;

internal partial class ChampionConverter
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
}
