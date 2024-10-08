﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lol_convert.Services;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

public sealed class Character
{
    public string Name { get; set; }
    public List<string> SkinNames { get; set; }
}

internal sealed class CharacterSkin
{
    public string Name { get; set; }
    public string DisplayName { get; set; }

    public string SkinMeshPath { get; set; }
    public float SkinScale { get; set; }

    public string Material { get; set; }
    public string Texture { get; set; }
    public List<SkinMeshMaterialOverridePackage> MaterialOverrides { get; set; }
    public Dictionary<string, StaticMaterialPackage> Materials { get; set; }
    public Dictionary<string, VfxSystem> VfxSystems { get; set; }
    public Dictionary<string, string> ResourceResolver { get; set; }
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
