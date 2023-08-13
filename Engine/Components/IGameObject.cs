namespace AtomicGames.Engine;

public interface IGameObject
{
    Transform Transform { get; }
    IGameObject Parent { get; }
    IGameObject AttachTo(IGameObject parent);
    IGameObject Detach();
    IGameObject Attach(params IGameObject[] children);
}