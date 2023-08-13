using System;
using System.Collections.Generic;
using AtomicGames.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine.Components;

public abstract class GameObject : IGameObject, IDrawable
{
    public Transform Transform { get; }
    public IGameObject Parent { get; private set; }

    private int drawOrder;
    public int DrawOrder 
    {
        get => drawOrder;
        set 
        {
            drawOrder = value;
            DrawOrderChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public event EventHandler<EventArgs> DrawOrderChanged;

    private bool isVisible;
    public bool Visible {
        get => isVisible;
        set 
        {
            isVisible = value;
            VisibleChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public event EventHandler<EventArgs> VisibleChanged;

    private HashSet<IGameObject> children;

    public GameObject() : this(Vector2.Zero) 
    { }

    public GameObject(Vector2 position)
    {
        Transform = new Transform().MoveTo(position);
        children = new HashSet<IGameObject>();
    }

    public IGameObject AttachTo(IGameObject parent)
    {
        Parent = parent;
        parent.Attach(this);
        Transform.AddParentTransform(parent.Transform);
        return this;
    }

    public IGameObject Detach()
    {
        Parent = null;
        Transform.RemoveParentTransform();
        return this;
    }

    public IGameObject Attach(params IGameObject[] children)
    {
        foreach (IGameObject child in children)
        {
            this.children.Add(child);
        }
        return this;
    }

    public void Draw(GameTime gameTime)
    { }

    public override string ToString() =>
        $"Position: {Transform.Position}, Origin: {Transform.Origin} HasParent: {Parent != null}";
}