using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine
{
    public abstract class GameState
    {
        private IGameObject[] gameObjects;

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.Draw(gameTime, spriteBatch);
            }
        }
    }
}