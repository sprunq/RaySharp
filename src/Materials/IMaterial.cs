using System;
using OpenTK.Mathematics;
using Raytracer.Core;
using Raytracer.Hitables;

namespace Raytracer.Materials
{
    public interface IMaterial
    {
        bool Scatter(Ray rayIn, ref HitRecord rec, out Vector3d attenuation, out Ray scattered);

        Vector3d Emitted(double u, double v, Vector3d p)
        {
            return new Vector3d(0);
        }
    }
}
