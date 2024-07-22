using lol_convert.Services;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

internal class VfxMaterial(MetaClass.VfxMaterialDefinitionData material)
{
    public string Material { get; set; } =
        BinHashtableService.TryResolveObjectLink(material.Material);
    public Dictionary<string, VfxMaterialDriverBase> Drivers { get; set; } =
        material
            .MaterialDrivers?.Select(x => new KeyValuePair<string, VfxMaterialDriverBase>(
                x.Key,
                x.Value switch
                {
                    MetaClass.VfxColorOverLifeMaterialDriver driver
                        => new VfxColorOverLifeMaterialDriver(driver),
                    MetaClass.VfxFloatOverLifeMaterialDriver driver
                        => new VfxFloatOverLifeMaterialDriver(driver),
                    MetaClass.VfxSineMaterialDriver driver => new VfxSineMaterialDriver(driver),
                    null => null,
                    _ => throw new NotImplementedException("Unknown Vfx Material driver"),
                }
            ))
            .ToDictionary();
}
