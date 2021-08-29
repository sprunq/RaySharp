using System;
using System.Numerics;
using Raytracer.Utility;

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

        public Camera(Vector3 lookFrom, Vector3 lookAt, Vector3 vectorUp, double fieldOfView, double aspectRatio, double aperture, double focusDistance)
        {
            var theta = GeneralHelper.ConvertToRadians(fieldOfView);
            var h = Math.Tan(theta / 2);
            var viewportHeight = 2.0 * h;
            var viewportWidth = aspectRatio * viewportHeight;

            _w = Vector3.Normalize(lookFrom - lookAt);
            _u = Vector3.Normalize(Vector3.Cross(vectorUp, _w));
            _v = Vector3.Cross(_w, _u);

            _origin = lookFrom;
            _horizontal = (float)focusDistance * (float)viewportWidth * _u;
            _vertical = (float)focusDistance * (float)viewportHeight * _v;
            _lowerLeftCorner = _origin - _horizontal / 2 - _vertical / 2 - (float)focusDistance * _w;
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
