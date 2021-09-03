using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Raytracer.Hitables;
using Raytracer.Utility;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using OpenTK.Mathematics;
using Raytracer.Core;
using System.Threading;

namespace Raytracer
{
    public class Raytracer
    {
        public Camera Camera;
        public ObjectList World;
        public int ImageWidth;
        public int ImageHeight;
        public Image<Rgba32> RenderImage;
        public bool FinishedRendering;

        private Vector3d _background;
        public double _apectRatio;
        private int _maxDepth;
        private int _samples;
        private bool _denoise;
        private string _denoiserPath;
        private string _outputName;
        private string _outputFolder;
        private bool _printProgress;


        public Raytracer(int imageWidth, double aspectRatio, int samples, int maxDepth, bool shouldDenoise, string denoiserPath, string outputName, string outputFolder, bool printProgress)
        {
            ImageWidth = imageWidth;
            _apectRatio = aspectRatio;
            ImageHeight = (int)(ImageWidth / _apectRatio);
            _samples = samples;
            _maxDepth = maxDepth;

            _denoise = shouldDenoise;
            _denoiserPath = denoiserPath;
            _outputName = outputName;
            _outputFolder = outputFolder;
            _printProgress = printProgress;

            RenderImage = new Image<Rgba32>(ImageWidth, ImageHeight);
            FinishedRendering = false;
            _background = new(1);
        }

        public async Task Render()
        {
            Stopwatch stopWatch = new();
            int progress = 0;
            stopWatch.Start();

            Parallel.For(0, ImageHeight, (j) =>
                {
                    if (_printProgress)
                        Console.WriteLine($"Rendering Line {++progress}/{ImageHeight} \t[{((float)progress / ImageHeight * 100.0).ToString("0.00")}%]");
                    int derivedIndex = ImageHeight - j;
                    Parallel.For(0, ImageWidth, (i) =>
                    {
                        Vector3d color = Vector3d.Zero;
                        for (int s = 0; s < _samples; s++)
                        {
                            var u = (i + RandomHelper.RandomDouble()) / (ImageWidth - 1);
                            var v = (j + RandomHelper.RandomDouble()) / (ImageHeight - 1);
                            Ray r = Camera.GetRay(u, v);
                            color += Ray.RayColor(r, _background, World, _maxDepth);
                        }
                        var sampledColor = Ray.GetColor(color, _samples);
                        RenderImage[(int)i, derivedIndex - 1] = new Rgba32((float)sampledColor.X / 255.0f,
                                                                            (float)sampledColor.Y / 255.0f,
                                                                            (float)sampledColor.Z / 255.0f,
                                                                            1);
                    });
                });

            stopWatch.Stop();
            if (_printProgress)
                Console.WriteLine($"Render time:\t\t {stopWatch.Elapsed.TotalSeconds.ToString("0.000 s")}");

            Thread.Sleep(100);
            FinishedRendering = true;
        }

        public void SaveImage()
        {
            System.IO.Directory.CreateDirectory(_outputFolder);
            string savePath = Path.Combine(_outputFolder, _outputName);
            RenderImage.Save(savePath);

            if (_denoise)
            {
                Stopwatch denoiseTimer = new();
                denoiseTimer.Start();

                string path = Path.GetFullPath(Path.Combine(_denoiserPath, "Denoiser.exe"));
                string outputPath = Path.Combine(_outputFolder, "denoised_" + _outputName);
                string parameters = $"-i {savePath} -o {outputPath}";

                ProcessStartInfo startInfo = new ProcessStartInfo(path);
                startInfo.Arguments = parameters;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                var proc = System.Diagnostics.Process.Start(startInfo);
                proc.WaitForExit();

                denoiseTimer.Stop();
                var denoiseTime = denoiseTimer.Elapsed;
                if (_printProgress)
                    Console.WriteLine($"Denoise time:\t\t {denoiseTime.TotalSeconds.ToString("0.000 s")}");
            }
        }

        public delegate void SceneCaller(ref double aspectRatio, ref Vector3d background, out ObjectList world, out Camera camera);

        public void LoadScene(SceneCaller loadScene)
        {
            loadScene(ref _apectRatio, ref _background, out World, out Camera);
        }
    }
}
