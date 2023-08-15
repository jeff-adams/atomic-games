using Microsoft.Xna.Framework;
using MonoGame.Aseprite;
using MonoGame.Aseprite.Content.Processors;
using MonoGame.Aseprite.Sprites;
using AtomicGames.Engine.Components;

namespace AtomicGames.Sample;

public class Player
{
    public Entity Entity => entity;

    private SpriteEntity entity;
    private PlayScene scene;
    private Vector2 direction;

    public Player(PlayScene scene)
    {
        this.scene = scene;
    }

    public void Initialize(ActionMapPlay input)
    {
        AsepriteFile asePlayer = scene.Load<AsepriteFile>("player/combined");
        Sprite sprite = SpriteProcessor.Process(scene.GraphicsDevice, asePlayer, aseFrameIndex: 0);
        entity = new SpriteEntity(sprite, scene.SpriteBatch, scene.ShapeBatch);
        scene.AddEntity(entity);
        scene.Camera.Follow(entity, 0.15f);

        SubscribeToActions(input);
    }

    public void Update(GameTime gameTime)
    {
        var deltaTime = gameTime.ElapsedGameTime.Ticks;
        entity.Transform.Move(direction * deltaTime);
    }

    private void Move(Vector2 dir)
    {
        float speed = 1f;
        direction *= speed;
    }

    public void SubscribeToActions(ActionMapPlay input)
    {
        input.OnDirectionInput += Move;
    }
}