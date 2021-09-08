using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using Raytracer.Display;
using System.Threading.Tasks;
using System.Threading;
using System;
using OpenTK.Mathematics;
using Raytracer.Core;
using Raytracer.Utility;

namespace Raytracer
{
    public class Window : GameWindow
    {
        private Raytracer _raytracer;
        private Vector2i _mdPos;
        private Vector2i _muPos;
        private Vector2 _textureOffset;
        private Shader _shader;
        private LiveTexture _previewTexture;
        private int _elementBufferObject;
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private float _zoom;
        private float[] _textureVertices;
        private readonly uint[] _textureIndices =
        {
            0, 1, 3,
            1, 2, 3
        };

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings, Raytracer raytracer)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            _raytracer = raytracer;
            _mdPos = new();
            _muPos = new();
            _textureOffset = new(0);
            _zoom = 1;
            _textureVertices = GetLiveTextureVertecies();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

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

            PrintHelpMessage();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            if (_raytracer.UpdateFrame)
            {
                UpdateLiveTexture();
                ulong totalRays = Ray.GetTotalRays();
                Title = $"Raytracer [{_raytracer.Progress.ToString("0.00%")}] Rays: {((decimal)totalRays).DynamicPostFix()}";
                _raytracer.UpdateFrame = false;
            }

            // Sleeping decreases the standby cpu usage from 10% to 0.5%
            Thread.Sleep(1);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var input = KeyboardState;
            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            else if (input.IsKeyPressed(Keys.H) || input.IsKeyPressed(Keys.F1))
            {
                PrintHelpMessage();
            }
            else if (input.IsKeyPressed(Keys.Enter))
            {
                _raytracer.SaveImage();
            }
            else if (input.IsKeyDown(Keys.W))
            {
                _textureOffset.Y -= 0.05f;
                UpdateLiveTexture();
            }
            else if (input.IsKeyDown(Keys.A))
            {
                _textureOffset.X += 0.05f;
                UpdateLiveTexture();
            }
            else if (input.IsKeyDown(Keys.S))
            {
                _textureOffset.Y += 0.05f;
                UpdateLiveTexture();
            }
            else if (input.IsKeyDown(Keys.D))
            {
                _textureOffset.X -= 0.05f;
                UpdateLiveTexture();
            }
            else if (input.IsKeyPressed(Keys.R))
            {
                Task.Run(() => _raytracer.RenderSpiral(20));
            }
            else if (input.IsKeyPressed(Keys.C))
            {
                Ray.ResetRayCount();
                _raytracer.RenderImage = new(_raytracer.ImageWidth, _raytracer.ImageHeight);
                UpdateLiveTexture();
            }
            else if (input.IsKeyDown(Keys.Up))
            {
                _raytracer.Samples++;
                Console.WriteLine($"Increased samples to {_raytracer.Samples}");
            }
            else if (input.IsKeyDown(Keys.Down))
            {
                _raytracer.Samples--;
                if (_raytracer.Samples < 1)
                    _raytracer.Samples = 1;
                Console.WriteLine($"Decresed samples to {_raytracer.Samples}");
            }
            else if (input.IsKeyPressed(Keys.Right))
            {
                _raytracer.Samples++;
                Console.WriteLine($"Increased samples to {_raytracer.Samples}");
            }
            else if (input.IsKeyPressed(Keys.Left))
            {
                _raytracer.Samples--;
                if (_raytracer.Samples < 1)
                    _raytracer.Samples = 1;
                Console.WriteLine($"Decreased samples to {_raytracer.Samples}");
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            UpdateLiveTexture();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                _mdPos.X = (int)(MousePosition.X);
                _mdPos.Y = (int)(MousePosition.Y);
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                _muPos.X = (int)(MousePosition.X);
                _muPos.Y = (int)(MousePosition.Y);

                int fromY = Math.Min(_mdPos.X, _muPos.X);
                int toY = Math.Max(_mdPos.X, _muPos.X);
                int toX = _raytracer.ImageHeight - Math.Min(_mdPos.Y, _muPos.Y);
                int fromX = _raytracer.ImageHeight - Math.Max(_mdPos.Y, _muPos.Y);

                Task.Run(() => _raytracer.Render(new Vector2i(fromX, fromY), new Vector2i(toX, toY)));
            }

            base.OnMouseUp(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.OffsetY > 0)
                _zoom += 0.05f;
            else if (e.OffsetY < 0 && _zoom > 0.1)
                _zoom -= 0.05f;

            UpdateLiveTexture();
        }

        private void UpdateLiveTexture()
        {
            _textureVertices = GetLiveTextureVertecies();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _textureVertices.Length * sizeof(float), _textureVertices, BufferUsageHint.StaticDraw);
            GL.Viewport(0, 0, Size.X, Size.Y);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.BindVertexArray(_vertexArrayObject);
            _previewTexture.UpdateImage(_raytracer.RenderImage);
            _previewTexture.Use(TextureUnit.Texture0);
            _shader.Use();
            GL.DrawElements(PrimitiveType.Triangles, _textureIndices.Length, DrawElementsType.UnsignedInt, 0);
            SwapBuffers();
        }

        private float[] GetLiveTextureVertecies()
        {
            float[] verts =
            {
                 1.0f*_zoom+_textureOffset.X,  1.0f*_zoom+_textureOffset.Y, 0.0f, 1.0f, 1.0f,
                 1.0f*_zoom+_textureOffset.X, -1.0f*_zoom+_textureOffset.Y, 0.0f, 1.0f, 0.0f,
                -1.0f*_zoom+_textureOffset.X, -1.0f*_zoom+_textureOffset.Y, 0.0f, 0.0f, 0.0f,
                -1.0f*_zoom+_textureOffset.X,  1.0f*_zoom+_textureOffset.Y, 0.0f, 0.0f, 1.0f
            };
            return verts;
        }

        private void PrintHelpMessage()
        {
            string helpMessage = "";
            helpMessage += "\n-----------------------------------";
            helpMessage += "\nRaySharp Controls";
            helpMessage += "\n// Movement";
            helpMessage += "\n- WASD: Movement";
            helpMessage += "\n- Mousewheel: Zoom";
            helpMessage += "\n// Rendering";
            helpMessage += "\n- Arrow Keys: In-/Decrease Samples";
            helpMessage += "\n- R: Render Image";
            helpMessage += "\n- Enter: Save Image";
            helpMessage += "\n// Misc";
            helpMessage += "\n- Mouse Drag: Render Area";
            helpMessage += "\n- C: Clear Image";
            helpMessage += "\n- Esc: Exit";
            helpMessage += "\n-----------------------------------";
            helpMessage += "\n";
            Console.WriteLine(helpMessage);
        }
    }
}