using System.Numerics;

namespace Raytracer.Core.Textures
{
    class SolidColor : Texture
    {
        private Vector3 _color;

        public SolidColor() { }

        public SolidColor(Vector3 color)
        {
            _color = color;
        }

        public SolidColor(double r, double g, double b)
        {
            _color = Color.RgbNormalizedVector((int)r, (int)g, (int)b);
        }

        public override Vector3 Value(double u, double v, Vector3 p)
        {
            return _color;
        }
    }
}
