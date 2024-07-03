using System.Numerics;
using System.Text.Json.Serialization;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

internal class VfxVector2(MetaClass.ValueVector2 vector)
{
    public Vector2 ConstantValue { get; set; } = vector.ConstantValue;
    public VfxVector2DynamicsBase Dynamics { get; set; } =
        vector.Dynamics switch
        {
            MetaClass.VfxAnimatedVector2f dynamics => new VfxVector2Dynamics(dynamics),
            MetaClass.VfxAnimatedVector2fVariableData dynamics => new VfxVector2VariableDynamics(dynamics),
            _ => throw new InvalidOperationException("Unknown Vector2 dynamics")
        };
}

[JsonDerivedType(typeof(VfxVector2Dynamics), typeDiscriminator: "static")]
[JsonDerivedType(typeof(VfxVector2VariableDynamics), typeDiscriminator: "variable")]
internal class VfxVector2DynamicsBase { }

internal class VfxVector2Dynamics(MetaClass.VfxAnimatedVector2f dynamics) : VfxVector2DynamicsBase
{
    public List<byte> Modes { get; set; } = [.. dynamics.Modes];
    public List<Vector2> Values { get; set; } = [.. dynamics.Values];
    public List<float> Times { get; set; } = [.. dynamics.Times];
}

internal class VfxVector2VariableDynamics(MetaClass.VfxAnimatedVector2fVariableData dynamics) : VfxVector2DynamicsBase
{
    public List<Vector2> Values { get; set; } = [.. dynamics.Values];
    public List<float> Times { get; set; } = [.. dynamics.Times];
    public List<VfxProbabilityTable> ProbabilityTables { get; set; } =
        dynamics
            .ProbabilityTables?.Select(probabilityTable => new VfxProbabilityTable(
                probabilityTable
            ))
            .ToList() ?? [];
}
