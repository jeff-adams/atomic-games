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
            SetBounds();
        }

        public override void DrawContent(GameTime gameTime, SpriteBatch spriteBatch, ShapeBatch shapeBatch)
        {
            spriteBatch.Draw(texture, Transform.Position, null, Color.White, Transform.Rotation, origin, scale, flip, 0f);
            base.DrawContent(gameTime, spriteBatch, shapeBatch);
        }
        
        protected override void SetBounds()
        {
            Vector2 position = Parent is null ? Transform.Position : Parent.Transform.Position;
            Bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }
    }
}