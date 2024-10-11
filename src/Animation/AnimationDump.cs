using LeagueToolkit.Core.Animation;
using System.Numerics;

namespace lol_convert.Animation;

public class AnimationDump
{
    public static void DumpAnimationAsset(string location, IAnimationAsset asset)
    {
        using BinaryWriter bw = new(File.Create(location));

        bw.Write("ANIMDUMP"u8);
        bw.Write(1); // Version
        bw.Write(asset.Fps);
        bw.Write(asset.Duration);

        int frameCount = (int)(asset.Fps * asset.Duration);
        bw.Write(frameCount);

        Dictionary<uint, (Quaternion Rotation, Vector3 Translation, Vector3 Scale)> pose = [];
        float frameDuration = 1.0f / asset.Fps;

        for (int frameId = 0; frameId < frameCount; frameId++)
        {
            var frameTime = frameId * frameDuration;
            asset.Evaluate(frameTime, pose);

            bw.Write(pose.Count); // joint count
            bw.Write(frameTime);

            foreach (var (jointId, (rotation, translation, scale)) in pose)
            {
                bw.Write(jointId); // joint name hash
                bw.Write(translation.X);
                bw.Write(translation.Y);
                bw.Write(translation.Z);
                bw.Write(rotation.X);
                bw.Write(rotation.Y);
                bw.Write(rotation.Z);
                bw.Write(rotation.W);
                bw.Write(scale.X);
                bw.Write(scale.Y);
                bw.Write(scale.Z);
            }
        }
    }
}
