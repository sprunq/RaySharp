using System;
using System.IO;
using Raytracer.Scenes;

namespace Raytracer
{
    class Program
    {
        static void Main(string[] args)
        {
            Raytracer raytracer = new(imageWidth: 800,
                                      aspectRatio: 3.0 / 2.0,
                                      samples: 400,
                                      maxDepth: 50,
                                      shouldDenoise: true,
                                      outputName: "output.png",
                                      outputFolder: Path.GetFullPath(@"..\Renders"),
                                      printProgress: true);

            raytracer.LoadScene(Scene.BunnyScene);
            raytracer.Render();
            raytracer.SaveImage();
        }
    }
}

