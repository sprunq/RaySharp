using System;
using System.Numerics;
using Raytracer.Core.Hitables;
using Raytracer.Utility;

namespace Raytracer.Core
{
    class Ray
    {
        public Vector3 Origin;
        public Vector3 Direction;

        public Ray() { }
        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public Vector3 At(double t)
        {
            return Origin + Vector3.Multiply((float)t, Direction);
        }

        public static Vector3 RayColor(Ray ray, Hitable world, int depth)
        {
            HitRecord rec = new();

            if (depth <= 0)
            {
                return new Vector3(0, 0, 0);
            }

            if (world.Hit(ray, 0.001, Double.PositiveInfinity, ref rec))
            {
                Ray scattered = new();
                Vector3 attenuation = new();
                if (rec.material.Scatter(ray, ref rec, out attenuation, out scattered))
                {
                    return attenuation * RayColor(scattered, world, depth - 1);
                }
                return new Vector3(0, 0, 0);
            }

            Vector3 unitDirection = Vector3.Normalize(ray.Direction);
            var t = 0.5f * (unitDirection.Y + 1.0f);
            return (float)(1.0 - t) * new Vector3(1.0f, 1.0f, 1.0f) + (float)t * new Vector3(0.5f, 0.7f, 1.0f);
        }

        public static Vector3 GetColor(Vector3 color, int samples)
        {
            var r = color.X;
            var g = color.Y;
            var b = color.Z;

            var scale = 1.0f / samples;
            r = (float)Math.Sqrt(scale * r);
            g = (float)Math.Sqrt(scale * g);
            b = (float)Math.Sqrt(scale * b);

            int ir = (int)(256 * GeneralHelper.Clamp(r, 0.0, 0.999));
            int ig = (int)(256 * GeneralHelper.Clamp(g, 0.0, 0.999));
            int ib = (int)(256 * GeneralHelper.Clamp(b, 0.0, 0.999));

            return new Vector3(ir, ig, ib);
        }
    }
}
