using System;
using System.IO;
using System.Numerics;
using Raytracer.Core;
using Raytracer.Core.Hittables;
using Raytracer.Common;

namespace Raytracer
{
    class Program
    {
        private static Vector3 ray_color(Ray r, Hittable world, int depth)
        {
            HitRecord rec = new();

            if (depth <= 0)
            {
                return new Vector3(0, 0, 0);
            }

            if (world.hit(r, 0.001, Double.PositiveInfinity, ref rec))
            {
                Ray scattered = new();
                Vector3 attenuation = new();
                if (rec.material.scatter(r, ref rec, out attenuation, out scattered))
                {
                    return attenuation * ray_color(scattered, world, depth - 1);
                }
                return new Vector3(0, 0, 0);
            }

            Vector3 unit_direction = Vector3.Normalize(r.dir);
            var t = 0.5f * (unit_direction.Y + 1.0f);
            return (float)(1.0 - t) * new Vector3(1.0f, 1.0f, 1.0f) + (float)t * new Vector3(0.5f, 0.7f, 1.0f);
        }

        static void Main(string[] args)
        {
            // Image
            const float aspect_ratio = 16.0f / 9.0f;
            const int image_width = 1080;
            const int image_height = (int)(image_width / aspect_ratio);
            const int samples = 150;
            const int max_depth = 50;

            // World
            HittableList world = new();

            var material_ground = new Lambertian(new Vector3(0.8f, 0.8f, 0.0f));
            var material_center = new Lambertian(new Vector3(0.7f, 0.3f, 0.3f));
            var material_left = new Metal(new Vector3(0.8f, 0.8f, 0.8f), 0.3);
            var material_right = new Metal(new Vector3(0.8f, 0.6f, 0.2f), 1.0);

            world.Add(new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0, material_ground));
            world.Add(new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5, material_center));
            world.Add(new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), 0.5, material_left));
            world.Add(new Sphere(new Vector3(1.0f, 0.0f, -1.0f), 0.5, material_right));


            // Camera
            Camera camera = new();

            var sw = new StreamWriter("out.ppm");

            sw.WriteLine($"P3\n{image_width} {image_height} \n255");

            for (var j = image_height - 1; j >= 0; --j)
            {
                Console.Error.WriteLine($"\rScanlines remaining {j}");

                for (var i = 0; i < image_width; ++i)
                {
                    Vector3 color = new Vector3(0, 0, 0);
                    for (int s = 0; s < samples; ++s)
                    {
                        var u = (i + Helpers.randomDouble()) / (image_width - 1);
                        var v = (j + Helpers.randomDouble()) / (image_height - 1);
                        Ray r = camera.GetRay(u, v);
                        color += ray_color(r, world, max_depth);
                    }
                    Color.write_color(sw, color, samples);
                }
            }
            Console.Error.WriteLine("Done");

        }
    }
}
