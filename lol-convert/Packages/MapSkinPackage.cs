using System.Numerics;
using lol_convert.Services;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

internal class MapSkinPackage
{
    public string Name { get; set; }
    public MapContainerPackage Container { get; set; }
    public Dictionary<string, StaticMaterialPackage> StaticMaterials { get; set; }

    public Dictionary<string, string> MeshVisibilityControllerResolver { get; set; }
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
                        MetaClass.GdsMapObject gdsMapObject
                            => new GdsMapObjectPackage(gdsMapObject),
                        MetaClass.MapPlaceable mapPlaceable => new MapPlaceableBase(mapPlaceable),
                        _ => null,
                    }
                ))
                .ToDictionary() ?? [];
    }
}

internal class ChildMapVisibilityControllerPackage
{

}