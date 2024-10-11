using MetaClass = LeagueToolkit.Meta.Classes;

namespace lol_convert.Packages;

internal class VfxDistortion(MetaClass.VfxDistortionDefinitionData distortion)
{
    public float Distortion { get; set; } = distortion.Distortion;
    public byte DistortionMode { get; set; } = distortion.DistortionMode;
    public string NormalMapTexture { get; set; } = distortion.NormalMapTexture;
}
