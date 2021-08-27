using System.Numerics;
using Raytracer.Core.Materials;
using Raytracer.Helpers;

namespace Raytracer.Core.Hitables
{
    class Lambertian : Material
    {
        public Vector3 albedo;

        public Lambertian(Vector3 a)
        {
            albedo = a;
        }

        public override bool scatter(Ray r_in, ref HitRecord rec, out Vector3 attenuation, out Ray scattered)
        {
            var scatterDirection = rec.normal + Vector3Helper.randomUnitVector();

            if (Vector3Helper.IsVector3NearZero(scatterDirection))
            {
                scatterDirection = rec.normal;
            }

            scattered = new Ray(rec.p, scatterDirection);
            attenuation = albedo;
            return true;
        }
    }
}
