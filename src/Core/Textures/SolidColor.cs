using System.Numerics;
using OpenTK.Mathematics;
using Raytracer.Utility;

namespace Raytracer.Core.Textures
{
    class SolidColor : Texture
    {
        private Vector3d _color;

        public SolidColor() { }

        public SolidColor(Vector3d color)
        {
            _color = color;
        }

        public SolidColor(double r, double g, double b)
        {
            _color = Vector3Helper.RgbNormalizedVector((int)r, (int)g, (int)b);
        }

        public override Vector3d Value(double u, double v, Vector3d p)
        {
            return _color;
        }
    }
}
