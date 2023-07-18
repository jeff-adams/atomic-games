using AtomicGames.Engine;
using AtomicGames.Sample;
using Microsoft.Xna.Framework;

var gs = new PlayState();
using var game = new AtomicGame(gs, "AtomicGames", new Point(1440, 900));
    game.Run();

