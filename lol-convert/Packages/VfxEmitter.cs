using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using lol_convert.Services;
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
    public bool HasPostRotateOrientation { get; set; } = emitter.HasPostRotateOrientation;
    public bool HasVariableStartTime { get; set; } = emitter.HasVariableStartTime;
    public byte Importance { get; set; } = emitter.Importance;
    public bool IsDirectionOriented { get; set; } = emitter.IsDirectionOriented;
    public bool IsEmitterSpace { get; set; } = emitter.IsEmitterSpace;
    public bool IsFollowingTerrain { get; set; } = emitter.IsFollowingTerrain;
    public bool IsGroundLayer { get; set; } = emitter.IsGroundLayer;
    public bool IsLocalOrientation { get; set; } = emitter.IsLocalOrientation;
    public bool IsRandomStartFrame { get; set; } = emitter.IsRandomStartFrame;
    public bool IsRotationEnabled { get; set; } = emitter.IsRotationEnabled;
    public bool IsSingleParticle { get; set; } = emitter.IsSingleParticle;
    public bool IsTexturePixelated { get; set; } = emitter.IsTexturePixelated;
    public bool IsUniformScale { get; set; } = emitter.IsUniformScale;
    public VfxLegacySimple LegacySimple { get; set; } =
        emitter.LegacySimple is null ? null : new(emitter.LegacySimple);
    public float? Lifetime { get; set; } = emitter.Lifetime;
    public VfxLinger Linger { get; set; } = emitter.Linger is null ? null : new(emitter.Linger);
    public float m3407065073 { get; set; } = emitter.m3407065073;
    public bool m3522070068 { get; set; } = emitter.m3522070068;
    public List<VfxMaterialOverride> MaterialOverrides { get; set; } =
        emitter.MaterialOverrideDefinitions?.Select(x => new VfxMaterialOverride(x)).ToList() ?? [];
    public float MaximumRateByVelocity { get; set; } = emitter.MaximumRateByVelocity;
    public byte MeshRenderFlags { get; set; } = emitter.MeshRenderFlags;
    public byte MiscRenderFlags { get; set; } = emitter.MiscRenderFlags;
    public Vector4 ModulationFactor { get; set; } = emitter.ModulationFactor;
    public ushort NumFrames { get; set; } = emitter.NumFrames;
    public byte OffsetLifeScalingSymmetryMode { get; set; } = emitter.OffsetLifeScalingSymmetryMode;
    public Vector3 OffsetLifetimeScaling { get; set; } = emitter.OffsetLifetimeScaling;
    public VfxPalette Palette { get; set; } =
        emitter.PaletteDefinition is null ? null : new(emitter.PaletteDefinition);
    public string ParticleColorTexture { get; set; } = emitter.ParticleColorTexture;
    public bool ParticleIsLocalOrientation { get; set; } = emitter.ParticleIsLocalOrientation;
    public VfxFloat ParticleLifetime { get; set; } =
        emitter.ParticleLifetime is null ? null : new(emitter.ParticleLifetime);
    public float? ParticleLinger { get; set; } = emitter.ParticleLinger;
    public byte ParticleLingerType { get; set; } = emitter.ParticleLingerType;
    public bool ParticlesShareRandomValue { get; set; } = emitter.ParticlesShareRandomValue;
    public VfxFloat ParticleUvRotateRate { get; set; } =
        emitter.ParticleUvRotateRate is null ? null : new(emitter.ParticleUvRotateRate);
    public VfxVector2 ParticleUvScrollRate { get; set; } =
        emitter.ParticleUvScrollRate is null ? null : new(emitter.ParticleUvScrollRate);
    public short Pass { get; set; }
    public float? Period { get; set; }
    public Vector3 PostRotateOrientationAxis { get; set; } = emitter.PostRotateOrientationAxis;
    public VfxPrimitiveBase Primitive { get; set; } =
        emitter.Primitive is null ? null : VfxPrimitiveBase.FromMeta(emitter.Primitive);
    public VfxFloat Rate { get; set; } = emitter.Rate is null ? null : new(emitter.Rate);
    public VfxVector2 RateByVelocityFunction { get; set; } =
        emitter.RateByVelocityFunction is null ? null : new(emitter.RateByVelocityFunction);
    public VfxReflection Reflection { get; set; } =
        emitter.ReflectionDefinition is null ? null : new(emitter.ReflectionDefinition);
    public byte RenderPhaseOverride { get; set; } = emitter.RenderPhaseOverride;
    public VfxVector3 Rotation0 { get; set; } = emitter.Rotation0 is null ? null : new(emitter.Rotation0);
    public Vector3 RotationOverride { get; set; } = emitter.RotationOverride;
    public VfxVector3 Scale0 { get; set; } = emitter.Scale0 is null ? null : new(emitter.Scale0);
    public Vector3 ScaleOverride { get; set; } = emitter.ScaleOverride;
    public float SliceTechniqueRange { get; set; } = emitter.SliceTechniqueRange;
    public VfxSoftParticle SoftParticleParams { get; set; } =
        emitter.SoftParticleParams is null ? null : new(emitter.SoftParticleParams);
    public bool SortEmittersByPos { get; set; } = emitter.SortEmittersByPos;
    public VfxShapeBase SpawnShape { get; set; } =
        emitter.SpawnShape is null ? null : new(emitter.SpawnShape);
    public ushort StartFrame { get; set; } = emitter.StartFrame;
    public byte StencilMode { get; set; } = emitter.StencilMode;
    public byte StencilRef { get; set; } = emitter.StencilRef;
    public uint StencilReferenceId { get; set; } = emitter.StencilReferenceId;
    public byte TexAddressModeBase { get; set; } = emitter.TexAddressModeBase;
    public Vector2 TexDiv { get; set; } = emitter.TexDiv;
    public string Texture { get; set; } = emitter.Texture;
    public bool TextureFlipU { get; set; } = emitter.TextureFlipU;
    public bool TextureFlipV { get; set; } = emitter.TextureFlipV;
    public VfxTextureMult TextureMult { get; set; } =
        emitter.TextureMult is null ? null : new(emitter.TextureMult);
    public float? TimeActiveDuringPeriod { get; set; } = emitter.TimeActiveDuringPeriod;
    public float TimeBeforeFirstEmission { get; set; } = emitter.TimeBeforeFirstEmission;
    public Vector3 TranslationOverride { get; set; } = emitter.TranslationOverride;
    public bool UseEmissionMeshNormalForBirth { get; set; } = emitter.UseEmissionMeshNormalForBirth;
    public bool UseNavmeshMask { get; set; } = emitter.UseNavmeshMask;
    public byte UvMode { get; set; } = emitter.UvMode;
    public float UvParallaxScale { get; set; } = emitter.UvParallaxScale;
    public VfxFloat UvRotation { get; set; } = emitter.UvRotation is null ? null : new(emitter.UvRotation);
    public VfxVector2 UvScale { get; set; } = emitter.UvScale is null ? null : new(emitter.UvScale);
    public bool UvScrollClamp { get; set; } = emitter.UvScrollClamp;
    public Vector2 UvTransformCenter { get; set; } = emitter.UvTransformCenter;
    public VfxVector3 Velocity { get; set; } = emitter.Velocity is null ? null : new(emitter.Velocity);
    public VfxVector3 WorldAcceleration { get; set; } =
        emitter.WorldAcceleration is null ? null : new(emitter.WorldAcceleration);
    public bool WriteAlphaOnly { get; set; } = emitter.WriteAlphaOnly;
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

internal class VfxLegacySimple(MetaClass.VfxEmitterLegacySimple legacySimple) { }

internal class VfxLinger(MetaClass.VfxLingerDefinitionData linger) { }

internal class VfxMaterialOverride(MetaClass.VfxMaterialOverrideDefinitionData materialOverride) { }

internal class VfxTextureMult(MetaClass.VfxTextureMultDefinitionData textureMult) { }

internal class VfxReflection(MetaClass.VfxReflectionDefinitionData reflection) 
{
    public float Fresnel { get; set; } = reflection.Fresnel;
    public Vector4 FresnelColor { get; set; } = reflection.FresnelColor;
    public float ReflectionFresnel { get; set; } = reflection.ReflectionFresnel;
    public Vector4 ReflectionFresnelColor { get; set; } = reflection.ReflectionFresnelColor;
    public string ReflectionMapTexture { get; set; } = reflection.ReflectionMapTexture;
    public float ReflectionOpacityDirect { get; set; } = reflection.ReflectionOpacityDirect;
    public float ReflectionOpacityGlancing { get; set; } = reflection.ReflectionOpacityGlancing;
}

internal class VfxSoftParticle(MetaClass.VfxSoftParticleDefinitionData softParticle) 
{
    public float BeginIn { get; set; } = softParticle.BeginIn;
    public float BeginOut { get; set; } = softParticle.BeginOut;
    public float DeltaIn { get; set; } = softParticle.DeltaIn;
    public float DeltaOut { get; set; } = softParticle.DeltaOut;
}

internal class VfxPalette(MetaClass.VfxPaletteDefinitionData palette) { }

[JsonDerivedType(typeof(VfxPrimitiveArbitraryQuad), "arbitrary_quad")]
[JsonDerivedType(typeof(VfxPrimitiveBeam), "beam")]
[JsonDerivedType(typeof(VfxPrimitiveCameraSegmentBeam), "camera_segment_beam")]
[JsonDerivedType(typeof(VfxPrimitiveCameraQuad), "camera_quad")]
[JsonDerivedType(typeof(VfxPrimitiveCameraUnitQuad), "camera_unit_quad")]
[JsonDerivedType(typeof(VfxPrimitiveAttachedMesh), "attached_mesh")]
[JsonDerivedType(typeof(VfxPrimitiveMesh), "mesh")]
[JsonDerivedType(typeof(VfxPrimitivePlanarProjection), "planar_projection")]
[JsonDerivedType(typeof(VfxPrimitiveRay), "ray")]
[JsonDerivedType(typeof(VfxPrimitiveArbitraryTrail), "arbitrary_trail")]
[JsonDerivedType(typeof(VfxPrimitiveCameraTrail), "camera_trail")]
internal class VfxPrimitiveBase
{
    public VfxPrimitiveBase(MetaClass.VfxPrimitiveBase primitive) { }

    public static VfxPrimitiveBase FromMeta(MetaClass.VfxPrimitiveBase primitive) =>
        primitive switch
        {
            MetaClass.VfxPrimitiveArbitraryQuad x => new VfxPrimitiveArbitraryQuad(x),
            MetaClass.VfxPrimitiveBeam x => new VfxPrimitiveBeam(x),
            MetaClass.VfxPrimitiveCameraSegmentBeam x => new VfxPrimitiveCameraSegmentBeam(x),
            MetaClass.VfxPrimitiveCameraQuad x => new VfxPrimitiveCameraQuad(x),
            MetaClass.VfxPrimitiveCameraUnitQuad x => new VfxPrimitiveCameraUnitQuad(x),
            MetaClass.VfxPrimitiveAttachedMesh x => new VfxPrimitiveAttachedMesh(x),
            MetaClass.VfxPrimitiveMesh x => new VfxPrimitiveMesh(x),
            MetaClass.VfxPrimitivePlanarProjection x => new VfxPrimitivePlanarProjection(x),
            MetaClass.VfxPrimitiveRay x => new VfxPrimitiveRay(x),
            MetaClass.VfxPrimitiveArbitraryTrail x => new VfxPrimitiveArbitraryTrail(x),
            MetaClass.VfxPrimitiveCameraTrail x => new VfxPrimitiveCameraTrail(x),
            null => null,
            _ => throw new NotImplementedException("Unknown primitive")
        };
}

internal class VfxPrimitiveArbitraryQuad(MetaClass.VfxPrimitiveArbitraryQuad primitive)
    : VfxPrimitiveBase(primitive) { }

internal class VfxPrimitiveBeam(MetaClass.VfxPrimitiveBeam primitive) : VfxPrimitiveBase(primitive)
{
    public VfxBeam Beam { get; set; } = primitive.Beam is null ? null : new(primitive.Beam);
    public VfxMesh Mesh { get; set; } = primitive.Mesh is null ? null : new(primitive.Mesh);
}

internal class VfxPrimitiveBeamBase(MetaClass.VfxPrimitiveBeamBase beam) : VfxPrimitiveBase(beam)
{
    public VfxBeam Beam { get; set; } = beam.Beam is null ? null : new(beam.Beam);
}

internal class VfxPrimitiveCameraSegmentBeam(MetaClass.VfxPrimitiveBeamBase beam)
    : VfxPrimitiveBeamBase(beam) { }

internal class VfxPrimitiveCameraQuad(MetaClass.VfxPrimitiveCameraQuad primitive)
    : VfxPrimitiveBase(primitive) { }

internal class VfxPrimitiveCameraUnitQuad(MetaClass.VfxPrimitiveCameraUnitQuad primitive)
    : VfxPrimitiveBase(primitive) { }

internal class VfxPrimitiveProjectionBase(MetaClass.VfxPrimitiveProjectionBase projection)
    : VfxPrimitiveBase(projection)
{
    public VfxProjection Projection { get; set; } =
        projection.Projection is null ? null : new(projection.Projection);
}

internal class VfxPrimitivePlanarProjection(MetaClass.VfxPrimitivePlanarProjection planarProjection)
    : VfxPrimitiveProjectionBase(planarProjection) { }

internal class VfxProjection(MetaClass.VfxProjectionDefinitionData projection)
{
    public VfxColor ColorModulate { get; set; } =
        projection.ColorModulate is null ? null : new(projection.ColorModulate);
    public float Fading { get; set; } = projection.Fading;
    public float YRange { get; set; } = projection.YRange;
}

internal class VfxPrimitiveRay(MetaClass.VfxPrimitiveRay ray) : VfxPrimitiveBase(ray) { }

internal class VfxPrimitiveTrailBase(MetaClass.VfxPrimitiveTrailBase trail)
    : VfxPrimitiveBase(trail)
{
    public VfxTrail Trail { get; set; } = trail.Trail is null ? null : new(trail.Trail);
}

internal class VfxPrimitiveArbitraryTrail(MetaClass.VfxPrimitiveArbitraryTrail trail)
    : VfxPrimitiveTrailBase(trail) { }

internal class VfxPrimitiveCameraTrail(MetaClass.VfxPrimitiveCameraTrail trail)
    : VfxPrimitiveTrailBase(trail) { }

internal class VfxTrail(MetaClass.VfxTrailDefinitionData trail)
{
    public VfxVector3 BirthTilingSize { get; set; } =
        trail.BirthTilingSize is null ? null : new(trail.BirthTilingSize);
    public float Cutoff { get; set; } = trail.Cutoff;
    public int MaxAddedPerFrame { get; set; } = trail.MaxAddedPerFrame;
    public byte Mode { get; set; } = trail.Mode;
    public byte SmoothingMode { get; set; } = trail.SmoothingMode;
}

internal class VfxBeam(MetaClass.VfxBeamDefinitionData beam)
{
    public VfxColor AnimatedColorWithDistance { get; set; } =
        beam.AnimatedColorWithDistance is null ? null : new(beam.AnimatedColorWithDistance);
    public VfxVector3 BirthTilingSize { get; set; } =
        beam.BirthTilingSize is null ? null : new(beam.BirthTilingSize);
    public bool IsColorBindedWithDistance { get; set; } = beam.IsColorBindedWithDistance;
    public Vector3 LocalSpaceSourceOffset { get; set; } = beam.LocalSpaceSourceOffset;
    public Vector3 LocalSpaceTargetOffset { get; set; } = beam.LocalSpaceTargetOffset;
    public byte Mode { get; set; } = beam.Mode;
    public int Segments { get; set; } = beam.Segments;
    public byte TrailMode { get; set; } = beam.TrailMode;
}

internal class VfxMesh(MetaClass.VfxMeshDefinitionData mesh)
{
    public string AnimationName { get; set; } = mesh.AnimationName;
    public List<string> AnimationVariants { get; set; } = mesh.AnimationVariants?.ToList();
    public bool LockMeshToAttachment { get; set; } = mesh.LockMeshToAttachment;
    public string MeshName { get; set; } = mesh.MeshName;
    public string MeshSkeletonName { get; set; } = mesh.MeshSkeletonName;
    public string SimpleMeshName { get; set; } = mesh.SimpleMeshName;
    public List<string> SubmeshesToDraw { get; set; } =
        mesh.SubmeshesToDraw?.Select(x => BinHashtableService.ResolveHash(x)).ToList();
    public List<string> SubmeshesToDrawAlways { get; set; } =
        mesh.SubmeshesToDrawAlways?.Select(x => BinHashtableService.ResolveHash(x)).ToList();
}
