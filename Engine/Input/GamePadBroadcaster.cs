using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtomicGames.Engine.Input;

public class GamePadBroadcaster : IGamePadBroadcaster
{
    public bool IsEnabled { get; private set; }
    public void Enable() => IsEnabled = true;
    public void Disable() => IsEnabled = false;

    public event Action<Buttons, InputState> OnButtonPressed;
    public event Action<Vector2> LeftAnalogStickMovement;
    public event Action<Vector2> RightAnalogStickMovement;

    private GamePadState previousState;     

    public GamePadBroadcaster()
    {
        IsEnabled = true;
        previousState = new GamePadState();
    }

    public void Update(GameTime gameTime)
    {
        var gamePadState = GamePad.GetState(0);
        LeftAnalogStickMovement?.Invoke(gamePadState.ThumbSticks.Left);
        RightAnalogStickMovement?.Invoke(gamePadState.ThumbSticks.Right);
        
        foreach (var button in gamePadMappings)
        {
            var currentButtonState = button.Value.Invoke(gamePadState);
            var pastButtonState = button.Value.Invoke(previousState);
            
            bool buttonPressed = currentButtonState == ButtonState.Pressed && pastButtonState == ButtonState.Released;
            bool buttonHeld = currentButtonState == ButtonState.Pressed && pastButtonState == ButtonState.Pressed;
            var inputState = new InputState(button.Key.ToString(), buttonPressed, buttonHeld);
            
            if (inputState.Pressed || inputState.Held)
            {
                OnButtonPressed?.Invoke(button.Key, inputState);
            }
        }

        previousState = gamePadState;
    }

    private Dictionary<Buttons, Func<GamePadState, ButtonState>> gamePadMappings = new Dictionary<Buttons, Func<GamePadState, ButtonState>>
    {
        {Buttons.A, s => s.Buttons.A},
        {Buttons.B, s => s.Buttons.B},
        {Buttons.Back, s => s.Buttons.Back},
        {Buttons.BigButton, s => s.Buttons.BigButton},
        {Buttons.DPadDown, s => s.DPad.Down},
        {Buttons.DPadLeft, s => s.DPad.Left},
        {Buttons.DPadRight, s => s.DPad.Right},
        {Buttons.DPadUp, s => s.DPad.Up},
        {Buttons.LeftShoulder, s => s.Buttons.LeftShoulder},
        {Buttons.LeftStick, s => s.Buttons.LeftStick},
        {Buttons.RightShoulder, s => s.Buttons.RightShoulder},
        {Buttons.RightStick, s => s.Buttons.RightStick},
        {Buttons.Start, s => s.Buttons.Start},
        {Buttons.X, s => s.Buttons.X},
        {Buttons.Y, s => s.Buttons.Y},
    };
}