using Microsoft.Xna.Framework;

namespace AtomicGames.Engine.Input
{
    public class BroadcastComponent : GameComponent, IUpdateable
    {
        private readonly IBroadcaster[] broadcasters;

        public BroadcastComponent(Game game, IBroadcaster[] broadcasters) 
            : base(game)
        {
            this.broadcasters = broadcasters;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var broadcaster in broadcasters)
            {
                if (broadcaster.IsEnabled)
                {
                    broadcaster.Update(gameTime);
                }
            }
        }
    }
}