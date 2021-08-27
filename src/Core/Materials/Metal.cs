using System.Numerics;
using Raytracer.Core.Materials;
using Raytracer.Helpers;

namespace Raytracer.Core.Hitables
{
    class Metal : Material
    {
        public Vector3 albedo;
        public double fuzz;

        public Metal(Vector3 color, double fuzziness)
        {
            albedo = color;
            fuzz = fuzziness < 1 ? fuzziness : 1;
        }

        public override bool scatter(Ray r_in, ref HitRecord rec, out Vector3 attenuation, out Ray scattered)
        {
            var reflected = reflect(Vector3.Normalize(r_in.dir), rec.normal);
            scattered = new Ray(rec.p, reflected + (float)fuzz * Vector3Helper.randomInUnitSphere());
            attenuation = albedo;
            return (Vector3.Dot(scattered.dir, rec.normal) > 0);
        }
    }
}
