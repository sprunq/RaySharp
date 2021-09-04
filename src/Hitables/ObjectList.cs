using System.Collections.Generic;
using Raytracer.Materials;
using System.Linq;
using OpenTK.Mathematics;
using JeremyAnsel.Media.WavefrontObj;
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

        public static List<Hitable> GetObjFaces(string objPath, string mtlPath, Material material, double scale)
        {
            var objFile = ObjFile.FromFile(objPath);
            if (mtlPath != "")
            {
                var mtlFile = ObjMaterialFile.FromFile(mtlPath);
                var x = mtlFile.Materials.Count;
            }
            var vertcies = objFile.Vertices;
            List<Hitable> faces = new();

            foreach (var f in objFile.Faces)
            {
                var v0 = f.Vertices[0].Vertex - 1;
                var v1 = f.Vertices[1].Vertex - 1;
                var v2 = f.Vertices[2].Vertex - 1;

                var p0 = vertcies[v0].Position;
                var p1 = vertcies[v1].Position;
                var p2 = vertcies[v2].Position;

                var normal = new Vector3d(f.Vertices[0].Normal, f.Vertices[1].Normal, f.Vertices[2].Normal);

                if (f.Vertices.Count == 4)
                {
                    // Face is quad
                    var v3 = f.Vertices[3].Vertex - 1;
                    var p3 = vertcies[v3].Position;

                    faces.Add(new Triangle(new Vector3d(p0.X * scale, p0.Y * scale, p0.Z * scale),
                                           new Vector3d(p1.X * scale, p1.Y * scale, p1.Z * scale),
                                           new Vector3d(p2.X * scale, p2.Y * scale, p2.Z * scale),
                                           normal,
                                           material));

                    faces.Add(new Triangle(new Vector3d(p2.X * scale, p2.Y * scale, p2.Z * scale),
                                           new Vector3d(p3.X * scale, p3.Y * scale, p3.Z * scale),
                                           new Vector3d(p0.X * scale, p0.Y * scale, p0.Z * scale),
                                           normal,
                                           material));
                }
                else
                {
                    // Face is triangle
                    faces.Add(new Triangle(new Vector3d(p0.X * scale, p0.Y * scale, p0.Z * scale),
                                           new Vector3d(p1.X * scale, p1.Y * scale, p1.Z * scale),
                                           new Vector3d(p2.X * scale, p2.Y * scale, p2.Z * scale),
                                           normal,
                                           material));
                }
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
