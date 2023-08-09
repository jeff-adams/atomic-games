using System;
using Microsoft.Xna.Framework;

namespace AtomicGames.Engine.Input;

public sealed class BroadcastComponent : IGameComponent, IUpdateable
{
    public event EventHandler<EventArgs> EnabledChanged;
    public event EventHandler<EventArgs> UpdateOrderChanged;
    public bool Enabled 
    {
        get => enabled;
        set { enabled = value; EnabledChanged?.Invoke(this, EventArgs.Empty); }
    }
    public int UpdateOrder
    {
        get => updateOrder;
        set { updateOrder = value; UpdateOrderChanged?.Invoke(this, EventArgs.Empty); }
    }

    private bool enabled;
    private int updateOrder;
    private readonly IBroadcaster[] broadcasters;

    public BroadcastComponent(IBroadcaster[] broadcasters) 
    {
        this.broadcasters = broadcasters;
        this.enabled = true;
        this.updateOrder = 1;
    }

    public void Initialize() { }    

    public void Update(GameTime gameTime)
    {
        foreach (var broadcaster in broadcasters)
        {
            if (broadcaster.IsEnabled) broadcaster.Update(gameTime);
        }
    }
}
