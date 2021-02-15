using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Graphics
{
    public class Display : IDisposable
    {
        private Game game;
        private SpriteBatch spriteBatch;
        private RenderTarget2D target;
        private bool targetIsSet = false;
        
        private float prefferedAspectRatio = 1f;
        private float currentAspectRatio = 1f;
        private float windowScale;
        private Rectangle renderRect;

        public Display(Game game, int width, int height)
        {
            this.game = game;
            target = new RenderTarget2D(game.GraphicsDevice, width, height);
            game.Window.ClientSizeChanged += new EventHandler<EventArgs>(UpdateScreenSize);
            UpdateScreenSize(null, null);
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        public void UpdateScreenSize(object sender, EventArgs e)
        {
            float currentAspectRatio = game.Window.ClientBounds.Width / (float) game.Window.ClientBounds.Height;
            windowScale = 1f / currentAspectRatio;
            renderRect = GetRenderTargetPositionAsRectangle(currentAspectRatio);
        }

        public void Set()
        {
            if (!targetIsSet)
            {
                game.GraphicsDevice.SetRenderTarget(target);
                targetIsSet = true;
            }
        }

        public void UnSet()
        {
            if (targetIsSet)
            {
                game.GraphicsDevice.SetRenderTarget(null);
                targetIsSet = false;
            }
        }

        public void BatchSprites(Texture2D sprite)
        {
            float scale = 2f;

            game.GraphicsDevice.Clear(Color.PapayaWhip);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(sprite, new Vector2(renderRect.Center.X - sprite.Width * scale * windowScale / 2, renderRect.Center.Y - sprite.Height * scale * windowScale / 2), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.End();
        }

        public void Draw()
        {
            game.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(target, renderRect, Color.White);
            spriteBatch.End();
        }

        private Rectangle GetRenderTargetPositionAsRectangle(float currentAspectRatio)
        {
            if (currentAspectRatio <= prefferedAspectRatio)
            {
                // output is taller than it is wider, bars on top/bottom
                int presentHeight = (int)((game.Window.ClientBounds.Width / prefferedAspectRatio) + 0.5f);
                int barHeight = (game.Window.ClientBounds.Height - presentHeight) / 2;
                return new Rectangle(0, barHeight, game.Window.ClientBounds.Width, presentHeight);
            }
            else
            {
                // output is wider than it is tall, bars left/right
                int presentWidth = (int)((game.Window.ClientBounds.Height * prefferedAspectRatio) + 0.5f);
                int barWidth = (game.Window.ClientBounds.Width - presentWidth) / 2;
                return new Rectangle(barWidth, 0, presentWidth, game.Window.ClientBounds.Height);
            }
        }

        public void Dispose()
        {
            game.Window.ClientSizeChanged -= UpdateScreenSize;
        }
    }
}