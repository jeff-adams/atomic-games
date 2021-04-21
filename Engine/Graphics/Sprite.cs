using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine.Graphics
{   
    public class Sprite : IGameObject
    {
        public Transform Transform { get; }

        private Texture2D texture;
        private Vector2 origin;
        private SpriteEffects flip;
        
        public bool IsActive { get; private set;}

        public Sprite(Texture2D texture, float scale = 1f, float rotationOffset = 0f)
        {
            this.texture = texture;
            flip = SpriteEffects.None;
            Transform = new Transform(scale, rotationOffset);
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            IsActive = true;
        }

        public Sprite(Texture2D texture, Vector2 origin, float scale = 1f)
        {
            this.texture = texture;
            flip = SpriteEffects.None;
            Transform = new Transform(scale);
            this.origin = origin;
            IsActive = true;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Transform.Position, null, Color.White, Transform.Rotation, origin, Transform.Scale, flip, 0f);
        }
    }
}