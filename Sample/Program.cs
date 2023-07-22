using AtomicGames.Engine;
using AtomicGames.Sample;

var gs = new PlayState();

using var game = new AtomicGame(
        gs, 
        "AtomicGames", 
        resolutionWidth: 3840, resolutionHeight: 2160, 
        virtualWidth: 1024, virtualHeight: 768);
game.Run();

