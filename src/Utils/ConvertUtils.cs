using LeagueToolkit.Core.Wad;
using lol_convert.Wad;
using System.Text.RegularExpressions;

namespace lol_convert;

public sealed partial class ConvertUtils
{
    private const string CHAMPIONS_PATH = "/Champions";
    private const string MAPS_PATH = "/Maps/Shipping";

    [GeneratedRegex(@"^[\w]+\.wad\.client")]
    private static partial Regex WadClientRegex();

    [GeneratedRegex(@"^Map[\d]+\.wad\.client")]
    private static partial Regex MapWadClientRegex();

    [GeneratedRegex(@"^TFTSet[\d\w]+\.wad\.client")]
    private static partial Regex TftSetRegex();

    [GeneratedRegex(@"^characters\/\w+")]
    private static partial Regex TftCharacterRegex();

    public static IEnumerable<string> GlobChampionWads(string finalPath) =>
        Directory
            .EnumerateFiles(Path.Join(finalPath, CHAMPIONS_PATH))
            .Where(path => WadClientRegex().IsMatch(Path.GetFileName(path)));

    public static IEnumerable<string> GlobCharacterSkinBinPaths(
        string characterName,
        IEnumerable<string> chunkPaths
    ) =>
        chunkPaths.Where(chunkPath =>
            chunkPath.StartsWith(
                $"data/characters/{characterName}/skins/",
                StringComparison.OrdinalIgnoreCase
            )
        );

    public static IEnumerable<string> GlobMapWads(string finalPath) =>
        Directory
            .EnumerateFiles(Path.Join(finalPath, MAPS_PATH))
            .Where(path => MapWadClientRegex().IsMatch(Path.GetFileName(path)));

    public static IEnumerable<string> GlobTftSets(string finalPath) =>
        Directory
            .EnumerateFiles(finalPath)
            .Where(path => TftSetRegex().IsMatch(Path.GetFileName(path)));

    public static IEnumerable<string> GlobTftCharacters(IEnumerable<string> chunkPaths) =>
        chunkPaths.Where(chunkPath => TftCharacterRegex().IsMatch(chunkPath));

    public static IEnumerable<string> ResolveWadChunkPaths(WadFile wad, WadHashtable hashtable) =>
        wad.Chunks.Keys.Select(chunk => hashtable.Resolve(chunk).ToLower());
}
