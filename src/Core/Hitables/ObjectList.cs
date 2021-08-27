using System.Collections.Generic;
using System.Numerics;
using Raytracer.Helpers;
using Raytracer.Core.Materials;

namespace Raytracer.Core.Hitables
{
    class ObjectList : Hitable
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

        public static ObjectList RandomScene()
        {
            ObjectList world = new();

            var material_ground = new Lambertian(new Vector3(0.5f, 0.5f, 0.5f));
            world.Add(new Sphere(new Vector3(0.0f, -1000f, 0.0f), 1000.0, material_ground));

            for (int a = -11; a < 11; a++)
            {
                for (int b = -11; b < 11; b++)
                {
                    var choose_mat = DoubleHelper.randomDouble();
                    Vector3 center = new(a + 0.9f * (float)DoubleHelper.randomDouble(), 0.2f, b + 0.9f * (float)DoubleHelper.randomDouble());

                    if ((center - new Vector3(4, 0.2f, 0)).Length() > 0.9)
                    {
                        Material sphere_material;

                        if (choose_mat < 0.8)
                        {
                            // diffuse
                            var albedo = Vector3Helper.randomVec3();
                            sphere_material = new Lambertian(albedo);
                            world.Add(new Sphere(center, 0.2, sphere_material));
                        }
                        else if (choose_mat < 0.95)
                        {
                            // metal
                            var albedo = Vector3Helper.randomVec3();
                            var fuzz = DoubleHelper.randomDouble();
                            sphere_material = new Metal(albedo, fuzz);
                            world.Add(new Sphere(center, 0.2, sphere_material));
                        }
                        else
                        {
                            // glass
                            sphere_material = new Dielectric(1.5);
                            world.Add(new Sphere(center, 0.2, sphere_material));
                        }
                    }
                }
            }

            var material1 = new Dielectric(1.5);
            world.Add(new Sphere(new Vector3(0, 1, 0), 1.0, material1));

            var material2 = new Lambertian(new Vector3(0.4f, 0.2f, 0.1f));
            world.Add(new Sphere(new Vector3(-4, 1, 0), 1.0, material2));

            var material3 = new Metal(new Vector3(0.7f, 0.6f, 0.5f), 0.1);
            world.Add(new Sphere(new Vector3(4, 1, 0), 1.0, material3));

            return world;
        }
    }
}
