using System;
using System.IO;
using System.Numerics;
using Raytracer.Core.Hitables;

namespace Raytracer.Core
{
    class Color
    {

        public static Vector3 rayColor(Ray r, Hitable world, int depth)
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
                    return attenuation * rayColor(scattered, world, depth - 1);
                }
                return new Vector3(0, 0, 0);
            }

            Vector3 unit_direction = Vector3.Normalize(r.dir);
            var t = 0.5f * (unit_direction.Y + 1.0f);
            return (float)(1.0 - t) * new Vector3(1.0f, 1.0f, 1.0f) + (float)t * new Vector3(0.5f, 0.7f, 1.0f);
        }

        public static Vector3 getColor(Vector3 color, int samples)
        {
            double clamp(double x, double min, double max)
            {
                if (x < min) return min;
                if (x > max) return max;
                return x;
            }

            var r = color.X;
            var g = color.Y;
            var b = color.Z;

            var scale = 1.0f / samples;
            r = (float)Math.Sqrt(scale * r);
            g = (float)Math.Sqrt(scale * g);
            b = (float)Math.Sqrt(scale * b);

            int ir = (int)(256 * clamp(r, 0.0, 0.999));
            int ig = (int)(256 * clamp(g, 0.0, 0.999));
            int ib = (int)(256 * clamp(b, 0.0, 0.999));

            return new Vector3(ir, ig, ib);
        }

        public static void writeColor(TextWriter output, Vector3 color)
        {
            output.WriteLine($"{color.X} {color.Y} {color.Z}");
            output.Flush();
        }

        public static void writeArrayToPPM(TextWriter output, Vector3[,] array)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    output.WriteLine($"{array[i, j].X} {array[i, j].Y} {array[i, j].Z}");
                    output.Flush();
                }
            }
        }

        public static Vector3 RgbNormalizedVector(int r, int g, int b)
        {
            return new Vector3(255.0f / r, 255.0f / g, 255.0f / b);
        }

        public static Vector3 randomColor()
        {
            Random rand = new();
            return new Vector3(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
        }

    }
}
