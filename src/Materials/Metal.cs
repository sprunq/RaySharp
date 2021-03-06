using OpenTK.Mathematics;
using Raytracer.Core;
using Raytracer.Materials;
using Raytracer.Utility;

namespace Raytracer.Hitables
{
    public class Metal : IMaterial
    {
        public Metal(Vector3d albedo, double fuzziness)
        {
            _albedo = albedo;
            _fuzziness = fuzziness < 1 ? fuzziness : 1;
        }

        bool IMaterial.Scatter(Ray rayIn, ref HitRecord rec, out Vector3d attenuation, out Ray scattered)
        {
            var reflected = Ray.reflect(Vector3d.Normalize(rayIn.Direction), rec.normal);
            scattered = new Ray(rec.position, reflected + _fuzziness * Vector3Helper.RandomInUnitSphere());
            attenuation = _albedo;
            return (Vector3d.Dot(scattered.Direction, rec.normal) > 0);
        }

        private Vector3d _albedo;
        private double _fuzziness;
    }
}
