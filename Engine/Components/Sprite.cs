using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine
{   
    public class Sprite : IGameObject
    {
        private Texture2D texture;
        private Transform transform;
        private Vector2 origin;
        private SpriteEffects flip;
        
        public bool IsActive { get; private set;}

        public Sprite(Texture2D texture, float scale = 1f)
        {
            this.texture = texture;
            flip = SpriteEffects.None;
            transform = new Transform(scale);
            IsActive = true;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, transform.Position, null, Color.White, transform.Rotation, origin, transform.Scale, flip, 0f);
        }
    }
}