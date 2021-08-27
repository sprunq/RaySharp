using System;
using System.Numerics;

namespace Raytracer.Helpers
{
    class Vector3Helper
    {
        public static Vector3 RandomVec3()
        {
            return new Vector3((float)DoubleHelper.RandomDouble(), (float)DoubleHelper.RandomDouble(), (float)DoubleHelper.RandomDouble());
        }

        public static Vector3 RandomVec3(double min, double max)
        {
            return new Vector3((float)DoubleHelper.RandomDouble(min, max), (float)DoubleHelper.RandomDouble(min, max), (float)DoubleHelper.RandomDouble(min, max));
        }

        public static Vector3 RandomInUnitSphere()
        {
            while (true)
            {
                var p = RandomVec3(-1, 1);
                if (p.LengthSquared() >= 1)
                    continue;
                return p;
            }
        }
        public static Vector3 RandomUnitVector()
        {
            return Vector3.Normalize(RandomInUnitSphere());
        }

        public static Vector3 RandomInHemisphere(Vector3 normal)
        {
            Vector3 inUnitSphere = RandomInUnitSphere();
            if (Vector3.Dot(inUnitSphere, normal) > 0.0)
            {
                return inUnitSphere;
            }
            else
            {
                return Vector3.Negate(inUnitSphere);
            }
        }
        public static Vector3 RandomInUnitDisk()
        {
            while (true)
            {
                var p = new Vector3((float)DoubleHelper.RandomDouble(-1, 1), (float)DoubleHelper.RandomDouble(-1, 1), 0);
                if (p.LengthSquared() >= 1) continue;
                return p;
            }
        }
        public static bool IsVector3NearZero(Vector3 to_check)
        {
            var s = 1e-8;
            return (Math.Abs(to_check.X) < s) && (Math.Abs(to_check.Y) < s) && (Math.Abs(to_check.Z) < s);
        }
    }
}