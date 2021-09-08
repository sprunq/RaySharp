using System;
using OpenTK.Mathematics;
using Raytracer.Core;
using Raytracer.Utility;

namespace Raytracer.Hitables.Instances
{
    public enum Axis
    {
        X,
        Y,
        Z
    };

    public class Rotate : IHitable
    {
        public Rotate() { }

        public Rotate(IHitable hitable, double angle, Axis axis)
        {
            _material = hitable;
            var radians = GeneralHelper.ConvertToRadians(angle);
            _sinTheta = Math.Sin(radians);
            _cosTheta = Math.Cos(radians);
            _hasBox = _material.BoundingBox(ref _boundingBox);
            _axis = axis;

            Vector3d min = new(Double.PositiveInfinity, Double.PositiveInfinity, Double.PositiveInfinity);
            Vector3d max = new(Double.NegativeInfinity, Double.NegativeInfinity, Double.NegativeInfinity);

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        var x = i * _boundingBox.Maximum.X + (1 - i) * _boundingBox.Minimum.X;
                        var y = j * _boundingBox.Maximum.Y + (1 - j) * _boundingBox.Minimum.Y;
                        var z = k * _boundingBox.Maximum.Z + (1 - k) * _boundingBox.Minimum.Z;

                        double newX = x;
                        double newY = y;
                        double newZ = z;

                        switch (axis)
                        {
                            case Axis.X:
                                newY = _cosTheta * y + _sinTheta * z;
                                newZ = -_sinTheta * y + _cosTheta * z;
                                break;
                            case Axis.Y:
                                newX = _cosTheta * x + _sinTheta * z;
                                newZ = -_sinTheta * x + _cosTheta * z;
                                break;
                            case Axis.Z:
                                newX = _cosTheta * x + _sinTheta * y;
                                newY = -_sinTheta * x + _cosTheta * y;
                                break;
                            default:
                                throw new IndexOutOfRangeException("Rotation axis out of range");
                        }


                        Vector3d tester = new(newX, newY, newZ);

                        for (int c = 0; c < 3; c++)
                        {
                            min[c] = Math.Min(min[c], tester[c]);
                            max[c] = Math.Max(max[c], tester[c]);
                        }
                    }
                }
            }

            _boundingBox = new AABB(min, max);
        }

        public bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            var origin = ray.Origin;
            var direction = ray.Direction;

            switch (_axis)
            {
                case Axis.X:
                    origin[1] = _cosTheta * ray.Origin[1] - _sinTheta * ray.Origin[2];
                    origin[2] = _sinTheta * ray.Origin[1] + _cosTheta * ray.Origin[2];
                    direction[1] = _cosTheta * ray.Direction[1] - _sinTheta * ray.Direction[2];
                    direction[2] = _sinTheta * ray.Direction[1] + _cosTheta * ray.Direction[2];
                    break;
                case Axis.Y:
                    origin[0] = _cosTheta * ray.Origin[0] - _sinTheta * ray.Origin[2];
                    origin[2] = _sinTheta * ray.Origin[0] + _cosTheta * ray.Origin[2];
                    direction[0] = _cosTheta * ray.Direction[0] - _sinTheta * ray.Direction[2];
                    direction[2] = _sinTheta * ray.Direction[0] + _cosTheta * ray.Direction[2];
                    break;
                case Axis.Z:
                    origin[0] = _cosTheta * ray.Origin[0] - _sinTheta * ray.Origin[1];
                    origin[1] = _sinTheta * ray.Origin[0] + _cosTheta * ray.Origin[1];
                    direction[0] = _cosTheta * ray.Direction[0] - _sinTheta * ray.Direction[1];
                    direction[1] = _sinTheta * ray.Direction[0] + _cosTheta * ray.Direction[1];
                    break;
                default:
                    throw new IndexOutOfRangeException("Rotation hit axis out of range");
            }

            Ray rotated_r = new(origin, direction);

            if (!_material.Hit(rotated_r, tMin, tMax, ref rec))
                return false;

            var p = rec.position;
            var normal = rec.normal;

            p[0] = _cosTheta * rec.position[0] + _sinTheta * rec.position[2];
            p[2] = -_sinTheta * rec.position[0] + _cosTheta * rec.position[2];
            normal[0] = _cosTheta * rec.normal[0] + _sinTheta * rec.normal[2];
            normal[2] = -_sinTheta * rec.normal[0] + _cosTheta * rec.normal[2];


            switch (_axis)
            {
                case Axis.X:
                    p[1] = _cosTheta * rec.position[1] + _sinTheta * rec.position[2];
                    p[2] = -_sinTheta * rec.position[1] + _cosTheta * rec.position[2];
                    normal[1] = _cosTheta * rec.normal[1] + _sinTheta * rec.normal[2];
                    normal[2] = -_sinTheta * rec.normal[1] + _cosTheta * rec.normal[2];
                    break;
                case Axis.Y:
                    p[0] = _cosTheta * rec.position[0] + _sinTheta * rec.position[2];
                    p[2] = -_sinTheta * rec.position[0] + _cosTheta * rec.position[2];
                    normal[0] = _cosTheta * rec.normal[0] + _sinTheta * rec.normal[2];
                    normal[2] = -_sinTheta * rec.normal[0] + _cosTheta * rec.normal[2];
                    break;
                case Axis.Z:
                    p[0] = _cosTheta * rec.position[0] + _sinTheta * rec.position[1];
                    p[1] = -_sinTheta * rec.position[0] + _cosTheta * rec.position[1];
                    normal[0] = _cosTheta * rec.normal[0] + _sinTheta * rec.normal[1];
                    normal[1] = -_sinTheta * rec.normal[0] + _cosTheta * rec.normal[1];
                    break;
                default:
                    throw new IndexOutOfRangeException("Rotation hit axis out of range");
            }

            rec.position = p;
            rec.SetFaceNormal(rotated_r, normal);

            return true;
        }

        public bool BoundingBox(ref AABB outputBox)
        {
            outputBox = _boundingBox;
            return _hasBox;
        }

        private IHitable _material;
        private double _sinTheta;
        private double _cosTheta;
        private bool _hasBox;
        private Axis _axis;
        private AABB _boundingBox;
    }
}
