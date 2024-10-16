using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using LeagueToolkit.Core.Primitives;
using lol_convert.Services;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

public sealed class Character
{
    public string Name { get; set; }
    public List<string> SkinNames { get; set; }
}

internal class CharacterSkin
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public SkinMeshDataProperties SkinMeshProperties { get; set; }
    public Dictionary<string, StaticMaterialPackage> Materials { get; set; }
    public Dictionary<string, VfxSystem> VfxSystems { get; set; }
    public Dictionary<string, string> ResourceResolver { get; set; }
}

public class SkinMeshDataProperties(MetaClass.SkinMeshDataProperties skinMeshDataProperties)
{
    public float ReflectionOpacityDirect { get; set; } =
        skinMeshDataProperties.ReflectionOpacityDirect;
    public float ReflectionFresnel { get; set; } = skinMeshDataProperties.ReflectionFresnel;
    public float ReflectionOpacityGlancing { get; set; } =
        skinMeshDataProperties.ReflectionOpacityGlancing;
    public string RoughnessMetallicAoTexture { get; set; } =
        skinMeshDataProperties.RoughnessMetallicAoTexture;
    public List<SkinMeshMaterialOverridePackage> MaterialOverrides { get; set; } =
        skinMeshDataProperties
            .MaterialOverride?.Select(x => new SkinMeshMaterialOverridePackage(x))
            .ToList();
    public float SelfIllumination { get; set; } = skinMeshDataProperties.SelfIllumination;
    public bool ForceDrawLast { get; set; } = skinMeshDataProperties.ForceDrawLast;
    public string SubmeshRenderOrder { get; set; } = skinMeshDataProperties.SubmeshRenderOrder;
    public string EmitterSubmeshAvatarToHide { get; set; } =
        skinMeshDataProperties.EmitterSubmeshAvatarToHide;
    public string InitialSubmeshMouseOversToHide { get; set; } =
        skinMeshDataProperties.InitialSubmeshMouseOversToHide;
    public Color FresnelColor { get; set; } = skinMeshDataProperties.FresnelColor;
    public string Texture { get; set; } = skinMeshDataProperties.Texture;
    public float BoundingCylinderRadius { get; set; } =
        skinMeshDataProperties.BoundingCylinderRadius;
    public Color ReflectionFresnelColor { get; set; } =
        skinMeshDataProperties.ReflectionFresnelColor;
    public Vector3? OverrideBoundingBox { get; set; } = skinMeshDataProperties.OverrideBoundingBox;
    public float BoundingCylinderHeight { get; set; } =
        skinMeshDataProperties.BoundingCylinderHeight;
    public SkinnedMeshMaterialController MaterialController { get; set; } =
        skinMeshDataProperties.MaterialController != null
            ? SkinnedMeshMaterialController.FromMeta(skinMeshDataProperties.MaterialController)
            : null;
    public string[] InitialSubmeshToHide { get; set; } =
        ConvertUtils.SplitSubmeshList(skinMeshDataProperties.InitialSubmeshToHide);
    public float Fresnel { get; set; } = skinMeshDataProperties.Fresnel;
    public float? BoundingSphereRadius { get; set; } = skinMeshDataProperties.BoundingSphereRadius;
    public string EmissiveTexture { get; set; } = skinMeshDataProperties.EmissiveTexture;
    public float SkinScale { get; set; } = skinMeshDataProperties.SkinScale;
    public bool CastShadows { get; set; } = skinMeshDataProperties.CastShadows;
    public bool EnablePicking { get; set; } = skinMeshDataProperties.EnablePicking;
    public string InitialSubmeshAvatarToHide { get; set; } =
        skinMeshDataProperties.InitialSubmeshAvatarToHide;
    public float BrushAlphaOverride { get; set; } = skinMeshDataProperties.BrushAlphaOverride;
    public string Material { get; set; } =
        BinHashtableService.TryResolveObjectLink(skinMeshDataProperties.Material);
    public bool ReducedBoneSkinning { get; set; } = skinMeshDataProperties.ReducedBoneSkinning;
    public string ReflectionMap { get; set; } = skinMeshDataProperties.ReflectionMap;
    public string GlossTexture { get; set; } = skinMeshDataProperties.GlossTexture;
    public bool UsesSkinVo { get; set; } = skinMeshDataProperties.UsesSkinVo;
    public string NormalMapTexture { get; set; } = skinMeshDataProperties.NormalMapTexture;
    public string RigPoseModifierData { get; set; } = "TODO";
    public string InitialSubmeshShadowsToHide { get; set; } =
        skinMeshDataProperties.InitialSubmeshShadowsToHide;
    public string SkinMeshPath { get; set; }
}

public class SkinMeshMaterialOverridePackage(
    MetaClass.SkinMeshDataProperties_MaterialOverride materialOverride
)
{
    public string Material { get; set; } =
        BinHashtableService.TryResolveObjectLink(materialOverride.Material);
    public string Submesh { get; set; } = materialOverride.Submesh;
    public string Texture { get; set; } = materialOverride.Texture;
}
