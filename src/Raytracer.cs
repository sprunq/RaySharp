using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using Raytracer.Core;
using Raytracer.Core.Hitables;
using Raytracer.Core.Materials;
using Raytracer.Core.Textures;
using Raytracer.Utility;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using ObjLoader;
using ObjLoader.Loader.Loaders;
using OpenTK.Mathematics;

namespace Raytracer
{
    class Raytracer
    {
        public Camera Camera;
        public ObjectList World;

        private Image<Rgba32> _renderImage;
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

            World = new();
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
                    Vector3d color = new Vector3d(0, 0, 0);
                    for (int s = 0; s < _samples; s++)
                    {
                        var u = (i + RandomHelper.RandomDouble()) / (_imageWidth - 1);
                        var v = (j + RandomHelper.RandomDouble()) / (_imageHeight - 1);
                        Ray r = Camera.GetRay(u, v);
                        color += Ray.RayColor(r, World, _maxDepth);
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
                if (_printProgress)
                    Console.WriteLine("Denoising image");
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

        public void LoadScene(int id)
        {
            switch (id)
            {
                case 1:
                    loadScene1();
                    break;
                case 2:
                    loadScene2();
                    break;
                case 3:
                    objTestScene();
                    break;
                default:
                    break;
            }
        }

        private void loadScene1()
        {
            _aspectRatio = 3.0 / 2.0;
            Vector3d lookfrom = new(5, 1.4, -5);
            Vector3d lookat = new(0.5, 0.7, -0.9);
            Vector3d vup = new(0, 1, 0);
            var focusDist = 1;
            var aperture = 0.0;
            var fov = 15;

            Camera = new(lookfrom, lookat, vup, fov, _aspectRatio, aperture, focusDist);

            var earthTexture = new ImageTexture(@"..\Textures\wood_planks.jpg", 20);
            var earthSurface = new Lambertian(earthTexture);
            var globe = new Sphere(new Vector3d(0, 2, 0), 2, earthSurface);
            World.Add(globe);

            var checkerTexture = new CheckerTexture(Vector3d.Zero, Vector3d.One);
            World.Add(new Sphere(new Vector3d(0, -1000, 0), 1000, new Lambertian(checkerTexture)));

            var m1 = new Lambertian(new Vector3d(0.2, 0.7, 0.1));
            var m2 = new Metal(new Vector3d(1, 1, 1), 0);
            var m3 = new Dielectric(1.5);
            var m4 = new Metal(new Vector3d(0.3, 0.3, 0.3), 0.2);

            World.Add(new Sphere(new Vector3d(2.3, 0.7, -.3), 0.7, m3));
            World.Add(new Sphere(new Vector3d(-3.5, 1.3, -2), 1.3, m2));
            World.Add(new Sphere(new Vector3d(-.4, 0.4, -2.1), 0.3, m4));
        }

        private void loadScene2()
        {
            Vector3d lookfrom = new(8, 1.5, 2);
            Vector3d lookat = new(0, 0.4, -0.25);
            Vector3d vup = new(0, 1, 0);
            var focusDist = 10;
            var aperture = 0.0;
            var fov = 27;

            Camera = new(lookfrom, lookat, vup, fov, _aspectRatio, aperture, focusDist);

            var checkerTexture = new CheckerTexture(Vector3d.Zero, Vector3d.One);
            World.Add(new Sphere(new Vector3d(0.0f, -1000, 0.0), 1000.0, new Lambertian(checkerTexture)));

            for (int a = -11; a < 11; a++)
            {
                for (int b = -11; b < 11; b++)
                {
                    var materialType = RandomHelper.RandomDouble();
                    Vector3d center = new(a + 0.9 * (float)RandomHelper.RandomDouble(), 0.2, b + 0.9 * (float)RandomHelper.RandomDouble());

                    if ((center - new Vector3d(4, 0.2, 0)).Length > 0.9)
                    {
                        Material material;

                        if (materialType < 0.8)
                        {
                            // diffuse
                            var albedo = Vector3Helper.RandomVec3();
                            material = new Lambertian(albedo);
                            World.Add(new Sphere(center, 0.2, material));
                        }
                        else if (materialType < 0.95)
                        {
                            // metal
                            var albedo = Vector3Helper.RandomVec3();
                            var fuzz = RandomHelper.RandomDouble();
                            material = new Metal(albedo, fuzz);
                            World.Add(new Sphere(center, 0.2, material));
                        }
                        else
                        {
                            // glass
                            material = new Dielectric(1.5);
                            World.Add(new Sphere(center, 0.2, material));
                        }
                    }
                }
            }

            var glass = new Dielectric(1.5);
            var lambert = new Lambertian(new Vector3d(0.4, 0.2, 0.1));
            var metal = new Metal(new Vector3d(0.7, 0.6, 0.5), 0.1);

            World.Add(new Sphere(new Vector3d(-4, 1, 0), 1.0, lambert));
            World.Add(new Sphere(new Vector3d(0, 0.75, 0), 0.75, glass));
            World.Add(new Sphere(new Vector3d(4, 0.5, 0), 0.5, metal));
        }


        private void objTestScene()
        {
            Vector3d lookfrom = new(8, 1.8, -3);
            Vector3d lookat = new(-2, 1.8, 4);
            Vector3d vup = new(0, 1, 0);
            var focusDist = 10;
            var aperture = 0.0;
            var fov = 45;

            Camera = new(lookfrom, lookat, vup, fov, _aspectRatio, aperture, focusDist);

            var checkerTexture = new CheckerTexture(Vector3d.Zero, Vector3d.One);
            World.Add(new Sphere(new Vector3d(0.0f, -1000f, 0.0f), 1000.0, new Lambertian(checkerTexture)));

            var glass = new Dielectric(1.5);
            var lambert = new Lambertian(new Vector3d(1, 0, 0));
            var metal = new Metal(new Vector3d(0.7, 0.6, 0.5), 0.1);

            World.Add(new Sphere(new Vector3d(3.8, 1.4, 2.2), 1.4, metal));
            World.Add(new Sphere(new Vector3d(3.1, 0.65, -0.7), 0.65, glass));
            World.Add(new Sphere(new Vector3d(4.3, 0.45, 0.2), 0.45, lambert));


            var gray = new Lambertian(new Vector3d(0.8, 0.8, 0.8));
            var pos = new Vector3d(0.3, 0, 0.2);
            World.AddObjModel(@"..\Models\teapot.obj", pos, gray);
        }
    }
}
