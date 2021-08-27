using System;
using System.Numerics;

namespace Raytracer.Helpers
{
    class Vector3Helper
    {
        public static Vector3 randomVec3()
        {
            return new Vector3((float)DoubleHelper.randomDouble(), (float)DoubleHelper.randomDouble(), (float)DoubleHelper.randomDouble());
        }

        public static Vector3 randomVec3(double min, double max)
        {
            return new Vector3((float)DoubleHelper.randomDouble(min, max), (float)DoubleHelper.randomDouble(min, max), (float)DoubleHelper.randomDouble(min, max));
        }

        public static Vector3 randomInUnitSphere()
        {
            while (true)
            {
                var p = randomVec3(-1, 1);
                if (p.LengthSquared() >= 1)
                    continue;
                return p;
            }
        }
        public static Vector3 randomUnitVector()
        {
            return Vector3.Normalize(randomInUnitSphere());
        }

        public static Vector3 randomInHemisphere(Vector3 normal)
        {
            Vector3 inUnitSphere = randomInUnitSphere();
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
                var p = new Vector3((float)DoubleHelper.randomDouble(-1, 1), (float)DoubleHelper.randomDouble(-1, 1), 0);
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