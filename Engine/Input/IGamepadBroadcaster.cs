using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtomicGames.Engine.Input;

public interface IGamePadBroadcaster : IBroadcaster
{
    event Action<Buttons, InputState> OnButtonPressed;
    event Action<Vector2> LeftAnalogStickMovement;
    event Action<Vector2> RightAnalogStickMovement;
}