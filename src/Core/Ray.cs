using System;
using System.IO;
using System.Numerics;

namespace Raytracer.Core
{
    class Ray
    {
        public Vector3 orig;
        public Vector3 dir;

        public Ray() { }
        public Ray(Vector3 origin, Vector3 direction)
        {
            orig = origin;
            dir = direction;
        }

        public Vector3 at(double t)
        {
            return orig + Vector3.Multiply((float)t, dir);
        }
    }
}
