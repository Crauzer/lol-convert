﻿using System.Numerics;
using System.Text.Json.Serialization;
using lol_convert.Interfaces;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

internal class VfxFlexVector3(MetaClass.FlexValueVector3 flexVector)
{
    public uint FlexId { get; set; } = flexVector.FlexID;
    public VfxVector3 Value { get; set; } = flexVector.Value is null ? null : new(flexVector.Value);
}

internal class VfxVector3(MetaClass.ValueVector3 vector)
{
    public Vector3 ConstantValue { get; set; } = vector.ConstantValue;
    public VfxVector3DynamicsBase Dynamics { get; set; } =
        vector.Dynamics switch
        {
            MetaClass.VfxAnimatedVector3f dynamics => new VfxVector3Dynamics(dynamics),
            MetaClass.VfxAnimatedVector3fVariableData dynamics
                => new VfxVector3VariableDynamics(dynamics),
            null => null,
            _ => throw new InvalidOperationException("Unknown Vector3 dynamics")
        };
}

[JsonDerivedType(typeof(VfxVector3Dynamics), typeDiscriminator: "static")]
[JsonDerivedType(typeof(VfxVector3VariableDynamics), typeDiscriminator: "variable")]
internal class VfxVector3DynamicsBase { }

internal class VfxVector3Dynamics(MetaClass.VfxAnimatedVector3f dynamics) : VfxVector3DynamicsBase
{
    public List<Vector3> Values { get; set; } =
        dynamics.Values is null ? null : [.. dynamics.Values];
    public List<float> Times { get; set; } = dynamics.Times is null ? null : [.. dynamics.Times];
    public List<uint> m2090586069 { get; set; } = dynamics.m2090586069?.ToList();
}

internal class VfxVector3VariableDynamics(MetaClass.VfxAnimatedVector3fVariableData dynamics)
    : VfxVector3DynamicsBase
{
    public List<Vector3> Values { get; set; } =
        dynamics.Values is null ? null : [.. dynamics.Values];
    public List<float> Times { get; set; } = dynamics.Times is null ? null : [.. dynamics.Times];
    public List<VfxProbabilityTable> ProbabilityTables { get; set; } =
        dynamics
            .ProbabilityTables?.Select(probabilityTable => new VfxProbabilityTable(
                probabilityTable
            ))
            .ToList() ?? [];
}
