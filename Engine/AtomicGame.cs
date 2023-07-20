using System;
using AtomicGames.Engine.Input;
using AtomicGames.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine
{
    public class AtomicGame : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private readonly Canvas canvas;
        private readonly Camera camera;
        private readonly UI ui;

        private SpriteBatch spriteBatch;
        private InputManager inputManager;
        private GameState currentGameState;

        public Canvas Canvas => canvas;
        public Camera Camera => camera;
        public UI UI => ui;

        public AtomicGame(GameState firstGameState, string gameTitle, int width, int height)
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;

            SetResolution(width, height);

            camera = new Camera(Window);
            canvas = new Canvas(GraphicsDevice, camera, width, height);
            ui = new UI(width, height);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Window.AllowUserResizing = true;
            Window.IsBorderless = false;
            Window.Title = gameTitle;
            Window.ClientSizeChanged += UpdateCanvasRenderSize;

            currentGameState = firstGameState;
        }

        protected override void Initialize()
        {
            canvas.UpdateRenderRectangle();

            currentGameState.Initialize(this);

            var broadcasters = new IBroadcaster[]
            {
                new KeyboardBroadcaster(),
                new MouseBroadcaster(),
                new GamePadBroadcaster(),
            };
            
            Components.Add(new BroadcastComponent(this, broadcasters));

            inputManager = new InputManager(broadcasters);
            inputManager.SetActionMap(currentGameState.ActionMap);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            currentGameState.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            currentGameState.Update(gameTime);
            camera.Update(gameTime);
            ui.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            canvas.Activate();
            GraphicsDevice.Clear(currentGameState.BackgroundColor);

            spriteBatch.Begin(transformMatrix: camera.ViewMatrix, samplerState: SamplerState.PointClamp);
            currentGameState.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            ui.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            canvas.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        private void SetResolution(int width, int height)
        {
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();
        }

        private void UpdateCanvasRenderSize(object sender, EventArgs e) =>
            canvas.UpdateRenderRectangle();

        private void SwitchGameState(GameState nextGameState)
        {
            if (currentGameState != null)
            {
                currentGameState.OnGameStateSwitch -= SwitchGameState;
                currentGameState.UnloadContent();
            }

            currentGameState = nextGameState;
            currentGameState.Initialize(this);
            currentGameState.LoadContent();
            currentGameState.OnGameStateSwitch += SwitchGameState;
            inputManager.SetActionMap(currentGameState.ActionMap);
        }

        protected override void UnloadContent()
        {
            currentGameState?.UnloadContent();
            base.UnloadContent();
        }
    }
}
