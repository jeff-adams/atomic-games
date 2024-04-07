using Microsoft.Xna.Framework;

namespace AtomicGames.Engine.Input;

public sealed class BroadcastComponent : GameComponent
{
    private readonly IBroadcaster[] broadcasters;

    public BroadcastComponent(Game game, IBroadcaster[] broadcasters) : base(game)
    {
        this.broadcasters = broadcasters;
        Enabled = true;
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var broadcaster in broadcasters)
        {
            if (broadcaster.IsEnabled) broadcaster.Update(gameTime);
        }
    }
}
