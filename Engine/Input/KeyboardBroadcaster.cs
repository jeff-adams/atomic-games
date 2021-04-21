using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtomicGames.Engine.Input
{
    public class KeyboardBroadcaster : IKeyboardBroadcaster
    {
        public bool IsEnabled { get; private set; }
        public void Enable() => IsEnabled = true;
        public void Disable() => IsEnabled = false;

        public event EventHandler<Keys> KeyPressed;

        public KeyboardBroadcaster()
        {
            IsEnabled = true;
        }

        public void Update(GameTime gameTime)
        {
            var keys = Keyboard.GetState().GetPressedKeys();

            foreach (var key in keys)
            {
                KeyPressed?.Invoke(this, key);
            }
        }
    }
}