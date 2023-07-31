using AtomicGames.Engine;
using AtomicGames.Sample;

PlayScene startingScene = new ();

using var game = new AtomicGame(
        startingScene, 
        "AtomicGames",
        virtualWidth: 64, virtualHeight: 64);
game.Run();

