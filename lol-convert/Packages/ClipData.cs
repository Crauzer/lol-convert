using System.Text.Json.Serialization;
using lol_convert.Services;
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

internal class AtomicClip : BaseClipData
{
    public string GltfName { get; set; }

    public AtomicClip(MetaClass.AtomicClipData data)
        : base(data)
    {
        this.GltfName = Path.GetFileNameWithoutExtension(
            data.AnimationResourceData.Value.AnimationFilePath
        );
    }
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
