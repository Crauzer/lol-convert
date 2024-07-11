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
    public VfxVector3 Acceleration { get; set; } =
        emitter.Acceleration is not null ? new(emitter.Acceleration) : null;
    public VfxAlphaErosion AlphaErosion { get; set; } =
        emitter.AlphaErosionDefinition is not null ? new(emitter.AlphaErosionDefinition) : null;
    public byte AlphaRef { get; set; } = emitter.AlphaRef;
    public VfxEmitterAudio Audio { get; set; } =
        emitter.Audio is not null ? new(emitter.Audio) : null;
    public VfxFloat BindWeight { get; set; } =
        emitter.BindWeight is null ? null : new(emitter.BindWeight);
    public VfxVector3 BirthAcceleration { get; set; } =
        emitter.BirthAcceleration is null ? null : new(emitter.BirthAcceleration);
    public VfxColor BirthColor { get; set; } =
        emitter.BirthColor is null ? null : new(emitter.BirthColor);
    public VfxVector3 BirthDrag { get; set; } =
        emitter.BirthDrag is null ? null : new(emitter.BirthDrag);
    public VfxFloat BirthFrameRate { get; set; } =
        emitter.BirthFrameRate is null ? null : new(emitter.BirthFrameRate);
    public VfxVector3 BirthOrbitalVelocity { get; set; } =
        emitter.BirthOrbitalVelocity is null ? null : new(emitter.BirthOrbitalVelocity);
    public VfxVector3 BirthRotation0 { get; set; } =
        emitter.BirthRotation0 is null ? null : new(emitter.BirthRotation0);
    public VfxVector3 BirthRotationalAcceleration { get; set; } =
        emitter.BirthRotationalAcceleration is null
            ? null
            : new(emitter.BirthRotationalAcceleration);
    public VfxVector3 BirthRotationalVelocity0 { get; set; } =
        emitter.BirthRotationalVelocity0 is null ? null : new(emitter.BirthRotationalVelocity0);
    public VfxVector3 BirthScale0 { get; set; } =
        emitter.BirthScale0 is null ? null : new(emitter.BirthScale0);
    public VfxVector2 BirthUvOffset { get; set; } =
        emitter.BirthUvoffset is null ? null : new(emitter.BirthUvoffset);
    public VfxFloat BirthUvRotateRate { get; set; } =
        emitter.BirthUvRotateRate is null ? null : new(emitter.BirthUvRotateRate);
    public VfxVector2 BirthUvScrollRate { get; set; } =
        emitter.BirthUvScrollRate is null ? null : new(emitter.BirthUvScrollRate);
    public VfxVector3 BirthVelocity { get; set; } =
        emitter.BirthVelocity is null ? null : new(emitter.BirthVelocity);
    public byte BlendMode { get; set; } = emitter.BlendMode;
    public Vector4 CensorModulateValue { get; set; } = emitter.CensorModulateValue;
    public float ChanceToNotExist { get; set; } = emitter.ChanceToNotExist;
    public VfxChildParticleSet ChildParticleSet { get; set; } =
        emitter.ChildParticleSetDefinition is null ? null : new(emitter.ChildParticleSetDefinition);
    public VfxColor Color { get; set; } = emitter.Color is null ? null : new(emitter.Color);
    public byte ColorblindVisibility { get; set; } = emitter.ColorblindVisibility;
    public Vector2 ColorLookUpOffsets { get; set; } = emitter.ColorLookUpOffsets;
    public Vector2 ColorLookUpScales { get; set; } = emitter.ColorLookUpScales;
    public byte ColorLookUpTypeX { get; set; } = emitter.ColorLookUpTypeX;
    public byte ColorLookUpTypeY { get; set; } = emitter.ColorLookUpTypeY;
    public byte ColorRenderFlags { get; set; } = emitter.ColorRenderFlags;
    public VfxMaterial Material { get; set; } =
        emitter.CustomMaterial is null ? null : new(emitter.CustomMaterial);
    public Vector2 DepthBiasFactors { get; set; } = emitter.DepthBiasFactors;
    public float DirectionVelocityMinScale { get; set; } = emitter.DirectionVelocityMinScale;
    public float DirectionVelocityScale { get; set; } = emitter.DirectionVelocityScale;
    public bool DisableBackfaceCulling { get; set; } = emitter.DisableBackfaceCull;
    public bool IsDisabled { get; set; } = emitter.Disabled;
    public VfxDistortion Distortion { get; set; } =
        emitter.DistortionDefinition is null ? null : new(emitter.DistortionDefinition);
    public byte DoesCastShadow { get; set; } = emitter.DoesCastShadow;
    public byte DoesLifetimeScale { get; set; } = emitter.DoesLifetimeScale;
    public VfxVector3 Drag { get; set; } = emitter.Drag is null ? null : new(emitter.Drag);
    public string EmissionMeshName { get; set; } = emitter.EmissionMeshName;
    public float EmissionMeshScale { get; set; } = emitter.EmissionMeshScale;
    public VfxEmissionSurface EmissionSurface { get; set; } =
        emitter.EmissionSurfaceDefinition is null ? null : new(emitter.EmissionSurfaceDefinition);
    public float? EmitterLinger { get; set; } = emitter.EmitterLinger;
    public string EmitterName { get; set; } = emitter.EmitterName;
    public VfxVector3 EmitterPosition { get; set; } =
        emitter.EmitterPosition is null ? null : new(emitter.EmitterPosition);
    public Vector2 EmitterUvScrollRate { get; set; } = emitter.EmitterUvScrollRate;
    public string FalloffTexture { get; set; } = emitter.FalloffTexture;
    public VfxFieldCollection FieldCollection { get; set; } =
        emitter.FieldCollectionDefinition is null ? null : new(emitter.FieldCollectionDefinition);
    public VfxEmitterFiltering Filtering { get; set; } =
        emitter.Filtering is null ? null : new(emitter.Filtering);
    public VfxFlexVector3 FlexBirthRotationalVelocity0 { get; set; } =
        emitter.FlexBirthRotationalVelocity0 is null
            ? null
            : new(emitter.FlexBirthRotationalVelocity0);
    public VfxFlexVector2 FlexBirthUvOffset { get; set; } =
        emitter.FlexBirthUvoffset is null ? null : new(emitter.FlexBirthUvoffset);
    public VfxFlexVector3 FlexBirthVelocity { get; set; } =
        emitter.FlexBirthVelocity is null ? null : new(emitter.FlexBirthVelocity);
    public VfxFlexTypeFloat FlexInstanceScale { get; set; } =
        emitter.FlexInstanceScale is null ? null : new(emitter.FlexInstanceScale);
    public VfxFlexVector3 FlexOffset { get; set; } =
        emitter.FlexOffset is null ? null : new(emitter.FlexOffset);
    public VfxFlexFloat FlexParticleLifetime { get; set; } =
        emitter.FlexParticleLifetime is null ? null : new(emitter.FlexParticleLifetime);
    public VfxFlexFloat FlexRate { get; set; } =
        emitter.FlexRate is null ? null : new(emitter.FlexRate);
    public VfxFlexTypeFloat FlexScaleBirthScale { get; set; } =
        emitter.FlexScaleBirthScale is null ? null : new(emitter.FlexScaleBirthScale);
    public VfxFlexShapeDefinition FlexShapeDefinition { get; set; } =
        emitter.FlexShapeDefinition is null ? null : new(emitter.FlexShapeDefinition);
    public float FrameRate { get; set; } = emitter.FrameRate;
}

internal class VfxAlphaErosion(MetaClass.VfxAlphaErosionDefinitionData alphaErosion) { }

internal class VfxEmitterAudio(MetaClass.VfxEmitterAudio emitterAudio) { }

internal class VfxChildParticleSet(MetaClass.VfxChildParticleSetDefinitionData childParticleSet) { }

internal class VfxMaterial(MetaClass.VfxMaterialDefinitionData material) { }

internal class VfxDistortion(MetaClass.VfxDistortionDefinitionData distortion) { }

internal class VfxEmissionSurface(MetaClass.VfxEmissionSurfaceData emissionSurface) { }

internal class VfxFieldCollection(MetaClass.VfxFieldCollectionDefinitionData fieldCollection) { }

internal class VfxEmitterFiltering(MetaClass.VfxEmitterFiltering emitterFiltering) { }

internal class VfxFlexShapeDefinition(MetaClass.VfxFlexShapeDefinitionData flexShape) { }
