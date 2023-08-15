using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AtomicGames.Engine.Input;
using AtomicGames.Engine.Graphics;
using AtomicGames.Engine.Components;

namespace AtomicGames.Engine;

public abstract class Scene : IDisposable
{
    public abstract IActionMap ActionMap { get; }
    public Camera Camera { get; private set; }
    public GraphicsDevice GraphicsDevice { get; private set; }
    public Canvas Canvas { get; private set; }
    public Color BackgroundColor { get; protected set; } = Color.Black;
    public SpriteBatch SpriteBatch { get; private set; }
    public ShapeBatch ShapeBatch { get; private set; }
    
    private List<Entity> entities;
    private ContentManager contentManager;

    public void Initialize(AtomicGame game)
    {
        entities = new List<Entity>();

        this.contentManager = game.Content;
        Camera = game.Camera;
        GraphicsDevice = game.GraphicsDevice;
        Canvas = game.Canvas;
        SpriteBatch = game.SpriteBatch;
        ShapeBatch = game.ShapeBatch;
    }

    public void AddEntity(Entity entity) => 
        entities.Add(entity);

    public T Load<T>(string name) =>
        contentManager.Load<T>(name);

    public void UnloadContent()
    {
        contentManager.Unload();
    }

    public void Draw(GameTime gameTime)
    {
        // TODO: use RenderSystem
    }

    public virtual void Dispose()
    {
        UnloadContent();
    }

    public event Action<Scene> OnGameStateSwitch;

    protected void SwitchGameState(Scene newGameState)
    {
        OnGameStateSwitch?.Invoke(newGameState);
    }

    public abstract void LoadContent();
    public abstract void Update(GameTime gameTime);
}