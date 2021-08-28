using System.Numerics;

namespace Raytracer.Core
{
    class Ray
    {
        public Vector3 Origin;
        public Vector3 Direction;

        public Ray() { }
        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public Vector3 At(double t)
        {
            return Origin + Vector3.Multiply((float)t, Direction);
        }
    }
}
