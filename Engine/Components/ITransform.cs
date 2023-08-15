using System;
using Microsoft.Xna.Framework;

namespace AtomicGames.Engine.Components;

public interface ITransform : IAttachable<ITransform>
{
    Vector2 Origin { get; set; }
    Vector2 Position { get; }
    float Rotation { get; }
    float Scale { get; }
    Vector2 Direction { get; }
    Matrix LocalMatrix { get; }
    Matrix WorldMatrix { get; }
    ITransform RotateToDirection(Vector2 direction);
    ITransform MoveTo(Vector2 position);
    ITransform Move(Vector2 direction);
    event Action OnUpdatedMatrices;
}