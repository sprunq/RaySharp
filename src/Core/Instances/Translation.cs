using System;
using System.Numerics;
using OpenTK.Mathematics;
using Raytracer.Core.Hitables;
using Raytracer.Core.Materials;

namespace Raytracer.Core.Instances
{
    class Translate : Hitable
    {
        private Hitable _hitable;
        private Vector3d _offset;

        public Translate() { }

        public Translate(Hitable hitable, Vector3d offset)
        {
            _hitable = hitable;
            _offset = offset;
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            Ray movedRay = new(ray.Origin - _offset, ray.Direction);
            if (!_hitable.Hit(movedRay, tMin, tMax, ref rec))
                return false;

            rec.position += _offset;
            rec.SetFaceNormal(movedRay, rec.normal);
            return true;
        }

        public override bool BoundingBox(ref AABB outputBox)
        {
            if (!_hitable.BoundingBox(ref outputBox))
                return false;

            outputBox = new AABB(outputBox.Minimum + _offset,
                                 outputBox.Maximum + _offset);
            return true;
        }
    }
}
