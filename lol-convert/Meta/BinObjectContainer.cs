using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Meta;
using LeagueToolkit.Core.Wad;

namespace lol_convert.Meta;

internal class BinObjectContainer
{
    public Dictionary<uint, BinTreeObject> Objects { get; set; } = [];

    public static BinObjectContainer FromPropertyBin(BinTree bin, WadFile wad)
    {
        Dictionary<uint, BinTreeObject> objects = [];
        
        foreach(var binObject in bin.Objects)
        {
            objects.Add(binObject.Key, binObject.Value);
        }

        foreach(var dependency in bin.Dependencies)
        {
            using var dependencyBinStream = wad.LoadChunkDecompressed(dependency).AsStream();
            var dependencyBin = new BinTree(dependencyBinStream);
            foreach (var binObject in dependencyBin.Objects)
            {
                objects.Add(binObject.Key, binObject.Value);
            }
        }


        return new() { Objects = objects };
    }
}
