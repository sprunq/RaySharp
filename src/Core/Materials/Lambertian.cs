using System.Numerics;
using Raytracer.Core.Materials;
using Raytracer.Helpers;
using Raytracer.Core.Textures;

namespace Raytracer.Core.Hitables
{
    class Lambertian : Material
    {
        private Texture _albedo;

        public Lambertian(Vector3 albedo)
        {
            _albedo = new SolidColor(albedo);
        }

        public Lambertian(Texture albedo)
        {
            _albedo = albedo;
        }

        public override bool Scatter(Ray rayIn, ref HitRecord rec, out Vector3 attenuation, out Ray scattered)
        {
            var scatterDirection = rec.normal + Vector3Helper.RandomUnitVector();

            if (Vector3Helper.IsVector3NearZero(scatterDirection))
            {
                scatterDirection = rec.normal;
            }

            scattered = new Ray(rec.position, scatterDirection);
            attenuation = _albedo.Value(rec.u, rec.v, rec.position);
            return true;
        }
    }
}
