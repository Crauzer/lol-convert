using lol_convert.Services;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

internal class VfxMaterialOverride(MetaClass.VfxMaterialOverrideDefinitionData materialOverride)
{
    public string BaseTexture { get; set; } = materialOverride.BaseTexture;
    public string GlossTexture { get; set; } = materialOverride.GlossTexture;
    public string Material { get; set; } =
        BinHashtableService.TryResolveObjectLink(materialOverride.Material);
    public uint OverrideBlendMode { get; set; } = materialOverride.OverrideBlendMode;
    public int Priority { get; set; } = materialOverride.Priority;
    public string SubmeshName { get; set; } = materialOverride.SubMeshName;
    public float TransitionSample { get; set; } = materialOverride.TransitionSample;
    public uint TransitionSource { get; set; } = materialOverride.TransitionSource;
    public string TransitionTexture { get; set; } = materialOverride.TransitionTexture;
}
