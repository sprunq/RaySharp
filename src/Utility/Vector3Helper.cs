using System;
using System.Numerics;
using OpenTK.Mathematics;

namespace Raytracer.Utility
{
    class Vector3Helper
    {
        public static Vector3d RandomVec3()
        {
            return new Vector3d(RandomHelper.RandomDouble(), RandomHelper.RandomDouble(), RandomHelper.RandomDouble());
        }

        public static Vector3d RandomVec3(double min, double max)
        {
            return new Vector3d(RandomHelper.RandomDouble(min, max), RandomHelper.RandomDouble(min, max), RandomHelper.RandomDouble(min, max));
        }

        public static Vector3d RandomInUnitSphere()
        {
            while (true)
            {
                var p = RandomVec3(-1, 1);
                if (p.LengthSquared >= 1)
                    continue;
                return p;
            }
        }
        public static Vector3d RandomUnitVector()
        {
            return Vector3d.Normalize(RandomInUnitSphere());
        }

        public static Vector3d RandomInHemisphere(Vector3d normal)
        {
            Vector3d inUnitSphere = RandomInUnitSphere();
            if (Vector3d.Dot(inUnitSphere, normal) > 0.0)
            {
                return inUnitSphere;
            }
            else
            {
                return Vector3d.Multiply(inUnitSphere, -1);
            }
        }
        public static Vector3d RandomInUnitDisk()
        {
            while (true)
            {
                var p = new Vector3d(RandomHelper.RandomDouble(-1, 1), RandomHelper.RandomDouble(-1, 1), 0);
                if (p.LengthSquared >= 1) continue;
                return p;
            }
        }
        public static bool IsVector3NearZero(Vector3d toCheck)
        {
            var s = 1e-8;
            return (Math.Abs(toCheck.X) < s) && (Math.Abs(toCheck.Y) < s) && (Math.Abs(toCheck.Z) < s);
        }
    }
}