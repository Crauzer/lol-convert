using lol_convert.Services;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

internal class AnimationGraph
{
    public List<uint> AnimStateGraphEntryClips { get; set; }
    public Dictionary<ulong, BaseBlendData> BlendData { get; set; }
    public bool UseCascadeBlend { get; set; }
    public float CascadeBlendValue { get; set; }
    public Dictionary<string, BaseClipData> ClipData { get; set; }
    public Dictionary<uint, MaskData> MaskData { get; set; }
    public Dictionary<uint, SyncGroupData> SyncGroupData { get; set; }
    public Dictionary<uint, TrackData> TrackData { get; set; }

    public AnimationGraph() { }

    public AnimationGraph(MetaClass.AnimationGraphData animationGraph)
    {
        this.AnimStateGraphEntryClips =
            animationGraph.AnimStateGraphEntryClips?.Select(x => x.Hash).ToList() ?? null;

        this.BlendData =
            animationGraph
                .BlendDataTable?.Select(x => new KeyValuePair<ulong, BaseBlendData>(
                    x.Key,
                    BaseBlendData.FromMeta(x.Value)
                ))
                .ToDictionary() ?? null;

        this.UseCascadeBlend = animationGraph.UseCascadeBlend;
        this.CascadeBlendValue = animationGraph.CascadeBlendValue;

        foreach (var clipData in animationGraph.ClipDataMap)
        {
            var clip = clipData.Value switch
            {
                MetaClass.AtomicClipData atomicClip => new AtomicClip(atomicClip),
                _ => null,
            };

            this.ClipData.Add(BinHashtableService.ResolveHash(clipData.Key), clip);
        }

        this.MaskData = animationGraph
            .MaskDataMap.Select(x => KeyValuePair.Create(x.Key.Hash, new MaskData(x.Value)))
            .ToDictionary();
        this.SyncGroupData = animationGraph
            .SyncGroupDataMap.Select(x =>
                KeyValuePair.Create(x.Key.Hash, new SyncGroupData(x.Value))
            )
            .ToDictionary();
        this.TrackData = animationGraph
            .TrackDataMap.Select(x => KeyValuePair.Create(x.Key.Hash, new TrackData(x.Value)))
            .ToDictionary();
    }
}

public class MaskData(MetaClass.MaskData data)
{
    public uint? Id { get; set; } = data.Id;
    public List<float> WeightList { get; set; } = data.WeightList?.ToList() ?? null;
}

public class SyncGroupData(MetaClass.SyncGroupData data)
{
    public uint Type { get; set; } = data.Type;
}

public class TrackData(MetaClass.TrackData data)
{
    public byte BlendMode { get; set; } = data.BlendMode;
    public float BlendWeight { get; set; } = data.BlendWeight;
    public byte Priority { get; set; } = data.Priority;
}
