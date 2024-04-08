using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using LeagueToolkit.Meta;
using LeagueToolkit.Meta.Classes;

namespace lol_convert;

public sealed class StaticMaterialPackage(StaticMaterialDef staticMaterial)
{
    public string Name { get; set; } = staticMaterial.Name;
    public uint Type { get; set; } = staticMaterial.Type;
    public List<StaticMaterialChildTechnique>? ChildTechniques { get; set; } =
        staticMaterial
            .ChildTechniques?.Select(childTechnique => new StaticMaterialChildTechnique(
                childTechnique
            ))
            ?.ToList();
    public List<StaticMaterialShaderParameterValue>? ParameterValues { get; set; } =
        staticMaterial
            .ParamValues?.Select(param => new StaticMaterialShaderParameterValue(param))
            ?.ToList();
    public List<StaticMaterialShaderSamplerValue>? SamplerValues { get; set; } =
        staticMaterial
            .SamplerValues?.Select(sampler => new StaticMaterialShaderSamplerValue(sampler))
            ?.ToList();
    public Dictionary<string, string>? ShaderMacros { get; set; } = staticMaterial.ShaderMacros;
    public List<StaticMaterialSwitch>? Switches { get; set; } =
        staticMaterial.Switches?.Select(switchDef => new StaticMaterialSwitch(switchDef))?.ToList();
    public List<StaticMaterialTechnique>? Techniques { get; set; } =
        staticMaterial
            .Techniques?.Select(technique => new StaticMaterialTechnique(technique))
            ?.ToList();
}

public sealed class StaticMaterialChildTechnique(StaticMaterialChildTechniqueDef childTechnique)
{
    public string? Name { get; set; } = childTechnique.Name;
    public string? ParentName { get; set; } = childTechnique.ParentName;
    public Dictionary<string, string>? ShaderMacros { get; set; } = childTechnique.ShaderMacros;
}

public sealed class StaticMaterialShaderParameterValue(StaticMaterialShaderParamDef param)
{
    public string? Name { get; set; } = param.Name;
    public Vector4 Value { get; set; } = param.Value;
}

public sealed class StaticMaterialShaderSamplerValue(StaticMaterialShaderSamplerDef sampler)
{
    public uint AddressU { get; set; } = sampler.AddressU;
    public uint AddressV { get; set; } = sampler.AddressV;
    public uint AddressW { get; set; } = sampler.AddressW;
    public uint FilterMag { get; set; } = sampler.FilterMag;
    public uint FilterMin { get; set; } = sampler.FilterMin;
    public uint FilterMip { get; set; } = sampler.FilterMip;
    public string? SamplerName { get; set; } = sampler.SamplerName;
    public string? TextureName { get; set; } = sampler.TextureName;
    public Dictionary<uint, string>? UncensoredTextures { get; set; } =
        sampler.UncensoredTextures is not null
            ? new(sampler.UncensoredTextures.Select(x => KeyValuePair.Create(x.Key.Hash, x.Value)))
            : null;
}

public sealed class StaticMaterialSwitch(StaticMaterialSwitchDef switchDef)
{
    public string Name { get; set; } = switchDef.Name;
    public bool On { get; set; } = switchDef.On;
}

public sealed class StaticMaterialTechnique(StaticMaterialTechniqueDef technique)
{
    public string Name { get; set; } = technique.Name;
    public List<StaticMaterialPass>? Passes { get; set; } =
        technique.Passes?.Select(pass => new StaticMaterialPass(pass)).ToList();
}

public sealed class StaticMaterialPass(StaticMaterialPassDef pass)
{
    public uint m376198341 { get; set; } = pass.m376198341;
    public uint SrcColorBlendFactor { get; set; } = pass.SrcColorBlendFactor;
    public bool BlendEnable { get; set; } = pass.BlendEnable;
    public uint Shader { get; set; } = pass.Shader;
    public float DepthOffsetSlope { get; set; } = pass.DepthOffsetSlope;
    public bool StencilEnable { get; set; } = pass.StencilEnable;
    public bool CullEnable { get; set; } = pass.CullEnable;
    public uint WindingToCull { get; set; } = pass.WindingToCull;
    public uint StencilPassDepthPassOp { get; set; } = pass.StencilPassDepthPassOp;
    public uint DstAlphaBlendFactor { get; set; } = pass.DstAlphaBlendFactor;
    public uint BlendEquation { get; set; } = pass.BlendEquation;
    public uint StencilCompareFunc { get; set; } = pass.StencilCompareFunc;
    public uint SrcAlphaBlendFactor { get; set; } = pass.SrcAlphaBlendFactor;
    public uint DepthCompareFunc { get; set; } = pass.DepthCompareFunc;
    public bool m2863927372 { get; set; } = pass.m2863927372;
    public uint StencilPassDepthFailOp { get; set; } = pass.StencilPassDepthFailOp;
    public uint StencilMask { get; set; } = pass.StencilMask;
    public uint WriteMask { get; set; } = pass.WriteMask;
    public uint DstColorBlendFactor { get; set; } = pass.DstColorBlendFactor;
    public byte StencilReferenceVal { get; set; } = pass.StencilReferenceVal;
    public List<StaticMaterialShaderParameterValue>? ParameterValues { get; set; } =
        pass.ParamValues?.Select(param => new StaticMaterialShaderParameterValue(param)).ToList();
    public bool DepthEnable { get; set; }
    public uint StencilFailOp { get; set; }
    public Dictionary<string, string>? ShaderMacros { get; set; }
    public float DepthOffsetBias { get; set; }
}
