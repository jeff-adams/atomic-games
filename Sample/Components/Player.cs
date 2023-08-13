using AtomicGames.Engine;
using AtomicGames.Engine.Components;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite;
using MonoGame.Aseprite.Content.Processors;
using MonoGame.Aseprite.Sprites;

namespace AtomicGames.Sample;

public class Player
{
    public Entity Object => go;

    private Entity go;
    private PlayScene scene;

    public Player(PlayScene scene)
    {
        this.scene = scene;
    }

    public void Initialize(ActionMapPlay input)
    {
        AsepriteFile asePlayer = scene.Load<AsepriteFile>("player/combined");
        Sprite sprite = SpriteProcessor.Process(scene.GraphicsDevice, asePlayer, aseFrameIndex: 0);
        go = new SpriteObject(sprite);
        scene.AddGameObject(go);
        scene.Camera.Follow(go, 0.15f);

        SubscribeToActions(input);
    }

    public void Update(GameTime gameTime)
    {
        
    }

    private void Move(Vector2 dir)
    {
        float speed = 1f;
        go.Move(dir * speed);
    }

    public void SubscribeToActions(ActionMapPlay input)
    {
        input.OnDirectionInput += Move;
    }
}