using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine.Graphics
{   
    public class Sprite : GameObject
    {
        private Texture2D texture;
        private Vector2 origin;
        private SpriteEffects flip;
        private float scale;

        public Sprite(Texture2D texture, float scale = 1f) 
            : this(texture, new Vector2(texture.Width / 2, texture.Height / 2), scale) { }

        public Sprite(Texture2D texture, Vector2 origin, float scale = 1f) : base()
        {
            this.texture = texture;
            this.scale = scale;
            this.flip = SpriteEffects.None;
            this.origin = origin;
            IsActive = true;
            IsVisible = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) =>
            spriteBatch.Draw(texture, Transform.Position, null, Color.White, Transform.Rotation, origin, scale, flip, 0f);
    }
}