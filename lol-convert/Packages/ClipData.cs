using System.Text.Json.Serialization;
using lol_convert.Services;
using lol_convert.Utils;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

[JsonDerivedType(typeof(AtomicClip), "atomic")]
[JsonDerivedType(typeof(ParametricClip), "parametric")]
[JsonDerivedType(typeof(ConditionBoolClip), "condition_bool")]
[JsonDerivedType(typeof(ConditionFloatClip), "condition_float")]
[JsonDerivedType(typeof(EventControlledSelectorClip), "event_controlled_selector")]
[JsonDerivedType(typeof(ParallelClipData), "parallel")]
[JsonDerivedType(typeof(SelectorClipData), "selector")]
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
            MetaClass.ParametricClipData parametricClip => new ParametricClip(parametricClip),
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
    public AnimationResourceData AnimationResource { get; set; } = new(data.AnimationResourceData);
    public float StartFrame { get; set; } = data.StartFrame;
    public float EndFrame { get; set; } = data.EndFrame;
    public float TickDuration { get; set; } = data.TickDuration;
    public UpdaterResourceData UpdaterResource { get; set; } =
        ObjectFactory.CreateInstanceOrNull<UpdaterResourceData>(data.UpdaterResourceData);
}

public class ParametricClip(MetaClass.ParametricClipData data) : BlendableClip(data)
{
    public List<ParametricPairData> ParametricPairs { get; set; } =
        data.ParametricPairDataList?.Select(x => new ParametricPairData(x)).ToList();
    public BaseParametricUpdater Updater { get; set; } =
        BaseParametricUpdater.FromMeta(data.Updater);
}

public class ConditionBoolClip(MetaClass.ConditionBoolClipData data) : BaseClipData(data)
{
    public bool ChangeAnimationMidPlay { get; set; } = data.ChangeAnimationMidPlay;
    public float ChildAnimDelaySwitchTime { get; set; } = data.ChildAnimDelaySwitchTime;
    public bool DontStompTransitionClip { get; set; } = data.DontStompTransitionClip;
    public uint FalseConditionClipName { get; set; } = data.FalseConditionClipName;
    public bool PlayAnimChangeFromBeginning { get; set; } = data.PlayAnimChangeFromBeginning;
    public bool SyncFrameOnChangeAnim { get; set; } = data.SyncFrameOnChangeAnim;
    public uint TrueConditionClipName { get; set; } = data.TrueConditionClipName;
    public BooleanParametricUpdater Updater { get; set; } =
        BooleanParametricUpdater.FromMeta(data.Updater);
}

public class ConditionFloatClip(MetaClass.ConditionFloatClipData data) : BaseClipData(data)
{
    public FloatParametricUpdater Updater { get; set; } =
        FloatParametricUpdater.FromMeta(data.Updater);
    public List<ConditionFloatPairData> ConditionFloatPairs { get; set; } =
        data.ConditionFloatPairDataList?.Select(x => new ConditionFloatPairData(x)).ToList();

    public bool PlayAnimChangeFromBeginning { get; set; } = data.PlayAnimChangeFromBeginning;
    public bool ChangeAnimationMidPlay { get; set; } = data.ChangeAnimationMidPlay;
    public bool DontStompTransitionClip { get; set; } = data.DontStompTransitionClip;
    public bool SyncFrameOnChangeAnim { get; set; } = data.SyncFrameOnChangeAnim;
    public float ChildAnimDelaySwitchTime { get; set; } = data.ChildAnimDelaySwitchTime;
}

public class EventControlledSelectorClip(MetaClass.EventControlledSelectorClipData data)
    : BaseClipData(data)
{
    public bool PlayAnimChangeFromBeginning { get; set; } = data.PlayAnimChangeFromBeginning;
    public bool ChangeAnimationMidPlay { get; set; } = data.ChangeAnimationMidPlay;
    public bool DontStompTransitionClip { get; set; } = data.DontStompTransitionClip;
    public bool SyncFrameOnChangeAnim { get; set; } = data.SyncFrameOnChangeAnim;
    public float ChildAnimDelaySwitchTime { get; set; } = data.ChildAnimDelaySwitchTime;
    public uint DefaultClipName { get; set; } = data.DefaultClipName;
    public List<EventControlledSelectorPairData> SelectorPairDataList { get; set; } =
        data.SelectorPairDataList?.Select(x => new EventControlledSelectorPairData(x)).ToList();
}

public class ParallelClipData(MetaClass.ParallelClipData data) : BaseClipData(data)
{
    public List<uint> ClipNames { get; set; } = data.ClipNameList?.Select(x => x.Hash).ToList();
}

public class SelectorClipData(MetaClass.SelectorClipData data) : BaseClipData(data)
{
    public List<SelectorPairData> SelectorPairs { get; set; } =
        data.SelectorPairDataList?.Select(x => new SelectorPairData(x)).ToList();
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

public class UpdaterResourceData(MetaClass.UpdaterResourceData data)
{
    // TODO
}

public class ParametricPairData(MetaClass.ParametricPairData data)
{
    public uint ClipName { get; set; } = data.ClipName;
    public float Value { get; set; } = data.Value;
}

public class ConditionFloatPairData(MetaClass.ConditionFloatPairData data)
{
    public float Value { get; set; } = data.Value;
    public float HoldAnimationToLower { get; set; } = data.HoldAnimationToLower;
    public float HoldAnimationToHigher { get; set; } = data.HoldAnimationToHigher;
    public uint ClipName { get; set; } = data.ClipName;
}

public class EventControlledSelectorPairData(MetaClass.EventControlledSelectorPairData data)
{
    public uint StateEventId { get; set; } = data.StateEventId;
    public uint ClipName { get; set; } = data.ClipName;
}

public class SelectorPairData(MetaClass.SelectorPairData data)
{
    public uint ClipName { get; set; } = data.ClipName;
    public float Probability { get; set; } = data.Probability;
}
