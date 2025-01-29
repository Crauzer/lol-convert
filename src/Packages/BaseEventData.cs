using lol_convert.Services;
using System.Numerics;
using System.Text.Json.Serialization;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

[JsonDerivedType(typeof(ConformToPathEventData), "conform_to_path")]
[JsonDerivedType(typeof(EnableLookAtEventData), "enable_look_at")]
[JsonDerivedType(typeof(FaceTargetEventData), "face_target")]
[JsonDerivedType(typeof(FadeEventData), "fade")]
[JsonDerivedType(typeof(IdleParticlesVisibilityEventData), "idle_particles_visibility")]
[JsonDerivedType(typeof(JointSnapEventData), "joint_snap")]
[JsonDerivedType(typeof(LockRootOrientationEventData), "lock_root_orientation")]
[JsonDerivedType(typeof(ParticleEventData), "particle")]
[JsonDerivedType(typeof(SoundEventData), "sound")]
[JsonDerivedType(typeof(SpringPhysicsEventData), "spring_physics")]
[JsonDerivedType(typeof(StateLogicEventData), "state_logic")]
[JsonDerivedType(typeof(StopAnimationEventData), "stop_animation")]
[JsonDerivedType(typeof(SubmeshVisibilityEventData), "submesh_visibility")]
[JsonDerivedType(typeof(SyncedAnimationEventData), "synced_animation")]
public abstract class BaseEventData(MetaClass.BaseEventData data)
{
    public string Name { get; set; } = BinHashtableService.TryResolveHash(data.Name);
    public float EndFrame { get; set; } = data.EndFrame;
    public float StartFrame { get; set; } = data.StartFrame;
    public bool FireIfAnimationEndsEarly { get; set; } = data.FireIfAnimationEndsEarly;
    public bool IsSelfOnly { get; set; } = data.IsSelfOnly;

    public static BaseEventData FromMeta(MetaClass.BaseEventData data) => data switch
    {
        MetaClass.ConformToPathEventData conformToPathData
            => new ConformToPathEventData(conformToPathData),
        MetaClass.EnableLookAtEventData enableLookAtData
            => new EnableLookAtEventData(enableLookAtData),
        MetaClass.FaceTargetEventData faceTargetData => new FaceTargetEventData(faceTargetData),
        MetaClass.FadeEventData fadeData => new FadeEventData(fadeData),
        MetaClass.IdleParticlesVisibilityEventData idleParticlesVisibilityData
            => new IdleParticlesVisibilityEventData(idleParticlesVisibilityData),
        MetaClass.JointSnapEventData jointSnapData => new JointSnapEventData(jointSnapData),
        MetaClass.LockRootOrientationEventData lockRootOrientationData
            => new LockRootOrientationEventData(lockRootOrientationData),
        MetaClass.ParticleEventData particleData => new ParticleEventData(particleData),
        MetaClass.SoundEventData soundData => new SoundEventData(soundData),
        MetaClass.SpringPhysicsEventData springPhysicsData
            => new SpringPhysicsEventData(springPhysicsData),
        MetaClass.StateLogicEventData stateLogicData => new StateLogicEventData(stateLogicData),
        MetaClass.StopAnimationEventData stopAnimationData
            => new StopAnimationEventData(stopAnimationData),
        MetaClass.SubmeshVisibilityEventData submeshVisibilityData
            => new SubmeshVisibilityEventData(submeshVisibilityData),
        MetaClass.SyncedAnimationEventData syncedAnimationData
            => new SyncedAnimationEventData(syncedAnimationData),
        null => null,
        _ => throw new ArgumentException("Unknown event data")
    };
}

public class ConformToPathEventData(MetaClass.ConformToPathEventData data) : BaseEventData(data)
{
    public float BlendInTime { get; set; } = data.BlendInTime;
    public float BlendOutTime { get; set; } = data.BlendOutTime;
    public string MaskDataName { get; set; } =
        BinHashtableService.TryResolveHash(data.MaskDataName);
}

public class EnableLookAtEventData(MetaClass.EnableLookAtEventData data) : BaseEventData(data)
{
    public bool EnableLookAt { get; set; } = data.EnableLookAt;
    public bool LockCurrentValues { get; set; } = data.LockCurrentValues;
}

public class FaceTargetEventData(MetaClass.FaceTargetEventData data) : BaseEventData(data)
{
    public float BlendInTime { get; set; } = data.BlendInTime;
    public float BlendOutTime { get; set; } = data.BlendOutTime;
    public byte FaceTarget { get; set; } = data.FaceTarget;
    public float YRotationDegrees { get; set; } = data.YRotationDegrees;
}

public class FadeEventData(MetaClass.FadeEventData data) : BaseEventData(data)
{
    public float TargetAlpha { get; set; } = data.TargetAlpha;
    public float TimeToFade { get; set; } = data.TimeToFade;
}

public class IdleParticlesVisibilityEventData(MetaClass.IdleParticlesVisibilityEventData data)
    : BaseEventData(data)
{
    public bool Show { get; set; } = data.Show;
}

public class JointSnapEventData(MetaClass.JointSnapEventData data) : BaseEventData(data)
{
    public string JointNameToOverride { get; set; } =
        BinHashtableService.TryResolveHash(data.JointNameToOverride);
    public string JointNameToSnapTo { get; set; } =
        BinHashtableService.TryResolveHash(data.JointNameToSnapTo);
    public Vector3 Offset { get; set; } = data.Offset;
}

public class LockRootOrientationEventData(MetaClass.LockRootOrientationEventData data)
    : BaseEventData(data)
{
    public float BlendOutTime { get; set; } = data.BlendOutTime;
    public string JointName { get; set; } = BinHashtableService.TryResolveHash(data.JointName);
}

public class ParticleEventData(MetaClass.ParticleEventData data) : BaseEventData(data)
{
    public string EffectKey { get; set; } = BinHashtableService.TryResolveHash(data.EffectKey);
    public string EffectName { get; set; } = data.EffectName;
    public string EnemyEffectKey { get; set; } =
        BinHashtableService.TryResolveHash(data.EnemyEffectKey);
    public bool IsDetachable { get; set; } = data.IsDetachable;
    public bool IsKillEvent { get; set; } = data.IsKillEvent;
    public bool IsLoop { get; set; } = data.IsLoop;
    public List<ParticleEventDataPair> ParticleEventDataPairs { get; set; } =
        data.ParticleEventDataPairList?.Select(x => new ParticleEventDataPair(x)).ToList();
    public float Scale { get; set; } = data.Scale;
    public bool ScalePlaySpeedWithAnimation { get; set; } = data.ScalePlaySpeedWithAnimation;
    public bool SkipIfPastEndFrame { get; set; } = data.SkipIfPastEndFrame;
}

public class SoundEventData(MetaClass.SoundEventData data) : BaseEventData(data)
{
    public ushort ConditionClipTransitionType { get; set; } = data.ConditionClipTransitionType;
    public bool IsKillEvent { get; set; } = data.IsKillEvent;
    public bool IsLoop { get; set; } = data.IsLoop;
    public bool SkipIfPastEndFrame { get; set; } = data.SkipIfPastEndFrame;
    public string SoundName { get; set; } = data.SoundName;
}

public class SpringPhysicsEventData(MetaClass.SpringPhysicsEventData data) : BaseEventData(data)
{
    public float BlendOutTime { get; set; } = data.BlendOutTime;
    public string SpringToAffect { get; set; } =
        BinHashtableService.TryResolveHash(data.SpringToAffect);
}

public class StateLogicEventData(MetaClass.StateLogicEventData data) : BaseEventData(data)
{
    public uint StateLogicEventSignalId { get; set; } = data.StateLogicEventSignalId;
}

public class StopAnimationEventData(MetaClass.StopAnimationEventData data) : BaseEventData(data)
{
    public string StopAnimationName { get; set; } =
        BinHashtableService.TryResolveHash(data.StopAnimationName);
}

public class SubmeshVisibilityEventData(MetaClass.SubmeshVisibilityEventData data)
    : BaseEventData(data)
{
    public bool ApplyOnlyToAvatarMeshes { get; set; } = data.ApplyOnlyToAvatarMeshes;
    public List<string> HideSubmeshList { get; set; } =
        data.HideSubmeshList?.Select(BinHashtableService.TryResolveHash).ToList();
    public List<string> ShowSubmeshList { get; set; } =
        data.ShowSubmeshList?.Select(BinHashtableService.TryResolveHash).ToList();
}

public class SyncedAnimationEventData(MetaClass.SyncedAnimationEventData data) : BaseEventData(data)
{
    public float LerpTime { get; set; } = data.LerpTime;
}

// ----------- Additional classes ------------
public class ParticleEventDataPair(MetaClass.ParticleEventDataPair data)
{
    public string BoneName { get; set; } = BinHashtableService.TryResolveHash(data.BoneName);
    public string TargetBoneName { get; set; } =
        BinHashtableService.TryResolveHash(data.TargetBoneName);
}
