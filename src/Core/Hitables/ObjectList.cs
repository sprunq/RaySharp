using System.Collections.Generic;
using System.Numerics;
using Raytracer.Utility;
using Raytracer.Core.Materials;
using System.Linq;
using Raytracer.Core.Textures;
using OpenTK.Mathematics;
using ObjLoader.Loader.Loaders;
using System.IO;

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

        public void AddObjModel(string path, Vector3d offset, Material material)
        {
            var objLoaderFactory = new ObjLoaderFactory();
            var objLoader = objLoaderFactory.Create();
            FileStream fileStream = File.OpenRead(path);
            var result = objLoader.Load(fileStream);
            var vertcies = result.Vertices;

            foreach (var f in result.Groups[0].Faces)
            {
                var v0 = f[0].VertexIndex - 1;
                var v1 = f[1].VertexIndex - 1;
                var v2 = f[2].VertexIndex - 1;

                Add(new Triangle(new Vector3d(vertcies[v0].X + offset.X, vertcies[v0].Y + offset.Y, vertcies[v0].Z + offset.Z),
                                       new Vector3d(vertcies[v1].X + offset.X, vertcies[v1].Y + offset.Y, vertcies[v1].Z + offset.Z),
                                       new Vector3d(vertcies[v2].X + offset.X, vertcies[v2].Y + offset.Y, vertcies[v2].Z + offset.Z),
                                       material));
            }
        }

        public List<Hitable> GetBHVFacesData(string path, Vector3d offset, Material material)
        {
            List<Hitable> faces = new();
            var objLoaderFactory = new ObjLoaderFactory();
            var objLoader = objLoaderFactory.Create();
            FileStream fileStream = File.OpenRead(path);
            var result = objLoader.Load(fileStream);
            var vertcies = result.Vertices;

            foreach (var f in result.Groups[0].Faces)
            {
                var v0 = f[0].VertexIndex - 1;
                var v1 = f[1].VertexIndex - 1;
                var v2 = f[2].VertexIndex - 1;

                faces.Add(new Triangle(new Vector3d(vertcies[v0].X + offset.X, vertcies[v0].Y + offset.Y, vertcies[v0].Z + offset.Z),
                                       new Vector3d(vertcies[v1].X + offset.X, vertcies[v1].Y + offset.Y, vertcies[v1].Z + offset.Z),
                                       new Vector3d(vertcies[v2].X + offset.X, vertcies[v2].Y + offset.Y, vertcies[v2].Z + offset.Z),
                                       material));
            }
            return faces;
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
