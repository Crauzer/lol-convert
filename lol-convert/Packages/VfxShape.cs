using System.Numerics;
using System.Text.Json.Serialization;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

[JsonDerivedType(typeof(VfxShape0xee39916f), "0xee39916f")]
[JsonDerivedType(typeof(VfxShapeLegacy), "legacy")]
[JsonDerivedType(typeof(VfxShapeBox), "box")]
[JsonDerivedType(typeof(VfxShapeCylinder), "cylinder")]
[JsonDerivedType(typeof(VfxShapeSphere), "sphere")]
internal class VfxShapeBase(MetaClass.IVfxShape shape) { }

internal class VfxShape0xee39916f(MetaClass.Class0xee39916f shape) : VfxShapeBase(shape)
{
    public Vector3 EmitOffset { get; set; } = shape.EmitOffset;
}

internal class VfxShapeLegacy(MetaClass.VfxShapeLegacy shape) : VfxShapeBase(shape)
{
    public VfxVector3 EmitOffset { get; set; } =
        shape.EmitOffset is null ? null : new(shape.EmitOffset);
    public List<VfxFloat> EmitRotationAngles { get; set; } =
        shape.EmitRotationAngles?.Select(x => new VfxFloat(x)).ToList();
    public List<Vector3> EmitRotationAxes { get; set; } = shape.EmitRotationAxes?.ToList();
}

internal class VfxShapeVolume(MetaClass.VfxShapeVolume shape) : VfxShapeBase(shape)
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
