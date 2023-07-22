using AtomicGames.Engine;
using AtomicGames.Sample;

var gs = new PlayState();
// using var game = new AtomicGame(gs, "AtomicGames", width: 1440, height: 900);
using var game = new AtomicGame(
        gs, 
        "AtomicGames", 
        resolutionWidth: 1400, resolutionHeight: 900, 
        virtualWidth: 1024, virtualHeight: 768);
game.Run();

