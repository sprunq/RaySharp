using System;
using System.Numerics;
using OpenTK.Mathematics;
using Raytracer.Core.Hitables;
using Raytracer.Utility;

namespace Raytracer.Core
{
    class Ray
    {
        public Vector3d Origin;
        public Vector3d Direction;

        public Ray() { }
        public Ray(Vector3d origin, Vector3d direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public Vector3d At(double t)
        {
            return Origin + Vector3d.Multiply(Direction, t);
        }

        public static Vector3d RayColor(Ray ray, Hitable world, int depth)
        {
            HitRecord rec = new();

            if (depth <= 0)
            {
                return new Vector3d(0, 0, 0);
            }

            if (world.Hit(ray, 0.001, Double.PositiveInfinity, ref rec))
            {
                Ray scattered = new();
                Vector3d attenuation = new();
                if (rec.material.Scatter(ray, ref rec, out attenuation, out scattered))
                {
                    return attenuation * RayColor(scattered, world, depth - 1);
                }
                return new Vector3d(0, 0, 0);
            }

            Vector3d unitDirection = Vector3d.Normalize(ray.Direction);
            var t = 0.5f * (unitDirection.Y + 1.0f);
            return (1.0 - t) * new Vector3d(1.0f, 1.0f, 1.0f) + t * new Vector3d(0.5f, 0.7f, 1.0f);
        }

        public static Vector3d GetColor(Vector3d color, int samples)
        {
            var r = color.X;
            var g = color.Y;
            var b = color.Z;

            var scale = 1.0f / samples;
            r = Math.Sqrt(scale * r);
            g = Math.Sqrt(scale * g);
            b = Math.Sqrt(scale * b);

            int ir = (int)(256 * GeneralHelper.Clamp(r, 0.0, 0.999));
            int ig = (int)(256 * GeneralHelper.Clamp(g, 0.0, 0.999));
            int ib = (int)(256 * GeneralHelper.Clamp(b, 0.0, 0.999));

            return new Vector3d(ir, ig, ib);
        }
    }
}
