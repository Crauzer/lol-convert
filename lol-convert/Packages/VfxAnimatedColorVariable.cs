using System.Numerics;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

internal class VfxAnimatedColorVariable(MetaClass.VfxAnimatedColorVariableData data)
{
    public List<VfxProbabilityTable> ProbabilityTables { get; set; } =
        data.ProbabilityTables?.Select(x => new VfxProbabilityTable(x)).ToList();
    public List<float> Times { get; set; } = data.Times?.ToList();
    public List<Vector4> Values { get; set; } = data.Values?.ToList();
}
