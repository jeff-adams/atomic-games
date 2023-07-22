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
        public UI UI { get; private set; }
        public GraphicsDevice GraphicsDevice { get; private set; }
        public Canvas Canvas { get; private set; }
        public Color BackgroundColor { get; protected set; } = Color.Black;
        
        private List<GameObject> gameObjects;
        private ContentManager contentManager;

        public void Initialize(AtomicGame game)
        {
            gameObjects = new List<GameObject>();

            this.contentManager = game.Content;
            Camera = game.Camera;
            UI = game.UI;
            GraphicsDevice = game.GraphicsDevice;
            Canvas = game.Canvas;
        }

        public void AddGameObject(GameObject gameObject) => 
            gameObjects.Add(gameObject);

        protected T Load<T>(string name) =>
            contentManager.Load<T>(name);

        public void UnloadContent()
        {
            contentManager.Unload();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, ShapeBatch shapeBatch)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.IsVisible)
                {
                    gameObject.DrawContent(gameTime, spriteBatch, shapeBatch);
                    gameObject.DrawChildren(gameTime, spriteBatch, shapeBatch);
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