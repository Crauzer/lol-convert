using System.Text.Json.Serialization;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

[JsonDerivedType(typeof(VfxColorOverLifeMaterialDriver), "color_over_life")]
[JsonDerivedType(typeof(VfxFloatOverLifeMaterialDriver), "float_over_life")]
[JsonDerivedType(typeof(VfxSineMaterialDriver), "sine")]
internal class VfxMaterialDriverBase(MetaClass.IVfxMaterialDriver driver) { }

internal class VfxColorOverLifeMaterialDriver(MetaClass.VfxColorOverLifeMaterialDriver driver)
    : VfxMaterialDriverBase(driver)
{
    public VfxAnimatedColorVariable Colors { get; set; } =
        driver.Colors is null ? null : new(driver.Colors);
    public byte Frequency { get; set; } = driver.Frequency;
}

internal class VfxFloatOverLifeMaterialDriver(MetaClass.VfxFloatOverLifeMaterialDriver driver)
    : VfxMaterialDriverBase(driver)
{
    public VfxAnimatedFloatVariable Graph { get; set; } =
        driver.Graph is null ? null : new(driver.Graph);
    public byte Frequency { get; set; } = driver.Frequency;
}

internal class VfxSineMaterialDriver(MetaClass.VfxSineMaterialDriver driver)
    : VfxMaterialDriverBase(driver)
{
    public float Bias { get; set; } = driver.Bias;
    public float Frequency { get; set; } = driver.Frequency;
    public float Scale { get; set; } = driver.Scale;
}
