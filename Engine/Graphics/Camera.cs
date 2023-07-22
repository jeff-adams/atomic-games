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
        private readonly Canvas canvas;

        private GameObject target;
        private float cameraSmoothing = 0.05f; // smaller means a longer delay
        
        public Camera(GameWindow gameWindow, Canvas canvas)
        {
            Position = Vector2.Zero;
            Zoom = 1.0f;
            Rotation = 0f;

            this.window = gameWindow;
            this.canvas = canvas;
            gameWindow.ClientSizeChanged += WindowSizeHasChanged;
            
            Origin = new Vector2(window.ClientBounds.Center.X, window.ClientBounds.Center.Y);
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

        public void Follow(GameObject target) => 
            Follow(target, this.cameraSmoothing);

        public void Follow(GameObject target, float smoothing)
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
            Vector2.Transform(screenPosition + new Vector2(canvas.RenderRectangle.X, canvas.RenderRectangle.Y), Matrix.Invert(TransformMatrix));

        public Vector2 GetScreenPosition(Vector2 worldPosition) =>
            Vector2.Transform(worldPosition, ProjectionMatrix);

        private void WindowSizeHasChanged(object sender, EventArgs e)
        {
            Origin = new Vector2(window.ClientBounds.Center.X, window.ClientBounds.Center.Y);
            UpdateMatrices();
        }

        public void UpdateMatrices()
        {
            TransformMatrix = GetTransformMatrix();
            ViewMatrix = GetViewMatrix();
            ProjectionMatrix = GetProjectionMatrix();
        }

        private Matrix GetTransformMatrix() => 
            GetTransformMatrix(Vector2.One);

        private Matrix GetTransformMatrix(Vector2 parallax) => 
            Matrix.CreateTranslation(new Vector3(-Position * parallax, 0.0f)) *
            Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(Zoom, Zoom, 1) *
            Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        
        private Matrix GetViewMatrix() =>
            target is null 
            ? Matrix.CreateLookAt(new Vector3(Position, 0f), new Vector3(Origin, 0), Vector3.Up)
            : Matrix.CreateLookAt(new Vector3(Position, 0f), new Vector3(target.Transform.Position, 0f), Vector3.Up);

        private Matrix GetProjectionMatrix() =>
            Matrix.CreateOrthographicOffCenter(canvas.RenderRectangle, 0.01f, 2048f);

        public override string ToString() =>
            $"Position: {Position}, Origin: {Origin}, Target: {target?.Transform.Position}";

        public void Dispose()
        {
            window.ClientSizeChanged -= WindowSizeHasChanged;
        }
    }
}