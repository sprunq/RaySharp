using Raytracer.Core;
using Raytracer.Hitables;
using Raytracer.Textures;
using Raytracer.Utility;
using Raytracer.Materials;
using Raytracer.Instances;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace Raytracer.Scenes
{
    public partial class Scene
    {
        public static void NightDragon(ref double _aspectRatio, ref Vector3d background, out ObjectList world, out Camera camera)
        {
            Vector3d lookfrom = new(-50, 8, 15);
            Vector3d lookat = new(0, 11.5, 0);
            Vector3d vup = new(0, 1, 0);
            var focusDist = 50;
            var aperture = 0.3;
            var fov = 55;

            camera = new(lookfrom, lookat, vup, fov, _aspectRatio, aperture, focusDist);

            // World
            world = new();
            background = new Vector3d(0);

            // Skybox
            var tSkybox = new ImageTexture(@"..\Textures\HDRI Maps\milkyway.png", 1, 0, 0.2);
            var mSkybox = new Light(tSkybox);
            var hSkybox = new Sphere(new Vector3d(0, -250, 0), 1000, mSkybox);
            var roSkybox = new Rotate(hSkybox, 80, Axis.Y);
            world.Add(roSkybox);

            // Ground
            var tWood = new ImageTexture(@"..\Textures\wood_planks.jpg", 300, 0, 1);
            var mWood = new Lambertian(tWood);
            var hground = new XZRect(new Vector2d(-1000, 1000), new Vector2d(-1000, 1000), 0, mWood);
            world.Add(hground);

            // Models
            // Dragon
            if (true)
            {
                var matDragonL = new Lambertian(new Vector3d(0.6, 0, 0.8));
                var matDragonM = new Metal(new Vector3d(0.7, 0, 1), 0.1);
                var matDragonG = new Dielectric(1.5, new Vector3d(1, 0, 1));
                var hModel = new Mesh(@"..\Models\bunny.obj", matDragonG, 0.2);
                var roXModel = new Rotate(hModel, 0, Axis.X);
                var roYModel = new Rotate(roXModel, 160, Axis.Y);
                var trModel = new Translate(roYModel, new Vector3d(0, 8, 4));
                world.Add(trModel);
            }

            // Eye Sphere
            var mEye = new Light(new Vector3d(0.6, 0, 0.8));
            var hSphereEye = new Sphere(new Vector3d(0, 0, 0), 0.65, mEye);
            var trEye = new Translate(hSphereEye, new Vector3d(-12.63, 13.9, -3.2));
            world.Add(trEye);


            // Spheres
            var spheres = new List<Hitable>();
            int sphereMax = 3;
            for (int a = -200; a < 200; a += sphereMax * 2)
            {
                for (int b = -200; b < 200; b += sphereMax * 2)
                {
                    if (RandomHelper.RandomDouble() > 0.5)
                    {
                        var materialType = RandomHelper.RandomDouble();
                        var sphereSize = RandomHelper.RandomDouble() * sphereMax;
                        Vector3d center = new(a + 0.9 * RandomHelper.RandomDouble(), sphereSize, b + 0.9 * RandomHelper.RandomDouble());
                        Material material;

                        if (Vector3d.Distance(new Vector3d(-95, 0, 100), center) > 22)
                        {
                            if (materialType < 0.4)
                            {
                                // light
                                var albedo = Vector3Helper.RandomVec3();
                                var intensity = RandomHelper.RandomDouble(1, 7);
                                material = new Light(albedo * intensity);
                                spheres.Add(new Sphere(center, sphereSize, material));
                            }
                            else
                            {
                                // metal
                                var albedo = Vector3Helper.RandomVec3();
                                var fuzz = RandomHelper.RandomDouble(0, 0.2);
                                material = new Metal(albedo, fuzz);
                                spheres.Add(new Sphere(center, sphereSize, material));
                            }

                        }
                    }
                }
            }
            var trSpheres = new Translate(new BVHNode(spheres), new Vector3d(100, 0, -100));
            world.Add(trSpheres);
        }
    }
}
