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
        private InputManager inputManager;

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
            inputManager = new InputManager();
            currentGameState = firstGameState;
        }

        protected override void Initialize()
        {
            currentGameState.Initialize(Content);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            currentGameState.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            inputManager.GetInput(currentGameState.InputMapper);
            currentGameState.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            display.SetTarget();
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            currentGameState.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            display.UnSetTarget();
            display.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        private void OnGameSwitched(object sender, GameState state)
        {
            SwitchGameState(state);
        }

        private void SwitchGameState(GameState nextGameState)
        {
            if (currentGameState != null)
            {
                currentGameState.OnGameStateSwitch -= OnGameSwitched;
                currentGameState.UnloadContent();
            }

            currentGameState = nextGameState;
            currentGameState.Initialize(Content);
            currentGameState.LoadContent();
            currentGameState.OnGameStateSwitch += OnGameSwitched;
        }

        protected override void UnloadContent()
        {
            currentGameState?.UnloadContent();
            base.UnloadContent();
        }
    }
}
