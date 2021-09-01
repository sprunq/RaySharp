using System;
using System.Numerics;
using OpenTK.Mathematics;
using Raytracer.Core.Materials;

namespace Raytracer.Core.Hitables
{
    class Sphere : Hitable
    {
        private Vector3d _center;
        private double _radius;
        private Material _material;

        public Sphere() { }

        public Sphere(Vector3d center, double radius, Material material)
        {
            _center = center;
            _radius = radius;
            _material = material;
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            Vector3d originCenter = ray.Origin - _center;
            var a = ray.Direction.LengthSquared;
            var half_b = Vector3d.Dot(originCenter, ray.Direction);
            var c = originCenter.LengthSquared - _radius * _radius;

            var discriminant = half_b * half_b - a * c;

            if (discriminant < 0)
            {
                return false;
            }

            var sqrtDiscriminant = Math.Sqrt(discriminant);

            // Find the nearest root that lies in the acceptable range.
            var root = (-half_b - sqrtDiscriminant) / a;
            if (root < tMin || tMax < root)
            {
                root = (-half_b + sqrtDiscriminant) / a;
                if (root < tMin || tMax < root)
                {
                    return false;
                }
            }

            rec.t = root;
            rec.position = ray.At(rec.t);
            Vector3d outward_normal = (rec.position - _center) / _radius;
            rec.SetFaceNormal(ray, outward_normal);
            GetSphereUV(outward_normal, ref rec.u, ref rec.v);
            rec.material = _material;

            return true;
        }

        private static void GetSphereUV(Vector3d p, ref double u, ref double v)
        {
            var theta = Math.Acos(-p.Y);
            var phi = Math.Atan2(-p.Z, p.X) + Math.PI;

            u = phi / (2 * Math.PI);
            v = theta / Math.PI;
        }

        public override bool BoundingBox(ref AABB outputBox)
        {
            outputBox = new AABB(_center - new Vector3d(_radius, _radius, _radius),
                                 _center + new Vector3d(_radius, _radius, _radius));
            return true;
        }

    }
}
