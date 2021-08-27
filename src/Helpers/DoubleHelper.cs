using System;

namespace Raytracer.Helpers
{
    class DoubleHelper
    {
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
    }
}