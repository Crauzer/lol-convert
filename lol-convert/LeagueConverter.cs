using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Meta;
using LeagueToolkit.Core.Wad;
using LeagueToolkit.Hashing;
using LeagueToolkit.Meta;
using LeagueToolkit.Meta.Classes;
using Serilog;

namespace lol_convert;

public sealed class LeagueConverter
{
    private const string CHAMPIONS_PATH = "/Champions";

    private readonly WadHashtable _hashtable;
    private readonly MetaEnvironment _metaEnvironment;

    public LeagueConverter(WadHashtable hashtable)
    {
        this._hashtable = hashtable;
        this._metaEnvironment = MetaEnvironment.Create(
            Assembly.Load("LeagueToolkit.Meta.Classes").ExportedTypes.Where(x => x.IsClass)
        );
    }

    public LeaguePackage CreateLeaguePackage(string finalPath, string outputPath)
    {
        var championPackages = CreateChampionPackages(finalPath);

        return new();
    }

    public List<ChampionPackage> CreateChampionPackages(string finalPath)
    {
        var championWadPaths = Directory
            .EnumerateFiles(Path.Join(finalPath, CHAMPIONS_PATH))
            .Where(path => Regex.IsMatch(Path.GetFileName(path), $@"^[\w]+\.wad\.client"))
            .ToList();

        foreach (string championWadPath in championWadPaths)
        {
            string championWadName = Path.GetFileName(championWadPath);
            string championName = championWadName.ToLower().Remove(championWadName.IndexOf('.'));
            WadFile wad = new(File.OpenRead(championWadPath));

            CreateChampionPackage(championName, wad);
        }

        return new();
    }

    private ChampionPackage CreateChampionPackage(string championName, WadFile wad)
    {
        string skinPrefixPath = $"data/characters/{championName}/skins/";

        var skinBinPaths = wad
            .Chunks.Keys.Select(x => this._hashtable.Resolve(x).ToLower())
            .Where(chunkPath =>
                chunkPath.StartsWith(skinPrefixPath, StringComparison.OrdinalIgnoreCase)
            )
            .ToList();

        foreach (string skinBinPath in skinBinPaths)
        {
            string skinName = Path.GetFileNameWithoutExtension(skinBinPath);

            try
            {
                var skin = CreateChampionSkin(
                    championName,
                    skinName,
                    new(wad.LoadChunkDecompressed(skinBinPath).AsStream()),
                    wad
                );
            }
            catch (Exception e)
            {
                Log.Error(
                    e,
                    "Failed to create champion skin package (championName: {championName}, skinName: {skinName})",
                    championName,
                    skinName
                );
            }
        }

        return new();
    }

    private ChampionSkin CreateChampionSkin(
        string championName,
        string skinName,
        BinTree bin,
        WadFile wad
    )
    {
        string skinPropertiesObjectPath = $"characters/{championName}/skins/{skinName}";

        var skinPropertiesObject = bin.Objects[Fnv1a.HashLower(skinPropertiesObjectPath)];
        var skinProperties = MetaSerializer.Deserialize<SkinCharacterDataProperties>(
            this._metaEnvironment,
            skinPropertiesObject
        );

        return new();
    }
}
