using System;
using System.IO;
using System.Numerics;
using Raytracer.Core;
using Raytracer.Core.Hitables;
using Raytracer.Helpers;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace Raytracer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Image
            const float aspectRatio = 3.0f / 2.0f;
            const int imageWidth = 400;
            const int imageHeight = (int)(imageWidth / aspectRatio);
            const int samples = 30;
            const int maxDepth = 50;

            // Camera
            Vector3 lookfrom = new(8, 1.5f, 2);
            Vector3 lookat = new(0, 0.4f, -0.25f);
            Vector3 vup = new(0, 1, 0);
            var focusDist = 10;
            var aperture = 0.0;
            var fov = 27;

            Camera camera = new(lookfrom, lookat, vup, fov, aspectRatio, aperture, focusDist);

            // World
            ObjectList world = ObjectList.RandomScene();

            // Output stream
            var sw = new StreamWriter("out.ppm");
            sw.WriteLine($"P3\n{imageWidth} {imageHeight} \n255");

            // Rendering (in Parallel)
            Stopwatch stopWatch = new();
            stopWatch.Start();
            Vector3[,] img_map = new Vector3[imageHeight, imageWidth];
            List<int> rayTimes = new();
            int max_h = imageHeight;
            int max_w = imageWidth;
            int progress = 0;
            Parallel.For(0, max_h, (j) =>
            {
                Console.WriteLine($"Rendering Line {++progress}/{imageHeight} \t[{((float)progress / imageHeight * 100.0).ToString("0.00")}%]");
                int derivedIndex = max_h - j;
                Parallel.For(0, max_w, (i) =>
                {
                    Stopwatch rayTime = new();
                    rayTime.Start();
                    Vector3 color = new Vector3(0, 0, 0);
                    for (int s = 0; s < samples; s++)
                    {
                        var u = (i + RandomHelper.RandomDouble()) / (imageWidth - 1);
                        var v = (j + RandomHelper.RandomDouble()) / (imageHeight - 1);
                        Ray r = camera.GetRay(u, v);
                        color += Color.RayColor(r, world, maxDepth);
                    }
                    var s_col = Color.GetColor(color, samples);
                    img_map[derivedIndex - 1, i] = s_col;
                    rayTime.Stop();
                    rayTimes.Add(rayTime.Elapsed.Milliseconds);
                });
            });
            var renderTime = stopWatch.Elapsed;
            Color.WriteArrayToPPM(sw, img_map);
            var writeTime = stopWatch.Elapsed;

            Console.WriteLine();
            Console.WriteLine($"Render time:\t\t {renderTime.TotalSeconds.ToString("0.000 s")}");
            Console.WriteLine($"Write time:\t\t {(writeTime - renderTime).TotalSeconds.ToString("0.000 s")}");
            Console.WriteLine($"Average Ray time:\t {rayTimes.Average().ToString("0.000")} ms");
            Console.WriteLine($"Image Array:\t\t {imageWidth * imageHeight * 12} bytes");
        }
    }

}

