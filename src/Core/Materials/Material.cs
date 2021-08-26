using System.Numerics;
using Raytracer.Core.Hittables;

namespace Raytracer.Core.Materials
{
    abstract class Material
    {
        public abstract bool scatter(Ray r_in, ref HitRecord rec, out Vector3 attenuation, out Ray scattered);
    }
}
