using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtomicGames.Engine.Input
{
    public class MouseBroadcaster : IMouseBroadcaster
    {
        public bool IsEnabled { get; private set; }
        public void Enable() => IsEnabled = true;
        public void Disable() => IsEnabled = false;

        public event Action<Vector2> MousePosition;
        public event Action<MouseButtons> OnMouseButtonPressed;

        public MouseBroadcaster()
        {
            IsEnabled = true;
        }

        public void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            
            MousePosition?.Invoke(new Vector2(mouseState.X, mouseState.Y));
            
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                OnMouseButtonPressed?.Invoke(MouseButtons.RightButton);
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                OnMouseButtonPressed?.Invoke(MouseButtons.LeftButton);
            }

            if (mouseState.MiddleButton == ButtonState.Pressed)
            {
                OnMouseButtonPressed?.Invoke(MouseButtons.RightButton);
            }
        }
    }
}