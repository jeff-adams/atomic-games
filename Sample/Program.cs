using AtomicGames.Engine;
using AtomicGames.Sample;

PlayScene startingScene = new ();

using var game = new AtomicGame(
        startingScene, 
        "AtomicGames",
        virtualWidth: 1024, virtualHeight: 768);
game.Run();

