using System;

namespace Raytracer.Helpers
{
    class DoubleHelper
    {
        public static double RandomDouble()
        {
            Random rand = new();
            return rand.NextDouble();
        }

        public static double RandomDouble(double min, double max)
        {
            Random rand = new();
            return min + (max - min) * rand.NextDouble();
        }
    }
}