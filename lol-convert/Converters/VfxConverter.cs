using LeagueToolkit.Meta;
using lol_convert.Wad;

namespace lol_convert.Converters;

internal class VfxConverter
{
    private readonly WadHashtable _wadHashtable;
    private readonly MetaEnvironment _metaEnvironment;
    private readonly string _outputPath;

    public VfxConverter(WadHashtable wadHashtable,
        MetaEnvironment metaEnvironment,
        string outputPath)
    {
        this._wadHashtable = wadHashtable;
        this._metaEnvironment = metaEnvironment;
        this._outputPath = outputPath;
    }

    
}
