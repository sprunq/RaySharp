using System;
using System.IO;
using System.Numerics;
using Raytracer.Common;

namespace Raytracer.Core
{
    class Color
    {
        public static void write_color(TextWriter output, Vector3 color, int samples)
        {
            var r = color.X;
            var g = color.Y;
            var b = color.Z;

            var scale = 1.0f / samples;
            r = (float)Math.Sqrt(scale * r);
            g = (float)Math.Sqrt(scale * g);
            b = (float)Math.Sqrt(scale * b);

            int ir = (int)(256 * Helpers.clamp(r, 0.0, 0.999));
            int ig = (int)(256 * Helpers.clamp(g, 0.0, 0.999));
            int ib = (int)(256 * Helpers.clamp(b, 0.0, 0.999));

            output.WriteLine($"{ir} {ig} {ib}");
            output.Flush();
        }

        public static Vector3 RgbNormalizedVector(int r, int g, int b)
        {
            return new Vector3(255.0f / r, 255.0f / g, 255.0f / b);
        }

    }
}
