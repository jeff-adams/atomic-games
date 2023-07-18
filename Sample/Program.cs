using AtomicGames.Engine;
using AtomicGames.Sample;

var gs = new PlayState();
using var game = new AtomicGame(gs, "AtomicGames", width: 1440, height: 900);
    game.Run();

