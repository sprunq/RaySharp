using System.IO;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace Raytracer
{
    public class Program
    {
        static void Main(string[] args)
        {
            Raytracer raytracer = new(imageWidth: 1000,
                                      aspectRatio: 3.0 / 2.0,
                                      samples: 1,
                                      maxDepth: 50,
                                      shouldDenoise: true,
                                      denoiserPath: Path.GetFullPath(@"..\Denoiser\"),
                                      outputName: "output.png",
                                      outputFolder: Path.GetFullPath(@"..\Renders"),
                                      printProgress: true);

            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(raytracer.ImageWidth, raytracer.ImageHeight),
                Title = "Raytracer",
            };

            using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings, raytracer))
            {
                window.Run();
            }
        }
    }
}
