using System.Numerics;
using lol_convert.Services;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

internal class VfxSystem(MetaClass.VfxSystemDefinitionData vfxSystem)
{
    public string AssetCategory { get; set; } = vfxSystem.AssetCategory;
    public List<VfxAssetRemap> AssetRemappingTable { get; set; } =
        vfxSystem.AssetRemappingTable?.Select(x => new VfxAssetRemap(x)).ToList() ?? [];
    public int AudioParameterFlexId { get; set; } = vfxSystem.AudioParameterFlexId;
    public float AudioParameterTimeScaledDuration { get; set; } =
        vfxSystem.AudioParameterTimeScaledDuration;
    public float BuildUpTime { get; set; } = vfxSystem.BuildUpTime;
    public byte ClockToUse { get; set; } = vfxSystem.ClockToUse;
    public List<VfxEmitter> ComplexEmitters { get; set; } =
        vfxSystem.ComplexEmitterDefinitionData?.Select(x => new VfxEmitter(x)).ToList() ?? [];
    public byte DrawingLayer { get; set; } = vfxSystem.DrawingLayer;
    public bool EyeCandy { get; set; } = vfxSystem.EyeCandy;
    public ushort Flags { get; set; } = vfxSystem.Flags;
    public bool HudAnchorPositionFromWorldProjection { get; set; } =
        vfxSystem.HudAnchorPositionFromWorldProjection;
    public float HudLayerAspect { get; set; } = vfxSystem.HudLayerAspect;
    public float HudLayerDimension { get; set; } = vfxSystem.HudLayerDimension;
    public bool IsPoseAfterimage { get; set; } = vfxSystem.IsPoseAfterimage;
    public List<VfxMaterialOverride> MaterialOverrides { get; set; } =
        vfxSystem.MaterialOverrideDefinitions?.Select(x => new VfxMaterialOverride(x)).ToList()
        ?? [];
    public float OverrideScaleCap { get; set; } = vfxSystem.OverrideScaleCap;
    public string ParticleName { get; set; } = vfxSystem.ParticleName;
    public string ParticlePath { get; set; } = vfxSystem.ParticlePath;
    public bool ScaleDynamicallyWithAttachedBone { get; set; } =
        vfxSystem.ScaleDynamicallyWithAttachedBone;
    public float SelfIllumination { get; set; } = vfxSystem.SelfIllumination;
    public List<VfxEmitter> SimpleEmitters { get; set; } =
        vfxSystem.SimpleEmitterDefinitionData?.Select(x => new VfxEmitter(x)).ToList() ?? [];
    public string SoundOnCreateDefault { get; set; } = vfxSystem.SoundOnCreateDefault;
    public string SoundPersistentDefault { get; set; } = vfxSystem.SoundPersistentDefault;
    public Matrix4x4 Transform { get; set; } = vfxSystem.Transform;
    public float VisibilityRadius { get; set; } = vfxSystem.VisibilityRadius;
    public string VoiceOverOnCreateDefault { get; set; } = vfxSystem.VoiceOverOnCreateDefault;
    public string VoiceOverPersistentDefault { get; set; } = vfxSystem.VoiceOverPersistentDefault;
}

internal class VfxAssetRemap(MetaClass.VfxAssetRemap assetRemap)
{
    public string NewAsset { get; set; } = assetRemap.NewAsset;
    public string OldAsset { get; set; } = BinHashtableService.ResolveHash(assetRemap.OldAsset);
    public uint Type { get; set; } = assetRemap.Type;
}

internal class VfxMaterialOverride(MetaClass.VfxMaterialOverrideDefinitionData materialOverride)
{
    public string BaseTexture { get; set; } = materialOverride.BaseTexture;
    public string GlossTexture { get; set; } = materialOverride.GlossTexture;
    public string Material { get; set; } =
        BinHashtableService.TryResolveObjectLink(materialOverride.Material);
    public uint OverrideBlendMode { get; set; } = materialOverride.OverrideBlendMode;
    public int Priority { get; set; } = materialOverride.Priority;
    public string SubmeshName { get; set; } = materialOverride.SubMeshName.Value;
    public float TransitionSample { get; set; } = materialOverride.TransitionSample;
    public uint TransitionSource { get; set; } = materialOverride.TransitionSource;
    public string TransitionTexture { get; set; } = materialOverride.TransitionTexture;
}

internal class VfxProbabilityTable(MetaClass.VfxProbabilityTableData table)
{
    public List<float> KeyTimes = [.. table.KeyTimes ?? []];
    public List<float> KeyValues = [.. table.KeyValues ?? []];
    public float SingleValue = table.SingleValue;
}
