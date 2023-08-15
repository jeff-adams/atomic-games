using Microsoft.Xna.Framework;

namespace AtomicGames.Engine.Components;

public abstract class Entity : IEntity
{
    public ITransform Transform { get; }
    public IEntity Parent { get; private set; }

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

    public override string ToString() =>
        $"Position: {Transform.Position}, Origin: {Transform.Origin} HasParent: {Parent != null}";
}