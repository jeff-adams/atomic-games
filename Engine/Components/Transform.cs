using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AtomicGames.Engine;

public class Transform
{
    private Transform parentTransform;
    private HashSet<Transform> childrenTransforms;
    private Vector2 position;
    private Vector2 origin;
    private float rotation;
    private float scale;

    public Vector2 Origin { get; set; }
    
    public Vector2 Position 
    { 
        get => 
            parentTransform is null 
            ? Vector2.Transform(position, WorldMatrix)
            : Vector2.Transform(position, parentTransform.WorldMatrix);
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

    public Transform AddParentTransform(Transform parentTransform)
    {
        if(this.parentTransform == parentTransform) return this;

        if(this.parentTransform is not null)    
            this.parentTransform.OnUpdatedMatrices -= UpdateMatrices;

        this.parentTransform = parentTransform.AddChildTransform(this);
        parentTransform.OnUpdatedMatrices += UpdateMatrices;

        return this;
    }

    protected Transform AddChildTransform(Transform childTransform)
    {
        if (childrenTransforms is null) 
            childrenTransforms = new HashSet<Transform>();
        childrenTransforms.Add(childTransform);
        return this;
    }

    public Transform RotateToDirection(Vector2 direction)
    {
        Rotation = (float)(Math.Atan2(direction.Y, direction.X));
        Direction = direction;
        return this;
    }

    public Transform MoveTo(Vector2 position)
    {
        this.position = Vector2.Transform(position, Matrix.Invert(WorldMatrix));
        UpdateMatrices();
        return this;
    }

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