using System;
using System.IO;

namespace Raytracer
{
    class Program
    {
        static void Main(string[] args)
        {
            Raytracer raytracer = new(imageWidth: 1620,
                                      aspectRatio: 3.0 / 2.0,
                                      samples: 200,
                                      maxDepth: 50,
                                      shouldDenoise: true,
                                      outputName: "out.png",
                                      outputFolder: Path.GetFullPath(@"..\Renders"),
                                      printProgress: true);

            raytracer.LoadScene(3);
            raytracer.Render();
            raytracer.SaveImage();

            Console.WriteLine("Element Count: " + raytracer.World.objects.Count);
        }
    }

}

