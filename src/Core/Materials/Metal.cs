using System.Numerics;
using Raytracer.Core.Materials;
using Raytracer.Helpers;

namespace Raytracer.Core.Hitables
{
    class Metal : Material
    {
        private Vector3 _albedo;
        private double _fuzziness;

        public Metal(Vector3 albedo, double fuzziness)
        {
            _albedo = albedo;
            _fuzziness = fuzziness < 1 ? fuzziness : 1;
        }

        public override bool scatter(Ray ray_in, ref HitRecord rec, out Vector3 attenuation, out Ray scattered)
        {
            var reflected = reflect(Vector3.Normalize(ray_in.direction), rec.normal);
            scattered = new Ray(rec.position, reflected + (float)_fuzziness * Vector3Helper.randomInUnitSphere());
            attenuation = _albedo;
            return (Vector3.Dot(scattered.direction, rec.normal) > 0);
        }
    }
}
