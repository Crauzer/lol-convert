using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueToolkit.Meta;
using lol_convert.Packages;

namespace lol_convert;

internal class MapConverter
{
    private readonly WadHashtable _wadHashtable;
    private readonly MetaEnvironment _metaEnvironment;
    private readonly string _outputPath;

    public MapConverter(
        WadHashtable wadHashtable,
        MetaEnvironment metaEnvironment,
        string outputPath
    )
    {
        this._wadHashtable = wadHashtable;
        this._metaEnvironment = metaEnvironment;
        this._outputPath = outputPath;
    }

    public List<string> CreateMapPackages(string finalPath)
    {
        var mapWadPaths = ConvertUtils.GlobMapWads(finalPath).ToArray();
        var mapPackages = new List<string>(mapWadPaths.Length);

        foreach (var mapWadPath in mapWadPaths)
        {
            var mapPackage = this.CreateMapPackage(mapWadPath);
            mapPackages.Add(mapPackage);
        }

        return mapPackages;
    }

    public MapPackage CreateMapPackage()
    {

    }
}
