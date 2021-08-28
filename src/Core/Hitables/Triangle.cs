using System;
using System.Numerics;
using Raytracer.Core.Materials;

namespace Raytracer.Core.Hitables
{
    class Triangle : Hitable
    {
        private Material _material;

        public Vector3 v0, v1, v2;

        public Triangle() { }

        public Triangle(Vector3 x, Vector3 y, Vector3 z, Material material)
        {
            v0 = x;
            v1 = y;
            v2 = z;
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

            Vector3 pvec = Vector3.Cross(ray.Direction, v0v2);
            double det = Vector3.Dot(v0v1, pvec);

            double kEpsilon = Double.Epsilon;

            //#ifdef CULLING 
            // if the determinant is negative the triangle is backfacing
            // if the determinant is close to 0, the ray misses the triangle
            if (det < kEpsilon)
            {
                return false;
            }
            //#else 
            // ray and triangle are parallel if det is close to 0
            // fabs is extremely slow
            /*if (fabs(det) < kEpsilon)
            {
                return false;
            }*/
            //#endif

            double invDet = 1 / det;

            Vector3 tvec = ray.Origin - v0;
            double u = Vector3.Dot(tvec, pvec) * invDet;
            if (u < 0 || u > 1)
            {
                return false;
            }

            Vector3 qvec = Vector3.Cross(tvec, v0v1);
            double v = Vector3.Dot(ray.Direction, qvec) * invDet;
            if (v < 0 || u + v > 1)
            {
                return false;
            }

            double t = Vector3.Dot(v0v2, qvec) * invDet;

            // For multiple objects, we take the closest one
            if (t < tMin || tMax < t)
            {
                return false;
            }

            // Hit Record
            //rec.u = u;
            //rec.v = v;
            rec.t = t;
            Vector3 outward_normal = Vector3.Cross(v0v1, v0v2);
            rec.set_face_normal(ray, outward_normal);
            rec.material = _material;
            rec.position = ray.At(t);

            return true;
        }

        public override bool BoundingBox(ref AABB outputBox)
        {
            throw new NotImplementedException();
        }

        /*
        public bool BoundingBox(AABB output_box)
        {
            Vec3 min(fmin(v0.x(), fmin(v1.x(), v2.x())),
                        fmin(v0.y(), fmin(v1.y(), v2.y())), 
                        fmin(v0.z(), fmin(v1.z(), v2.z())));

            Vec3 max(fmax(v0.x(), fmax(v1.x(), v2.x())),
                        fmax(v0.y(), fmax(v1.y(), v2.y())),
                        fmax(v0.z(), fmax(v1.z(), v2.z())));

            output_box = AABB(min, max);

            return true;
        }
        */
    }
}
