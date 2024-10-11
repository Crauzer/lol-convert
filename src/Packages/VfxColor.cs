using System.Numerics;
using System.Text.Json.Serialization;
using LeagueToolkit.Meta.Classes;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

internal class VfxColor(MetaClass.ValueColor color)
{
    public Vector4 ConstantValue { get; set; } = color.ConstantValue;
    public VfxColorDynamicsBase Dynamics { get; set; } =
        color.Dynamics switch
        {
            MetaClass.VfxAnimatedColor dynamics => new VfxColorDynamics(dynamics),
            MetaClass.VfxAnimatedColorVariableData dynamics
                => new VfxColorVariableDynamics(dynamics),
            null => null,
            _ => throw new InvalidOperationException("Unknown Color dynamics")
        };
}

[JsonDerivedType(typeof(VfxColorDynamics), typeDiscriminator: "static")]
[JsonDerivedType(typeof(VfxColorVariableDynamics), typeDiscriminator: "variable")]
internal class VfxColorDynamicsBase { }

internal class VfxColorDynamics(MetaClass.VfxAnimatedColor dynamics) : VfxColorDynamicsBase
{
    public List<Vector4> Values { get; set; } =
        dynamics.Values is null ? null : [.. dynamics.Values];
    public List<float> Times { get; set; } = dynamics.Times is null ? null : [.. dynamics.Times];
    public List<uint> m2090586069 { get; set; } = dynamics.m2090586069?.ToList();
    public List<VfxProbabilityTable> ProbabilityTables { get; set; } =
        dynamics
            .ProbabilityTables?.Select(probabilityTable => new VfxProbabilityTable(
                probabilityTable
            ))
            .ToList();
}

internal class VfxColorVariableDynamics(MetaClass.VfxAnimatedColorVariableData dynamics)
    : VfxColorDynamicsBase
{
    public List<Vector4> Values { get; set; } =
        dynamics.Values is null ? null : [.. dynamics.Values];
    public List<float> Times { get; set; } = dynamics.Times is null ? null : [.. dynamics.Times];
    public List<VfxProbabilityTable> ProbabilityTables { get; set; } =
        dynamics
            .ProbabilityTables?.Select(probabilityTable => new VfxProbabilityTable(
                probabilityTable
            ))
            .ToList();
}
