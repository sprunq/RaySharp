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

        public override bool Scatter(Ray rayIn, ref HitRecord rec, out Vector3 attenuation, out Ray scattered)
        {
            var reflected = reflect(Vector3.Normalize(rayIn.Direction), rec.normal);
            scattered = new Ray(rec.position, reflected + (float)_fuzziness * Vector3Helper.RandomInUnitSphere());
            attenuation = _albedo;
            return (Vector3.Dot(scattered.Direction, rec.normal) > 0);
        }
    }
}
