using System;
using System.Numerics;
using Raytracer.Core.Materials;
using Raytracer.Helpers;

namespace Raytracer.Core.Hitables
{
    class Dielectric : Material
    {
        private double _indexOfRefraction;

        public Dielectric(double indexOfRefraction)
        {
            _indexOfRefraction = indexOfRefraction;
        }

        public override bool scatter(Ray r_in, ref HitRecord rec, out Vector3 attenuation, out Ray scattered)
        {
            attenuation = new Vector3(1);
            double refractionRatio = rec.front_face ? (1.0 / _indexOfRefraction) : _indexOfRefraction;
            Vector3 unit_direction = Vector3.Normalize(r_in.dir);

            double cos_theta = Math.Min(Vector3.Dot(-unit_direction, rec.normal), 1.0);
            double sin_theta = Math.Sqrt(1.0 - cos_theta * cos_theta);

            bool cannot_refract = refractionRatio * sin_theta > 1.0;
            Vector3 direction;

            if (cannot_refract || reflectance(cos_theta, refractionRatio) > DoubleHelper.randomDouble())
                direction = reflect(unit_direction, rec.normal);
            else
                direction = refract(unit_direction, rec.normal, refractionRatio);

            scattered = new Ray(rec.p, direction);
            return true;
        }

        private static double reflectance(double cosine, double ref_idx)
        {
            // Use Schlick's approximation for reflectance.
            var r0 = (1 - ref_idx) / (1 + ref_idx);
            r0 = r0 * r0;
            return r0 + (1 - r0) * Math.Pow((1 - cosine), 5);
        }
    }
}
