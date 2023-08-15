using Microsoft.Xna.Framework;
using AtomicGames.Engine.Components;

namespace AtomicGames.Engine.Systems;

public interface ISystem<T> where T : IComponent
{
    void Initialize(AtomicGame game);
    ISystem<T> Add(T component);
    ISystem<T> Remove(T component);
}