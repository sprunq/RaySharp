using System;
using System.IO;
using System.Numerics;
using Raytracer.Core.Materials;

namespace Raytracer.Core.Hittables
{
    class Sphere : Hittable
    {
        public Vector3 center;
        public double radius;
        public Material material;

        public Sphere() { }

        public Sphere(Vector3 cen, double r, Material m)
        {
            center = cen;
            radius = r;
            material = m;
        }

        public override bool hit(Ray r, double t_min, double t_max, ref HitRecord rec)
        {
            Vector3 oc = r.orig - center;
            var a = r.dir.LengthSquared();
            var half_b = Vector3.Dot(oc, r.dir);
            var c = oc.LengthSquared() - radius * radius;

            var discriminant = half_b * half_b - a * c;

            if (discriminant < 0)
            {
                return false;
            }

            var sqrtd = Math.Sqrt(discriminant);

            // Find the nearest root that lies in the acceptable range.
            var root = (-half_b - sqrtd) / a;
            if (root < t_min || t_max < root)
            {
                root = (-half_b + sqrtd) / a;
                if (root < t_min || t_max < root)
                {
                    return false;
                }
            }

            rec.t = root;
            rec.p = r.at(rec.t);
            Vector3 outward_normal = (rec.p - center) / (float)radius;
            rec.set_face_normal(r, outward_normal);
            rec.material = material;

            return true;
        }
    }
}
