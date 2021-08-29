using System;
using System.IO;
using System.Numerics;
using Raytracer.Core.Hitables;
using Raytracer.Utility;

namespace Raytracer.Core
{
    class Color
    {
        public static void WriteColor(TextWriter output, Vector3 color)
        {
            output.WriteLine($"{color.X} {color.Y} {color.Z}");
            output.Flush();
        }

        public static void WriteArrayToPPM(TextWriter output, Vector3[,] colorArray)
        {
            for (var i = 0; i < colorArray.GetLength(0); i++)
            {
                for (var j = 0; j < colorArray.GetLength(1); j++)
                {
                    output.WriteLine($"{colorArray[i, j].X} {colorArray[i, j].Y} {colorArray[i, j].Z}");
                    output.Flush();
                }
            }
        }

        public static Vector3 RgbNormalizedVector(int r, int g, int b)
        {
            return new Vector3(255.0f / r, 255.0f / g, 255.0f / b);
        }

        public static Vector3 RandomColor()
        {
            Random rand = new();
            return new Vector3(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
        }

    }
}
