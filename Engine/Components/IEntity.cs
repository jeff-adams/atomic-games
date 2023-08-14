namespace AtomicGames.Engine.Components;

public interface IEntity : IAttachable<IEntity>
{
    ITransform Transform { get; }
}