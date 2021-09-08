using System;
using OpenTK.Mathematics;
using Raytracer.Utility;

namespace Raytracer.Core
{
    public class Camera
    {
        public Camera(Vector3d lookFrom, Vector3d lookAt, Vector3d vectorUp, double fieldOfView, double aspectRatio, double aperture, double focusDistance)
        {
            var theta = GeneralHelper.ConvertToRadians(fieldOfView);
            var h = Math.Tan(theta / 2);
            var viewportHeight = 2.0 * h;
            var viewportWidth = aspectRatio * viewportHeight;

            _w = Vector3d.Normalize(lookFrom - lookAt);
            _u = Vector3d.Normalize(Vector3d.Cross(vectorUp, _w));
            _v = Vector3d.Cross(_w, _u);

            _origin = lookFrom;
            _horizontal = focusDistance * viewportWidth * _u;
            _vertical = focusDistance * viewportHeight * _v;
            _lowerLeftCorner = _origin - _horizontal / 2 - _vertical / 2 - focusDistance * _w;
            _lensRadius = aperture / 2;
        }

        public Ray GetRay(double s, double t)
        {
            Vector3d rd = _lensRadius * Vector3Helper.RandomInUnitDisk();
            Vector3d offest = _u * rd.X + _v * rd.Y;
            return new Ray(_origin + offest,
                           _lowerLeftCorner + s * _horizontal + t * _vertical - _origin - offest);
        }

        private Vector3d _origin;
        private Vector3d _lowerLeftCorner;
        private Vector3d _horizontal;
        private Vector3d _vertical;
        private Vector3d _u, _v, _w;
        private double _lensRadius;
    }
}
