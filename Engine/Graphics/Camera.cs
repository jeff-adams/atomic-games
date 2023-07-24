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
    public Matrix TransformMatrix { get; private set; }

    private readonly GameWindow window;
    private readonly Canvas canvas;

    private GameObject target;
    private GameObject previousTarget;
    private float cameraSmoothing = 0.05f; // smaller means a longer delay
    
    public Camera(GameWindow gameWindow, Canvas canvas)
    {
        Position = Vector2.Zero;
        Zoom = 1.0f;
        Rotation = 0f;

        this.window = gameWindow;
        this.canvas = canvas;
        gameWindow.ClientSizeChanged += WindowSizeHasChanged;

        // Origin = new Vector2(window.ClientBounds.Center.X, window.ClientBounds.Center.Y);
        Origin = Vector2.Zero;
        UpdateMatrices();
    }

    public void Update(GameTime gameTime)
    {
        if (target != null)
        {
            Position = Vector2.Lerp(Position, target.Position + target.Origin - canvas.VirtualCenter, cameraSmoothing);
            UpdateMatrices();
        }
    }

    public void Follow(GameObject target) => 
        Follow(target, this.cameraSmoothing);

    public void Follow(GameObject target, float smoothing)
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
            GameObject temp = this.target;
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
        Vector2.Transform(screenPosition + canvas.RenderRectangle.PositionToVector2(), Matrix.Invert(TransformMatrix));

    public Vector2 GetScreenPosition(Vector2 worldPosition) =>
        Vector2.Transform(worldPosition, TransformMatrix);

    private void WindowSizeHasChanged(object sender, EventArgs e)
    {
        Origin = window.ClientBounds.Center.ToVector2();
        UpdateMatrices();
    }

    private void UpdateMatrices()
    {
        TransformMatrix = GetTransformMatrix();
    }

    private Matrix GetTransformMatrix() =>
        // Matrix.CreateOrthographicOffCenter(0, canvas.VirtualWidth, canvas.VirtualHeight, 0, 0, 1);
        GetTransformMatrix(Vector2.One);

    private Matrix GetTransformMatrix(Vector2 parallax) => 
        Matrix.CreateTranslation(new Vector3(-Position * parallax, 0.0f)) *
        Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
        Matrix.CreateRotationZ(Rotation) *
        Matrix.CreateScale(Zoom, Zoom, 1) *
        Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
    
    public override string ToString() =>
        $"Position: {Position}, Origin: {Origin}, Target: {target?.Position}";

    public void Dispose()
    {
        window.ClientSizeChanged -= WindowSizeHasChanged;
    }
}