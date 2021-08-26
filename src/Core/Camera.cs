using System;
using System.IO;
using System.Numerics;

namespace Raytracer.Core
{
    class Camera
    {
        private Vector3 origin;
        private Vector3 lower_left_corner;
        private Vector3 horizontal;
        private Vector3 vertical;

        public Camera()
        {
            var aspect_ratio = 16.0 / 9.0;
            var viewport_height = 2.0;
            var viewport_width = aspect_ratio * viewport_height;
            var focal_length = 1.0;
            origin = Vector3.Zero;
            horizontal = new Vector3((float)viewport_width, 0, 0);
            vertical = new Vector3(0, (float)viewport_height, 0);
            lower_left_corner = origin - horizontal / 2 - vertical / 2 - new Vector3(0, 0, (float)focal_length);
        }

        public Ray GetRay(double u, double v)
        {
            return new Ray(origin, lower_left_corner + (float)u * horizontal + (float)v * vertical - origin);
        }
    }
}
