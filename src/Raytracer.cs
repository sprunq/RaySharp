using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Raytracer.Core;
using Raytracer.Core.Hitables;
using Raytracer.Core.Materials;
using Raytracer.Core.Textures;
using Raytracer.Utility;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using OpenTK.Mathematics;
using System.Collections.Generic;
using Raytracer.Scenes;

namespace Raytracer
{
    class Raytracer
    {
        public Camera Camera;
        public ObjectList World;

        private Image<Rgba32> _renderImage;
        private Vector3d _background;
        private double _aspectRatio;
        private int _imageWidth;
        private int _imageHeight;
        private int _maxDepth;
        private int _samples;
        private bool _denoise;
        private string _outputName;
        private string _outputFolder;
        private bool _printProgress;


        public Raytracer(int imageWidth, double aspectRatio, int samples, int maxDepth, bool shouldDenoise, string outputName, string outputFolder, bool printProgress)
        {
            _imageWidth = imageWidth;
            _aspectRatio = aspectRatio;
            _imageHeight = (int)(_imageWidth / _aspectRatio);
            _samples = samples;
            _maxDepth = maxDepth;

            _denoise = shouldDenoise;
            _outputName = outputName;
            _outputFolder = outputFolder;
            _printProgress = printProgress;

            _background = new(1);
        }

        public void Render()
        {
            _renderImage = new Image<Rgba32>(_imageWidth, _imageHeight);
            Stopwatch stopWatch = new();
            int progress = 0;
            stopWatch.Start();

            Parallel.For(0, _imageHeight, (j) =>
                {
                    if (_printProgress)
                        Console.WriteLine($"Rendering Line {++progress}/{_imageHeight} \t[{((float)progress / _imageHeight * 100.0).ToString("0.00")}%]");
                    int derivedIndex = _imageHeight - j;
                    Parallel.For(0, _imageWidth, (i) =>
                    {
                        Vector3d color = Vector3d.Zero;
                        for (int s = 0; s < _samples; s++)
                        {
                            var u = (i + RandomHelper.RandomDouble()) / (_imageWidth - 1);
                            var v = (j + RandomHelper.RandomDouble()) / (_imageHeight - 1);
                            Ray r = Camera.GetRay(u, v);
                            color += Ray.RayColor(r, _background, World, _maxDepth);
                        }
                        var sampledColor = Ray.GetColor(color, _samples);
                        _renderImage[(int)i, derivedIndex - 1] = new Rgba32((float)sampledColor.X / 255.0f,
                                                                            (float)sampledColor.Y / 255.0f,
                                                                            (float)sampledColor.Z / 255.0f,
                                                                            1);
                    });
                });

            stopWatch.Stop();
            if (_printProgress)
                Console.WriteLine($"Render time:\t\t {stopWatch.Elapsed.TotalSeconds.ToString("0.000 s")}");
        }

        public void SaveImage()
        {
            System.IO.Directory.CreateDirectory(_outputFolder);
            string savePath = Path.Combine(_outputFolder, _outputName);
            _renderImage.Save(savePath);

            if (_denoise)
            {
                Stopwatch denoiseTimer = new();
                denoiseTimer.Start();

                string path = Path.GetFullPath(Path.Combine(@"..\Denoiser\Denoiser_v2.4\", "Denoiser.exe"));
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

        public delegate void Loader(ref double aspectRatio, ref Vector3d background, out ObjectList world, out Camera camera);

        public void LoadScene(Loader func)
        {
            func(ref _aspectRatio, ref _background, out World, out Camera);
        }
    }
}
