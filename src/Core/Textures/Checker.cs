using System;
using System.Numerics;

namespace Raytracer.Core.Textures
{
    class CheckerTexture : Texture
    {
        public Texture Odd;
        public Texture Even;

        public CheckerTexture() { }

        public CheckerTexture(Texture odd, Texture even)
        {
            Odd = odd;
            Even = even;
        }

        public CheckerTexture(Vector3 odd, Vector3 even)
        {
            Odd = new SolidColor(odd);
            Even = new SolidColor(even);
        }

        public override Vector3 Value(double u, double v, Vector3 p)
        {
            var sines = Math.Sin(10 * p.X) * Math.Sin(10 * p.Y) * Math.Sin(10 * p.Z);
            if (sines < 0)
            {
                return Odd.Value(u, v, p);
            }
            else
            {
                return Even.Value(u, v, p);
            }
        }
    }
}
