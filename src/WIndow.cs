using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using Raytracer.Display;
using System.Threading.Tasks;
using System.Threading;
using System;
using OpenTK.Mathematics;

namespace Raytracer
{
    public class Window : GameWindow
    {
        private Raytracer _raytracer;
        private Vector2i _mdPos;
        private Vector2i _muPos;

        private readonly float[] _textureVertices =
        {
             1.0f,  1.0f, 0.0f, 1.0f, 1.0f,
             1.0f, -1.0f, 0.0f, 1.0f, 0.0f,
            -1.0f, -1.0f, 0.0f, 0.0f, 0.0f,
            -1.0f,  1.0f, 0.0f, 0.0f, 1.0f
        };
        private readonly uint[] _textureIndices =
        {
            0, 1, 3,
            1, 2, 3
        };

        private int _elementBufferObject;
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private Shader _shader;
        private LiveTexture _previewTexture;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings, Raytracer raytracer)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            _raytracer = raytracer;
            _mdPos = new();
            _muPos = new();
        }


        protected override void OnLoad()
        {
            base.OnLoad();

            // Live Render
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _textureVertices.Length * sizeof(float), _textureVertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _textureIndices.Length * sizeof(uint), _textureIndices, BufferUsageHint.StaticDraw);

            _shader = new Shader("Display/Shaders/shader.vert", "Display/Shaders/shader.frag");
            _shader.Use();
            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            _previewTexture = new();
            _previewTexture.UpdateImage(_raytracer.RenderImage);
            _previewTexture.Use(TextureUnit.Texture0);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            if (!_raytracer.FinishedRendering)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit);
                GL.BindVertexArray(_vertexArrayObject);
                _previewTexture.UpdateImage(_raytracer.RenderImage);
                _previewTexture.Use(TextureUnit.Texture0);
                _shader.Use();
                GL.DrawElements(PrimitiveType.Triangles, _textureIndices.Length, DrawElementsType.UnsignedInt, 0);
                SwapBuffers();
                Title = $"Raytracer [{_raytracer.Progress.ToString("0.00%")}]";

                Thread.Sleep(10);
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var input = KeyboardState;
            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            else if (input.IsKeyDown(Keys.S))
            {
                _raytracer.SaveImage();
            }
            else if (input.IsKeyDown(Keys.R))
            {
                Task.Run(() => _raytracer.RenderSpiral());
                Thread.Sleep(100);
            }
            else if (input.IsKeyDown(Keys.Up))
            {
                _raytracer.Samples++;
                Console.WriteLine($"Increased samples to {_raytracer.Samples}");
                Thread.Sleep(10);
            }
            else if (input.IsKeyDown(Keys.Down))
            {
                _raytracer.Samples--;
                if (_raytracer.Samples < 1)
                    _raytracer.Samples = 1;
                Console.WriteLine($"Decresed samples to {_raytracer.Samples}");
                Thread.Sleep(10);
            }
            else if (input.IsKeyDown(Keys.Right))
            {
                _raytracer.Samples++;
                Console.WriteLine($"Increased samples to {_raytracer.Samples}");
                Thread.Sleep(200);
            }
            else if (input.IsKeyDown(Keys.Left))
            {
                _raytracer.Samples--;
                if (_raytracer.Samples < 1)
                    _raytracer.Samples = 1;
                Console.WriteLine($"Decreased samples to {_raytracer.Samples}");
                Thread.Sleep(200);
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.BindVertexArray(_vertexArrayObject);
            _previewTexture.UpdateImage(_raytracer.RenderImage);
            _previewTexture.Use(TextureUnit.Texture0);
            _shader.Use();
            GL.DrawElements(PrimitiveType.Triangles, _textureIndices.Length, DrawElementsType.UnsignedInt, 0);
            SwapBuffers();
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                _mdPos.X = (int)MousePosition.X;
                _mdPos.Y = (int)MousePosition.Y;
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                _muPos.X = (int)MousePosition.X;
                _muPos.Y = (int)MousePosition.Y;

                int fromY = Math.Min(_mdPos.X, _muPos.X);
                int toY = Math.Max(_mdPos.X, _muPos.X);
                int toX = _raytracer.ImageHeight - Math.Min(_mdPos.Y, _muPos.Y);
                int fromX = _raytracer.ImageHeight - Math.Max(_mdPos.Y, _muPos.Y);

                Task.Run(() => _raytracer.Render(new Vector2i(fromX, fromY), new Vector2i(toX, toY)));
            }
            base.OnMouseUp(e);
        }
    }
}