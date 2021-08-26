using System;
using System.Numerics;

namespace Raytracer.Common
{
    class Helpers
    {
        public static double clamp(double x, double min, double max)
        {
            if (x < min) return min;
            if (x > max) return max;
            return x;
        }

        public static double randomDouble()
        {
            Random rand = new();
            return rand.NextDouble();
        }


        public static double randomDouble(double min, double max)
        {
            Random rand = new();
            return min + (max - min) * rand.NextDouble();
        }

        public static Vector3 randomVec3()
        {
            return new Vector3((float)randomDouble(), (float)randomDouble(), (float)randomDouble());
        }

        public static Vector3 randomVec3(double min, double max)
        {
            return new Vector3((float)randomDouble(min, max), (float)randomDouble(min, max), (float)randomDouble(min, max));
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
            return Vector3.Normalize(Helpers.randomInUnitSphere());
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

        public static bool Vector3NearZero(Vector3 to_check)
        {
            var s = 1e-8;
            return (Math.Abs(to_check.X) < s) && (Math.Abs(to_check.Y) < s) && (Math.Abs(to_check.Z) < s);
        }

        public static Vector3 reflect(Vector3 v, Vector3 n)
        {
            return v - 2 * Vector3.Dot(v, n) * n;
        }
    }
}