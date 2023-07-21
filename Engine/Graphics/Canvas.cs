using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine.Graphics
{
    public class Canvas : IDisposable
    {
        public int Width { get; }
        public int Height { get; }
        public Rectangle RenderRectangle => renderRect;

        private readonly GraphicsDevice graphics;
        private readonly RenderTarget2D target;

        private Rectangle renderRect;
        private float prefferedAspectRatio;
        
        public Canvas(GraphicsDevice graphics, int width, int height)
        {
            Width = width;
            Height = height;
            prefferedAspectRatio = (float)width / height;

            this.graphics = graphics;

            target = new RenderTarget2D(this.graphics, width, height);
        }


        internal void Activate()
        {          
            graphics.SetRenderTarget(target);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            graphics.SetRenderTarget(null);
            graphics.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(target, renderRect, Color.White);
            spriteBatch.End();
        }

        internal Rectangle UpdateRenderRectangle()
        {
            Rectangle screenSize = graphics.PresentationParameters.Bounds;
            
            float scaleX = (float)screenSize.Width / target.Width;
            float scaleY = (float)screenSize.Height / target.Height;
            float scale = Math.Min(scaleX, scaleY);

            int width = (int)(target.Width * scale);
            int height = (int)(target.Height * scale);

            int x = (screenSize.Width - width) / 2;
            int y = (screenSize.Height - height) / 2;

            renderRect = new Rectangle(x, y, width, height);
            return renderRect;
        }

        public void Dispose()
        {
            target?.Dispose();
        }
    }
}