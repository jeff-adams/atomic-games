using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine.Graphics
{
    public class Display : IDisposable
    {
        private readonly GraphicsDevice graphics;
        private readonly GameWindow window;
        private readonly Camera camera;
        private readonly RenderTarget2D target;
        private readonly float prefferedAspectRatio;

        private bool targetIsSet;
        private Rectangle renderRect;

        public Rectangle RenderRectangle => renderRect;

        public int Width { get; }
        public int Height { get; }

        public Display(AtomicGame game, int width, int height)
        {
            this.camera = game.Camera;
            this.graphics = game.GraphicsDevice;
            this.window = game.Window;
            prefferedAspectRatio = (float)width / height;
            target = new RenderTarget2D(this.graphics, width, height);
            this.window.ClientSizeChanged += UpdateScreenSize;
            UpdateScreenSize(null, null);

            Width = width;
            Height = height;

            targetIsSet = false;
        }


        public void SetTarget()
        {
            if (targetIsSet)
            {
                throw new Exception("RenderTarget is already set.");
            }
            
            graphics.SetRenderTarget(target);
            targetIsSet = true;
        }

        public void UnSetTarget()
        {
            if (!targetIsSet)
            {
                throw new Exception("RenderTarget is not set.");
            }

            graphics.SetRenderTarget(null);
            targetIsSet = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            graphics.Clear(Color.Black);
            spriteBatch.Begin(transformMatrix: camera.ViewMatrix);
            spriteBatch.Draw(target, renderRect, Color.White);
            spriteBatch.End();
        }
        
        private void UpdateScreenSize(object sender, EventArgs e) =>
            renderRect = GetRenderTargetPositionAsRectangle();

        private Rectangle GetRenderTargetPositionAsRectangle()
        {
            Rectangle clientBounds = window.ClientBounds;
            float currentAspectRatio = (float) clientBounds.Width / clientBounds.Height;
            int x = 0;
            int y = 0;
            int width = clientBounds.Width;
            int height = clientBounds.Height;

            if (currentAspectRatio < prefferedAspectRatio)
            {
                // output is taller than it is wider, bars on top/bottom
                height = (int)((clientBounds.Width / prefferedAspectRatio) + 0.5f);
                y = (clientBounds.Height - height) / 2;
            }
            else if (currentAspectRatio > prefferedAspectRatio)
            {
                // output is wider than it is tall, bars left/right
                width = (int)((clientBounds.Height * prefferedAspectRatio) + 0.5f);
                x = (clientBounds.Width - width) / 2;
            }

            return new Rectangle(x, y, width, height);
        }

        public void Dispose()
        {
            window.ClientSizeChanged -= UpdateScreenSize;
            target?.Dispose();
        }
    }
}