using LeagueToolkit.Core.Wad;
using LeagueToolkit.Meta;
using lol_convert.Wad;

namespace lol_convert.Converters;

internal class ChampionConverter
{
    private readonly WadHashtable _wadHashtable;
    private readonly MetaEnvironment _metaEnvironment;
    private readonly string _outputPath;

    private readonly CharacterConverter _characterConverter;

    public ChampionConverter(
        WadHashtable wadHashtable,
        MetaEnvironment metaEnvironment,
        string outputPath
    )
    {
        _wadHashtable = wadHashtable;
        _metaEnvironment = metaEnvironment;
        _outputPath = outputPath;

        _characterConverter = new(wadHashtable, metaEnvironment, outputPath);
    }

    public void ConvertChampionsFromLeague(string finalPath)
    {
        var championWadPaths = ConvertUtils.GlobChampionWads(finalPath).ToList();
        foreach (string championWadPath in championWadPaths)
        {
            var championWadName = Path.GetFileName(championWadPath);
            var championName = championWadName.ToLower().Remove(championWadName.IndexOf('.'));

            WadFile wad = new(File.OpenRead(championWadPath));
            var chunkPaths = ConvertUtils.ResolveWadChunkPaths(wad, _wadHashtable).ToList();

            var characterData = _characterConverter.CreateCharacterData(
                championName,
                wad,
                chunkPaths
            );
            _characterConverter.SaveCharacterData(characterData);
        }
    }

    public void ConvertChampionFromLeague(string finalPath, string championName)
    {
        var wadPath = Path.Combine(finalPath, "Champions", $"{championName}.wad.client");

        WadFile wad = new(File.OpenRead(wadPath));
        var chunkPaths = ConvertUtils.ResolveWadChunkPaths(wad, _wadHashtable).ToList();

        var characterData = _characterConverter.CreateCharacterData(championName, wad, chunkPaths);
        _characterConverter.SaveCharacterData(characterData);
    }
}
