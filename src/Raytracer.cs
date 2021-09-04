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
using System.Collections.Generic;

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
        public float Progress;
        public int Samples;

        private Vector3d _background;
        public double _apectRatio;
        private int _maxDepth;
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
            Samples = samples;
            _maxDepth = maxDepth;

            _denoise = shouldDenoise;
            _denoiserPath = denoiserPath;
            _outputName = outputName;
            _outputFolder = outputFolder;
            _printProgress = printProgress;

            RenderImage = new Image<Rgba32>(ImageWidth, ImageHeight);
            FinishedRendering = true;
            _background = new(1);
            Progress = 0;
        }

        public delegate void SceneCaller(ref double aspectRatio, ref Vector3d background, out ObjectList world, out Camera camera);

        public void LoadScene(SceneCaller loadScene)
        {
            loadScene(ref _apectRatio, ref _background, out World, out Camera);
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

        public Task Render(Vector2i from, Vector2i to)
        {
            if (!FinishedRendering)
            {
                Console.WriteLine("An Instance is currently rendering. Please wait for it to finish.");
                return Task.CompletedTask;
            }

            Stopwatch stopWatch = new();
            stopWatch.Start();
            FinishedRendering = false;

            Parallel.For(from.X, to.X, (j) =>
                {
                    int derivedIndex = ImageHeight - j;
                    Parallel.For(from.Y, to.Y, (i) =>
                    {
                        Vector3d color = Vector3d.Zero;
                        for (int s = 0; s < Samples; s++)
                        {
                            var u = (i + RandomHelper.RandomDouble()) / (ImageWidth - 1);
                            var v = (j + RandomHelper.RandomDouble()) / (ImageHeight - 1);
                            Ray r = Camera.GetRay(u, v);
                            color += Ray.RayColor(r, _background, World, _maxDepth);
                        }
                        var sampledColor = Ray.GetColor(color, Samples);
                        RenderImage[(int)i, derivedIndex - 1] = new Rgba32((float)sampledColor.X / 255.0f,
                                                                            (float)sampledColor.Y / 255.0f,
                                                                            (float)sampledColor.Z / 255.0f,
                                                                            1);
                    });
                });

            stopWatch.Stop();
            if (_printProgress)
                Console.WriteLine($"Render time:\t\t {stopWatch.Elapsed.TotalSeconds.ToString("0.000 s")}");

            // Need to wait for a while since otherwise writing to the image array could be interrupted
            Thread.Sleep(100);
            FinishedRendering = true;
            return Task.CompletedTask;
        }

        struct Chunk
        {
            public int xStart;
            public int xEnd;
            public int yStart;
            public int yEnd;
            public Chunk(int x0, int x1, int y0, int y1)
            {
                xStart = x0;
                xEnd = x1;
                yStart = y0;
                yEnd = y1;
            }
        };

        private List<Chunk> GetSpiralChunks()
        {
            // Split Image into chunks
            List<Chunk> chunks = new();
            int chunkSize = 10;
            int numChunksX = ImageHeight / chunkSize;
            int numChunksY = ImageWidth / chunkSize;
            int leftOverX = ImageHeight % chunkSize;
            int leftOverY = ImageWidth % chunkSize;

            // Get chunk spiral pattern
            List<Vector2> chunkIndexes = new();
            int X = numChunksX;
            int Y = numChunksY;
            int sp_x, sp_y, dx, dy;
            sp_x = sp_y = dx = 0;
            dy = -1;
            int t = Math.Max(X, Y);
            int maxI = t * t;
            for (int i = 0; i < maxI; i++)
            {
                if ((-X / 2 <= sp_x) && (sp_x <= X / 2) && (-Y / 2 <= sp_y) && (sp_y <= Y / 2))
                {
                    int x0 = sp_x * chunkSize - chunkSize / 2 + ImageHeight / 2;
                    int x1 = sp_x * chunkSize + chunkSize / 2 + ImageHeight / 2;
                    int y0 = sp_y * chunkSize - chunkSize / 2 + ImageWidth / 2;
                    int y1 = sp_y * chunkSize + chunkSize / 2 + ImageWidth / 2;
                    if (x0 > 0 && x1 < ImageHeight && y0 > 0 && y1 < ImageWidth)
                    {
                        chunks.Add(new Chunk(x0, x1, y0, y1));
                    }
                }
                if ((sp_x == sp_y) || ((sp_x < 0) && (sp_x == -sp_y)) || ((sp_x > 0) && (sp_x == 1 - sp_y)))
                {
                    t = dx;
                    dx = -dy;
                    dy = t;
                }
                sp_x += dx;
                sp_y += dy;
            }

            // Construct spiral edge chunks
            for (var x = 0; x < numChunksX; x++)
            {
                // Left
                chunks.Add(new Chunk(x * chunkSize, x * chunkSize + chunkSize, 0, chunkSize));
                // Right
                chunks.Add(new Chunk(x * chunkSize, x * chunkSize + chunkSize, ImageWidth - chunkSize, ImageWidth));
            }
            for (var y = 0; y < numChunksY; y++)
            {
                // Top
                chunks.Add(new Chunk(ImageHeight - chunkSize, ImageHeight, y * chunkSize, y * chunkSize + chunkSize));
                // Bottom
                chunks.Add(new Chunk(0, chunkSize + 1, y * chunkSize, y * chunkSize + chunkSize));
            }

            return chunks;

        }
        public Task RenderSpiral()
        {
            if (!FinishedRendering)
            {
                Console.WriteLine("An Instance is currently rendering. Please wait for it to finish.");
                return Task.CompletedTask;
            }

            Stopwatch stopWatch = new();
            stopWatch.Start();
            FinishedRendering = false;

            List<Chunk> chunks = GetSpiralChunks();

            // Rendering
            float processedChunks = 0;
            foreach (var chunk in chunks)
            {
                processedChunks++;
                Progress = processedChunks / chunks.Count;
                Parallel.For(chunk.xStart, chunk.xEnd, (j) =>
                {
                    int derivedIndex = ImageHeight - j;
                    Parallel.For(chunk.yStart, chunk.yEnd, (i) =>
                    {
                        Vector3d color = Vector3d.Zero;
                        for (int s = 0; s < Samples; s++)
                        {
                            var u = (i + RandomHelper.RandomDouble()) / (ImageWidth - 1);
                            var v = (j + RandomHelper.RandomDouble()) / (ImageHeight - 1);
                            Ray r = Camera.GetRay(u, v);
                            color += Ray.RayColor(r, _background, World, _maxDepth);
                        }
                        var sampledColor = Ray.GetColor(color, Samples);
                        RenderImage[i, derivedIndex - 1] = new Rgba32((float)sampledColor.X / 255.0f,
                                                                      (float)sampledColor.Y / 255.0f,
                                                                      (float)sampledColor.Z / 255.0f,
                                                                      1);
                    });
                });
            }

            stopWatch.Stop();
            if (_printProgress)
                Console.WriteLine($"Render time:\t\t {stopWatch.Elapsed.TotalSeconds.ToString("0.000 s")}");

            Thread.Sleep(100);
            FinishedRendering = true;
            return Task.CompletedTask;
        }
    }
}
