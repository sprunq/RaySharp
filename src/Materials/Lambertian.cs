using Raytracer.Materials;
using Raytracer.Utility;
using Raytracer.Textures;
using OpenTK.Mathematics;
using Raytracer.Core;

namespace Raytracer.Hitables
{
    public class Lambertian : IMaterial
    {
        public Lambertian(Vector3d albedo)
        {
            _albedo = new SolidColor(albedo);
        }

        public Lambertian(Texture albedo)
        {
            _albedo = albedo;
        }

        public bool Scatter(Ray rayIn, ref HitRecord rec, out Vector3d attenuation, out Ray scattered)
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

        private Texture _albedo;
    }
}
