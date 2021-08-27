using System;
using System.IO;
using System.Numerics;
using Raytracer.Core;
using Raytracer.Core.Hitables;
using Raytracer.Helpers;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Raytracer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Image
            const float aspectRatio = 3.0f / 2.0f;
            const int imageWidth = 1000;
            const int imageHeight = (int)(imageWidth / aspectRatio);
            const int samples = 1;
            const int maxDepth = 50;

            // Camera
            Vector3 lookfrom = new(13, 2, 3);
            Vector3 lookat = new(0, 0, 0);
            Vector3 vup = new(0, 1, 0);
            var dist_to_focus = 10;
            var aperture = 0.1;
            var fov = 25;

            Camera camera = new(lookfrom, lookat, vup, fov, aspectRatio, aperture, dist_to_focus);

            // World
            ObjectList world = ObjectList.RandomScene();

            // Output stream
            var sw = new StreamWriter("out.ppm");
            sw.WriteLine($"P3\n{imageWidth} {imageHeight} \n255");

            // Rendering (in Parallel)
            Stopwatch stopWatch = new();
            stopWatch.Start();
            Vector3[,] img_map = new Vector3[imageHeight, imageWidth];
            int max_h = imageHeight;
            int max_w = imageWidth;
            int progress = 0;
            Parallel.For(0, max_h, (j) =>
            {
                Console.WriteLine($"Running Line {++progress}/{imageHeight} \t[{((float)progress / imageHeight * 100.0).ToString("0.00")}%]");
                int derivedIndex = max_h - j;
                Parallel.For(0, max_w, (i) =>
                {
                    Vector3 color = new Vector3(0, 0, 0);
                    for (int s = 0; s < samples; s++)
                    {
                        var u = (i + DoubleHelper.randomDouble()) / (imageWidth - 1);
                        var v = (j + DoubleHelper.randomDouble()) / (imageHeight - 1);
                        Ray r = camera.GetRay(u, v);
                        color += Color.rayColor(r, world, maxDepth);
                    }
                    var s_col = Color.getColor(color, samples);
                    img_map[derivedIndex - 1, i] = s_col;

                });
            });
            var renderTime = stopWatch.Elapsed;
            Color.writeArrayToPPM(sw, img_map);
            var writeTime = stopWatch.Elapsed;

            Console.WriteLine();
            Console.WriteLine($"Render time:\t\t {renderTime.TotalSeconds.ToString("0.00 s")}");
            Console.WriteLine($"Write time:\t\t {(writeTime - renderTime).TotalSeconds.ToString("0.00 s")}");
            unsafe
            {
                var temp_map_size = imageWidth * imageHeight * sizeof(Vector3);
                Console.WriteLine($"Image Array:\t\t {temp_map_size} bytes");
            }
        }
    }

}

