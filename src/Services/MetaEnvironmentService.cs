using LeagueToolkit.Meta;
using System.Reflection;

namespace lol_convert.Services;

internal static class MetaEnvironmentService
{
    public static MetaEnvironment Environment = MetaEnvironment.Create(
        Assembly.Load("LeagueToolkit.Meta.Classes").ExportedTypes.Where(x => x.IsClass)
    );
}
