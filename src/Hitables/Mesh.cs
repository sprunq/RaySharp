using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JeremyAnsel.Media.WavefrontObj;
using OpenTK.Mathematics;
using Raytracer.Core;
using Raytracer.Materials;
using Raytracer.Utility;

namespace Raytracer.Hitables
{
    public class Mesh : IHitable
    {
        public Mesh() { }

        public Mesh(string objPath, IMaterial material, double scale)
        {
            var buildTime = new Stopwatch();
            buildTime.Start();
            _objPath = objPath;
            _material = material;
            _scale = scale;
            LoadObjFaces();
            buildTime.Stop();
            Console.WriteLine($"Built Model {objPath} | {((decimal)NumberOfFaces).DynamicPostFix()} | {buildTime.Elapsed.Seconds}s");
        }

        public bool BoundingBox(ref AABB outputBox)
        {
            outputBox = _faces.Box;
            return true;
        }

        public bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            return _faces.Hit(ray, tMin, tMax, ref rec);
        }

        public void LoadObjFaces()
        {
            var objFile = ObjFile.FromFile(_objPath);
            var vertcies = objFile.Vertices;
            NumberOfFaces = objFile.Faces.Count();
            List<IHitable> faces = new();

            foreach (var face in objFile.Faces)
            {
                var v0 = face.Vertices[0].Vertex - 1;
                var v1 = face.Vertices[1].Vertex - 1;
                var v2 = face.Vertices[2].Vertex - 1;

                var p0 = vertcies[v0].Position;
                var p1 = vertcies[v1].Position;
                var p2 = vertcies[v2].Position;

                var normal = new Vector3d(face.Vertices[0].Normal, face.Vertices[1].Normal, face.Vertices[2].Normal);

                if (face.Vertices.Count == 4)
                {
                    // Face is quad
                    var v3 = face.Vertices[3].Vertex - 1;
                    var p3 = vertcies[v3].Position;

                    faces.Add(new Triangle(new Vector3d(p2.X * _scale, p2.Y * _scale, p2.Z * _scale),
                                           new Vector3d(p3.X * _scale, p3.Y * _scale, p3.Z * _scale),
                                           new Vector3d(p0.X * _scale, p0.Y * _scale, p0.Z * _scale),
                                           normal,
                                           _material));
                }
                // Face is triangle
                faces.Add(new Triangle(new Vector3d(p0.X * _scale, p0.Y * _scale, p0.Z * _scale),
                                       new Vector3d(p1.X * _scale, p1.Y * _scale, p1.Z * _scale),
                                       new Vector3d(p2.X * _scale, p2.Y * _scale, p2.Z * _scale),
                                       normal,
                                       _material));
            }

            // Most of the time is spent during Construction of the BVH Nodes 
            // Model with 250k faces: Loading Faces 70ms, BVH Node Construction 7200ms
            _faces = new(faces);
        }

        public int NumberOfFaces;
        private BVHNode _faces;
        private string _objPath;
        private IMaterial _material;
        private double _scale;
    }
}
