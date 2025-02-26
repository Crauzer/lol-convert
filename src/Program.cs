using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using CommandLine;
using LeagueToolkit.Meta;
using lol_convert.Converters;
using lol_convert.Services;
using lol_convert.Utils;
using lol_convert.Wad;
using Serilog;

namespace lol_convert;

internal class Program
{
    static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger();

        Parser
            .Default.ParseArguments<ConvertLeagueOptions, ConvertChampionOptions, ConvertMapOptions>(args)
            .MapResult(
                (ConvertLeagueOptions options) => RunConvertLeague(options),
                (ConvertChampionOptions options) => RunConvertChampion(options),
                (ConvertMapOptions options) => RunConvertMap(options),
                errors => 1
            );
    }

    static int RunConvertLeague(ConvertLeagueOptions options)
    {
        BinHashtableService.LoadBinHashes(options.BinHashesPath);
        BinHashtableService.LoadBinObjects(options.BinObjectHashesPath);

        Log.Information(
            "Clearing output directory (directory: {outputDirectory})",
            options.OutputPath
        );
        FsUtils.ClearDirectory(options.OutputPath);

        LeagueConverter converter =
            new(
                options.OutputPath,
                WadHashtable.FromFile(options.WadHashtablePath),
                new()
                {
                    ConvertChampions = options.ConvertChampions ?? true,
                    ConvertChampionSkins = options.ConvertChampionSkins ?? true
                }
            );

        var leaguePackage = converter.CreateLeaguePackage(options.FinalPath);

        using var indexStream = File.Create(Path.Join(options.OutputPath, "index.json"));
        JsonSerializer.Serialize(
            indexStream,
            leaguePackage,
            options: new()
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            }
        );

        Process.Start("explorer.exe", options.OutputPath);

        return 0;
    }

    static int RunConvertChampion(ConvertChampionOptions options)
    {
        BinHashtableService.LoadBinHashes(options.BinHashesPath);
        BinHashtableService.LoadBinObjects(options.BinObjectHashesPath);

        Log.Information(
            "Clearing output directory (directory: {outputDirectory})",
            options.OutputPath
        );
        FsUtils.ClearDirectory(options.OutputPath);

        var metaEnvironment = MetaEnvironment.Create(
            Assembly.Load("LeagueToolkit.Meta.Classes").ExportedTypes.Where(x => x.IsClass)
        );

        ChampionConverter converter =
            new(
                WadHashtable.FromFile(options.WadHashtablePath),
                metaEnvironment,
                options.OutputPath
            );

        converter.ConvertChampionFromLeague(options.FinalPath, options.ChampionName);

        return 0;
    }

    static int RunConvertMap(ConvertMapOptions options)
    {
        BinHashtableService.LoadBinHashes(options.BinHashesPath);
        BinHashtableService.LoadBinObjects(options.BinObjectHashesPath);

        Log.Information(
            "Clearing output directory (directory: {outputDirectory})",
            options.OutputPath
        );
        FsUtils.ClearDirectory(options.OutputPath);

        var metaEnvironment = MetaEnvironment.Create(
            Assembly.Load("LeagueToolkit.Meta.Classes").ExportedTypes.Where(x => x.IsClass)
        );

        var mapConverter = new MapConverter(
            WadHashtable.FromFile(options.WadHashtablePath),
            metaEnvironment,
            options.OutputPath
        );

        var _ = mapConverter.CreateMapPackageFromLeague(options.MapName, options.FinalPath);

        return 0;
    }
}
