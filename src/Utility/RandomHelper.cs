using System;

namespace Raytracer.Utility
{
    class RandomHelper
    {
        private static readonly Random random = new Random(42);
        private static readonly object syncLock = new object();
        public static double RandomDouble()
        {
            lock (syncLock)
            {
                return random.NextDouble();
            }
        }

        public static double RandomDouble(double min, double max)
        {
            lock (syncLock)
            {
                return min + (max - min) * random.NextDouble();
            }
        }

        public static int RandomInt(int min, int max)
        {
            return (int)RandomDouble(min, max + 1);
        }
    }
}
