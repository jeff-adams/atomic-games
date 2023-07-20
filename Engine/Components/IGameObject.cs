using AtomicGames.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine
{
    public interface IGameObject
    {
        bool IsVisible { get; }
        bool IsActive { get; }
        Transform Transform { get; }

        void UpdateContent(GameTime gameTime);
        void DrawContent(GameTime gameTime, SpriteBatch spriteBatch, ShapeBatch shapeBatch);
    }
}