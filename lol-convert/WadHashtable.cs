using LeagueToolkit.Core.Wad;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lol_convert;

public sealed class WadHashtable
{
    public Dictionary<ulong, string> _hashtable;

    public WadHashtable(Dictionary<ulong, string> hashtable)
    {
        this._hashtable = hashtable;
    }

    public static WadHashtable FromFile(string path)
    {
        Dictionary<ulong, string> hashtable = [];
        using StreamReader reader = new(path);
        while (reader.ReadLine() is string line)
        {
            int separatorIndex = line.IndexOf(' ');
            ulong pathHash = ulong.Parse(line.AsSpan(0, separatorIndex), NumberStyles.HexNumber);

            hashtable.TryAdd(pathHash, line[(separatorIndex + 1)..]);
        }

        return new(hashtable);
    }

    public string ResolveChunk(WadChunk chunk) => Resolve(chunk.PathHash);

    public string Resolve(ulong pathHash) => this._hashtable.GetValueOrDefault(pathHash, string.Format("0x{0:x8}", pathHash));
}
