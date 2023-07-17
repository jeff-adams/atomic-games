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
        private float scale;
        
        public bool IsActive { get; private set;}
        public bool IsVisible { get; private set;}

        public Sprite(Texture2D texture, float scale = 1f) 
            : this(texture, new Vector2(texture.Width / 2, texture.Height / 2), scale) { }

        public Sprite(Texture2D texture, Vector2 origin, float scale = 1f)
        {
            this.texture = texture;
            this.scale = scale;
            this.flip = SpriteEffects.None;
            this.origin = origin;
            Transform = new Transform();
            IsActive = true;
            IsVisible = true;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) =>
            spriteBatch.Draw(texture, Transform.Position, null, Color.White, Transform.Rotation, origin, scale, flip, 0f);

        public void Update(GameTime gameTime) { }
    }
}