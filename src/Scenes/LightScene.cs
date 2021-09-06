using Raytracer.Core;
using Raytracer.Hitables;
using Raytracer.Textures;
using OpenTK.Mathematics;

namespace Raytracer.Scenes
{
    public partial class Scene
    {
        public static void LightScene(ref double _aspectRatio, ref Vector3d background, out ObjectList world, out Camera camera)
        {
            _aspectRatio = 3.0 / 2.0;
            Vector3d lookfrom = new Vector3d(-10, 2, 0);
            Vector3d lookat = new(0, 1.8, 0);
            Vector3d vup = new(0, 1, 0);
            var focusDist = 1;
            var aperture = 0.0;
            var fov = 40;

            camera = new(lookfrom, lookat, vup, fov, _aspectRatio, aperture, focusDist);

            // World
            world = new();

            // Skybox
            var tSkybox = new ImageTexture(@"..\Textures\HDRI Maps\hilly_terrain.jpg", 1, 0, 1);
            var mSkybox = new Light(tSkybox);
            var hSkybox = new Sphere(new Vector3d(0, 0, 0), 10000, mSkybox);
            world.Add(hSkybox);

            // Ground
            var tWood = new ImageTexture(@"..\Textures\wood_planks.jpg", 1000, 0, 1);
            var mWood = new Lambertian(tWood);
            var hground = new XZRect(new Vector2d(-1000, 1000), new Vector2d(-1000, 1000), 0, mWood);
            world.Add(hground);

            // Lights
            var mlightS1 = new Light(new Vector3d(5, 0, 0));
            var hLightS1 = new Sphere(new Vector3d(-2.6, 0.3, 2.5), 0.3, mlightS1);
            world.Add(hLightS1);

            var mlightS2 = new Light(new Vector3d(0, 0, 5));
            var hLightS2 = new Sphere(new Vector3d(-3.2, 0.3, 1.6), 0.3, mlightS2);
            world.Add(hLightS2);

            var mLightSmall = new Light(new Vector3d(5));
            var hLight1 = new Sphere(new Vector3d(-2.4, 0.2, -2.35), 0.2, mLightSmall);
            world.Add(hLight1);

            // Spheres
            var tEarth = new ImageTexture(@"..\Textures\earthmap8k.jpg", 1, 4000, 1);
            var mEarth = new Lambertian(tEarth);
            var hEarth = new Sphere(new Vector3d(0, 2, 0), 2, mEarth);
            world.Add(hEarth);

            var mMetalBlue = new Metal(new Vector3d(1, 1, 1), 0);
            var hMetalSphereBlue = new Sphere(new Vector3d(-2.2, 0.35, -1.6), 0.35, mMetalBlue);
            world.Add(hMetalSphereBlue);

            var mMetalRed = new Metal(new Vector3d(0.6, 0.6, 0.6), 0.4);
            var hMetalSphereRed = new Sphere(new Vector3d(-1.6, 0.45, -2.3), 0.45, mMetalRed);
            world.Add(hMetalSphereRed);

            var mGlass = new Dielectric(1.5);
            var hGlassSphere = new Sphere(new Vector3d(-2.9, 0.35, 0.8), 0.35, mGlass);
            world.Add(hGlassSphere);

            var mlight = new Lambertian(new Vector3d(0.5, 0.7, 0.2));
            var hLight = new Sphere(new Vector3d(-2.3, 0.7, 1.6), 0.7, mlight);
            world.Add(hLight);
        }
    }
}
