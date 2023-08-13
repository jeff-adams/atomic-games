namespace AtomicGames.Engine;

public interface IEntity
{
    Transform Transform { get; }
    IEntity Parent { get; }
    IEntity AttachTo(IEntity parent);
    IEntity Detach();
    IEntity Attach(params IEntity[] children);
}