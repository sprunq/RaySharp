using System;
using System.Numerics;
using OpenTK.Mathematics;
using Raytracer.Core.Hitables;

namespace Raytracer.Core.Materials
{
    abstract class Material
    {
        public abstract bool Scatter(Ray rayIn, ref HitRecord rec, out Vector3d attenuation, out Ray scattered);
        public virtual Vector3d Emitted(double u, double v, Vector3d p) { return Vector3d.Zero; }

        public static Vector3d refract(Vector3d uv, Vector3d n, double refractionRatio)
        {
            var cosTheta = Math.Min(Vector3d.Dot(-uv, n), 1.0);
            Vector3d rayOutPerpendicular = refractionRatio * (uv + cosTheta * n);
            Vector3d rayOutParallel = -Math.Sqrt(Math.Abs(1.0f - rayOutPerpendicular.LengthSquared)) * n;
            return rayOutPerpendicular + rayOutParallel;
        }

        public static Vector3d reflect(Vector3d v, Vector3d n)
        {
            return v - 2 * Vector3d.Dot(v, n) * n;
        }
    }
}
