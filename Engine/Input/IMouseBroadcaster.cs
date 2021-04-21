using System;
using Microsoft.Xna.Framework;

namespace AtomicGames.Engine.Input
{
    public interface IMouseBroadcaster : IBroadcaster
    {
        event EventHandler<Vector2> MousePosition;
        event EventHandler<MouseButtons> MouseButtonPressed;
    }
}