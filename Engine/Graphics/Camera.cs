using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine.Graphics
{
    public class Camera : IDisposable
    {
        public Vector2 Position { get; private set; }
        public Vector2 Origin { get; private set; }
        public float Zoom { get; private set; }
        public float Rotation { get; private set; }
        public Matrix ViewMatrix { get; private set; }

        private readonly GameWindow window;
        
        public Camera(GameWindow gameWindow)
        {
            window = gameWindow;
            WindowSizeHasChanged(null, null);
            Zoom = 1.0f;
            ViewMatrix = GetViewMatrix(Vector2.One);
            gameWindow.ClientSizeChanged += WindowSizeHasChanged;
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

        public void Dispose()
        {
            window.ClientSizeChanged -= WindowSizeHasChanged;
        }
    }
}