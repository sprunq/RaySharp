using System.Collections.Generic;
using System.Linq;
using Raytracer.Core;

namespace Raytracer.Hitables
{
    public class ObjectList : Hitable
    {
        public List<Hitable> objects = new();

        public ObjectList() { }
        public ObjectList(Hitable obj)
        {
            Add(obj);
        }

        public void Clear()
        {
            objects.Clear();
        }

        public void Add(Hitable obj)
        {
            objects.Add(obj);
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            HitRecord tempRec = new();
            bool hasHitAnything = false;
            var closestSoFar = tMax;

            foreach (var obj in objects)
            {
                if (obj.Hit(ray, tMin, closestSoFar, ref tempRec))
                {
                    hasHitAnything = true;
                    closestSoFar = tempRec.t;
                    rec = tempRec;
                }
            }

            return hasHitAnything;
        }

        public override bool BoundingBox(ref AABB outputBox)
        {
            if (!objects.Any())
            {
                return false;
            }

            AABB tempBox = new();
            bool firstBox = true;

            foreach (var obj in objects)
            {
                if (!obj.BoundingBox(ref tempBox)) return false;
                outputBox = firstBox ? tempBox : AABB.SurroundingBox(outputBox, tempBox);
                firstBox = false;
            }
            return true;
        }
    }
}
