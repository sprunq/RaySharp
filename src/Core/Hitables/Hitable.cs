using System.Numerics;
using Raytracer.Core.Materials;
using OpenTK.Mathematics;

namespace Raytracer.Core.Hitables
{
    struct HitRecord
    {
        public Vector3d position;
        public Vector3d normal;
        public Material material;
        public double t;
        public double u;
        public double v;
        public bool frontFace;

        public void SetFaceNormal(Ray ray, Vector3d outwardNormal)
        {
            frontFace = Vector3d.Dot(ray.Direction, outwardNormal) < 0;
            normal = frontFace ? outwardNormal : -outwardNormal;
        }
    };

    abstract class Hitable
    {
        public abstract bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec);
        public abstract bool BoundingBox(ref AABB outputBox);
    }
}
