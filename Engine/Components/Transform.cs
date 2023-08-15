using System;
using Microsoft.Xna.Framework;

namespace AtomicGames.Engine.Components;

public sealed class Transform : ITransform
{
    public ITransform Parent { get; private set; }
    public Vector2 Origin { get; set; }
    
    // Gives the world coordinates
    public Vector2 Position 
    { 
        get => 
            Parent is null 
            ? Vector2.Transform(position, LocalMatrix)
            : Parent.Position + Vector2.Transform(position, LocalMatrix);
    }

    public float Rotation 
    { 
        get => rotation;
        private set
        {
            rotation = value;
            UpdateMatrices();
        }
    }

    public float Scale
    {
        get => scale;
        private set
        {
            scale = value;
            UpdateMatrices();
        }
    }

    public Vector2 Direction { get; private set; }
    public Matrix LocalMatrix { get; private set; }
    public Matrix WorldMatrix { get; private set; }

    private Vector2 position;
    private Vector2 origin;
    private float rotation;
    private float scale;

    public Transform() : this(Vector2.Zero) 
    { }

    public Transform(Vector2 origin)
    {
        this.position = Vector2.Zero;
        this.origin = origin;
        this.scale = 1f;
        UpdateMatrices();
    }

    /// <summary>
    /// Adds a parent <see cref="ITransform"/> to this <see cref="ITransform"/> and will update it's World Matrix according to the parent
    /// </summary>
    /// <typeparam name="parent">The parent <see cref="ITransform"/> to attach this <see cref="ITransform"/> to</param>
    /// <returns>This <see cref="ITransform"/> object for method chaining</returns>
    public ITransform AttachTo(ITransform parent)
    {
        if(Parent == parent) return this;

        if(Parent is not null)    
            Parent.OnUpdatedMatrices -= UpdateMatrices;

        Parent = parent;
        Parent.OnUpdatedMatrices += UpdateMatrices;

        return this;
    }

    /// <summary>
    /// Removes the parent <see cref=" Transform"/> from this <see cref=" Transform"/>
    /// </summary>
    /// <returns>This <see cref=" Transform"/> object for method chaining</returns>
    public ITransform Detach()
    {
        if(Parent is null) return this;
        
        Parent.OnUpdatedMatrices -= UpdateMatrices;
        Parent = null;
        UpdateMatrices();

        return this;
    }

    /// <summary>
    /// Rotates the <see cref=" Transform"/> in a direction
    /// </summary>
    /// <param name="direction">The direction to rotate towards</param>
    /// <returns>This <see cref=" Transform"/> object for method chaining</returns>
    public ITransform RotateToDirection(Vector2 direction)
    {
        Rotation = (float)Math.Atan2(direction.Y, direction.X);
        Direction = direction;
        return this;
    }

    /// <summary>
    /// Moves the <see cref=" Transform"/> position to a specific world space location
    /// </summary>
    /// <param name="position">The world space location</param>
    /// <returns>This <see cref=" Transform"/> object for method chaining</returns>
    public ITransform MoveTo(Vector2 position)
    {
        this.position = Vector2.Transform(position, Matrix.Invert(WorldMatrix));
        UpdateMatrices();
        return this;
    }

    /// <summary>
    /// Moves the <see cref=" Transform"/> position in a given direction
    /// </summary>
    /// <param name="direction">The direction and velocity of movement</param>
    /// <returns>This <see cref=" Transform"/> object for method chaining</returns>
    public ITransform Move(Vector2 direction)
    {
        Vector2 newWorldPosition = Vector2.Transform(position + direction, WorldMatrix);
        return MoveTo(newWorldPosition);
    }

    /// <summary>
    /// Triggered when the Local or World Matrix of this <see cref=" Transform"/> are updated
    /// </summary>
    public event Action OnUpdatedMatrices;

    private void UpdateMatrices()
    {
        LocalMatrix = GetLocalMatrix();
        WorldMatrix = GetWorldMatrix();

        OnUpdatedMatrices?.Invoke();
    }

    private Matrix GetWorldMatrix()
    {
        if (Parent is null) return LocalMatrix;

        return LocalMatrix * Matrix.Invert(Parent.WorldMatrix);
    }

    private Matrix GetLocalMatrix() =>
        Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
        Matrix.CreateScale(scale) *
        Matrix.CreateRotationZ(rotation) *
        Matrix.CreateTranslation(new Vector3(position, 0));

    public override string ToString() =>
        $"Position: {Position}, Rotation: {Rotation}, Scale: {Scale}, HasParent: {Parent != null}";

}