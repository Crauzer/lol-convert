using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LeagueToolkit.Core.Meta;
using LeagueToolkit.Meta;
using LeagueToolkit.Meta.Classes;
using lol_convert.Services;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

internal class MapSkinPackage
{
    public string Name { get; set; }
    public MapContainerPackage Container { get; set; }
    public Dictionary<string, StaticMaterialPackage> StaticMaterials { get; set; }

    public Dictionary<string, uint> MeshSceneGraphLinks { get; set; }
    public Dictionary<string, MapPlaceableContainerPackage> Chunks { get; set; }
}

internal class MapContainerPackage
{
    public string MapPath { get; set; }
    public Vector2? BoundsMin { get; set; }
    public Vector2? BoundsMax { get; set; }
    public bool m3572349073 { get; set; }
    public float m4027637499 { get; set; }
    public float LowestWalkableHeight { get; set; }

    public Dictionary<string, string> Chunks { get; set; } = [];

    public MapContainerPackage(MetaClass.MapContainer mapContainer)
    {
        this.MapPath = mapContainer.MapPath;
        this.BoundsMin = mapContainer.BoundsMin;
        this.BoundsMax = mapContainer.BoundsMax;
        this.m3572349073 = mapContainer.m3572349073;
        this.m4027637499 = mapContainer.m4027637499;
        this.LowestWalkableHeight = mapContainer.LowestWalkableHeight;

        foreach (var chunk in mapContainer.Chunks)
        {
            this.Chunks.Add(
                BinHashtableService.ResolveHash(chunk.Key),
                BinHashtableService.ResolveObjectHash(chunk.Value)
            );
        }
    }
}

internal class MapPlaceableContainerPackage
{
    public Dictionary<string, MapPlaceableBase> Items;

    public MapPlaceableContainerPackage(MetaClass.MapPlaceableContainer mapPlaceableContainer)
    {
        this.Items =
            mapPlaceableContainer
                .Items?.Select(x => new KeyValuePair<string, MapPlaceableBase>(
                    BinHashtableService.ResolveHash(x.Key),
                    x.Value switch
                    {
                        MetaClass.MapParticle mapParticle => new MapParticlePackage(mapParticle),
                        MetaClass.MapLocator mapLocator => new MapLocatorPackage(mapLocator),
                        MetaClass.MapAudio mapAudio => new MapAudioPackage(mapAudio),
                        MetaClass.MapPlaceable mapPlaceable => new MapPlaceableBase(mapPlaceable),
                        _ => null,
                    }
                ))
                .ToDictionary() ?? [];
    }
}

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
    public Vector4 ColorModulate { get; set; } = mapParticle.ColorModulate;
    public bool EyeCandy { get; set; } = mapParticle.EyeCandy;
    public string GroupName { get; set; } = mapParticle.GroupName;
    public uint m1364240033 { get; set; } = mapParticle.m1364240033;
    public bool m840895505 { get; set; } = mapParticle.m840895505;
    public int Quality { get; set; } = mapParticle.Quality;
    public bool StartDisabled { get; set; } = mapParticle.StartDisabled;
    public uint System { get; set; } = mapParticle.System;
    public bool Transitional { get; set; } = mapParticle.Transitional;
    public uint VisibilityMode { get; set; } = mapParticle.VisibilityMode;
}

internal class MapAudioPackage(MetaClass.MapAudio mapAudio) : MapPlaceableBase(mapAudio)
{
    public uint AudioType { get; set; } = mapAudio.AudioType;
    public string EventName { get; set; } = mapAudio.EventName;
    public string m1364240033 { get; set; } =
        BinHashtableService.ResolveObjectHash(mapAudio.m1364240033);
    public float m3743124039 { get; set; } = mapAudio.m3743124039;
    public float StartTime { get; set; } = mapAudio.StartTime;
}

internal class GdsMapObjectPackage(MetaClass.GdsMapObject gdsMapObject)
    : MapPlaceableBase(gdsMapObject)
{
    public Vector3 BoxMin { get; set; } = gdsMapObject.BoxMin;
    public Vector3 BoxMax { get; set; } = gdsMapObject.BoxMax;
    public List<GdsMapObjectExtraInfoPackage> ExtraInfo { get; set; } =
        gdsMapObject
            .ExtraInfo.Select<MetaClass.GDSMapObjectExtraInfo, GdsMapObjectExtraInfoPackage>(x =>
                x switch
                {
                    MetaClass.GDSMapObjectAnimationInfo gdsMapObjectAnimationInfo
                        => new GdsMapObjectAnimationInfoPackage(gdsMapObjectAnimationInfo),
                    MetaClass.GDSMapObjectBannerInfo bannerInfo
                        => new GdsMapObjectBannerInfoPackage(bannerInfo),
                    _ => null
                }
            )
            .ToList();
    public bool EyeCandy { get; set; } = gdsMapObject.EyeCandy;
    public bool IgnoreCollisionOnPlacement { get; set; } = gdsMapObject.IgnoreCollisionOnPlacement;
    public string m1364240033 { get; set; } =
        BinHashtableService.ResolveObjectHash(gdsMapObject.m1364240033);
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
        BinHashtableService.ResolveObjectHash(bannerInfo.BannerData);
}
