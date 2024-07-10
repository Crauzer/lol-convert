using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

internal class VfxEmitter(MetaClass.VfxEmitterDefinitionData emitter)
{
    public VfxVector3 Acceleration { get; set; } = new(emitter.Acceleration);
    public VfxAlphaErosion AlphaErosion { get; set; } = new(emitter.AlphaErosionDefinition);
    public byte AlphaRef { get; set; } = emitter.AlphaRef;
    public VfxEmitterAudio Audio { get; set; } = new(emitter.Audio);
    public VfxFloat BindWeight { get; set; } = new(emitter.BindWeight);
    public VfxVector3 BirthAcceleration { get; set; } = new(emitter.BirthAcceleration);
    public VfxColor BirthColor { get; set; } = new(emitter.BirthColor);
    public VfxVector3 BirthDrag { get; set; } = new(emitter.BirthDrag);
    public VfxFloat BirthFrameRate { get; set; } = new(emitter.BirthFrameRate);
    public VfxVector3 BirthOrbitalVelocity { get; set; } = new(emitter.BirthOrbitalVelocity);
    public VfxVector3 BirthRotation0 { get; set; } = new(emitter.BirthRotation0);
    public VfxVector3 BirthRotationalAcceleration { get; set; } = new(emitter.BirthRotationalAcceleration);
    public VfxVector3 BirthRotationalVelocity0 { get; set; } = new(emitter.BirthRotationalVelocity0);
    public VfxVector3 BirthScale0 { get; set; } = new(emitter.BirthScale0);
    public VfxVector2 BirthUvOffset { get; set; } = new(emitter.BirthUvoffset);
    public VfxFloat BirthUvRotateRate { get; set; } = new(emitter.BirthUvRotateRate);
    public VfxVector2 BirthUvScrollRate { get; set; } = new(emitter.BirthUvScrollRate);
    public VfxVector3 BirthVelocity { get; set; } = new(emitter.BirthVelocity);
    public byte BlendMode { get; set; } = emitter.BlendMode;
    public Vector4 CensorModulateValue { get; set; } = emitter.CensorModulateValue;
    public float ChanceToNotExist { get; set; } = emitter.ChanceToNotExist;
    public VfxChildParticleSet ChildParticleSet { get; set; } = new(emitter.ChildParticleSetDefinition);
    public VfxColor Color { get; set; } = new(emitter.Color);
    public byte ColorblindVisibility { get; set; } = emitter.ColorblindVisibility;
    public Vector2 ColorLookUpOffsets { get; set; } = emitter.ColorLookUpOffsets;
    public Vector2 ColorLookUpScales { get; set; } = emitter.ColorLookUpScales;
    public byte ColorLookUpTypeX { get; set; } = emitter.ColorLookUpTypeX;
    public byte ColorLookUpTypeY { get; set; } = emitter.ColorLookUpTypeY;
    public byte ColorRenderFlags { get; set; } = emitter.ColorRenderFlags;
    public VfxMaterial Material { get; set; } = new(emitter.CustomMaterial);
    public Vector2 DepthBiasFactors { get; set; } = emitter.DepthBiasFactors;
    public float DirectionVelocityMinScale { get; set; } = emitter.DirectionVelocityMinScale;
    public float DirectionVelocityScale { get; set; } = emitter.DirectionVelocityScale;
    public bool DisableBackfaceCulling { get; set; } = emitter.DisableBackfaceCull;
    public bool IsDisabled { get; set; } = emitter.Disabled;
    public VfxDistortion Distortion { get; set; } = new(emitter.DistortionDefinition);
    public byte DoesCastShadow { get; set; } = emitter.DoesCastShadow;
    public byte DoesLifetimeScale { get; set; } = emitter.DoesLifetimeScale;
    public VfxVector3 Drag { get; set; } = new(emitter.Drag);
    public string EmissionMeshName { get; set; } = emitter.EmissionMeshName;
    public float EmissionMeshScale { get; set; } = emitter.EmissionMeshScale;
    public VfxEmissionSurface EmissionSurface { get; set; } = new(emitter.EmissionSurfaceDefinition);
    public float? EmitterLinger { get; set; } = emitter.EmitterLinger;
    public string EmitterName { get; set; } = emitter.EmitterName;
    public VfxVector3 EmitterPosition { get; set; } = new(emitter.EmitterPosition);
    public Vector2 EmitterUvScrollRate { get; set; } = emitter.EmitterUvScrollRate;
    public string FalloffTexture { get; set; } = emitter.FalloffTexture;
    public VfxFieldCollection FieldCollection { get; set; } = new(emitter.FieldCollectionDefinition);
    public VfxEmitterFiltering Filtering { get; set; } = new(emitter.Filtering);
    public VfxFlexVector3 FlexBirthRotationalVelocity0 { get; set; } = new(emitter.FlexBirthRotationalVelocity0);
    public VfxFlexVector2 FlexBirthUvOffset { get; set; } = new(emitter.FlexBirthUvoffset);
    public VfxFlexVector3 FlexBirthVelocity { get; set; } = new(emitter.FlexBirthVelocity);
}

internal class VfxAlphaErosion(MetaClass.VfxAlphaErosionDefinitionData alphaErosion)
{

}

internal class VfxEmitterAudio(MetaClass.VfxEmitterAudio emitterAudio)
{

}

internal class VfxChildParticleSet(MetaClass.VfxChildParticleSetDefinitionData childParticleSet)
{

}

internal class VfxMaterial(MetaClass.VfxMaterialDefinitionData material)
{

}

internal class VfxDistortion(MetaClass.VfxDistortionDefinitionData distortion)
{

}

internal class VfxEmissionSurface(MetaClass.VfxEmissionSurfaceData emissionSurface)
{

}

internal class VfxFieldCollection(MetaClass.VfxFieldCollectionDefinitionData fieldCollection)
{

}

internal class VfxEmitterFiltering(MetaClass.VfxEmitterFiltering emitterFiltering)
{

}