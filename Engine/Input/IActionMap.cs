using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtomicGames.Engine.Input
{
    public interface IActionMap
    {
        Dictionary<Keys, Action> KeyboardInputs { get; }
        Dictionary<Buttons, Action<InputState>> GamepadButtonInputs { get; }
        Action<Vector2> GamepadThumbstickLeftInput { get; }
        Action<Vector2> GamepadThumbstickRightInput { get; }
        Dictionary<MouseButtons, Action> MouseButtonInputs { get; }
        Action<Vector2> MousePosition { get; }
    }
}