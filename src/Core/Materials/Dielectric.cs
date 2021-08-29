using System;
using System.Numerics;
using OpenTK.Mathematics;
using Raytracer.Core.Materials;
using Raytracer.Utility;

namespace Raytracer.Core.Hitables
{
    class Dielectric : Material
    {
        private double _indexOfRefraction;

        public Dielectric(double indexOfRefraction)
        {
            _indexOfRefraction = indexOfRefraction;
        }

        public override bool Scatter(Ray rayIn, ref HitRecord rec, out Vector3d attenuation, out Ray scattered)
        {
            attenuation = new Vector3d(1);
            double refractionRatio = rec.frontFace ? (1.0 / _indexOfRefraction) : _indexOfRefraction;
            Vector3d unitDirection = Vector3d.Normalize(rayIn.Direction);

            double cosTheta = Math.Min(Vector3d.Dot(-unitDirection, rec.normal), 1.0);
            double sinTheta = Math.Sqrt(1.0 - cosTheta * cosTheta);

            bool cannotRefract = refractionRatio * sinTheta > 1.0;
            Vector3d direction;

            if (cannotRefract || reflectance(cosTheta, refractionRatio) > RandomHelper.RandomDouble())
                direction = reflect(unitDirection, rec.normal);
            else
                direction = refract(unitDirection, rec.normal, refractionRatio);

            scattered = new Ray(rec.position, direction);
            return true;
        }

        private static double reflectance(double cosine, double refractionIndex)
        {
            // Schlick's approximation for reflectance
            var r0 = (1 - refractionIndex) / (1 + refractionIndex);
            r0 = r0 * r0;
            return r0 + (1 - r0) * Math.Pow((1 - cosine), 5);
        }
    }
}
