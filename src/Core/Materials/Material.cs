using System;
using System.Numerics;
using Raytracer.Core.Hitables;

namespace Raytracer.Core.Materials
{
    abstract class Material
    {
        public abstract bool scatter(Ray r_in, ref HitRecord rec, out Vector3 attenuation, out Ray scattered);

        public static Vector3 refract(Vector3 uv, Vector3 n, double etai_over_etat)
        {
            var cos_theta = Math.Min(Vector3.Dot(-uv, n), 1.0);
            Vector3 r_out_perp = (float)etai_over_etat * (uv + (float)cos_theta * n);
            Vector3 r_out_parallel = -(float)Math.Sqrt(Math.Abs(1.0f - r_out_perp.LengthSquared())) * n;
            return r_out_perp + r_out_parallel;
        }

        public static Vector3 reflect(Vector3 v, Vector3 n)
        {
            return v - 2 * Vector3.Dot(v, n) * n;
        }
    }
}
