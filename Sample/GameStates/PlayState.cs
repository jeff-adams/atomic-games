using AtomicGames.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace AtomicGames.Sample
{
    public class PlayState : GameState
    {
        public override void HandleInput()
        {
        }

        public override void LoadContent()
        {
            var bomb = new Sprite(LoadTexture("bomb"), 4f);
            AddGameObject(bomb);
        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                System.Environment.Exit(0);
        }
    }
}