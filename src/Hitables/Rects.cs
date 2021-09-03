using OpenTK.Mathematics;
using Raytracer.Core;
using Raytracer.Materials;

namespace Raytracer.Hitables
{
    public class XYRect : Hitable
    {
        private double _x0;
        private double _x1;
        private double _y0;
        private double _y1;
        private double _k;
        private Material _mat;

        public XYRect()
        {
        }

        public XYRect(Vector2d x, Vector2d y, double offset, Material mat)
        {
            _x0 = x.X;
            _x1 = x.Y;
            _y0 = y.X;
            _y1 = y.Y;
            _k = offset;
            _mat = mat;
        }

        public override bool Hit(Ray r, double t0, double t1, ref HitRecord rec)
        {
            double t = (_k - r.Origin.Z) / r.Direction.Z;
            if (t < t0 || t > t1)
            {
                return false;
            }

            double x = r.Origin.X + t * r.Direction.X;
            double y = r.Origin.Y + t * r.Direction.Y;
            if (x < _x0 || x > _x1 || y < _y0 || y > _y1)
            {
                return false;
            }

            rec.u = (x - _x0) / (_x1 - _x0);
            rec.v = (y - _y0) / (_y1 - _y0);
            rec.t = t;
            rec.material = _mat;
            rec.position = r.At(t);
            var outwardNormal = new Vector3d(0, 0, 1);
            rec.SetFaceNormal(r, outwardNormal);

            return true;
        }

        public override bool BoundingBox(ref AABB box)
        {
            box = new AABB(new Vector3d(_x0, _y0, _k - 0.0001), new Vector3d(_x1, _y1, _k + 0.0001));
            return true;
        }
    }

    class XZRect : Hitable
    {
        private double _x0;
        private double _x1;
        private double _z0;
        private double _z1;
        private double _k;
        private Material _mat;

        public XZRect()
        {
        }

        public XZRect(Vector2d x, Vector2d z, double offset, Material mat)
        {
            _x0 = x.X;
            _x1 = x.Y;
            _z0 = z.X;
            _z1 = z.Y;
            _k = offset;
            _mat = mat;
        }

        public override bool Hit(Ray r, double t0, double t1, ref HitRecord rec)
        {
            double t = (_k - r.Origin.Y) / r.Direction.Y;
            if (t < t0 || t > t1)
            {
                return false;
            }

            double x = r.Origin.X + t * r.Direction.X;
            double z = r.Origin.Z + t * r.Direction.Z;
            if (x < _x0 || x > _x1 || z < _z0 || z > _z1)
            {
                return false;
            }

            rec.u = (x - _x0) / (_x1 - _x0);
            rec.v = (z - _z0) / (_z1 - _z0);
            rec.t = t;
            rec.material = _mat;
            rec.position = r.At(t);
            var outwardNormal = new Vector3d(0, 1, 0);
            rec.SetFaceNormal(r, outwardNormal);

            return true;
        }

        public override bool BoundingBox(ref AABB box)
        {
            box = new AABB(new Vector3d(_x0, _k - 0.0001, _z0), new Vector3d(_x1, _k + 0.0001, _z0));
            return true;
        }
    }


    class YZRect : Hitable
    {
        private double _y0;
        private double _y1;
        private double _z0;
        private double _z1;
        private double _k;
        private Material _mat;

        public YZRect()
        {
        }

        public YZRect(Vector2d y, Vector2d z, double offset, Material mat)
        {
            _y0 = y.X;
            _y1 = y.Y;
            _z0 = z.X;
            _z1 = z.Y;
            _k = offset;
            _mat = mat;
        }

        public override bool Hit(Ray r, double t0, double t1, ref HitRecord rec)
        {
            double t = (_k - r.Origin.X) / r.Direction.X;
            if (t < t0 || t > t1)
            {
                return false;
            }

            double y = r.Origin.Y + t * r.Direction.Y;
            double z = r.Origin.Z + t * r.Direction.Z;
            if (y < _y0 || y > _y1 || z < _z0 || z > _z1)
            {
                return false;
            }

            rec.u = (y - _y0) / (_y1 - _y0);
            rec.v = (z - _z0) / (_z1 - _z0);
            rec.t = t;
            rec.material = _mat;
            rec.position = r.At(t);
            var outwardNormal = new Vector3d(1, 0, 0);
            rec.SetFaceNormal(r, outwardNormal);

            return true;
        }

        public override bool BoundingBox(ref AABB box)
        {
            box = new AABB(new Vector3d(_k - 0.0001, _y0, _z0), new Vector3d(_k + 0.0001, _y1, _z1));
            return true;
        }
    }


}