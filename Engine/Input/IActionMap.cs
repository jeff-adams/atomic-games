using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtomicGames.Engine.Input;

public interface IActionMap
{
    Dictionary<Keys, Action<InputState>> KeyboardInputs { get; }
    Dictionary<Buttons, Action<InputState>> GamepadButtonInputs { get; }
    Action<Vector2> GamepadThumbstickLeftInput { get; }
    Action<Vector2> GamepadThumbstickRightInput { get; }
    Dictionary<MouseButtons, Action<InputState>> MouseButtonInputs { get; }
    Action<Vector2> MousePosition { get; }
}
