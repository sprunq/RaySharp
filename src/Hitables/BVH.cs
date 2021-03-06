// AABB & BVH Code mostly from: https://github.com/Herbstein/SharpRays

using System;
using System.Collections.Generic;
using System.Linq;
using Raytracer.Core;
using Raytracer.Utility;

namespace Raytracer.Hitables
{
    public class BVHNode : IHitable
    {
        public BVHNode(List<IHitable> hitables)
        {
            var axis = (int)(RandomHelper.RandomInt(0, 3));
            switch (axis)
            {
                case 0:
                    hitables.Sort(BVHNode.BoxXCompare);
                    break;
                case 1:
                    hitables.Sort(BVHNode.BoxYCompare);
                    break;
                default:
                    hitables.Sort(BVHNode.BoxZCompare);
                    break;
            }

            switch (hitables.Count)
            {
                case 1:
                    Left = Right = hitables[0];
                    break;
                case 2:
                    Left = hitables[0];
                    Right = hitables[1];
                    break;
                default:
                    Left = new BVHNode(hitables.Take(hitables.Count / 2).ToList());
                    Right = new BVHNode(hitables.Skip(hitables.Count / 2).ToList());
                    break;
            }
            AABB boxLeft = new(), boxRight = new();
            if (!Left.BoundingBox(ref boxLeft) ||
                !Right.BoundingBox(ref boxRight))
            {
                throw new Exception("No bounding box in BVHNode");
            }

            Box = AABB.SurroundingBox(boxRight, boxLeft);
        }

        public bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            if (Box.Hit(ray, tMin, tMax))
            {
                HitRecord leftRecord = new HitRecord(), rightRecord = new HitRecord();
                bool hitLeft = Left.Hit(ray, tMin, tMax, ref leftRecord);
                bool hitRight = Right.Hit(ray, tMin, tMax, ref rightRecord);
                if (hitLeft && hitRight)
                {
                    rec = leftRecord.t < rightRecord.t ? leftRecord : rightRecord;
                    return true;
                }

                if (hitLeft)
                {
                    rec = leftRecord;
                    return true;
                }

                if (hitRight)
                {
                    rec = rightRecord;
                    return true;
                }

                return false;
            }

            return false;
        }

        public bool BoundingBox(ref AABB box)
        {
            box = Box;
            return true;
        }

        private static int BoxXCompare(IHitable a, IHitable b)
        {
            if (a == null || b == null)
            {
                Console.WriteLine("Null in x");
                return 1;
            }

            AABB boxLeft = new(), boxRight = new();
            if (!a.BoundingBox(ref boxLeft) || !b.BoundingBox(ref boxRight))
            {
                throw new Exception("No bounding box in BVHNode");
            }

            if (boxLeft.Minimum.X - boxRight.Minimum.X < 0)
            {
                return -1;
            }

            return 1;
        }

        private static int BoxYCompare(IHitable a, IHitable b)
        {
            if (a == null || b == null)
            {
                Console.WriteLine("Null in y");
                return 1;
            }
            AABB boxLeft = new(), boxRight = new();
            if (!a.BoundingBox(ref boxLeft) || !b.BoundingBox(ref boxRight))
            {
                throw new Exception("No bounding box in BVHNode");
            }

            if (boxLeft.Minimum.Y - boxRight.Minimum.Y < 0)
            {
                return -1;
            }

            return 1;
        }

        private static int BoxZCompare(IHitable a, IHitable b)
        {
            if (a == null || b == null)
            {
                Console.WriteLine("Null in z");
                return 1;
            }
            AABB boxLeft = new(), boxRight = new();
            if (!a.BoundingBox(ref boxLeft) || !b.BoundingBox(ref boxRight))
            {
                throw new Exception("No bounding box in BVHNode");
            }

            if (boxLeft.Minimum.Z - boxRight.Minimum.Z < 0)
            {
                return -1;
            }

            return 1;
        }

        public AABB Box;
        public IHitable Left;
        public IHitable Right;
    }
}