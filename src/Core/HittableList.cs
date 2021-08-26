using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Raytracer.Core.Hittables;

namespace Raytracer.Core.Hittables
{
    class HittableList : Hittable
    {
        public List<Hittable> objects = new();

        public HittableList() { }
        public HittableList(Hittable obj)
        {
            Add(obj);
        }

        public void Clear()
        {
            objects.Clear();
        }
        public void Add(Hittable obj)
        {
            objects.Add(obj);
        }


        public override bool hit(Ray r, double t_min, double t_max, ref HitRecord rec)
        {
            HitRecord temp_rec = new();
            bool hit_anything = false;
            var closest_so_far = t_max;

            foreach (var obj in objects)
            {
                if (obj.hit(r, t_min, closest_so_far, ref temp_rec))
                {
                    hit_anything = true;
                    closest_so_far = temp_rec.t;
                    rec = temp_rec;
                }
            }

            return hit_anything;
        }
    }
}
