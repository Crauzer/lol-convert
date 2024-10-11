using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

internal class VfxPrimitiveMeshBase(MetaClass.VfxPrimitiveMeshBase mesh) : VfxPrimitiveBase(mesh)
{
    public bool AlignPitchToCamera { get; set; } = mesh.AlignPitchToCamera;
    public bool AlignYawToCamera { get; set; } = mesh.AlignYawToCamera;
    public bool m1793891962 { get; set; } = mesh.m1793891962;
    public VfxMesh Mesh { get; set; } = mesh.Mesh is null ? null : new(mesh.Mesh);
}

internal class VfxPrimitiveAttachedMesh(MetaClass.VfxPrimitiveAttachedMesh primitive)
    : VfxPrimitiveMeshBase(primitive)
{
    public bool UseAvatarSpecificSubmeshMask { get; set; } = primitive.UseAvatarSpecificSubmeshMask;
}

internal class VfxPrimitiveMesh(MetaClass.VfxPrimitiveMesh primitive)
    : VfxPrimitiveMeshBase(primitive) { }
