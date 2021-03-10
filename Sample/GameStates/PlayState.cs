using AtomicGames.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace AtomicGames.Sample
{
    public class PlayState : GameState
    {
        private Sprite bomb;
        private float speed = 1f;
        private float deltaTime;

        public override IInputMapper InputMapper => input;
        private PlayInputMap input;

        public PlayState()
        {
            input = new PlayInputMap(this);
        }

        public override void LoadContent()
        {
            bomb = new Sprite(LoadTexture("bomb"), 4f);
            AddGameObject(bomb);
        }

        public override void Update(GameTime gameTime)
        {
            deltaTime = gameTime.ElapsedGameTime.Milliseconds;

            if (input.MouseLeftClick)
            {
                var mouseDirection = Vector2.Normalize(input.MousePosition - bomb.Transform.Position);
                Direction(mouseDirection);
            }
        }

        public void Direction(Vector2 dir)
        {
            bomb.Transform.Move(dir, deltaTime, speed);
        }

        public void Quit() =>
            System.Environment.Exit(0);
    }
}