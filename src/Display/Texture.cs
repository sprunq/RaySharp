using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using Image = SixLabors.ImageSharp.Image;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace Raytracer.Display
{
    public class LiveTexture
    {
        public readonly int Handle;

        public void UpdateImage(ref Image<Rgba32> img)
        {
            var _image = img.Clone(x => x.Flip(FlipMode.Vertical));
            var pixels = new byte[4 * _image.Width * _image.Height];

            Parallel.For(0, _image.Height, (y) =>
            {
                var row = _image.GetPixelRowSpan(y);

                for (int x = 0; x < _image.Width; x++)
                {
                    pixels[y * _image.Width * 4 + x * 4] = row[x].R;
                    pixels[y * _image.Width * 4 + x * 4 + 1] = row[x].G;
                    pixels[y * _image.Width * 4 + x * 4 + 2] = row[x].B;
                    pixels[y * _image.Width * 4 + x * 4 + 3] = row[x].A;
                }
            });

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            GL.TexImage2D(TextureTarget.Texture2D,
                          0,
                          PixelInternalFormat.Rgba,
                          _image.Width,
                          _image.Height,
                          0,
                          PixelFormat.Rgba,
                          PixelType.UnsignedByte,
                          pixels);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public LiveTexture()
        {
            Handle = GL.GenTexture();
        }

        public void Use(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
    }
}