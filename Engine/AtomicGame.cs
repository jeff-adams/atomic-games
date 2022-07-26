﻿using AtomicGames.Engine.Input;
using AtomicGames.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine
{
    public class AtomicGame : Game
    {

        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private readonly Display display;
        public Display Display => display;
        private InputManager inputManager;
        private readonly Camera camera;
        public Camera Camera => camera;

        private const int gameWidth = 1440;
        private const int gameHeight = 900;

        private GameState currentGameState;

        public AtomicGame(GameState firstGameState, string gameTitle)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = gameWidth;
            graphics.PreferredBackBufferHeight = gameHeight;
            graphics.ApplyChanges();

            Window.AllowUserResizing = true;
            Window.Title = gameTitle;

            camera = new Camera(Window);
            display = new Display(this, gameWidth, gameHeight);

            currentGameState = firstGameState;
        }

        protected override void Initialize()
        {
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            display.SetTarget();
            GraphicsDevice.Clear(Color.Black);

            //spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Begin(transformMatrix: camera.ViewMatrix);
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
            currentGameState.Initialize(this);
            currentGameState.LoadContent();
            currentGameState.OnGameStateSwitch += OnGameSwitched;
            inputManager.SetActionMap(currentGameState.ActionMap);
        }

        protected override void UnloadContent()
        {
            currentGameState?.UnloadContent();
            base.UnloadContent();
        }
    }
}
