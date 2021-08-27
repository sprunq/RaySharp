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

        public static ObjectList RandomScene()
        {
            ObjectList world = new();

            var materialGround = new Lambertian(new Vector3(0.5f, 0.5f, 0.5f));
            world.Add(new Sphere(new Vector3(0.0f, -1000f, 0.0f), 1000.0, materialGround));

            for (int a = -11; a < 11; a++)
            {
                for (int b = -11; b < 11; b++)
                {
                    var materialType = DoubleHelper.RandomDouble();
                    Vector3 center = new(a + 0.9f * (float)DoubleHelper.RandomDouble(), 0.2f, b + 0.9f * (float)DoubleHelper.RandomDouble());

                    if ((center - new Vector3(4, 0.2f, 0)).Length() > 0.9)
                    {
                        Material material;

                        if (materialType < 0.8)
                        {
                            // diffuse
                            var albedo = Vector3Helper.RandomVec3();
                            material = new Lambertian(albedo);
                            world.Add(new Sphere(center, 0.2, material));
                        }
                        else if (materialType < 0.95)
                        {
                            // metal
                            var albedo = Vector3Helper.RandomVec3();
                            var fuzz = DoubleHelper.RandomDouble();
                            material = new Metal(albedo, fuzz);
                            world.Add(new Sphere(center, 0.2, material));
                        }
                        else
                        {
                            // glass
                            material = new Dielectric(1.5);
                            world.Add(new Sphere(center, 0.2, material));
                        }
                    }
                }
            }

            var glass = new Dielectric(1.5);
            var lambert = new Lambertian(new Vector3(0.4f, 0.2f, 0.1f));
            var metal = new Metal(new Vector3(0.7f, 0.6f, 0.5f), 0.1);

            world.Add(new Sphere(new Vector3(-4, 1, 0), 1.0, lambert));
            world.Add(new Sphere(new Vector3(0, 0.75f, 0), 0.75, glass));
            world.Add(new Sphere(new Vector3(4, 0.5f, 0), 0.5, metal));

            return world;
        }
    }
}
