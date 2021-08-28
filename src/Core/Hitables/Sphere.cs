using System;
using System.Numerics;
using Raytracer.Core.Materials;

namespace Raytracer.Core.Hitables
{
    class Sphere : Hitable
    {
        private Vector3 _center;
        private double _radius;
        private Material _material;

        public Sphere() { }

        public Sphere(Vector3 center, double radius, Material material)
        {
            _center = center;
            _radius = radius;
            _material = material;
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            Vector3 originCenter = ray.Origin - _center;
            var a = ray.Direction.LengthSquared();
            var half_b = Vector3.Dot(originCenter, ray.Direction);
            var c = originCenter.LengthSquared() - _radius * _radius;

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
            Vector3 outward_normal = (rec.position - _center) / (float)_radius;
            rec.set_face_normal(ray, outward_normal);
            GetSphereUV(outward_normal, ref rec.u, ref rec.v);
            rec.material = _material;

            return true;
        }

        private static void GetSphereUV(Vector3 p, ref double u, ref double v)
        {
            // p: a given point on the sphere of radius one, centered at the origin.
            // u: returned value [0,1] of angle around the Y axis from X=-1.
            // v: returned value [0,1] of angle from Y=-1 to Y=+1.
            //     <1 0 0> yields <0.50 0.50>       <-1  0  0> yields <0.00 0.50>
            //     <0 1 0> yields <0.50 1.00>       < 0 -1  0> yields <0.50 0.00>
            //     <0 0 1> yields <0.25 0.50>       < 0  0 -1> yields <0.75 0.50>

            var theta = Math.Acos(-p.Y);
            var phi = Math.Atan2(-p.Z, p.X) + Math.PI;

            u = phi / (2 * Math.PI);
            v = theta / Math.PI;
        }

        public override bool BoundingBox(ref AABB outputBox)
        {
            outputBox = new AABB(_center - new Vector3((float)_radius, (float)_radius, (float)_radius),
                                 _center + new Vector3((float)_radius, (float)_radius, (float)_radius));
            return true;
        }

    }
}
