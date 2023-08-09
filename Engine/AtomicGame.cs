using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AtomicGames.Engine.Input;
using AtomicGames.Engine.Graphics;
using AtomicGames.Engine.Components;

namespace AtomicGames.Engine;

public class AtomicGame : Game
{
    private readonly GraphicsDeviceManager graphics;
    private readonly Canvas canvas;
    private readonly Camera camera;
    private readonly UI ui;

    private SpriteBatch spriteBatch;
    private ShapeBatch shapeBatch;
    private InputManager inputManager;
    private Scene currentScene;

    public Canvas Canvas => canvas;
    public Camera Camera => camera;
    public UI UI => ui;

    public AtomicGame(
        Scene firstScene, 
        string gameTitle,
        int resolutionWidth, int resolutionHeight, 
        int virtualWidth, int virtualHeight)
        : this(firstScene, gameTitle, virtualWidth, virtualHeight) 
    {
        graphics.IsFullScreen = false;
        graphics.PreferredBackBufferWidth = resolutionWidth;
        graphics.PreferredBackBufferHeight = resolutionHeight;
        graphics.ApplyChanges();
    }

    public AtomicGame(
        Scene firstScene, 
        string gameTitle,
        int virtualWidth, int virtualHeight)
    {
        graphics = new GraphicsDeviceManager(this);
        graphics.ApplyChanges();
        graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
        graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
        graphics.ApplyChanges();

        canvas = new Canvas(GraphicsDevice, virtualWidth, virtualHeight);
        camera = new Camera(Window, canvas);
        ui = new UI(virtualWidth, virtualHeight);
        shapeBatch = new ShapeBatch(GraphicsDevice);

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        Window.AllowUserResizing = false;
        Window.IsBorderless = true;
        Window.Title = gameTitle;
        Window.ClientSizeChanged += UpdateCanvasRenderSize;

        currentScene = firstScene;
    }

    protected override void Initialize()
    {
        canvas.UpdateRenderRectangle();

        currentScene.Initialize(this);

        var broadcasters = new IBroadcaster[]
        {
            new KeyboardBroadcaster(),
            new MouseBroadcaster(),
            new GamePadBroadcaster(),
        };
        
        Components.Add(new BroadcastComponent(broadcasters));

        inputManager = new InputManager(broadcasters);
        inputManager.SetActionMap(currentScene.ActionMap);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        currentScene.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        currentScene.Update(gameTime);
        camera.Update(gameTime);
        ui.UpdateContent(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        canvas.Activate();
        GraphicsDevice.Clear(currentScene.BackgroundColor);

        spriteBatch.Begin(transformMatrix: camera.ViewMatrix, samplerState: SamplerState.PointClamp);
        shapeBatch.Begin(viewMatrix: camera.ViewMatrix, projectionMatrix: camera.ProjectionMatrix);
        currentScene.Draw(gameTime, spriteBatch, shapeBatch);
        spriteBatch.End();
        shapeBatch.End();

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        shapeBatch.Begin(projectionMatrix: camera.ProjectionMatrix);
        ui.DrawContent(gameTime, spriteBatch, shapeBatch);
        spriteBatch.End();
        shapeBatch.End();

        canvas.Draw(spriteBatch);

        base.Draw(gameTime);
    }

    private void UpdateCanvasRenderSize(object sender, EventArgs e) =>
        canvas.UpdateRenderRectangle();

    private void SwitchGameState(Scene nextGameState)
    {
        if (currentScene != null)
        {
            currentScene.OnGameStateSwitch -= SwitchGameState;
            currentScene.UnloadContent();
        }

        currentScene = nextGameState;
        currentScene.Initialize(this);
        currentScene.LoadContent();
        currentScene.OnGameStateSwitch += SwitchGameState;
        inputManager.SetActionMap(currentScene.ActionMap);
    }

    protected override void UnloadContent()
    {
        currentScene?.UnloadContent();
        base.UnloadContent();
    }
}