using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames
{
    public class Transform
    {
        public Vector2 Position { get; private set; }
        public float Rotation { get; private set; }
        public float Scale { get; private set; }

        public Transform(float scale)
        {
            Scale = scale;
            Position = Vector2.Zero;
            Rotation = 0f;
        }
    }
}