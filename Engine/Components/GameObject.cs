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
    public Transform Transform { get; }

    public bool IsActive { get; set; } = true;
    public bool IsVisible { get; set; } = true;
    public bool IsBoundsVisible { get; set; } = false;

    public GameObject() : this(Vector2.Zero) 
    { }

    public GameObject(Vector2 position)
    {
        Bounds = new Rectangle();
        Transform = new Transform().MoveTo(position);
        Children = new HashSet<GameObject>();
    }

    protected virtual void SetBounds() { }

    public void AddParentObject(GameObject parentGameObject)
    {
        Parent = parentGameObject;
        Parent.AddChildObject(this);
        Transform.AddParentTransform(parentGameObject.Transform);
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
        Transform.Move(direction);
        SetBounds();
    }

    public void MoveTo(Vector2 position)
    {
        Transform.MoveTo(position);
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
        $"Position: {Transform.Position}, Bounds: {Bounds}, HasParent: {Parent != null}";
}