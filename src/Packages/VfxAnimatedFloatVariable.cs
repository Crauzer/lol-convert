using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

internal class VfxAnimatedFloatVariable(MetaClass.VfxAnimatedFloatVariableData data)
{
    public List<VfxProbabilityTable> ProbabilityTables { get; set; } =
        data.ProbabilityTables?.Select(x => new VfxProbabilityTable(x)).ToList();
    public List<float> Times { get; set; } = data.Times?.ToList();
    public List<float> Values { get; set; } = data.Values?.ToList();
}
