using Raytracer.Core;
using Raytracer.Hitables;
using Raytracer.Textures;
using Raytracer.Utility;
using Raytracer.Materials;
using OpenTK.Mathematics;

namespace Raytracer.Scenes
{
    public partial class Scene
    {
        public static void BookScene(ref double _aspectRatio, ref Vector3d background, out ObjectList world, out Camera camera)
        {
            Vector3d lookfrom = new(8, 1.5, 2);
            Vector3d lookat = new(0, 0.4, -0.25);
            Vector3d vup = new(0, 1, 0);
            var focusDist = 10;
            var aperture = 0.0;
            var fov = 27;

            camera = new(lookfrom, lookat, vup, fov, _aspectRatio, aperture, focusDist);

            // World
            world = new();
            background = new Vector3d(0);

            // Skybox
            var tSkybox = new ImageTexture(@"..\Textures\HDRI Maps\sunflowers.jpg", 1, 0, 1);
            var mSkybox = new Light(tSkybox);
            var hSkybox = new Sphere(new Vector3d(0, -10, 0), 100, mSkybox);
            world.Add(hSkybox);

            // Floor
            var checkerTexture = new CheckerTexture(Vector3d.Zero, Vector3d.One);
            world.Add(new Sphere(new Vector3d(0.0f, -1000, 0.0), 1000.0, new Lambertian(checkerTexture)));

            // Spheres
            for (int a = -11; a < 11; a++)
            {
                for (int b = -11; b < 11; b++)
                {
                    var materialType = RandomHelper.RandomDouble();
                    Vector3d center = new(a + 0.9 * (float)RandomHelper.RandomDouble(), 0.2, b + 0.9 * (float)RandomHelper.RandomDouble());

                    if ((center - new Vector3d(4, 0.2, 0)).Length > 0.9)
                    {
                        IMaterial material;

                        if (materialType < 0.8)
                        {
                            // diffuse
                            var albedo = Vector3Helper.RandomVec3();
                            material = new Lambertian(albedo);
                            world.Add(new Sphere(center, 0.2, material));
                        }
                        else if (materialType < 0.95)
                        {
                            // metal
                            var albedo = Vector3Helper.RandomVec3();
                            var fuzz = RandomHelper.RandomDouble();
                            material = new Metal(albedo, fuzz);
                            world.Add(new Sphere(center, 0.2, material));
                        }
                        else
                        {
                            // glass
                            material = new Dielectric(1.5);
                            world.Add(new Sphere(center, 0.2, material));
                        }
                    }
                }
            }

            var glass = new Dielectric(1.5);
            var lambert = new Lambertian(new Vector3d(0.4, 0.2, 0.1));
            var metal = new Metal(new Vector3d(0.7, 0.6, 0.5), 0.1);

            world.Add(new Sphere(new Vector3d(-4, 1, 0), 1.0, lambert));
            world.Add(new Sphere(new Vector3d(0, 0.75, 0), 0.75, glass));
            world.Add(new Sphere(new Vector3d(4, 0.5, 0), 0.5, metal));
        }
    }
}
