using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine
{   
    public class Animation
    {
        private Texture2D[] frames;
        private int currentFrame = 0;
        private float timeSinceLastFrame = 0f;
        private readonly float speed = 1f;
        private const float fps = 60f;

        public Animation(Texture2D spriteSheet, int spriteWidth, GraphicsDevice graphicsDevice, float speed)
        {
            frames = SpliceSpriteSheet(spriteSheet, spriteWidth, graphicsDevice);
            this.speed = speed > 0 ? speed : 0.1f;
        }

        public Animation(Texture2D[] spriteFrames, float speed)
        {
            frames = spriteFrames;
            this.speed = speed > 0 ? speed : 0.1f;
        }

        private Texture2D[] SpliceSpriteSheet(Texture2D spriteSheet, int spriteWidth, GraphicsDevice graphicsDevice)
        {
            var numberOfFrames = spriteSheet.Width / spriteWidth;
            var spriteFrames = new Texture2D[numberOfFrames];
            for (int i = 0; i < numberOfFrames; i++)
            {
                var splice = new Rectangle(i * spriteWidth, 0, spriteWidth, spriteSheet.Height);
                spriteFrames[i] = spriteSheet.CreateTexture2D(graphicsDevice, splice);
            }
            return spriteFrames;
        }

        public Texture2D GetCurrentFrame(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame >= fps / speed)
            {
                currentFrame = (currentFrame + 1) % frames.Length;
            }
            return frames[currentFrame];
        }
    }
}