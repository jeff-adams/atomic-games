namespace AtomicGames.Engine.Components;

public interface IAttachable<T>
{
    T Parent { get; }
    T AttachTo(T parent);
    T Detach();
}