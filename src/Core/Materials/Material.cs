using System;
using System.Numerics;
using Raytracer.Core.Hitables;

namespace Raytracer.Core.Materials
{
    abstract class Material
    {
        public abstract bool Scatter(Ray rayIn, ref HitRecord rec, out Vector3 attenuation, out Ray scattered);

        public static Vector3 refract(Vector3 uv, Vector3 n, double refractionRatio)
        {
            var cosTheta = Math.Min(Vector3.Dot(-uv, n), 1.0);
            Vector3 rayOutPerpendicular = (float)refractionRatio * (uv + (float)cosTheta * n);
            Vector3 rayOutParallel = -(float)Math.Sqrt(Math.Abs(1.0f - rayOutPerpendicular.LengthSquared())) * n;
            return rayOutPerpendicular + rayOutParallel;
        }

        public static Vector3 reflect(Vector3 v, Vector3 n)
        {
            return v - 2 * Vector3.Dot(v, n) * n;
        }
    }
}
