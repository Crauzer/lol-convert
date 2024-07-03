using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using lol_convert.Services;
using MetaClasses = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

internal class AnimationGraphPackage
{
    public Dictionary<string, ClipDataPackageBase> Clips { get; set; } = [];
    
    public AnimationGraphPackage() { }
    public AnimationGraphPackage(MetaClasses.AnimationGraphData animationGraph)
    {
        foreach (var clipData in animationGraph.ClipDataMap)
        {
            var clip = clipData.Value switch
            {
                MetaClasses.AtomicClipData atomicClip => new AtomicClipPackage(atomicClip),
                _ => null,
            };

            this.Clips.Add(BinHashtableService.ResolveHash(clipData.Key), clip);
        }
    }
}

[JsonDerivedType(typeof(AtomicClipPackage), "atomic")]
internal class ClipDataPackageBase { }

internal class AtomicClipPackage : ClipDataPackageBase
{
    public string GltfName { get; set; }

    public AtomicClipPackage(MetaClasses.AtomicClipData atomicClip)
    {
        this.GltfName = Path.GetFileNameWithoutExtension(
            atomicClip.AnimationResourceData.Value.AnimationFilePath
        );
    }
}
