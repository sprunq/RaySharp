using System.Numerics;

namespace Raytracer.Core
{
    class Ray
    {
        public Vector3 origin;
        public Vector3 direction;

        public Ray() { }
        public Ray(Vector3 origin, Vector3 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }

        public Vector3 At(double t)
        {
            return origin + Vector3.Multiply((float)t, direction);
        }
    }
}
