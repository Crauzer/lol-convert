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
