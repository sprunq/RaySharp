using OpenTK.Mathematics;
using Raytracer.Core;
using Raytracer.Materials;

namespace Raytracer.Hitables
{
    public class Box : Hitable
    {
        public Box() { }
        public Box(Vector3d p0, Vector3d p1, Material material)
        {
            _boxMin = p0;
            _boxMax = p1;

            _sides = new();
            _sides.Add(new XYRect(new Vector2d(p0.X, p1.X), new Vector2d(p0.Y, p1.Y), p1.Z, material));
            _sides.Add(new XYRect(new Vector2d(p0.X, p1.X), new Vector2d(p0.Y, p1.Y), p0.Z, material));

            _sides.Add(new XZRect(new Vector2d(p0.X, p1.X), new Vector2d(p0.Z, p1.Z), p1.Y, material));
            _sides.Add(new XZRect(new Vector2d(p0.X, p1.X), new Vector2d(p0.Z, p1.Z), p0.Y, material));

            _sides.Add(new YZRect(new Vector2d(p0.Y, p1.Y), new Vector2d(p0.Z, p1.Z), p1.X, material));
            _sides.Add(new YZRect(new Vector2d(p0.Y, p1.Y), new Vector2d(p0.Z, p1.Z), p0.X, material));
        }

        public override bool BoundingBox(ref AABB outputBox)
        {
            outputBox = new AABB(_boxMin, _boxMax);
            return true;
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            return _sides.Hit(ray, tMin, tMax, ref rec);
        }

        private Vector3d _boxMin;
        private Vector3d _boxMax;
        private ObjectList _sides;
    }
}
