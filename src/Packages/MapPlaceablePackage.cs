using System.Numerics;
using System.Text.Json.Serialization;
using LeagueToolkit.Meta.Classes;
using lol_convert.Services;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

[JsonDerivedType(typeof(MapParticlePackage), typeDiscriminator: "map_particle")]
[JsonDerivedType(typeof(MapLocatorPackage), typeDiscriminator: "map_locator")]
[JsonDerivedType(typeof(MapAudioPackage), typeDiscriminator: "map_audio")]
[JsonDerivedType(typeof(GdsMapObjectPackage), typeDiscriminator: "gds_map_object")]
internal class MapPlaceableBase(MetaClass.MapPlaceable mapPlaceable)
{
    public string Name { get; set; } = mapPlaceable.Name;
    public byte VisibilityFlags { get; set; } = mapPlaceable.VisibilityFlags;
    public Matrix4x4 Transform { get; set; } = mapPlaceable.Transform;
}

internal class MapLocatorPackage(MetaClass.MapLocator mapLocator) : MapPlaceableBase(mapLocator) { }

internal class MapParticlePackage(MetaClass.MapParticle mapParticle) : MapPlaceableBase(mapParticle)
{
    public bool AllDimensions { get; set; } = mapParticle.AllDimensions;
    public Vector4 ColorModulate { get; set; } = mapParticle.ColorModulate;
    public bool EyeCandy { get; set; } = mapParticle.EyeCandy;
    public string GroupName { get; set; } = mapParticle.GroupName;
    public int Quality { get; set; } = mapParticle.Quality;
    public bool StartDisabled { get; set; } = mapParticle.StartDisabled;
    public string System { get; set; } =
        BinHashtableService.TryResolveObjectLink(mapParticle.System);
    public bool Transitional { get; set; } = mapParticle.Transitional;
    public string VisibilityController { get; set; } =
        BinHashtableService.TryResolveObjectLink(mapParticle.VisibilityController);
    public uint VisibilityMode { get; set; } = mapParticle.VisibilityMode;
}

internal class MapAudioPackage(MetaClass.MapAudio mapAudio) : MapPlaceableBase(mapAudio)
{
    public uint AudioType { get; set; } = mapAudio.AudioType;
    public string EventName { get; set; } = mapAudio.EventName;
    public float MaxIntervalSec { get; set; } = mapAudio.MaxIntervalSec;
    public float MinIntervalSec { get; set; } = mapAudio.MinIntervalSec;
    public float StartTime { get; set; } = mapAudio.StartTime;
    public string VisibilityController { get; set; } =
        BinHashtableService.TryResolveObjectLink(mapAudio.VisibilityController);
}

internal class GdsMapObjectPackage(MetaClass.GdsMapObject gdsMapObject)
    : MapPlaceableBase(gdsMapObject)
{
    public Vector3 BoxMin { get; set; } = gdsMapObject.BoxMin;
    public Vector3 BoxMax { get; set; } = gdsMapObject.BoxMax;
    public List<GdsMapObjectExtraInfoPackage> ExtraInfo { get; set; } =
        gdsMapObject
            .ExtraInfo?.Select<MetaClass.GDSMapObjectExtraInfo, GdsMapObjectExtraInfoPackage>(x =>
                x switch
                {
                    MetaClass.GDSMapObjectAnimationInfo gdsMapObjectAnimationInfo
                        => new GdsMapObjectAnimationInfoPackage(gdsMapObjectAnimationInfo),
                    MetaClass.GDSMapObjectBannerInfo bannerInfo
                        => new GdsMapObjectBannerInfoPackage(bannerInfo),
                    _ => null
                }
            )
            .ToList() ?? [];
    public bool EyeCandy { get; set; } = gdsMapObject.EyeCandy;
    public bool IgnoreCollisionOnPlacement { get; set; } = gdsMapObject.IgnoreCollisionOnPlacement;
    public string VisibilityController { get; set; } =
        BinHashtableService.TryResolveObjectLink(gdsMapObject.VisibilityController);
    public uint MapObjectSkinId { get; set; } = gdsMapObject.MapObjectSkinID;
    public byte Type { get; set; } = gdsMapObject.Type;
}

[JsonDerivedType(typeof(GdsMapObjectAnimationInfoPackage), typeDiscriminator: "animation_info")]
[JsonDerivedType(typeof(GdsMapObjectBannerInfoPackage), typeDiscriminator: "banner_info")]
internal class GdsMapObjectExtraInfoPackage { }

internal class GdsMapObjectAnimationInfoPackage(
    MetaClass.GDSMapObjectAnimationInfo gdsMapObjectAnimationInfo
) : GdsMapObjectExtraInfoPackage
{
    public string DefaultAnimation { get; set; } = gdsMapObjectAnimationInfo.DefaultAnimation;
    public bool DestroyOnCompletion { get; set; } = gdsMapObjectAnimationInfo.DestroyOnCompletion;
    public float Duration { get; set; } = gdsMapObjectAnimationInfo.Duration;
    public bool Looping { get; set; } = gdsMapObjectAnimationInfo.Looping;
}

internal class GdsMapObjectBannerInfoPackage(MetaClass.GDSMapObjectBannerInfo bannerInfo)
    : GdsMapObjectExtraInfoPackage
{
    public string BannerData { get; set; } =
        BinHashtableService.TryResolveObjectLink(bannerInfo.BannerData);
}
