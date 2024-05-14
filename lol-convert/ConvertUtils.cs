using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace lol_convert;

public sealed partial class ConvertUtils
{
    private const string CHAMPIONS_PATH = "/Champions";
    private const string MAPS_PATH = "/Maps/Shipping";

    [GeneratedRegex(@"^[\w]+\.wad\.client")]
    private static partial Regex WadClientRegex();

    [GeneratedRegex(@"^Map[\d]+\.wad\.client")]
    private static partial Regex MapWadClientRegex();

    public static IEnumerable<string> GlobChampionWads(string finalPath) =>
        Directory
            .EnumerateFiles(Path.Join(finalPath, CHAMPIONS_PATH))
            .Where(path => WadClientRegex().IsMatch(Path.GetFileName(path)));

    public static IEnumerable<string> GlobChampionSkinBinPaths(
        string championName,
        IEnumerable<string> chunkPaths
    ) =>
        chunkPaths.Where(chunkPath =>
            chunkPath.StartsWith(
                $"data/characters/{championName}/skins/",
                StringComparison.OrdinalIgnoreCase
            )
        );

    public static IEnumerable<string> GlobMapWads(string finalPath) =>
        Directory
            .EnumerateFiles(Path.Join(finalPath, MAPS_PATH))
            .Where(path => MapWadClientRegex().IsMatch(Path.GetFileName(path)));
}
