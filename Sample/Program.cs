using System;
using AtomicGames.Engine;

namespace AtomicGames.Sample
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            var gs = new PlayState();
            using (var game = new BaseGame(gs, "AtomicGames"))
                game.Run();
        }
    }
}
