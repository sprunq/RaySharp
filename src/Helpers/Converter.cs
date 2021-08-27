using System;

namespace Raytracer.Helpers
{
    class Converter
    {
        public static double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
    }
}
