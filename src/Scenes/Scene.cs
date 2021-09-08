using Raytracer.Hitables;
using Raytracer.Materials;
using Raytracer.Instances;
using OpenTK.Mathematics;

namespace Raytracer.Scenes
{
    public partial class Scene
    {
        public static void BoxSpherePillar(double boxWidth, double boxHeight, double sphereSize, double angle, Vector3d offset, Material mBox, Material mSphere, ref ObjectList world)
        {
            var hBox = new Box(new Vector3d(-boxWidth / 2.0, 0, -boxWidth / 2.0), new Vector3d(boxWidth / 2.0, boxHeight, boxWidth / 2.0), mBox);
            var roBox = new Rotate(hBox, angle, Axis.Y);
            var trBox = new Translate(roBox, new Vector3d(-boxWidth, 0, 0) + offset);
            world.Add(trBox);

            var hSphere = new Sphere(new Vector3d(0, sphereSize, 0), sphereSize, mSphere);
            var roSphere = new Rotate(hSphere, angle, Axis.Y);
            var trSphere = new Translate(roSphere, new Vector3d(-boxWidth, boxHeight, 0) + offset);
            world.Add(trSphere);
        }
    }
}
