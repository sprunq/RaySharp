using System.Collections.Generic;
using JeremyAnsel.Media.WavefrontObj;
using OpenTK.Mathematics;
using Raytracer.Core;
using Raytracer.Materials;

namespace Raytracer.Hitables
{
    public class Mesh : Hitable
    {
        private BVHNode _faces;
        private string _objPath;
        private string _mtlPath;
        private Material _material;
        private double _scale;

        public Mesh() { }
        public Mesh(string objPath, string mtlPath, Material material, double scale)
        {
            _objPath = objPath;
            _mtlPath = mtlPath;
            _material = material;
            _scale = scale;
            LoadObjFaces();
        }

        public override bool BoundingBox(ref AABB outputBox)
        {
            outputBox = _faces.Box;
            return true;
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            return _faces.Hit(ray, tMin, tMax, ref rec);
        }

        public void LoadObjFaces()
        {
            var objFile = ObjFile.FromFile(_objPath);
            if (_mtlPath != "")
            {
                var mtlFile = ObjMaterialFile.FromFile(_mtlPath);
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

                    faces.Add(new Triangle(new Vector3d(p0.X * _scale, p0.Y * _scale, p0.Z * _scale),
                                           new Vector3d(p1.X * _scale, p1.Y * _scale, p1.Z * _scale),
                                           new Vector3d(p2.X * _scale, p2.Y * _scale, p2.Z * _scale),
                                           normal,
                                           _material));

                    faces.Add(new Triangle(new Vector3d(p2.X * _scale, p2.Y * _scale, p2.Z * _scale),
                                           new Vector3d(p3.X * _scale, p3.Y * _scale, p3.Z * _scale),
                                           new Vector3d(p0.X * _scale, p0.Y * _scale, p0.Z * _scale),
                                           normal,
                                           _material));
                }
                else
                {
                    // Face is triangle
                    faces.Add(new Triangle(new Vector3d(p0.X * _scale, p0.Y * _scale, p0.Z * _scale),
                                           new Vector3d(p1.X * _scale, p1.Y * _scale, p1.Z * _scale),
                                           new Vector3d(p2.X * _scale, p2.Y * _scale, p2.Z * _scale),
                                           normal,
                                           _material));
                }
            }

            _faces = new(faces);
        }
    }
}
