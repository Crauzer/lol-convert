using System.Text.Json.Serialization;
using lol_convert.Services;
using lol_convert.Utils;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

[JsonDerivedType(typeof(AtomicClip), "atomic")]
public abstract class BaseClipData(MetaClass.ClipBaseData data)
{
    public List<ClipAccessoryData> Accessories { get; set; } =
        data.Accessorylist.Select(ClipAccessoryData.FromMeta)?.ToList() ?? null;
    public List<string> AnimationInterruptionGroupNames = data
        .AnimationInterruptionGroupNames?.Select(BinHashtableService.ResolveHash)
        .ToList();
    public uint Flags { get; set; } = data.Flags;

    public static BaseClipData FromMeta(MetaClass.ClipBaseData data) =>
        data switch
        {
            MetaClass.AtomicClipData atomicClip => new AtomicClip(atomicClip),
            _ => null,
        };
}

public abstract class BlendableClip(MetaClass.BlendableClipData data) : BaseClipData(data)
{
    public Dictionary<uint, BaseEventData> Events { get; set; } =
        data.EventDataMap?.ToDictionary(x => x.Key.Hash, x => BaseEventData.FromMeta(x.Value));
    public uint MaskDataName { get; set; } = data.MaskDataName;
    public uint SyncGroupDataName { get; set; } = data.SyncGroupDataName;
    public uint TrackDataName { get; set; } = data.TrackDataName;
}

public class AtomicClip(MetaClass.AtomicClipData data) : BlendableClip(data)
{
    public AnimationResourceData AnimationResource { get; set; } =
        ObjectFactory.CreateInstanceOrNull<AnimationResourceData>(data.AnimationResourceData.Value);
    public float StartFrame { get; set; } = data.StartFrame;
    public float EndFrame { get; set; } = data.EndFrame;
    public float TickDuration { get; set; } = data.TickDuration;
    public UpdaterResourceData UpdaterResource { get; set; } =
        ObjectFactory.CreateInstanceOrNull<UpdaterResourceData>(data.UpdaterResourceData);
}

[JsonDerivedType(typeof(KeyframeFloatmapClipAccessoryData), "keyframe_floatmap")]
public abstract class ClipAccessoryData(MetaClass.ClipAccessoryData data)
{
    public string Name { get; set; } = BinHashtableService.ResolveHash(data.Name);

    public static ClipAccessoryData FromMeta(MetaClass.ClipAccessoryData data) =>
        data switch
        {
            MetaClass.KeyFrameFloatMapClipAccessoryData keyframeFloatmap
                => new KeyframeFloatmapClipAccessoryData(keyframeFloatmap),
            _ => null,
        };
}

public class KeyframeFloatmapClipAccessoryData(MetaClass.KeyFrameFloatMapClipAccessoryData data)
    : ClipAccessoryData(data)
{
    public Dictionary<float, float> KeyframeFloatmap = data.KeyFrameFloatmap;
}

public class AnimationResourceData(MetaClass.AnimationResourceData data)
{
    public string AnimationFilePath { get; set; } = data.AnimationFilePath;
}

public class UpdaterResourceData(MetaClass.UpdaterResourceData data) { }
