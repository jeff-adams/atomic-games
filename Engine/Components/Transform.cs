using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AtomicGames.Engine;

public sealed class Transform
{
    private Transform parentTransform;
    private Vector2 position;
    private Vector2 origin;
    private float rotation;
    private float scale;

    public Vector2 Origin { get; set; }
    
    // Gives the world coordinates
    public Vector2 Position 
    { 
        get => 
            parentTransform is null 
            ? Vector2.Transform(position, LocalMatrix)
            : parentTransform.Position + Vector2.Transform(position, LocalMatrix);
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

    public Transform() : this(Vector2.Zero) 
    { }

    public Transform(Vector2 origin)
    {
        this.position = Vector2.Zero;
        this.origin = origin;
        scale = 1f;
        UpdateMatrices();
    }

    /// <summary>
    /// Adds a parent <see cref=" Transform"/> to this <see cref=" Transform"/> and will update it's World Matrix according to the parent
    /// </summary>
    /// <param name="parentTransform">The parent <see cref=" Transform"/> to attach this <see cref=" Transform"/> to</param>
    /// <returns>This <see cref=" Transform"/> object for method chaining</returns>
    public Transform AddParentTransform(Transform parentTransform)
    {
        if(this.parentTransform == parentTransform) return this;

        if(this.parentTransform is not null)    
            this.parentTransform.OnUpdatedMatrices -= UpdateMatrices;

        parentTransform.OnUpdatedMatrices += UpdateMatrices;

        return this;
    }

    /// <summary>
    /// Removes the parent <see cref=" Transform"/> from this <see cref=" Transform"/>
    /// </summary>
    /// <returns>This <see cref=" Transform"/> object for method chaining</returns>
    public Transform RemoveParentTransform()
    {
        if(parentTransform == null) return this;
        
        parentTransform.OnUpdatedMatrices -= UpdateMatrices;
        parentTransform = null;
        UpdateMatrices();

        return this;
    }

    /// <summary>
    /// Rotates the <see cref=" Transform"/> in a direction
    /// </summary>
    /// <param name="direction">The direction to rotate towards</param>
    /// <returns>This <see cref=" Transform"/> object for method chaining</returns>
    public Transform RotateToDirection(Vector2 direction)
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
    public Transform MoveTo(Vector2 position)
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
    public Transform Move(Vector2 direction)
    {
        Vector2 newWorldPosition = Vector2.Transform(this.position + direction, WorldMatrix);
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
        if (parentTransform is null) return LocalMatrix;

        return LocalMatrix * Matrix.Invert(parentTransform.WorldMatrix);
    }

    private Matrix GetLocalMatrix() =>
        Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
        Matrix.CreateScale(scale) *
        Matrix.CreateRotationZ(rotation) *
        Matrix.CreateTranslation(new Vector3(position, 0));

    public override string ToString() =>
        $"Position: {Position}, Rotation: {Rotation}, Scale: {Scale}, HasParent: {parentTransform != null}";
}