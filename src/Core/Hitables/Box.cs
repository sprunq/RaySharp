using System;
using System.Numerics;
using OpenTK.Mathematics;
using Raytracer.Core.Materials;

namespace Raytracer.Core.Hitables
{
    class Box : Hitable
    {
        private Vector3d BoxMin;
        private Vector3d BoxMax;
        public ObjectList sides;

        public Box() { }
        public Box(Vector3d p0, Vector3d p1, Material material)
        {
            BoxMin = p0;
            BoxMax = p1;

            sides = new();
            sides.Add(new XYRect(new Vector2d(p0.X, p1.X), new Vector2d(p0.Y, p1.Y), p1.Z, material));
            sides.Add(new XYRect(new Vector2d(p0.X, p1.X), new Vector2d(p0.Y, p1.Y), p0.Z, material));

            sides.Add(new XZRect(new Vector2d(p0.X, p1.X), new Vector2d(p0.Z, p1.Z), p1.Y, material));
            sides.Add(new XZRect(new Vector2d(p0.X, p1.X), new Vector2d(p0.Z, p1.Z), p0.Y, material));

            sides.Add(new YZRect(new Vector2d(p0.Y, p1.Y), new Vector2d(p0.Z, p1.Z), p1.X, material));
            sides.Add(new YZRect(new Vector2d(p0.Y, p1.Y), new Vector2d(p0.Z, p1.Z), p0.X, material));
        }

        public override bool BoundingBox(ref AABB outputBox)
        {
            outputBox = new AABB(BoxMin, BoxMax);
            return true;
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            return sides.Hit(ray, tMin, tMax, ref rec);
        }
    }
}
