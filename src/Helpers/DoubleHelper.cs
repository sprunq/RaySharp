using System;

namespace Raytracer.Helpers
{
    class DoubleHelper
    {
        private static readonly Random random = new Random(0);
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
    }
}
