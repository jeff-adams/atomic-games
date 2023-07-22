using System;
using Microsoft.Xna.Framework;

namespace AtomicGames.Engine.Input;

public interface IMouseBroadcaster : IBroadcaster
{
    event Action<Vector2> MousePosition;
    event Action<MouseButtons> OnMouseButtonPressed;
}