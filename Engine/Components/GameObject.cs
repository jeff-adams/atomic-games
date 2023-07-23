using System.Collections.Generic;
using AtomicGames.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine.Components;

public abstract class GameObject : IGameObject
{

    public GameObject Parent { get; private set; }
    public HashSet<GameObject> Children { get; private set; }
    public Rectangle Bounds { get; protected set; }

    public bool IsActive { get; set; } = true;
    public bool IsVisible { get; set; } = true;
    public bool IsBoundsVisible { get; set; } = false;

    public Vector2 Position => transform.Position;
    public float Rotation => transform.Rotation;
    public float Scale => transform.Scale;
    public Vector2 Direction => transform.Direction;
    public Matrix Translation => transform.LocalMatrix;
    public Vector2 Origin
    {
        get => transform.Origin;
        set => transform.Origin = value;
    }

    protected Transform transform;

    public GameObject() : this(Vector2.Zero) 
    { }

    public GameObject(Vector2 position)
    {
        Bounds = new Rectangle();
        transform = new Transform().MoveTo(position);
        Children = new HashSet<GameObject>();
    }

    protected virtual void SetBounds() { }

    public void AddParentObject(GameObject parentGameObject)
    {
        Parent = parentGameObject;
        Parent.AddChildObject(this);
        transform.AddParentTransform(parentGameObject.transform);
    }

    public void AddChildObject(GameObject childGameObject)
    {
        if (Children.Add(childGameObject))
            childGameObject.AddParentObject(this);
        SetBounds();
    }

    public void DrawChildren(GameTime gameTime, SpriteBatch spriteBatch, ShapeBatch shapeBatch)
    {
        foreach (GameObject child in Children)
        {
            if (child.IsVisible)
            {
                child.DrawContent(gameTime, spriteBatch, shapeBatch);
                child.DrawChildren(gameTime, spriteBatch, shapeBatch);
            }
        }
    }

    public void Move(Vector2 direction)
    {
        transform.Move(direction);
        SetBounds();
    }

    public void MoveTo(Vector2 position)
    {
        transform.MoveTo(position);
        SetBounds();
    }

    public void RotateToDirection(Vector2 direction)
    {
        transform.RotateToDirection(direction);
        SetBounds();
    }

    public virtual void UpdateContent(GameTime gameTime) { }

    public virtual void DrawContent(GameTime gameTime, SpriteBatch spriteBatch, ShapeBatch shapeBatch) 
    { 
        if (IsBoundsVisible)
        {
            float thickness = 1f;
            Color color = Color.Fuchsia;

            shapeBatch.Rectangle(Bounds, thickness, color);
        }
    }

    public virtual void Enable()
    {
        IsActive = true;
        IsVisible = true;
    }

    public virtual void Disable()
    {
        IsActive = false;
        IsVisible = false;
    }

    public override string ToString() =>
        $"Position: {transform.Position}, Bounds: {Bounds}, HasParent: {Parent != null}";
}