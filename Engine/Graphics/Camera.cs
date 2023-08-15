using System;
using AtomicGames.Engine.Components;
using Microsoft.Xna.Framework;

namespace AtomicGames.Engine.Graphics;

public class Camera : IDisposable
{
    public Vector2 Position { get; private set; }
    public Vector2 Origin { get; private set; }
    public float Zoom { get; private set; }
    public float Rotation { get; private set; }
    public Matrix ViewMatrix { get; private set; }
    public Matrix VirtualViewMatrix { get; private set; }
    public Matrix ProjectionMatrix { get; private set; }

    private readonly GameWindow window;
    private readonly Canvas canvas;

    private IEntity target;
    private IEntity previousTarget;
    private float cameraSmoothing = 0.05f; // smaller means a longer delay
    
    public Camera(GameWindow gameWindow, Canvas canvas)
    {
        Position = Vector2.Zero;
        Zoom = 1.0f;
        Rotation = 0f;

        this.window = gameWindow;
        this.canvas = canvas;
        gameWindow.ClientSizeChanged += WindowSizeHasChanged;

        Origin = new Vector2(canvas.RenderRectangle.Center.X, canvas.RenderRectangle.Center.Y);
        UpdateMatrices();
    }

    public void Update(GameTime gameTime)
    {
        if (target != null)
        {
            Position = Vector2.Lerp(Position, target.Transform.Position + target.Transform.Origin - canvas.VirtualCenter, cameraSmoothing);
            UpdateMatrices();
        }
    }

    public void Follow(IEntity target) => 
        Follow(target, this.cameraSmoothing);

    public void Follow(IEntity target, float smoothing)
    {
        if (this.target != target) previousTarget = this.target;
        this.target = target;
        cameraSmoothing = smoothing;
    } 

    public void Unfollow()
    {
        this.previousTarget = target;
        this.target = null;
    }

    public void ReFollow()
    {
        if (this.previousTarget != null) 
        {
            IEntity temp = this.target;
            this.target = this.previousTarget;
            this.previousTarget = temp;
        }
    }

    public void Reset()
    {
        Position = Vector2.Zero;
        Zoom = 1.0f;
        UpdateMatrices();
    }

    public void Pan(Vector2 direction, float speed = 1f)
    {
        target = null;
        Position += direction * speed;
        UpdateMatrices();
    }

    public void AdjustZoom(float zoom)
    {
        Zoom += zoom;
        UpdateMatrices();
    }

    public Vector2 GetWorldPosition(Vector2 screenPosition) =>
        Vector2.Transform(screenPosition - canvas.RenderRectangle.PositionToVector2(), Matrix.Invert(VirtualViewMatrix));

    public Vector2 WorldToScreenPostition(Vector2 worldPosition) =>
        Vector2.Transform(worldPosition + canvas.RenderRectangle.PositionToVector2(), VirtualViewMatrix);

    private void WindowSizeHasChanged(object sender, EventArgs e)
    {
        Origin = window.ClientBounds.Center.ToVector2();
        UpdateMatrices();
    }

    private void UpdateMatrices()
    {
        ViewMatrix = GetTransformMatrix();
        VirtualViewMatrix = ViewMatrix * canvas.VirtualScaleMatrix;
        ProjectionMatrix = Matrix.CreateOrthographicOffCenter(0f, window.ClientBounds.Width, window.ClientBounds.Height, 0f, 0f, 1f);
    }

    private Matrix GetTransformMatrix() =>
        CalculateViewMatrix(Vector2.One);

    private Matrix CalculateViewMatrix(Vector2 parallax) =>
        Matrix.CreateTranslation(new Vector3(-Position * parallax, 0.0f)) *
        Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
        Matrix.CreateRotationZ(Rotation) *
        Matrix.CreateScale(Zoom, Zoom, 1) *
        Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
    
    public override string ToString() =>
        $"Position: {Position}, Origin: {Origin}, Target: {target?.Transform.Position}";

    public void Dispose()
    {
        window.ClientSizeChanged -= WindowSizeHasChanged;
    }
}