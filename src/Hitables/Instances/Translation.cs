using OpenTK.Mathematics;
using Raytracer.Core;

namespace Raytracer.Hitables.Instances
{
    public class Translate : IHitable
    {
        public Translate() { }

        public Translate(IHitable hitable, Vector3d offset)
        {
            _hitable = hitable;
            _offset = offset;
        }

        public bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            Ray movedRay = new(ray.Origin - _offset, ray.Direction);
            if (!_hitable.Hit(movedRay, tMin, tMax, ref rec))
                return false;

            rec.position += _offset;
            rec.SetFaceNormal(movedRay, rec.normal);
            return true;
        }

        public bool BoundingBox(ref AABB outputBox)
        {
            if (!_hitable.BoundingBox(ref outputBox))
                return false;

            outputBox = new AABB(outputBox.Minimum + _offset,
                                 outputBox.Maximum + _offset);
            return true;
        }

        private IHitable _hitable;
        private Vector3d _offset;
    }
}
