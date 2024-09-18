using lol_convert.Services;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

internal class AnimationGraph
{
    public List<uint> AnimStateGraphEntryClips { get; set; }
    public Dictionary<ulong, BaseBlendData> BlendData { get; set; }
    public bool UseCascadeBlend { get; set; }
    public float CascadeBlendValue { get; set; }
    public Dictionary<uint, BaseClipData> Clips { get; set; }
    public Dictionary<uint, MaskData> Masks { get; set; }
    public Dictionary<uint, SyncGroupData> SyncGroups { get; set; }
    public Dictionary<uint, TrackData> Tracks { get; set; }

    public AnimationGraph() { }

    public AnimationGraph(MetaClass.AnimationGraphData animationGraph)
    {
        this.AnimStateGraphEntryClips = animationGraph
            .AnimStateGraphEntryClips?.Select(x => x.Hash)
            .ToList();

        this.BlendData = animationGraph.BlendDataTable?.ToDictionary(
            x => x.Key,
            x => BaseBlendData.FromMeta(x.Value)
        );

        this.UseCascadeBlend = animationGraph.UseCascadeBlend;
        this.CascadeBlendValue = animationGraph.CascadeBlendValue;

        this.Clips = animationGraph.ClipDataMap?.ToDictionary(
            x => x.Key.Hash,
            x => BaseClipData.FromMeta(x.Value)
        );
        this.Masks = animationGraph.MaskDataMap?.ToDictionary(
            x => x.Key.Hash,
            x => new MaskData(x.Value)
        );
        this.SyncGroups = animationGraph.SyncGroupDataMap?.ToDictionary(
            x => x.Key.Hash,
            x => new SyncGroupData(x.Value)
        );
        this.Tracks = animationGraph.TrackDataMap?.ToDictionary(
            x => x.Key.Hash,
            x => new TrackData(x.Value)
        );
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
