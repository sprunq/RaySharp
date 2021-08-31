using Raytracer.Core;
using Raytracer.Core.Hitables;
using Raytracer.Core.Textures;
using OpenTK.Mathematics;

namespace Raytracer.Scenes
{
    partial class Scene
    {
        public static void MixedScene(ref double _aspectRatio, ref Vector3d background, out ObjectList world, out Camera camera)
        {
            Vector3d lookfrom = new(-10, 3, 0);
            Vector3d lookat = new(0, 2.5, 0);
            Vector3d vup = new(0, 1, 0);
            var focusDist = 10;
            var aperture = 0.0;
            var fov = 90;

            camera = new(lookfrom, lookat, vup, fov, _aspectRatio, aperture, focusDist);

            // World
            world = new();
            background = new Vector3d(0);

            // Skybox
            var tSkybox = new ImageTexture(@"..\Textures\HDRI Maps\sunflowers.jpg", 1, 0);
            var mSkybox = new Light(tSkybox);
            var hSkybox = new Sphere(new Vector3d(0, -15, 0), 1000, mSkybox);
            world.Add(hSkybox);

            // Ground
            var tWood = new ImageTexture(@"..\Textures\wood_planks.jpg", 1000, 0);
            var mWood = new Lambertian(tWood);
            var hground = new XZRect(new Vector2d(-1000, 1000), new Vector2d(-1000, 1000), 0, mWood);
            world.Add(hground);


            // Box stands
            var mBox1 = new Lambertian(new Vector3d(1));
            var hBox1 = new Box(new Vector3d(-3, 0, -3), new Vector3d(-2, 1, -2), mBox1);
            world.Add(hBox1);


        }
    }
}
