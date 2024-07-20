using System.Numerics;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

internal class VfxTextureMult(MetaClass.VfxTextureMultDefinitionData textureMult)
{
    public VfxVector2 BirthUvOffsetMult { get; set; } =
        textureMult.BirthUvoffsetMult is null ? null : new(textureMult.BirthUvoffsetMult);
    public VfxFloat BirthUvRotateRateMult { get; set; } =
        textureMult.BirthUvRotateRateMult is null ? null : new(textureMult.BirthUvRotateRateMult);
    public VfxVector2 BirthUvScrollRateMult { get; set; } =
        textureMult.BirthUvScrollRateMult is null ? null : new(textureMult.BirthUvScrollRateMult);
    public Vector2 EmitterUvScrollRateMult { get; set; } = textureMult.EmitterUvScrollRateMult;
    public VfxFlexVector2 FlexBirthUvScrollRateMult { get; set; } =
        textureMult.FlexBirthUvScrollRateMult is null
            ? null
            : new(textureMult.FlexBirthUvScrollRateMult);
    public bool IsRandomStartFrameMult { get; set; } = textureMult.IsRandomStartFrameMult;
    public VfxFloat ParticleIntegratedUvRotateMult { get; set; } =
        textureMult.ParticleIntegratedUvRotateMult is null
            ? null
            : new(textureMult.ParticleIntegratedUvRotateMult);
    public VfxVector2 ParticleIntegratedUvScrollMult { get; set; } =
        textureMult.ParticleIntegratedUvScrollMult is null
            ? null
            : new(textureMult.ParticleIntegratedUvScrollMult);
    public byte TexAddressModeMult { get; set; } = textureMult.TexAddressModeMult;
    public Vector2 TexDivMult { get; set; } = textureMult.TexDivMult;
    public string TextureMult { get; set; } = textureMult.TextureMult;
    public bool TextureMultFlipU { get; set; } = textureMult.TextureMultFilpU;
    public bool TextureMultFlipV { get; set; } = textureMult.TextureMultFilpV;
    public VfxFloat UvRotationMult { get; set; } =
        textureMult.UvRotationMult is null ? null : new(textureMult.UvRotationMult);
    public VfxVector2 UvScaleMult { get; set; } =
        textureMult.UvScaleMult is null ? null : new(textureMult.UvScaleMult);
    public bool UvScrollAlphaMult { get; set; } = textureMult.UvScrollAlphaMult;
    public bool UvScrollClampMult { get; set; } = textureMult.UvScrollClampMult;
    public Vector2 UvTransformCenterMult { get; set; } = textureMult.UvTransformCenterMult;
}
