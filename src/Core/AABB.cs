// AABB & BVH Code mostly from: https://github.com/Herbstein/SharpRays

using OpenTK.Mathematics;
using Raytracer.Utility;


namespace Raytracer.Core
{
    public class AABB
    {
        public Vector3d Maximum;
        public Vector3d Minimum;

        public AABB() { }

        public AABB(Vector3d v0, Vector3d v1)
        {
            Minimum = v0;
            Maximum = v1;
        }

        public bool Hit(Ray ray, double tMin, double tMax)
        {
            for (var a = 0; a < 3; a++)
            {
                double invD = 1 / ray.Direction.Get(a);
                double t0 = (Minimum.Get(a) - ray.Origin.Get(a)) * invD;
                double t1 = (Maximum.Get(a) - ray.Origin.Get(a)) * invD;
                if (invD < 0)
                {
                    AABB.FastSwap(ref t0, ref t1);
                }

                tMin = t0 > tMin ? t0 : tMin;
                tMax = t1 < tMax ? t1 : tMax;
                if (tMax <= tMin)
                {
                    return false;
                }
            }

            return true;
        }

        private static double FastMin(double a, double b)
        {
            return a < b ? a : b;
        }

        private static double FastMax(double a, double b)
        {
            return a > b ? a : b;
        }

        private static void FastSwap(ref double a, ref double b)
        {
            double t = a;
            a = b;
            b = t;
        }

        public static AABB SurroundingBox(AABB box0, AABB box1)
        {
            var small = new Vector3d(AABB.FastMin(box0.Minimum.X, box1.Minimum.X),
                                     AABB.FastMin(box0.Minimum.Y, box1.Minimum.Y),
                                     AABB.FastMin(box0.Minimum.Z, box1.Minimum.Z));
            var big = new Vector3d(AABB.FastMax(box0.Maximum.X, box1.Maximum.X),
                                   AABB.FastMax(box0.Maximum.Y, box1.Maximum.Y),
                                   AABB.FastMax(box0.Maximum.Z, box1.Maximum.Z));
            return new AABB(small, big);
        }
    }
}
