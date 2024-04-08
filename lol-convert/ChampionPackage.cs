using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lol_convert;

public sealed class ChampionPackage
{
    public string Name { get; set; }
    public List<string> SkinNames { get; set; }    

}

public sealed class ChampionSkin
{
    public required string Name { get; set; }
    public string? DisplayName { get; set; }
    public string? SkinMeshPath { get; set; }
    public float SkinScale { get; set; }

    public List<StaticMaterialPackage> Materials { get; set; }
}
