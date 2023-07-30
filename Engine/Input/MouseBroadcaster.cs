using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtomicGames.Engine.Input;

public class MouseBroadcaster : IMouseBroadcaster
{
    public bool IsEnabled { get; private set; }
    public void Enable() => IsEnabled = true;
    public void Disable() => IsEnabled = false;

    public event Action<Vector2> MousePosition;
    public event Action<MouseButtons, InputState> OnMouseButtonPressed;

    private MouseState previousMouseState;

    public MouseBroadcaster()
    {
        IsEnabled = true;
        previousMouseState = new MouseState();
    }

    public void Update(GameTime gameTime)
    {
        MouseState mouseState = Mouse.GetState();
        
        MousePosition?.Invoke(new Vector2(mouseState.X, mouseState.Y));

        foreach (var button in mouseMappings)
        {
            var currentButtonState = button.Value.Invoke(mouseState);
            var pastButtonState = button.Value.Invoke(previousMouseState);
            
            bool buttonPressed = currentButtonState == ButtonState.Pressed && pastButtonState == ButtonState.Released;
            bool buttonHeld = currentButtonState == ButtonState.Pressed && pastButtonState == ButtonState.Pressed;
            bool buttonReleased = currentButtonState == ButtonState.Released && pastButtonState == ButtonState.Pressed;
            var inputState = new InputState(button.Key.ToString(), buttonPressed, buttonHeld, buttonReleased);
            
            if (inputState.Pressed || inputState.Held || inputState.Released)
            {
                OnMouseButtonPressed?.Invoke(button.Key, inputState);
            }
        }

        previousMouseState = mouseState;
    }

    private Dictionary<MouseButtons, Func<MouseState, ButtonState>> mouseMappings = new Dictionary<MouseButtons, Func<MouseState, ButtonState>>
    {
        {MouseButtons.LeftButton, s => s.LeftButton},
        {MouseButtons.RightButton, s => s.RightButton},
        {MouseButtons.MiddleButton, s => s.MiddleButton},
    };
}