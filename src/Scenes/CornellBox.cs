using Raytracer.Core;
using Raytracer.Hitables;
using Raytracer.Instances;
using OpenTK.Mathematics;

namespace Raytracer.Scenes
{
    public partial class Scene
    {
        public static void CornellBox(ref double _aspectRatio, ref Vector3d background, out ObjectList world, out Camera camera)
        {
            Vector3d lookfrom = new(278, 278, -800);
            Vector3d lookat = new(278, 278, 0);
            Vector3d vup = new(0, 1, 0);
            var focusDist = 10;
            var aperture = 0.0;
            var fov = 40.0;

            camera = new(lookfrom, lookat, vup, fov, _aspectRatio, aperture, focusDist);

            world = new();
            background = new Vector3d(0);

            var red = new Lambertian(new Vector3d(.65, .05, .05));
            var white = new Lambertian(new Vector3d(.73, .73, .73));
            var green = new Lambertian(new Vector3d(.12, .45, .15));
            var light = new Light(new Vector3d(7, 7, 7));

            world.Add(new YZRect(new Vector2d(0, 555), new Vector2d(0, 555), 555, green));
            world.Add(new YZRect(new Vector2d(0, 555), new Vector2d(0, 555), 0, red));
            world.Add(new XZRect(new Vector2d(113, 443), new Vector2d(127, 432), 554, light));
            world.Add(new XZRect(new Vector2d(0, 555), new Vector2d(0, 555), 555, white));
            world.Add(new XZRect(new Vector2d(0, 555), new Vector2d(0, 555), 0, white));
            world.Add(new XYRect(new Vector2d(0, 555), new Vector2d(0, 555), 555, white));

            Hitable box1 = new Box(new Vector3d(0, 0, 0), new Vector3d(165, 330, 165), white);
            box1 = new Rotate(box1, 15, Axis.Y);
            box1 = new Translate(box1, new Vector3d(265, 0, 295));

            Hitable box2 = new Box(new Vector3d(0, 0, 0), new Vector3d(165, 165, 165), white);
            box2 = new Rotate(box2, -18, Axis.Y);
            box2 = new Translate(box2, new Vector3d(130, 0, 65));

            world.Add(box1);
            world.Add(box2);
        }
    }
}
