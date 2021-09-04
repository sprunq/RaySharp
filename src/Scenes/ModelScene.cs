using Raytracer.Core;
using Raytracer.Hitables;
using Raytracer.Textures;
using OpenTK.Mathematics;
using Raytracer.Instances;
using Raytracer.Materials;

namespace Raytracer.Scenes
{
    public partial class Scene
    {
        public static void ModelScene(ref double _aspectRatio, ref Vector3d background, out ObjectList world, out Camera camera)
        {
            Vector3d lookfrom = new(-50, 12, 0);
            Vector3d lookat = new(1, 11.5, 0);
            Vector3d vup = new(0, 1, 0);
            var focusDist = 10;
            var aperture = 0.0;
            var fov = 50;

            camera = new(lookfrom, lookat, vup, fov, _aspectRatio, aperture, focusDist);

            // World
            world = new();
            background = new Vector3d(0);

            // Skybox
            var tSkybox = new ImageTexture(@"..\Textures\HDRI Maps\hilly_terrain.jpg", 1, 0);
            var mSkybox = new Light(tSkybox);
            var hSkybox = new Sphere(new Vector3d(0, -10, 0), 1000, mSkybox);
            var roSkybox = new Rotate(hSkybox, 90, Axis.Y);
            //world.Add(roSkybox);

            // Ground
            var tWood = new ImageTexture(@"..\Textures\wood_planks.jpg", 300, 0);
            var mWood = new Lambertian(tWood);
            var hground = new XZRect(new Vector2d(-1000, 1000), new Vector2d(-1000, 1000), 0, mWood);
            world.Add(hground);

            // Box
            var checkerTexture = new Lambertian(new CheckerTexture(Vector3d.Zero, Vector3d.One));
            var mBox = new Lambertian(new Vector3d(1, 1, 1));
            var hBack = new YZRect(new Vector2d(-50, 50), new Vector2d(-50, 50), 5, checkerTexture);
            var hLeft = new XYRect(new Vector2d(-40, 40), new Vector2d(-40, 40), 20, mBox);
            var hRight = new XYRect(new Vector2d(-40, 40), new Vector2d(-40, 40), -20, mBox);
            var hTop = new XZRect(new Vector2d(-30, 30), new Vector2d(-30, 30), 40, mBox);
            world.Add(hBack);
            world.Add(hLeft);
            world.Add(hRight);
            world.Add(hTop);


            void AddPillar(double boxWidth, double boxHeight, double sphereSize, double angle, Vector3d offset, Material mBox, Material mSphere, ref ObjectList world)
            {
                var hBox = new Box(new Vector3d(-boxWidth / 2.0, 0, -boxWidth / 2.0), new Vector3d(boxWidth / 2.0, boxHeight, boxWidth / 2.0), mBox);
                var roBox = new Rotate(hBox, angle, Axis.Y);
                var trBox = new Translate(roBox, new Vector3d(-boxWidth, 0, 0) + offset);
                world.Add(trBox);

                var hSphere = new Sphere(new Vector3d(0, sphereSize, 0), sphereSize, mSphere);
                var roSphere = new Rotate(hSphere, angle, Axis.Y);
                var trSphere = new Translate(roSphere, new Vector3d(-boxWidth, boxHeight, 0) + offset);
                world.Add(trSphere);
            }

            AddPillar(boxWidth: 6,
                      boxHeight: 5,
                      sphereSize: 4,
                      angle: 140,
                      offset: new Vector3d(-11, 0, -10.5),
                      mBox: new Lambertian(new Vector3d(1)),
                      mSphere: new Metal(new Vector3d(1), 0),
                      ref world);

            AddPillar(boxWidth: 6,
                      boxHeight: 3,
                      sphereSize: 3.5,
                      angle: 40,
                      offset: new Vector3d(-7, 0, 12),
                      mBox: new Lambertian(new Vector3d(1)),
                      mSphere: new Dielectric(1.5, new Vector3d(1)),
                      ref world);


            // Lights
            var mLMain = new Light(new Vector3d(2));
            var hLMain = new Sphere(new Vector3d(-20, 40, 0), 6, mLMain);
            world.Add(hLMain);

            var mL1 = new Light(new Vector3d(10, 0, 0));
            var hL1 = new Sphere(new Vector3d(-10, 3, -5), 3, mL1);
            world.Add(hL1);

            var mL2 = new Light(new Vector3d(0, 10, 0));
            var hL2 = new Sphere(new Vector3d(-7, 1.6, -1), 1.6, mL2);
            world.Add(hL2);

            var mL3 = new Light(new Vector3d(0, 0, 10));
            var hL3 = new Sphere(new Vector3d(-13, 1.2, -6.5), 1.2, mL3);
            world.Add(hL3);

            // Bunny Model
            var modelHeightOffset = 4.0;
            var mBunnyBox = new Lambertian(new Vector3d(1));
            var hBox = new Box(new Vector3d(0, 0, 0), new Vector3d(10, 5 + modelHeightOffset, 10), mBunnyBox);
            var trBox = new Translate(hBox, new Vector3d(-5, 0, -5));
            world.Add(trBox);

            var mModelLambert = new Lambertian(new Vector3d(0.3, 0, 0.5));
            var mModelGlass = new Dielectric(1.5, new Vector3d(0.1, 0, 0.3));
            var mModelMetal = new Metal(new Vector3d(0.3, 1, 0.8), 0.1);
            var hModel = new BVHNode(ObjectList.GetObjFaces(@"..\Models\statue\12328_Statue_v1_L2.obj",
                                                            @"..\Models\statue\12328_Statue_v1_L2.mtl",
                                                            mModelGlass,
                                                            0.1));
            var roXModel = new Rotate(hModel, 90, Axis.X);
            var roYModel = new Rotate(roXModel, 90, Axis.Y);
            var trModel = new Translate(roYModel, new Vector3d(0, 2.8 + modelHeightOffset, 0.1));
            world.Add(trModel);
        }
    }
}
