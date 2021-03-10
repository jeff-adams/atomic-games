using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine
{
    public abstract class GameState
    {
        public abstract IInputMapper InputMapper { get; }
        
        private List<IGameObject> gameObjects;
        private ContentManager contentManager;
        private Texture2D defaultTexture;

        public void Initialize(ContentManager contentManager)
        {
            gameObjects = new List<IGameObject>();

            this.contentManager = contentManager;
            defaultTexture = contentManager.Load<Texture2D>("default");
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
                if (gameObject.IsActive)
                {
                    gameObject.Draw(gameTime, spriteBatch);
                }
            }
        }

        public event EventHandler<GameState> OnGameStateSwitch;

        protected void SwitchGameState(GameState newGameState)
        {
            OnGameStateSwitch?.Invoke(this, newGameState);
        }

        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);
    }
}