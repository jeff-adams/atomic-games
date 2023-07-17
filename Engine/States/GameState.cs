using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AtomicGames.Engine.Input;
using AtomicGames.Engine.Graphics;

namespace AtomicGames.Engine
{
    public abstract class GameState : IDisposable
    {
        public abstract IActionMap ActionMap { get; }
        public Camera Camera { get; private set; }
        public GraphicsDevice GraphicsDevice { get; private set; }
        public Display Display { get; private set; }
        
        private List<IGameObject> gameObjects;
        private ContentManager contentManager;
        private Texture2D defaultTexture;

        public void Initialize(AtomicGame game)
        {
            gameObjects = new List<IGameObject>();

            this.contentManager = game.Content;
            defaultTexture = contentManager.Load<Texture2D>("default");
            Camera = game.Camera;
            GraphicsDevice = game.GraphicsDevice;
            Display = game.Display;
        }

        public void AddGameObject(IGameObject gameObject) => 
            gameObjects.Add(gameObject);

        protected Texture2D LoadTexture(string textureName) =>
            contentManager.Load<Texture2D>(textureName) ?? defaultTexture;

        public void UnloadContent()
        {
            contentManager.Unload();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.IsVisible)
                {
                    gameObject.Draw(gameTime, spriteBatch);
                }
            }
        }

        public virtual void Dispose()
        {
            UnloadContent();
        }

        public event Action<GameState> OnGameStateSwitch;

        protected void SwitchGameState(GameState newGameState)
        {
            OnGameStateSwitch?.Invoke(newGameState);
        }

        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);
    }
}