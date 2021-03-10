using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtomicGames.Engine
{
    public class InputManager
    {
        public void GetInput(IInputMapper inputMapper)
        {
            if (inputMapper.KeyboardInputs != null)
            {
                var keyboardState = Keyboard.GetState();
                inputMapper.KeyboardInputs.InputMappingToAction(keyboardState.IsKeyDown);
            }

            if (inputMapper.GamepadButtonInputs != null)
            {
                var gamepadState = GamePad.GetState(PlayerIndex.One);
                inputMapper.GamepadButtonInputs.InputMappingToAction(gamepadState.IsButtonDown);
            }

            if (inputMapper.GamepadThumbstickLeftInput != null)
            {
                inputMapper.GamepadThumbstickLeftInput(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left);
            }
            
            if (inputMapper.GamepadThumbstickRightInput != null)
            {
                inputMapper.GamepadThumbstickRightInput(GamePad.GetState(PlayerIndex.One).ThumbSticks.Right);
            }

            if (inputMapper.MouseInput != null)
            {
                inputMapper.MouseInput(Mouse.GetState());
            }
        }
    }
}