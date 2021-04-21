using System;
using AtomicGames.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtomicGames.Engine
{
    public class InputManager : IDisposable
    {
        private readonly IKeyboardBroadcaster keyboardBroadcaster;
        private readonly IMouseBroadcaster mouseBroadcaster;
        private readonly IGamePadBroadcaster gamePadBroadcaster;

        private IActionMap actionMap;

        public InputManager(IBroadcaster[] broadcasters)
        {
            foreach (var broadcaster in broadcasters)
            {
                if(broadcaster is IKeyboardBroadcaster kb) 
                {
                    this.keyboardBroadcaster = kb;
                    keyboardBroadcaster.KeyPressed += KeyboardPressedReciever;
                }
                if(broadcaster is IMouseBroadcaster mb) 
                {
                    this.mouseBroadcaster = mb;
                    mouseBroadcaster.MousePosition += MousePositionReciever;
                    mouseBroadcaster.MouseButtonPressed += MouseButtonReceiver;
                }
                if(broadcaster is IGamePadBroadcaster gpb) 
                {
                    this.gamePadBroadcaster = gpb;
                    gamePadBroadcaster.ButtonPressed += GamePadButtonReceiver;
                    gamePadBroadcaster.LeftAnalogStickMovement += GamePadLeftStickReceiver;
                    gamePadBroadcaster.RightAnalogStickMovement += GamePadRightStickReceiver;
                }
            }

        }

        public void SetActionMap(IActionMap actionMap) =>
            this.actionMap = actionMap;

        private void KeyboardPressedReciever(object sender, Keys key)
        {
            if (actionMap.KeyboardInputs.ContainsKey(key))
            {
                actionMap.KeyboardInputs[key]();
            }
        }

        private void MousePositionReciever(object sender, Vector2 mousePosition) =>
            actionMap.MousePosition(mousePosition);

        private void MouseButtonReceiver(object sender, MouseButtons mouseButton)
        {
            if (actionMap.MouseButtonInputs.ContainsKey(mouseButton))
            {
                actionMap.MouseButtonInputs[mouseButton]();
            }
        }

        private void GamePadButtonReceiver(Buttons button, InputState state)
        {
            if (actionMap.GamepadButtonInputs.ContainsKey(button))
            {
                actionMap.GamepadButtonInputs[button](state);
            }
        }

        private void GamePadLeftStickReceiver(Vector2 leftMovement) =>
            actionMap.GamepadThumbstickLeftInput(leftMovement);

        private void GamePadRightStickReceiver(Vector2 rightMovement) =>
            actionMap.GamepadThumbstickRightInput(rightMovement);

        public void Dispose()
        {
            keyboardBroadcaster.KeyPressed -= KeyboardPressedReciever;
            mouseBroadcaster.MousePosition -= MousePositionReciever;
            mouseBroadcaster.MouseButtonPressed -= MouseButtonReceiver;
            gamePadBroadcaster.ButtonPressed -= GamePadButtonReceiver;
            gamePadBroadcaster.LeftAnalogStickMovement -= GamePadLeftStickReceiver;
            gamePadBroadcaster.RightAnalogStickMovement -= GamePadRightStickReceiver;
        }
    }
}