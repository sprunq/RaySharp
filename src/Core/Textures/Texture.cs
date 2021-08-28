using System.Numerics;

namespace Raytracer.Core.Textures
{
    abstract class Texture
    {
        public abstract Vector3 Value(double u, double v, Vector3 p);
    }
}
