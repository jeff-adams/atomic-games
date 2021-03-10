using System;
using System.Collections.Generic;
using AtomicGames.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtomicGames.Sample
{
    public class PlayInputMap : IInputMapper
    {
        private PlayState playState;

        public Dictionary<Keys, Action> KeyboardInputs { get; private set;}
        public Dictionary<Buttons, Action> GamepadButtonInputs { get; private set;}
        public Action<Vector2> GamepadThumbstickLeftInput => null;
        public Action<Vector2> GamepadThumbstickRightInput => null;
        public Action<MouseState> MouseInput => GetMouseInput;

        public Vector2 DirectionDigital { get; private set; }
        public Vector2 DirectionAnalog { get; private set; }

        public Vector2 MousePosition { get; private set; }
        public bool MouseLeftClick { get; private set; }

        public PlayInputMap(PlayState playState)
        {
            this.playState = playState;

            KeyboardInputs = new Dictionary<Keys, Action> {
                {Keys.Escape, this.playState.Quit},
                // Directional Inputs
                {Keys.Left, DirectionLeft},
                {Keys.Right, DirectionRight},
                {Keys.Up, DirectionUp},
                {Keys.Down, DirectionDown},
                {Keys.W, DirectionUp},
                {Keys.S, DirectionDown},
                {Keys.A, DirectionLeft},
                {Keys.D, DirectionRight},
            };

            GamepadButtonInputs = new Dictionary<Buttons, Action> {
                {Buttons.Back, this.playState.Quit},
                // Directional Inputs
                {Buttons.DPadLeft, DirectionLeft},
                {Buttons.DPadRight, DirectionRight},
                {Buttons.DPadUp, DirectionUp},
                {Buttons.DPadDown, DirectionDown},
                // Digital Thumbstick Directional Inputs
                {Buttons.LeftThumbstickLeft, DirectionLeft},
                {Buttons.LeftThumbstickRight, DirectionRight},
                {Buttons.LeftThumbstickUp, DirectionUp},
                {Buttons.LeftThumbstickDown, DirectionDown},
            };
        }

        private void DirectionLeft() => playState.Direction(new Vector2(-1, 0));
        private void DirectionRight() => playState.Direction(new Vector2(1, 0));
        private void DirectionUp() => playState.Direction(new Vector2(0, -1));
        private void DirectionDown() => playState.Direction(new Vector2(0, 1));

        private void GetMouseInput(MouseState mouseState)
        {
            MousePosition = new Vector2(mouseState.X, mouseState.Y);
            MouseLeftClick = mouseState.LeftButton == ButtonState.Pressed;
        }
    }
}