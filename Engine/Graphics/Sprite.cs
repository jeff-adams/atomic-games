using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Graphics
{   
    public class Sprite
    {
        public Texture2D Texture { get; }
        public float Scale { get; }
        public Vector2 Position
        {
            get => position;
            set { position = value; }
        }
        
        private Vector2 position;

        public Sprite(Texture2D texture, float scale)
        {
            Texture = texture;
            Scale = scale;
            position = Vector2.Zero;
        }
    }
}