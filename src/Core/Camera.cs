using System;
using System.Numerics;
using Raytracer.Helpers;

namespace Raytracer.Core
{
    class Camera
    {
        private Vector3 _origin;
        private Vector3 _lowerLeftCorner;
        private Vector3 _horizontal;
        private Vector3 _vertical;
        private Vector3 _u, _v, _w;
        private double _lendRadius;

        public Camera(Vector3 look_from, Vector3 look_at, Vector3 vector_up, double field_of_view, double aspect_ratio, double aperture, double focus_distance)
        {
            var theta = Converter.ConvertToRadians(field_of_view);
            var h = Math.Tan(theta / 2);
            var viewportHeight = 2.0 * h;
            var viewportWidth = aspect_ratio * viewportHeight;

            _w = Vector3.Normalize(look_from - look_at);
            _u = Vector3.Normalize(Vector3.Cross(vector_up, _w));
            _v = Vector3.Cross(_w, _u);

            _origin = look_from;
            _horizontal = (float)focus_distance * (float)viewportWidth * _u;
            _vertical = (float)focus_distance * (float)viewportHeight * _v;
            _lowerLeftCorner = _origin - _horizontal / 2 - _vertical / 2 - (float)focus_distance * _w;
            _lendRadius = aperture / 2;
        }

        public Ray GetRay(double s, double t)
        {
            Vector3 rd = (float)_lendRadius * Vector3Helper.RandomInUnitDisk();
            Vector3 offest = _u * rd.X + _v * rd.Y;
            return new Ray(_origin + offest,
                           _lowerLeftCorner + (float)s * _horizontal + (float)t * _vertical - _origin - offest);
        }
    }
}
