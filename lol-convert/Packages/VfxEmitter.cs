using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

internal class VfxEmitter(MetaClass.VfxEmitterDefinitionData emitter)
{
    public VfxVector3 Acceleration { get; set; } = new(emitter.Acceleration);
    public VfxAlphaErosion AlphaErosion { get; set; } = new(emitter.AlphaErosionDefinition);
    public byte AlphaRef { get; set; } = emitter.AlphaRef;
    public VfxEmitterAudio Audio { get; set; } = new(emitter.Audio);
    public VfxFloat BindWeight { get; set; } = new(emitter.BindWeight);
    public VfxVector3 BirthAcceleration { get; set; } = new(emitter.BirthAcceleration);
    public VfxColor BirthColor { get; set; } = new(emitter.BirthColor);
    public VfxVector3 BirthDrag { get; set; } = new(emitter.BirthDrag);
    public VfxFloat BirthFrameRate { get; set; } = new(emitter.BirthFrameRate);
    public VfxVector3 BirthOrbitalVelocity { get; set; } = new(emitter.BirthOrbitalVelocity);
    public VfxVector3 BirthRotation0 { get; set; } = new(emitter.BirthRotation0);
    public VfxVector3 BirthRotationalAcceleration { get; set; } = new(emitter.BirthRotationalAcceleration);
    public VfxVector3 BirthRotationalVelocity0 { get; set; } = new(emitter.BirthRotationalVelocity0);
    public VfxVector3 BirthScale0 { get; set; } = new(emitter.BirthScale0);
    public VfxVector2 BirthUvOffset { get; set; } = new(emitter.BirthUvoffset);
    public VfxFloat BirthUvRotateRate { get; set; } = new(emitter.BirthUvRotateRate);
    public VfxVector2 BirthUvScrollRate { get; set; } = new(emitter.BirthUvScrollRate);
    public VfxVector3 BirthVelocity { get; set; } = new(emitter.BirthVelocity);
    public byte BlendMode { get; set; } = emitter.BlendMode;
    public Vector4 CensorModulateValue { get; set; } = emitter.CensorModulateValue;
}

internal class VfxAlphaErosion(MetaClass.VfxAlphaErosionDefinitionData alphaErosion)
{

}

internal class VfxEmitterAudio(MetaClass.VfxEmitterAudio emitterAudio)
{

}
