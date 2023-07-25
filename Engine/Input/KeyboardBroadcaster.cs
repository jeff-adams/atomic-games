using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtomicGames.Engine.Input;

public class KeyboardBroadcaster : IKeyboardBroadcaster
{
    public bool IsEnabled { get; private set; }
    public void Enable() => IsEnabled = true;
    public void Disable() => IsEnabled = false;

    public event Action<Keys, InputState> OnKeyPressed;

    private KeyboardState previousState;

    public KeyboardBroadcaster()
    {
        IsEnabled = true;
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState currentState = Keyboard.GetState();
        Keys[] previousKeys = previousState.GetPressedKeys();

        foreach(Keys key in currentState.GetPressedKeys())
        {
            bool isKeyHeld = previousKeys.Contains(key);
            OnKeyPressed?.Invoke(key, new InputState(key.ToString(), pressed: true, held: isKeyHeld));
        }

        previousState = currentState;
    }
}