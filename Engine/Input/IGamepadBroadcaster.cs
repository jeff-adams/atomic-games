using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtomicGames.Engine.Input
{
    public interface IGamePadBroadcaster : IBroadcaster
    {
        //event EventHandler<Buttons> ButtonPressed;
        event Action<Buttons, InputState> ButtonPressed;
        event Action<Vector2> LeftAnalogStickMovement;
        event Action<Vector2> RightAnalogStickMovement;
    }
}