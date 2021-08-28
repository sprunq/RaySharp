using System;
using System.Numerics;

namespace Raytracer.Core
{
    class AABB
    {
        public Vector3 Minimum;
        public Vector3 Maximum;

        public AABB() { }
        public AABB(Vector3 minimum, Vector3 maximum)
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
            Vector3 small = new(Math.Min(box0.Minimum.X, box1.Minimum.X),
                                Math.Min(box0.Minimum.Y, box1.Minimum.Y),
                                Math.Min(box0.Minimum.Z, box1.Minimum.Z));

            Vector3 big = new(Math.Max(box0.Maximum.X, box1.Maximum.X),
                              Math.Max(box0.Maximum.Y, box1.Maximum.Y),
                              Math.Max(box0.Maximum.Z, box1.Maximum.Z));

            return new AABB(small, big);
        }
    }
}
