using System;
using System.Collections.Generic;
using AtomicGames.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtomicGames.Sample
{
    public class PlayActionMap : IActionMap
    {
        public Dictionary<Keys, Action<InputState>> KeyboardInputs { get; private set;}
        public Dictionary<Buttons, Action<InputState>> GamepadButtonInputs { get; private set;}
        public Action<Vector2> GamepadThumbstickLeftInput => GetGamePadLeftThumbstickPosition;
        public Action<Vector2> GamepadThumbstickRightInput => GetGamePadRightThumbstickPosition;
        public Dictionary<MouseButtons, Action> MouseButtonInputs { get; private set; }
        public Action<Vector2> MousePosition => GetMousePosition;


        public PlayActionMap()
        {
            KeyboardInputs = new Dictionary<Keys, Action<InputState>> {
                {Keys.Escape, QuitInput},
                //{Keys.OemTilde, ToggleDebugInput},
                //{Keys.Space, ThrustInput},
                {Keys.Right, DirectionInputRight},
                {Keys.Left, DirectionInputLeft},
                {Keys.Up, DirectionInputUp},
                {Keys.Down, DirectionInputDown},

            };

            GamepadButtonInputs = new Dictionary<Buttons, Action<InputState>> {
                {Buttons.A, DebugInputPress},
                {Buttons.B,  DebugInputPress},
                {Buttons.Back,  DebugInputPress},
                {Buttons.BigButton,  DebugInputPress},
                {Buttons.DPadDown,  DebugInputPress},
                {Buttons.DPadLeft,  DebugInputPress},
                {Buttons.DPadRight,  DebugInputPress},
                {Buttons.DPadUp,  DebugInputPress},
                {Buttons.LeftShoulder,  DebugInputPress},
                {Buttons.LeftStick,  DebugInputPress},
                {Buttons.RightShoulder,  DebugInputPress},
                {Buttons.RightStick,  DebugInputPress},
                {Buttons.Start,  DebugInputPress},
                {Buttons.X,  DebugInputPress},
                {Buttons.Y,  DebugInputPress},
            };

            MouseButtonInputs = new Dictionary<MouseButtons, Action> {
                //{MouseButtons.LeftButton, ThrustInput},
            };
        }

        private void GetMousePosition(Vector2 mousePosition) =>
            MousePositionAction?.Invoke(mousePosition);

        private void GetGamePadLeftThumbstickPosition(Vector2 leftStickPosition) =>
            GamePadLeftStickPositionAction?.Invoke(leftStickPosition);

        private void GetGamePadRightThumbstickPosition(Vector2 rightStickPosition) =>
            GamePadRightStickPositionAction?.Invoke(rightStickPosition);

        private void DirectionInputRight(InputState state) => DirectionAction?.Invoke(Vector2.UnitX);
        private void DirectionInputLeft(InputState state) => DirectionAction?.Invoke(-Vector2.UnitX);
        private void DirectionInputUp(InputState state) => DirectionAction?.Invoke(-Vector2.UnitY);
        private void DirectionInputDown(InputState state) => DirectionAction?.Invoke(Vector2.UnitY);

        private void ThrustInput(InputState state) => ThrustAction?.Invoke();

        private void QuitInput(InputState state) => Quit?.Invoke();

        //Debug
        private void ToggleDebugInput(InputState state)
        {
            if (state.Pressed && !state.Held) ToggleDebugAction?.Invoke();
        }
        private void DebugInputPress(InputState state) =>
            DebugInputPressed?.Invoke(state);


        public event Action<InputState> DebugInputPressed;
        public event Action ToggleDebugAction;
        public event Action ThrustAction;
        public event Action<Vector2> DirectionAction;
        //public event Action<float> ScaleAction;
        public event Action<Vector2> MousePositionAction;
        public event Action<Vector2> GamePadLeftStickPositionAction;
        public event Action<Vector2> GamePadRightStickPositionAction;
        public event Action Quit;
    }
}