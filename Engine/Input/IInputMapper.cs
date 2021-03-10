using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtomicGames.Engine
{
    public interface IInputMapper
    {
        Dictionary<Keys, Action> KeyboardInputs { get; }
        Dictionary<Buttons, Action> GamepadButtonInputs { get; }
        Action<Vector2> GamepadThumbstickLeftInput { get; }
        Action<Vector2> GamepadThumbstickRightInput { get; }
        Action<MouseState> MouseInput { get; }
    }
}