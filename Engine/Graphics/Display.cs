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
        private float windowScale;
        private Rectangle renderRect;

        public Display(Game game, int width, int height)
        {
            this.game = game;
            prefferedAspectRatio = (float) width / height;
            target = new RenderTarget2D(game.GraphicsDevice, width, height);
            game.Window.ClientSizeChanged += new EventHandler<EventArgs>(UpdateScreenSize);
            UpdateScreenSize(null, null);
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        public void UpdateScreenSize(object sender, EventArgs e)
        {
            renderRect = GetRenderTargetPositionAsRectangle();
            Console.WriteLine($"RenderTarget Bounds: {target.Bounds.Width}, {target.Bounds.Height}\nClient Bounds: {game.Window.ClientBounds.Width}, {game.Window.ClientBounds.Height}\nRenderRect Size: {renderRect.Size.X}, {renderRect.Size.Y}\nRenderRect Center: {renderRect.Center.X}, {renderRect.Center.Y}.");
            Console.WriteLine("----------------------------");
        }

        public void BatchSprites(Sprite[] sprites)
        {
            if (targetIsSet)
            {
                throw new Exception("RenderTarget is already set. Must call Draw()");
            }

            game.GraphicsDevice.SetRenderTarget(target);
            targetIsSet = true;

            game.GraphicsDevice.Clear(Color.DeepPink);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            foreach(Sprite s in sprites)
            {
                spriteBatch.Draw(s.Texture, s.Position, null, Color.White, 0f, Vector2.Zero, s.Scale, SpriteEffects.None, 0f);
            }
            spriteBatch.End();
        }

        public void Draw()
        {
            if (!targetIsSet)
            {
                throw new Exception("RenderTarget is not set. Must call BatchSprites()");
            }

            game.GraphicsDevice.SetRenderTarget(null);
            targetIsSet = false;

            game.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(target, renderRect, Color.White);
            spriteBatch.End();
        }

        private Rectangle GetRenderTargetPositionAsRectangle()
        {
            Rectangle backbufferBounds = game.GraphicsDevice.PresentationParameters.Bounds;
            float currentAspectRatio = (float) backbufferBounds.Width / backbufferBounds.Height;
            int x = 0;
            int y = 0;
            int width = backbufferBounds.Width;
            int height = backbufferBounds.Height;

            if (currentAspectRatio < prefferedAspectRatio)
            {
                // output is taller than it is wider, bars on top/bottom
                height = (int)((backbufferBounds.Height / prefferedAspectRatio) + 0.5f);
                y = (backbufferBounds.Height - height) / 2;
            }
            else if (currentAspectRatio > prefferedAspectRatio)
            {
                // output is wider than it is tall, bars left/right
                width = (int)((backbufferBounds.Height * prefferedAspectRatio) + 0.5f);
                x = (backbufferBounds.Width - width) / 2;
            }

            return new Rectangle(x, y, width, height);
        }

        public void Dispose()
        {
            game.Window.ClientSizeChanged -= UpdateScreenSize;
        }
    }
}