using System.Text.Json.Serialization;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

[JsonDerivedType(typeof(TimeBlendData), "time")]
[JsonDerivedType(typeof(TransitionClipBlendData), "transition_clip")]
public class BaseBlendData
{
    public static BaseBlendData FromMeta(MetaClass.BaseBlendData data) =>
        data switch
        {
            MetaClass.TimeBlendData timeBlendData => new TimeBlendData(timeBlendData),
            MetaClass.TransitionClipBlendData transitionClipBlendData
                => new TransitionClipBlendData(transitionClipBlendData),
            _ => throw new InvalidOperationException("Unknown blend data")
        };
}

public class TimeBlendData(MetaClass.TimeBlendData data) : BaseBlendData
{
    public float Time { get; set; } = data.Time;
}

public class TransitionClipBlendData(MetaClass.TransitionClipBlendData data) : BaseBlendData
{
    public uint ClipName { get; set; } = data.ClipName;
}
