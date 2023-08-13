using AtomicGames.Engine;
using AtomicGames.Sample;

PlayScene startingScene = new ();

using var game = new AtomicGame(
        startingScene, 
        "AtomicGames",
        virtualWidth: 512, virtualHeight: 512);
game.Run();

