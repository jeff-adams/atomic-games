using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AtomicGames.Engine
{
    public class BaseGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Display display;

        private const int gameWidth = 1440;
        private const int gameHeight = 900;

        private GameState currentGameState;

        public BaseGame(GameState firstGameState, string gameTitle)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = gameWidth;
            graphics.PreferredBackBufferHeight = gameHeight;
            graphics.ApplyChanges();

            Window.AllowUserResizing = true;
            Window.Title = gameTitle;

            display = new Display(this, gameWidth, gameHeight);
            currentGameState = firstGameState;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            display.SetTarget();
            GraphicsDevice.Clear(Color.Black);

            currentGameState.Draw(gameTime, spriteBatch);

            display.UnSetTarget();
            display.Draw();

            base.Draw(gameTime);
        }
    }
}
