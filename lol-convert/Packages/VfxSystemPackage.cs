using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

internal class VfxSystem { }


internal class VfxProbabilityTable(MetaClass.VfxProbabilityTableData table)
{
    public List<float> KeyTimes = [.. table.KeyTimes];
    public List<float> KeyValues = [.. table.KeyValues];
    public float SingleValue = table.SingleValue;
}
