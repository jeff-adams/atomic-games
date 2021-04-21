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
            using (var game = new AtomicGame(gs, "AtomicGames"))
                game.Run();
        }
    }
}
