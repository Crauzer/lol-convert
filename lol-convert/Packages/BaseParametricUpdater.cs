using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

public abstract class BaseParametricUpdater
{
    public static BaseParametricUpdater FromMeta(MetaClass.IBaseParametricUpdater data) =>
        data switch
        {
            // TODO
            _ => null,
        };
}

public abstract class BooleanParametricUpdater : BaseParametricUpdater
{
    public static BooleanParametricUpdater FromMeta(MetaClass.IBooleanParametricUpdater data) =>
        data switch
        {
            // TODO
            _ => null,
        };
}

public abstract class FloatParametricUpdater : BaseParametricUpdater
{
    public static FloatParametricUpdater FromMeta(MetaClass.IFloatParametricUpdater data) =>
        data switch
        {
            // TODO
            _ => null,
        };
}