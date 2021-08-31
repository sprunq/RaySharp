using System.Numerics;
using OpenTK.Mathematics;
using Raytracer.Core.Materials;
using Raytracer.Core.Textures;
using Raytracer.Utility;

namespace Raytracer.Core.Hitables
{
    class Light : Material
    {
        private Texture _emit;

        public Light(Texture emit)
        {
            _emit = emit;
        }

        public Light(Vector3d color)
        {
            _emit = new SolidColor(color);
        }

        public override bool Scatter(Ray rayIn, ref HitRecord rec, out Vector3d attenuation, out Ray scattered)
        {
            rec.material = null;
            rec.normal = new Vector3d();
            attenuation = new();
            scattered = new();
            return false;
        }

        public override Vector3d Emitted(double u, double v, Vector3d p)
        {
            return _emit.Value(u, v, p);
        }

    }
}
