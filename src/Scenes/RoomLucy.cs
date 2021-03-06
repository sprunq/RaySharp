using Raytracer.Core;
using Raytracer.Hitables;
using Raytracer.Textures;
using OpenTK.Mathematics;
using Raytracer.Hitables.Instances;

namespace Raytracer.Scenes
{
    public partial class Scene
    {
        public static void RoomLucy(ref double _aspectRatio, ref Vector3d background, out ObjectList world, out Camera camera)
        {
            Vector3d lookfrom = new(-50, 12, 0);
            Vector3d lookat = new(0, 11.5, 0);
            Vector3d vup = new(0, 1, 0);
            var focusDist = 10;
            var aperture = 0.0;
            var fov = 50;

            camera = new(lookfrom, lookat, vup, fov, _aspectRatio, aperture, focusDist);

            // World
            world = new();
            background = new Vector3d(0);

            // Skybox
            var tSkybox = new ImageTexture(@"..\Textures\HDRI Maps\sunflowers.jpg", 1, 0, 1);
            var mSkybox = new Light(tSkybox);
            var hSkybox = new Sphere(new Vector3d(0, -100, 0), 1000, mSkybox);
            var roSkybox = new Rotate(hSkybox, 90, Axis.Y);
            world.Add(roSkybox);

            // Ground
            var tWood = new ImageTexture(@"..\Textures\wood_planks.jpg", 300, 0, 1);
            var mWood = new Lambertian(tWood);
            var hground = new XZRect(new Vector2d(-1000, 1000), new Vector2d(-1000, 1000), 0, mWood);
            world.Add(hground);

            // Box
            var checkerTexture = new Lambertian(new CheckerTexture(Vector3d.Zero, Vector3d.One, 0.2));
            var mBox = new Lambertian(new Vector3d(1, 1, 1));
            var hBack = new YZRect(new Vector2d(-50, 50), new Vector2d(-50, 50), 5, checkerTexture);
            var hLeft = new XYRect(new Vector2d(-40, 40), new Vector2d(-40, 40), 20, mBox);
            var hRight = new XYRect(new Vector2d(-40, 40), new Vector2d(-40, 40), -20, mBox);
            var hTop = new XZRect(new Vector2d(-40, 40), new Vector2d(-40, 40), 40, mBox);
            world.Add(hBack);
            world.Add(hLeft);
            world.Add(hRight);
            world.Add(hTop);

            BoxSpherePillar(boxWidth: 6,
                            boxHeight: 5,
                            sphereSize: 4,
                            angle: 140,
                            offset: new Vector3d(-11, 0, -10.5),
                            mBox: new Lambertian(new Vector3d(1)),
                            mSphere: new Metal(new Vector3d(1), 0),
                            ref world);

            BoxSpherePillar(boxWidth: 6,
                            boxHeight: 3,
                            sphereSize: 3.5,
                            angle: 40,
                            offset: new Vector3d(-7, 0, 12),
                            mBox: new Lambertian(new Vector3d(1)),
                            mSphere: new Dielectric(1.6, new Vector3d(1)),
                            ref world);


            // Lights
            var mLMain = new Light(new Vector3d(4));
            var hLMain = new Sphere(new Vector3d(-20, 40, 0), 6, mLMain);
            //world.Add(hLMain);

            var mL1 = new Light(new Vector3d(20, 0, 0));
            var hL1 = new Sphere(new Vector3d(-10, 3, -5), 3, mL1);
            world.Add(hL1);

            var mL2 = new Light(new Vector3d(0, 15, 0));
            var hL2 = new Sphere(new Vector3d(-7, 1.6, -1), 1.6, mL2);
            world.Add(hL2);

            var mL3 = new Light(new Vector3d(0, 0, 15));
            var hL3 = new Sphere(new Vector3d(-13, 1.2, -6.5), 1.2, mL3);
            world.Add(hL3);

            // Model
            var modelHeightOffset = 4.0;
            var mBunnyBox = new Lambertian(new Vector3d(1));
            var hBox = new Box(new Vector3d(0, 0, 0), new Vector3d(10, 5 + modelHeightOffset, 10), mBunnyBox);
            var trBox = new Translate(hBox, new Vector3d(-5, 0, -5));
            world.Add(trBox);

            var mModelLambert = new Lambertian(new Vector3d(0.3, 0, 0.5));
            var mModelGlass = new Dielectric(1.5, new Vector3d(0.4, 0, 0.5));
            var mModelMetal = new Metal(new Vector3d(1), 0.1);

            var hModel = new Mesh(@"..\Models\lucy.obj", mModelLambert, 0.065);
            var roXModel = new Rotate(hModel, 0, Axis.X);
            var roYModel = new Rotate(roXModel, -90, Axis.Y);
            var trModel = new Translate(roYModel, new Vector3d(0, 5 + modelHeightOffset, 0));
            world.Add(trModel);
        }
    }
}
