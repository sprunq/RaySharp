using System;
using System.Numerics;
using OpenTK.Mathematics;
using Raytracer.Core;
using Raytracer.Materials;
using Raytracer.Utility;

namespace Raytracer.Hitables
{
    public class Triangle : Hitable
    {
        private Material _material;

        public Vector3d v0, v1, v2;
        private Vector3d _normal;

        public Triangle() { }

        public Triangle(Vector3d x, Vector3d y, Vector3d z, Vector3d normal, Material material)
        {
            v0 = x;
            v1 = y;
            v2 = z;
            _normal = Vector3d.Normalize(Vector3d.Cross(v1 - v0, v2 - v0));
            _material = material;
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            /*
            www.scratchapixel.com/lessons/3d-basic-rendering/
            ray-tracing-rendering-a-triangle/
            moller-trumbore-ray-triangle-intersection
            */

            var v0v1 = v1 - v0;
            var v0v2 = v2 - v0;

            Vector3d pvec = Vector3d.Cross(ray.Direction, v0v2);
            double det = Vector3d.Dot(pvec, v0v1);

            double kEpsilon = Double.Epsilon;

            if (det < kEpsilon)
            {
                return false;
            }

            double invDet = 1 / det;

            Vector3d tvec = ray.Origin - v0;
            double u = Vector3d.Dot(tvec, pvec) * invDet;
            if (u < 0 || u > 1)
            {
                return false;
            }

            Vector3d qvec = Vector3d.Cross(tvec, v0v1);
            double v = Vector3d.Dot(ray.Direction, qvec) * invDet;
            if (v < 0 || u + v > 1)
            {
                return false;
            }

            double t = Vector3d.Dot(v0v2, qvec) * invDet;

            if (t < tMin || tMax < t)
            {
                return false;
            }

            rec.position = ray.At(t);
            rec.t = t;
            rec.SetFaceNormal(ray, _normal);
            rec.material = _material;

            return true;
        }

        public override bool BoundingBox(ref AABB output_box)
        {
            Vector3d min = new(Math.Min(v0.X, Math.Min(v1.X, v2.X)),
                               Math.Min(v0.Y, Math.Min(v1.Y, v2.Y)),
                               Math.Min(v0.Z, Math.Min(v1.Z, v2.Z)));

            Vector3d max = new(Math.Max(v0.X, Math.Max(v1.X, v2.X)),
                               Math.Max(v0.Y, Math.Max(v1.Y, v2.Y)),
                               Math.Max(v0.Z, Math.Max(v1.Z, v2.Z)));

            output_box = new AABB(min, max);

            return true;
        }

    }
}
