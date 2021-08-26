using System.Numerics;
using Raytracer.Core.Materials;

namespace Raytracer.Core.Hittables
{
    struct HitRecord
    {
        public Vector3 p;
        public Vector3 normal;
        public Material material;
        public double t;
        public bool front_face;

        public void set_face_normal(Ray ray, Vector3 outward_normal)
        {
            front_face = Vector3.Dot(ray.dir, outward_normal) < 0;
            normal = front_face ? outward_normal : -outward_normal;
        }
    };

    abstract class Hittable
    {
        public abstract bool hit(Ray r, double t_min, double t_max, ref HitRecord rec);
    }
}
