using CommandLine;
using Serilog;

namespace lol_convert;

internal class Program
{
    static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();

        Parser
            .Default.ParseArguments<ConvertLeagueOptions>(args)
            .MapResult(RunConvertLeague, errors => 1);
    }

    static int RunConvertLeague(ConvertLeagueOptions options)
    {
        LeagueConverter converter = new(WadHashtable.FromFile(options.HashtablePath));

        converter.CreateLeaguePackage(options.FinalPath, "");

        return 0;
    }
}
