using Microsoft.Xna.Framework;

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

        public void Move(Vector2 direction, float deltaTime, float speed)
        {
            Position += direction * deltaTime * speed;
        }

        public void Rotate(float radians)
        {
            Rotation += radians;
        }

        public void ChangeScale(float amount)
        {
            Scale += amount;
        }
    }
}