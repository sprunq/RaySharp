using System.IO;

namespace Raytracer
{
    class Program
    {
        static void Main(string[] args)
        {
            Raytracer raytracer = new(imageWidth: 800,
                                      aspectRatio: 3.0 / 2.0,
                                      samples: 3,
                                      maxDepth: 50,
                                      shouldDenoise: true,
                                      outputName: "out.png",
                                      outputFolder: Path.GetFullPath(@"..\Renders"),
                                      printProgress: true);

            raytracer.LoadScene(2);
            raytracer.Render();
            raytracer.SaveImage();
        }
    }

}

