using System.Reflection;
using LeagueToolkit.Meta;
using lol_convert.Packages;

namespace lol_convert;

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
        var maps = this._mapConverter.CreateMapPackages(finalPath);
        var championPackagePaths = this._championConverter.CreateChampionPackages(finalPath);

        return new() { ChampionPackagePaths = championPackagePaths, Maps = maps };
    }
}

public sealed class LeagueConverterOptions
{
    public bool ConvertChampions { get; set; } = true;
    public bool ConvertChampionSkins { get; set; } = true;
}
