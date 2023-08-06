using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AtomicGames.Engine.Graphics;

namespace AtomicGames.Engine;

public interface IGameObject
{
    bool IsVisible { get; }
    bool IsActive { get; }

    void UpdateContent(GameTime gameTime);
    void DrawContent(GameTime gameTime, SpriteBatch spriteBatch, ShapeBatch shapeBatch);
}