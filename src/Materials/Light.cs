using OpenTK.Mathematics;
using Raytracer.Core;
using Raytracer.Materials;
using Raytracer.Textures;

namespace Raytracer.Hitables
{
    public class Light : IMaterial
    {
        public Light(Texture emit)
        {
            _emit = emit;
        }

        public Light(Vector3d color)
        {
            _emit = new SolidColor(color);
        }

        public bool Scatter(Ray rayIn, ref HitRecord rec, out Vector3d attenuation, out Ray scattered)
        {
            rec.material = null;
            rec.normal = new Vector3d();
            attenuation = new();
            scattered = new();
            return false;
        }

        public Vector3d Emitted(double u, double v, Vector3d p)
        {
            return _emit.Value(u, v, p);
        }

        private Texture _emit;
    }
}
