using System;
using Microsoft.Xna.Framework.Input;

namespace AtomicGames.Engine.Input
{
    public interface IKeyboardBroadcaster : IBroadcaster
    {
        event EventHandler<Keys> KeyPressed;
    }
}