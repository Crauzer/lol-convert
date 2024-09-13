using LeagueToolkit.Meta;

namespace lol_convert.Utils;

public static class ObjectFactory
{
    public static T CreateInstanceOrNull<T>(IMetaClass data)
        where T : class => data is null ? null : (T)Activator.CreateInstance(typeof(T), data);
}
