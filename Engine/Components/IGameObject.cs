using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine
{
    public interface IGameObject
    {
        bool IsActive { get; }
        Transform Transform { get; }

        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}