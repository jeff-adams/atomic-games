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
        public Matrix TransformMatrix { get; private set; }
        public Matrix ViewMatrix { get; private set; }
        public Matrix ProjectionMatrix { get; private set; }

        private readonly GameWindow window;

        private GameObject target;
        private float cameraSmoothing = 0.05f; // smaller means a longer delay
        
        public Camera(GameWindow gameWindow)
        {
            Position = Vector2.Zero;
            Zoom = 1.0f;
            Rotation = 0f;

            window = gameWindow;
            gameWindow.ClientSizeChanged += WindowSizeHasChanged;
            WindowSizeHasChanged(null, null);
            
            UpdateMatrices();
        }

        public void Update(GameTime gameTime)
        {
            if (target != null)
            {
                Position = Vector2.Lerp(Position, target.Transform.Position - Origin, cameraSmoothing);
            }

            UpdateMatrices();
        }

        public void Follow(GameObject target, float smoothing = 0.05f)
        {
            this.target = target;
            cameraSmoothing = smoothing;
        } 

        public void Unfollow() =>
            this.target = null;

        public void Reset()
        {
            Position = Vector2.Zero;
            Zoom = 1.0f;
        }

        public void Pan(Vector2 direction, float speed = 1f)
        {
            target = null;
            Position += direction * speed;
        }

        public void AdjustZoom(float zoom)
        {
            Zoom += zoom;
        }

        public Vector2 GetWorldPosition(Vector2 screenPosition) =>
            Vector2.Transform(screenPosition, Matrix.Invert(ViewMatrix));

        public Vector2 GetScreenPosition(Vector2 worldPosition) =>
            Vector2.Transform(worldPosition, TransformMatrix);

        private void WindowSizeHasChanged(object sender, EventArgs e)
        {
            Origin = new Vector2(window.ClientBounds.Width * 0.5f, window.ClientBounds.Height * 0.5f);
            UpdateMatrices();
        }

        private void UpdateMatrices()
        {
            TransformMatrix = GetTransformMatrix();
            ProjectionMatrix = GetProjectionMatrix();
            ViewMatrix = GetViewMatrix();
        }

        private Matrix GetTransformMatrix() => 
            GetTransformMatrix(Vector2.One);

        private Matrix GetTransformMatrix(Vector2 parallax) => 
            Matrix.CreateTranslation(new Vector3(-Position * parallax, 0.0f)) *
            Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(Zoom, Zoom, 1) *
            Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        
        private Matrix GetViewMatrix()
        {
            if (target is null) 
                return Matrix.CreateLookAt(new Vector3(Position, 0f), new Vector3(Origin, 0), Vector3.Up);
            
            return Matrix.CreateLookAt(new Vector3(Position, 0f), new Vector3(target.Transform.Position, 0f), Vector3.Up);
        }

        private Matrix GetProjectionMatrix() =>
            Matrix.CreateOrthographic((float)window.ClientBounds.Width, (float)window.ClientBounds.Height, 0.1f, 2048f);

        public void Dispose()
        {
            window.ClientSizeChanged -= WindowSizeHasChanged;
        }
    }
}