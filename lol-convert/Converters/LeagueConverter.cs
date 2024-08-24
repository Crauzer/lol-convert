using System.Reflection;
using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Meta;
using LeagueToolkit.Core.Wad;
using LeagueToolkit.Hashing;
using LeagueToolkit.Meta;
using lol_convert.Packages;
using lol_convert.Wad;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Converters;

public sealed class LeagueConverter
{
    public string OutputPath { get; private set; }
    public LeagueConverterOptions Options { get; private set; }

    private readonly ChampionConverter _championConverter;
    private readonly MapConverter _mapConverter;

    private readonly WadHashtable _hashtable;
    private readonly MetaEnvironment _metaEnvironment;

    public LeagueConverter(
        string outputPath,
        WadHashtable hashtable,
        LeagueConverterOptions options
    )
    {
        this.OutputPath = outputPath;
        this.Options = options;

        this._hashtable = hashtable;
        this._metaEnvironment = MetaEnvironment.Create(
            Assembly.Load("LeagueToolkit.Meta.Classes").ExportedTypes.Where(x => x.IsClass)
        );

        this._championConverter = new(hashtable, _metaEnvironment, outputPath);
        this._mapConverter = new(hashtable, _metaEnvironment, outputPath);
    }

    public LeaguePackage CreateLeaguePackage(string finalPath)
    {
        var maps = _mapConverter.CreateMapPackages(finalPath);
        var championPackagePaths = _championConverter.CreateChampionPackages(finalPath);

        return new() { ChampionPackagePaths = championPackagePaths, Maps = maps };
    }

    private List<string> GetChampionsList(string finalPath)
    {
        var globalWad = new WadFile(Path.Join(finalPath, "Global.wad.client"));

        using var championsBinStream = globalWad.LoadChunkDecompressed(
            "global/champions/champions.bin"
        );
        var championsBin = new BinTree(championsBinStream.AsStream());

        var championHash = Fnv1a.HashLower(nameof(MetaClass.Champion));
        var championObjects = championsBin.Objects.Values.Where(x => x.ClassHash == championHash);
        List<string> championNames = [];
        foreach (var championObject in championObjects)
        {
            var champion = MetaSerializer.Deserialize<MetaClass.Champion>(
                this._metaEnvironment,
                championObject
            );

            championNames.Add(champion.Name);
        }

        return championNames;
    }
}

public sealed class LeagueConverterOptions
{
    public bool ConvertChampions { get; set; } = true;
    public bool ConvertChampionSkins { get; set; } = true;
}
