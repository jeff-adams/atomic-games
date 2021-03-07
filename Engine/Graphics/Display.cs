using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine
{
    public class Display : IDisposable
    {
        private Game game;
        private RenderTarget2D target;
        private bool targetIsSet;
        
        private float prefferedAspectRatio;
        private Rectangle renderRect;

        public Display(Game game, int width, int height)
        {
            this.game = game ?? throw new ArgumentNullException("game");
            prefferedAspectRatio = (float)width / height;
            target = new RenderTarget2D(game.GraphicsDevice, width, height);
            game.Window.ClientSizeChanged += new EventHandler<EventArgs>(UpdateScreenSize);
            UpdateScreenSize(null, null);

            targetIsSet = false;
        }

        public void UpdateScreenSize(object sender, EventArgs e)
        {
            renderRect = GetRenderTargetPositionAsRectangle();
        }

        public void SetTarget()
        {
            if (targetIsSet)
            {
                throw new Exception("RenderTarget is already set.");
            }
            
            game.GraphicsDevice.SetRenderTarget(target);
            targetIsSet = true;
        }

        public void UnSetTarget()
        {
            if (!targetIsSet)
            {
                throw new Exception("RenderTarget is not set.");
            }

            game.GraphicsDevice.SetRenderTarget(null);
            targetIsSet = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            game.GraphicsDevice.Clear(Color.DarkMagenta);
            spriteBatch.Begin();
            spriteBatch.Draw(target, renderRect, Color.White);
            spriteBatch.End();
        }

        private Rectangle GetRenderTargetPositionAsRectangle()
        {
            Rectangle clientBounds = game.Window.ClientBounds;
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
            game.Window.ClientSizeChanged -= UpdateScreenSize;
            target?.Dispose();
        }
    }
}