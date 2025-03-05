using LeagueToolkit.Meta.Classes;
using System.Numerics;
using System.Text.Json.Serialization;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

[JsonDerivedType(typeof(VfxShape0xee39916f), "0xee39916f")]
[JsonDerivedType(typeof(VfxShapeLegacy), "legacy")]
[JsonDerivedType(typeof(VfxShapeBox), "box")]
[JsonDerivedType(typeof(VfxShapeCylinder), "cylinder")]
[JsonDerivedType(typeof(VfxShapeSphere), "sphere")]
internal class VfxShapeBase 
{
    public static VfxShapeBase FromMeta(MetaClass.IVfxShape shape)
    {
        return shape switch
        {
            MetaClass.Class0xee39916f unk1 => new VfxShape0xee39916f(unk1),
            MetaClass.VfxShapeLegacy legacy => new VfxShapeLegacy(legacy),
            MetaClass.VfxShapeBox box => new VfxShapeBox(box),
            MetaClass.VfxShapeCylinder cylinder => new VfxShapeCylinder(cylinder),
            MetaClass.VfxShapeSphere sphere => new VfxShapeSphere(sphere),
            _ => throw new NotImplementedException("Unknown vfx shape"),
        };
    }
}

internal class VfxShape0xee39916f(MetaClass.Class0xee39916f shape) : VfxShapeBase
{
    public Vector3 EmitOffset { get; set; } = shape.EmitOffset;
}

internal class VfxShapeLegacy(MetaClass.VfxShapeLegacy shape) : VfxShapeBase
{
    public VfxVector3 EmitOffset { get; set; } =
        shape.EmitOffset is null ? null : new(shape.EmitOffset);
    public List<VfxFloat> EmitRotationAngles { get; set; } =
        shape.EmitRotationAngles?.Select(x => new VfxFloat(x)).ToList();
    public List<Vector3> EmitRotationAxes { get; set; } = shape.EmitRotationAxes?.ToList();
}

internal class VfxShapeVolume(MetaClass.VfxShapeVolume shape) : VfxShapeBase
{
    public byte Flags { get; set; } = shape.Flags;
}

internal class VfxShapeBox(MetaClass.VfxShapeBox shape) : VfxShapeVolume(shape)
{
    public Vector3 Size { get; set; } = shape.Size;
}

internal class VfxShapeCylinder(MetaClass.VfxShapeCylinder shape) : VfxShapeVolume(shape)
{
    public float Height { get; set; } = shape.Height;
    public float Radius { get; set; } = shape.Radius;
}

internal class VfxShapeSphere(MetaClass.VfxShapeSphere shape) : VfxShapeVolume(shape)
{
    public float Radius { get; set; } = shape.Radius;
}
