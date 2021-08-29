using System;
using System.Numerics;
using OpenTK.Mathematics;

namespace Raytracer.Core
{
    class AABB
    {
        public Vector3d Minimum;
        public Vector3d Maximum;

        public AABB() { }
        public AABB(Vector3d minimum, Vector3d maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public bool Hit(Ray ray, double tMin, double tMax)
        {
            for (int a = 0; a < 3; a++)
            {
                var t0 = Math.Min((Minimum.X - ray.Origin.X) / ray.Direction.X,
                               (Maximum.X - ray.Origin.X) / ray.Direction.X);
                var t1 = Math.Max((Minimum.X - ray.Origin.X) / ray.Direction.X,
                               (Maximum.X - ray.Origin.X) / ray.Direction.X);
                tMin = Math.Max(t0, tMin);
                tMax = Math.Min(t1, tMax);
                if (tMax <= tMin)
                    return false;
            }
            return true;
        }

        public static AABB SurroundingBox(AABB box0, AABB box1)
        {
            Vector3d small = new(Math.Min(box0.Minimum.X, box1.Minimum.X),
                                Math.Min(box0.Minimum.Y, box1.Minimum.Y),
                                Math.Min(box0.Minimum.Z, box1.Minimum.Z));

            Vector3d big = new(Math.Max(box0.Maximum.X, box1.Maximum.X),
                              Math.Max(box0.Maximum.Y, box1.Maximum.Y),
                              Math.Max(box0.Maximum.Z, box1.Maximum.Z));

            return new AABB(small, big);
        }
    }
}
