using System.IO;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using Raytracer.Scenes;

namespace Raytracer
{
    public class Program
    {
        static void Main(string[] args)
        {
            Raytracer raytracer = new(imageWidth: 1600,
                                      aspectRatio: 3.0 / 2.0,
                                      samples: 800,
                                      maxDepth: 100,
                                      shouldDenoise: true,
                                      denoiserPath: Path.GetFullPath(@"..\Denoiser\"),
                                      outputName: "output.png",
                                      outputFolder: Path.GetFullPath(@"..\Renders"),
                                      printProgress: true);

            raytracer.LoadScene(Scene.ModelScene);

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
