using Raytracer.Core;
using Raytracer.Hitables;
using Raytracer.Textures;
using OpenTK.Mathematics;
using Raytracer.Utility;
using Raytracer.Materials;
using System.Collections.Generic;
using Raytracer.Instances;

namespace Raytracer.Scenes
{
    public partial class Scene
    {
        public static void Book2Scene(ref double _aspectRatio, ref Vector3d background, out ObjectList world, out Camera camera)
        {
            // Need to add other materials
            Vector3d lookfrom = new(478, 278, -600);
            Vector3d lookat = new(278, 278, 0);
            Vector3d vup = new(0, 1, 0);
            var focusDist = 10;
            var aperture = 0.0;
            var fov = 40;

            camera = new(lookfrom, lookat, vup, fov, _aspectRatio, aperture, focusDist);

            background = new Vector3d(0);
            world = new();

            var ground = new Lambertian(new Vector3d(0.48, 0.83, 0.53));
            const int boxes_per_side = 20;
            List<Hitable> boxes1 = new();

            for (int i = 0; i < boxes_per_side; i++)
            {
                for (int j = 0; j < boxes_per_side; j++)
                {
                    var w = 100.0;
                    var x0 = -1000.0 + i * w;
                    var z0 = -1000.0 + j * w;
                    var y0 = 0.0;
                    var x1 = x0 + w;
                    var y1 = RandomHelper.RandomDouble(1, 101);
                    var z1 = z0 + w;

                    boxes1.Add(new Box(new Vector3d(x0, y0, z0), new Vector3d(x1, y1, z1), ground));
                }
            }

            foreach (var box in boxes1)
            {
                world.Add(box);
            }

            var light = new Light(new Vector3d(7, 7, 7));
            world.Add(new XZRect(new Vector2d(123, 423), new Vector2d(147, 412), 554, light));

            world.Add(new Sphere(new Vector3d(260, 150, 45), 50, new Dielectric(1.5)));
            world.Add(new Sphere(
                new Vector3d(0, 150, 145), 50, new Metal(new Vector3d(0.8, 0.8, 0.9), 1.0)
            ));

            var boundary = new Sphere(new Vector3d(360, 150, 145), 70, new Dielectric(1.5));
            world.Add(boundary);

            List<Hitable> boxes2 = new();
            var white = new Lambertian(new Vector3d(.73, .73, .73));
            int ns = 1000;
            for (int j = 0; j < ns; j++)
            {
                boxes2.Add(new Sphere(Vector3Helper.RandomVec3(0, 165), 10, white));
            }

            foreach (var box in boxes2)
            {
                world.Add(
                    new Translate(
                        new Rotate(box, 15, Axis.Y),
                        new Vector3d(-100, 270, 395)));
            }
        }
    }
}
