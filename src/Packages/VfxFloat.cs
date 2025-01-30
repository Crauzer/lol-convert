using System.Text.Json.Serialization;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

internal class VfxFlexFloat(MetaClass.FlexValueFloat flexValueFloat)
{
    public uint FlexId { get; set; } = flexValueFloat.FlexID;
    public VfxFloat Value { get; set; } =
        flexValueFloat.Value is null ? null : new(flexValueFloat.Value);
}

internal class VfxFlexTypeFloat(MetaClass.FlexTypeFloat flexTypeFloat)
{
    public uint FlexId { get; set; } = flexTypeFloat.FlexID;
    public float Value { get; set; } = flexTypeFloat.Value;
}

internal class VfxFloat(MetaClass.ValueFloat valueFloat)
{
    public float ConstantValue { get; set; } = valueFloat.ConstantValue;
    public VfxFloatDynamicsBase Dynamics { get; set; } =
        valueFloat.Dynamics switch
        {
            MetaClass.VfxAnimatedFloat dynamics => new VfxFloatDynamics(dynamics),
            MetaClass.VfxAnimatedFloatVariableData dynamics
                => new VfxFloatVariableDynamics(dynamics),
            null => null,
            _ => throw new InvalidOperationException("Unknown Float dynamics")
        };
}

[JsonDerivedType(typeof(VfxFloatDynamics), typeDiscriminator: "static")]
[JsonDerivedType(typeof(VfxFloatVariableDynamics), typeDiscriminator: "variable")]
internal class VfxFloatDynamicsBase { }

internal class VfxFloatDynamics(MetaClass.VfxAnimatedFloat dynamics) : VfxFloatDynamicsBase
{
    public List<float> Values { get; set; } = dynamics.Values?.ToList();
    public List<float> Times { get; set; } = dynamics.Times?.ToList();
    public List<uint> m2090586069 { get; set; } = dynamics.m2090586069?.ToList();
    public List<VfxProbabilityTable> ProbabilityTables { get; set; } =
        dynamics
            .ProbabilityTables?.Select(probabilityTable => new VfxProbabilityTable(
                probabilityTable
            ))
            .ToList();
}

internal class VfxFloatVariableDynamics(MetaClass.VfxAnimatedFloatVariableData dynamics)
    : VfxFloatDynamicsBase
{
    public List<float> Values { get; set; } = dynamics.Values is null ? null : [.. dynamics.Values];
    public List<float> Times { get; set; } = dynamics.Times is null ? null : [.. dynamics.Times];
    public List<VfxProbabilityTable> ProbabilityTables { get; set; } =
        dynamics
            .ProbabilityTables?.Select(probabilityTable => new VfxProbabilityTable(
                probabilityTable
            ))
            .ToList();
}
