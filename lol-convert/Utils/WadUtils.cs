using LeagueToolkit.Core.Wad;
using lol_convert.Wad;

namespace lol_convert.Utils;

internal static class WadUtils
{
    public static IEnumerable<string> ResolveWadChunkPaths(WadFile wad, WadHashtable hashtable) =>
        wad.Chunks.Keys.Select(chunk => hashtable.Resolve(chunk).ToLower());
}
