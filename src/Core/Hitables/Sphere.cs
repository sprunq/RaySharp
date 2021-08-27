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
            rec.material = _material;

            return true;
        }
    }
}
