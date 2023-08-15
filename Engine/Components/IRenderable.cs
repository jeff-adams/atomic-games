using Microsoft.Xna.Framework;

namespace AtomicGames.Engine.Components;

public interface IRenderable : IComponent
{
    bool Visible { get; }
    void Draw(GameTime gameTime);
}