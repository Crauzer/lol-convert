﻿using System.Numerics;
using System.Text.Json.Serialization;
using LeagueToolkit.Core.Environment;
using lol_convert.Services;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

internal class MapSkinPackage
{
    public string Name { get; set; }
    public string AssetPath { get; set; }
    public MapContainerPackage Container { get; set; }
    public Dictionary<string, VfxSystem> VfxSystems { get; set; }
    public Dictionary<string, StaticMaterialPackage> StaticMaterials { get; set; }
    public List<MapShaderTextureOverridePackage> ShaderTextureOverrides { get; set; } = [];
    public Dictionary<string, MapMeshPackage> Meshes { get; set; }
    public Dictionary<string, MapVisibilityControllerBase> VisibilityControllers { get; set; }
    public Dictionary<string, MapPlaceableContainerPackage> Chunks { get; set; }
}

internal class MapMeshPackage
{
    public string VisibilityController { get; set; }
    public byte LegacyVisibilityFlags { get; set; }

    public bool DisableBackfaceCulling { get; set; }
    public byte QualityFilter { get; set; }
    public byte LayerTransitionBehavior { get; set; }
    public ushort RenderFlags { get; set; }

    public MapMeshChannelPackage StationaryLight { get; set; }
    public MapMeshChannelPackage BakedLight { get; set; }
    public List<MapMeshTextureOverridePackage> TextureOverrides { get; set; } = [];
    public Vector2 BakedPaintScale { get; set; }
    public Vector2 BakedPaintBias { get; set; }
}

internal class MapMeshTextureOverridePackage(EnvironmentAssetMeshTextureOverride textureOverride)
{
    public int Index { get; set; } = textureOverride.Index;
    public string Texture { get; set; } = textureOverride.Texture;
}

internal class MapShaderTextureOverridePackage(EnvironmentAssetShaderTextureOverride textureOverride)
{
    public int Index { get; set; } = textureOverride.Index;
    public string Name { get; set; } = textureOverride.Name;
}

internal class MapMeshChannelPackage(EnvironmentAssetChannel channel)
{
    public string Texture { get; set; } = channel.Texture;
    public Vector2 Scale { get; set; } = channel.Scale;
    public Vector2 Bias { get; set; } = channel.Bias;
}

internal class MapContainerPackage
{
    public string MapPath { get; set; }
    public Vector2? BoundsMin { get; set; }
    public Vector2? BoundsMax { get; set; }
    public float LowestWalkableHeight { get; set; }

    public Dictionary<string, string> Chunks { get; set; } = [];

    public MapContainerPackage(MetaClass.MapContainer mapContainer)
    {
        this.MapPath = mapContainer.MapPath;
        this.BoundsMin = mapContainer.BoundsMin;
        this.BoundsMax = mapContainer.BoundsMax;
        this.LowestWalkableHeight = mapContainer.LowestWalkableHeight;

        foreach (var chunk in mapContainer.Chunks)
        {
            this.Chunks.Add(
                BinHashtableService.ResolveHash(chunk.Key),
                BinHashtableService.ResolveObjectLink(chunk.Value)
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
                        MetaClass.GdsMapObject gdsMapObject
                            => new GdsMapObjectPackage(gdsMapObject),
                        MetaClass.MapPlaceable mapPlaceable => new MapPlaceableBase(mapPlaceable),
                        _ => null,
                    }
                ))
                .ToDictionary() ?? [];
    }
}

[JsonDerivedType(typeof(ChildMapVisibilityControllerPackage), "child_controller")]
[JsonDerivedType(typeof(LegacyMapVisibilityControllerPackage), "legacy_visflags_controller")]
[JsonDerivedType(typeof(Parent2MapVisibilityControllerPackage), "parent2_controller")]
[JsonDerivedType(typeof(Parent1MapVisibilityControllerPackage), "parent1_controller")]
internal class MapVisibilityControllerBase(MetaClass.IMapVisibilityController controller)
{
    public string Path { get; set; } = BinHashtableService.ResolveObjectLink(controller.PathHash);
}

internal class ChildMapVisibilityControllerPackage(
    MetaClass.ChildMapVisibilityController controller
) : MapVisibilityControllerBase(controller)
{
    public uint ParentMode { get; set; } = controller.ParentMode;
    public List<string> Parents { get; set; } =
        controller.Parents.Select(x => BinHashtableService.ResolveObjectLink(x)).ToList();
}

internal class LegacyMapVisibilityControllerPackage(MetaClass.Class0x6b863734 controller)
    : MapVisibilityControllerBase(controller)
{
    public byte VisibilityFlags { get; set; } = controller.VisFlags;
}

internal class Parent1MapVisibilityControllerPackage(MetaClass.Class0xe07edfa4 controller)
    : MapVisibilityControllerBase(controller)
{
    public bool DefaultVisible { get; set; } = controller.DefaultVisible;
    public string Name { get; set; } = BinHashtableService.ResolveHash(controller.Name);
}

internal class Parent2MapVisibilityControllerPackage(MetaClass.Class0xec733fe2 controller)
    : Parent1MapVisibilityControllerPackage(controller)
{
    public byte m2348780767 { get; set; } = controller.m2348780767;
}
