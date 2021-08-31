using OpenTK.Mathematics;

namespace Raytracer.Core.Textures
{
    abstract class Texture
    {
        public abstract Vector3d Value(double u, double v, Vector3d p);
    }
}
