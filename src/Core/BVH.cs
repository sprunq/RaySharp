using System;
using System.Collections.Generic;
using System.Numerics;
using Raytracer.Core.Hitables;
using Raytracer.Helpers;

namespace Raytracer.Core
{
    class BVHNode : Hitable
    {
        public Hitable Left;
        public Hitable Right;
        public AABB box;

        public BVHNode() { }
        public BVHNode(ObjectList list)
        {

        }

        public BVHNode(List<Hitable> srcObjects, int start, int end)
        {

        }

        public override bool BoundingBox(ref AABB outputBox)
        {
            outputBox = box;
            return true;
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            if (!box.Hit(ray, tMin, tMax))
                return false;

            bool hitLeft = Left.Hit(ray, tMin, tMax, ref rec);
            bool hitRight = Right.Hit(ray, tMin, tMax, ref rec);

            return hitLeft || hitRight;
        }
    }
}
