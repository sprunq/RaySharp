using Raytracer.Core;
using Raytracer.Hitables;
using Raytracer.Instances;
using OpenTK.Mathematics;
using Raytracer.Utility;
using System.Collections.Generic;
using System;

namespace Raytracer.Scenes
{
    public partial class Scene
    {
        public static void LucyDragonBox(ref double _aspectRatio, ref Vector3d background, out ObjectList world, out Camera camera)
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

            var white = new Metal(new Vector3d(1), 0.9);

            world.Add(new YZRect(new Vector2d(0, 555), new Vector2d(0, 555), 555, white));
            world.Add(new YZRect(new Vector2d(0, 555), new Vector2d(0, 555), 0, white));
            world.Add(new XZRect(new Vector2d(0, 555), new Vector2d(0, 555), 555, white));
            world.Add(new XZRect(new Vector2d(0, 555), new Vector2d(0, 555), 0, white));

            // Back Lights
            List<Hitable> lights = new();
            int width = 20;
            int spacing = 60;
            for (int i = 0; i < 555; i += spacing)
            {
                lights.Add(new Translate(new XYRect(new Vector2d(i, i + width),
                                                    new Vector2d(0, 555), 554,
                                                    new Light(new Vector3d((double)(i) / 555.0 + 0.1,
                                                                           0,
                                                                           1.0 - (double)(i) / 555.0)
                                                                           * 1)),
                                        new Vector3d(-2, 0, 0)));
            }
            world.Add(new BVHNode(lights));

            // Light Sphere
            var light = new Light(new Vector3d(2));
            Hitable lightSphere = new Sphere(new Vector3d(260, 30, 140), 30, light);
            world.Add(lightSphere);

            // Lucy
            var lucyMaterial = new Dielectric(1.5);
            Hitable lucy = new Mesh(@"..\Models\lucy.obj", lucyMaterial, 1.3);
            lucy = new Rotate(lucy, 90, Axis.Y);
            lucy = new Translate(lucy, new Vector3d(150, 0, 150));
            world.Add(lucy);

            // Dragon
            var dragonMaterial = new Metal(new Vector3d(1), 0.1);
            Hitable dragon = new Mesh(@"..\Models\xyzrgb_dragon.obj", dragonMaterial, 2.5);
            dragon = new Rotate(dragon, 65, Axis.Y);
            dragon = new Translate(dragon, new Vector3d(300, 90, 200));
            world.Add(dragon);
        }
    }
}
