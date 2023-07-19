using System;
using Microsoft.Xna.Framework;

namespace AtomicGames.Engine.Graphics
{
    public class Camera : IDisposable
    {
        public Vector2 Position { get; private set; }
        public Vector2 Origin { get; private set; }
        public float Zoom { get; private set; }
        public float Rotation { get; private set; }
        public Matrix ViewMatrix { get; private set; }

        public Matrix TranslationMatrix { get; private set; } // Is this the same as the ViewMatrix???

        private readonly GameWindow window;

        private GameObject target;
        private float cameraSmoothing = 0.15f;
        
        public Camera(GameWindow gameWindow)
        {
            window = gameWindow;
            Zoom = 1.0f;
            ViewMatrix = GetViewMatrix(Vector2.One);
            gameWindow.ClientSizeChanged += WindowSizeHasChanged;
            WindowSizeHasChanged(null, null);
        }

        public void Update(GameTime gameTime)
        {
            if (target != null)
            {
                Position = Vector2.Lerp(Position, target.Transform.Position - Origin, cameraSmoothing);
                ViewMatrix = GetViewMatrix(Vector2.One);
            }
        }

        public void Follow(GameObject target) =>
            this.target = target;

        public void Unfollow() =>
            this.target = null;

        public void Reset()
        {
            Position = Vector2.Zero;
            Zoom = 1.0f;
            ViewMatrix = GetViewMatrix(Vector2.One);
        }

        public void Move(Vector2 direction)
        {
            target = null;
            Position += direction;
            ViewMatrix = GetViewMatrix(Vector2.One);
        }

        public void AdjustZoom(float zoom)
        {
            Zoom += zoom;
            ViewMatrix = GetViewMatrix(Vector2.One);
        }

        private void WindowSizeHasChanged(object sender, EventArgs e)
        {
            Origin = new Vector2(window.ClientBounds.Width / 2.0f, window.ClientBounds.Height / 2.0f);
            ViewMatrix = GetViewMatrix(Vector2.One);
        }

        private Matrix GetViewMatrix(Vector2 parallax) => 
            Matrix.CreateTranslation(new Vector3(-Position * parallax, 0.0f)) *
            Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(Zoom, Zoom, 1) *
            Matrix.CreateTranslation(new Vector3(Origin, 0.0f));

        // Matrix.CreateTranslation(new Vector3(pos.X, pos.Y, 0f)) *
        // Matrix.CreateRotationZ(rot) *
        // Matrix.CreateScale(scale.x, scale.y, 1) *
        // Matrix.CreateTranslation(offset.x, offset.y, 0f);

         // Not working, needs to be scaled somehow
        public Vector2 ConvertToWorldPosition(Vector2 screenPosition) =>
            Vector2.Transform(screenPosition, Matrix.Invert(ViewMatrix));

        public Vector2 ConvertToScreenPosition(Vector2 worldPosition) =>
            Vector2.Transform(worldPosition, ViewMatrix);

        private Matrix CalculateTranslationMatrix() => 
            Matrix.CreateTranslation(new Vector3(-Position, 0.0f));

        public void Dispose()
        {
            window.ClientSizeChanged -= WindowSizeHasChanged;
        }
    }
}