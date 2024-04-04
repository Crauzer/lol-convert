using LeagueToolkit.Meta;
using LeagueToolkit.Meta.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace lol_convert;

public sealed class StaticMaterialPackage
{
    public required string Name { get; set; }
    public List<StaticMaterialChildTechnique>? ChildTechniques { get; set; }
    public List<StaticMaterialShaderParameterValue>? ParameterValues { get; set; }
    public List<StaticMaterialShaderSamplerValue>? SamplerValues { get; set; }
    public Dictionary<string, string>? ShaderMacros { get; set; }
    public List<StaticMaterialSwitch>? Switches { get; set; }
    public List<StaticMaterialTechnique>? Techniques { get; set; }
    public uint Type { get; set; }
}

public sealed class StaticMaterialChildTechnique
{
    public required string Name { get; set; }
    public string? ParentName { get; set; }
    public Dictionary<string, string>? ShaderMacros { get; set; }
}

public sealed class StaticMaterialShaderParameterValue
{
    public required string Name { get;  set; }
    public Vector4 Value { get;  set; }
}

public sealed class StaticMaterialShaderSamplerValue
{
    public uint AddressU { get; set; }
    public uint AddressV { get; set; }
    public uint AddressW { get; set; }
    public uint FilterMag { get; set; }
    public uint FilterMin { get; set; }
    public uint FilterMip { get; set; }
    public string? SamplerName { get; set; }
    public string? TextureName { get; set; }
    public Dictionary<uint, string>? UncensoredTextures { get; set; }

}

public sealed class StaticMaterialSwitch
{
    public required string Name { get; set; }
    public bool On { get; set; }
}

public sealed class StaticMaterialTechnique
{
    public required string Name { get; set; }
    public List<StaticMaterialPass>? Passes { get; set; }
}

public sealed class StaticMaterialPass
{
    public uint m376198341 { get; set; }
    public uint SrcColorBlendFactor { get; set; }
    public bool BlendEnable { get; set; }
    public uint Shader { get; set; }
    public float DepthOffsetSlope { get; set; }
    public bool StencilEnable { get; set; }
    public bool CullEnable { get; set; }
    public uint WindingToCull { get; set; }
    public uint StencilPassDepthPassOp { get; set; }
    public uint DstAlphaBlendFactor { get; set; }
    public uint BlendEquation { get; set; }
    public uint StencilCompareFunc { get; set; }
    public uint SrcAlphaBlendFactor { get; set; }
    public uint DepthCompareFunc { get; set; }
    public bool m2863927372 { get; set; }
    public uint StencilPassDepthFailOp { get; set; }
    public uint StencilMask { get; set; }
    public uint WriteMask { get; set; }
    public uint DstColorBlendFactor { get; set; }
    public byte StencilReferenceVal { get; set; }
    public List<StaticMaterialShaderParameterValue>? ParameterValues { get; set; }
    public bool DepthEnable { get; set; }
    public uint StencilFailOp { get; set; }
    public Dictionary<string, string>? ShaderMacros { get; set; }
    public float DepthOffsetBias { get; set; }
}