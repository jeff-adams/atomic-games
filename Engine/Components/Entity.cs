using System;
using Microsoft.Xna.Framework;

namespace AtomicGames.Engine.Components;

public abstract class Entity : IEntity, IDrawable
{
    public ITransform Transform { get; }
    public IEntity Parent { get; private set; }

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


    public Entity() : this(Vector2.Zero) 
    { }

    public Entity(Vector2 position)
    {
        Transform = new Transform().MoveTo(position);
    }

    public IEntity AttachTo(IEntity parent)
    {
        Parent = parent;
        Transform.AttachTo(parent.Transform);
        return this;
    }

    public IEntity Detach()
    {
        Parent = null;
        Transform.Detach();
        return this;
    }

    public void Draw(GameTime gameTime)
    { }

    public override string ToString() =>
        $"Position: {Transform.Position}, Origin: {Transform.Origin} HasParent: {Parent != null}";
}