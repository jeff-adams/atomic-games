using Microsoft.Xna.Framework;

namespace AtomicGames.Engine.Input;

public interface IBroadcaster
{
    bool IsEnabled { get; }
    void Enable();
    void Disable();
    void Update(GameTime gameTime);
}