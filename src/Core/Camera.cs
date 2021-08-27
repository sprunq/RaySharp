using System;
using System.Numerics;
using Raytracer.Helpers;

namespace Raytracer.Core
{
    class Camera
    {
        private Vector3 origin;
        private Vector3 lower_left_corner;
        private Vector3 horizontal;
        private Vector3 vertical;
        private Vector3 u, v, w;
        private double lens_radius;

        public Camera(Vector3 lookfrom, Vector3 lookat, Vector3 vup, double vert_fov, double aspect_ratio, double aperture, double focus_dist)
        {
            var theta = Converter.ConvertToRadians(vert_fov);
            var h = Math.Tan(theta / 2);
            var viewport_height = 2.0 * h;
            var viewport_width = aspect_ratio * viewport_height;

            w = Vector3.Normalize(lookfrom - lookat);
            u = Vector3.Normalize(Vector3.Cross(vup, w));
            v = Vector3.Cross(w, u);

            origin = lookfrom;
            horizontal = (float)focus_dist * (float)viewport_width * u;
            vertical = (float)focus_dist * (float)viewport_height * v;
            lower_left_corner = origin - horizontal / 2 - vertical / 2 - (float)focus_dist * w;
            lens_radius = aperture / 2;
        }

        public Ray GetRay(double s, double t)
        {
            Vector3 rd = (float)lens_radius * Vector3Helper.RandomInUnitDisk();
            Vector3 offest = u * rd.X + v * rd.Y;
            return new Ray(origin + offest,
                           lower_left_corner + (float)s * horizontal + (float)t * vertical - origin - offest);
        }
    }
}
