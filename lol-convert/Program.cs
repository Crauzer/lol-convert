using System.Diagnostics;
using System.Text.Json;
using CommandLine;
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
            .Default.ParseArguments<ConvertLeagueOptions>(args)
            .MapResult(RunConvertLeague, errors => 1);
    }

    static int RunConvertLeague(ConvertLeagueOptions options)
    {
        if (Directory.Exists(options.OutputPath))
        {
            Log.Information(
                "Clearing output directory (directory: {outputDirectory})",
                options.OutputPath
            );
            Directory.Delete(options.OutputPath, true);
        }

        LeagueConverter converter =
            new(
                options.OutputPath,
                WadHashtable.FromFile(options.HashtablePath),
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
}
