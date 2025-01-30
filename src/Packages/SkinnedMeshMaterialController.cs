using System.Text.Json.Serialization;
using lol_convert.Services;
using MetaClass = LeagueToolkit.Meta.Classes;

[JsonDerivedType(typeof(Eaf8e31eMaterialController), "0xeaf8e31e")]
[JsonDerivedType(typeof(TftItemMaterialController), "tft_item")]
[JsonDerivedType(typeof(EsportsBannerMaterialController), "esports_banner")]
public abstract class SkinnedMeshMaterialController
{
    public static SkinnedMeshMaterialController FromMeta(MetaClass.SkinnedMeshDataMaterialController controller)
    {
        return controller switch
        {
            MetaClass.TFTItemMaterialController tftItemController => new TftItemMaterialController(tftItemController),
            MetaClass.EsportsBannerMaterialController esportsBannerController => new EsportsBannerMaterialController(esportsBannerController),
            MetaClass.Class0xeaf8e31e eaf8e31eController => new Eaf8e31eMaterialController(eaf8e31eController),
            _ => throw new NotImplementedException("Unknown material controller type"),
        };
    }
}

public class TftItemMaterialController(MetaClass.TFTItemMaterialController _controller) : SkinnedMeshMaterialController() { }

public class EsportsBannerMaterialController(MetaClass.EsportsBannerMaterialController _controller) : SkinnedMeshMaterialController() { }

public class Eaf8e31eMaterialController(MetaClass.Class0xeaf8e31e _controller) : SkinnedMeshMaterialController()
{
    public List<Class0xf4da6088> m1282894485 { get; set; } = _controller.m1282894485?.Select(x => new Class0xf4da6088(x)).ToList();
}

public class Class0xf4da6088(MetaClass.Class0xf4da6088 data)
{
    public string m449208730 { get; set; } = BinHashtableService.TryResolveObjectLink(data.m449208730);
    public string Submeshes { get; set; } = data.Submeshes;
}
