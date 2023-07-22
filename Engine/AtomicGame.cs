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
    private GameState currentGameState;

    public Canvas Canvas => canvas;
    public Camera Camera => camera;
    public UI UI => ui;

    public AtomicGame(
        GameState firstGameState, 
        string gameTitle, 
        int resolutionWidth, int resolutionHeight,
        int virtualWidth, int virtualHeight)
    {
        graphics = new GraphicsDeviceManager(this);
        graphics.IsFullScreen = true;

        SetResolution(resolutionWidth, resolutionHeight);

        canvas = new Canvas(GraphicsDevice, virtualWidth, virtualHeight);
        camera = new Camera(Window, canvas);
        ui = new UI(virtualWidth, virtualHeight);
        shapeBatch = new ShapeBatch(GraphicsDevice, camera);

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        Window.AllowUserResizing = true;
        Window.IsBorderless = true;
        Window.Title = gameTitle;
        Window.ClientSizeChanged += UpdateCanvasRenderSize;

        currentGameState = firstGameState;
    }

    protected override void Initialize()
    {
        canvas.UpdateRenderRectangle();

        currentGameState.Initialize(this);

        var broadcasters = new IBroadcaster[]
        {
            new KeyboardBroadcaster(),
            new MouseBroadcaster(),
            new GamePadBroadcaster(),
        };
        
        Components.Add(new BroadcastComponent(this, broadcasters));

        inputManager = new InputManager(broadcasters);
        inputManager.SetActionMap(currentGameState.ActionMap);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        currentGameState.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        currentGameState.Update(gameTime);
        camera.Update(gameTime);
        ui.UpdateContent(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        canvas.Activate();
        GraphicsDevice.Clear(currentGameState.BackgroundColor);

        // var effect = new BasicEffect(GraphicsDevice);
        // effect.View = camera.ViewMatrix;
        // effect.Projection = camera.ProjectionMatrix;
        // spriteBatch.Begin(samplerState: SamplerState.PointClamp, effect: effect);

        spriteBatch.Begin(transformMatrix: camera.TransformMatrix, samplerState: SamplerState.PointClamp);
        shapeBatch.Begin(Vector2.Zero);
        currentGameState.Draw(gameTime, spriteBatch, shapeBatch);
        spriteBatch.End();
        shapeBatch.End();

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        shapeBatch.Begin(Vector2.Zero);
        ui.DrawContent(gameTime, spriteBatch, shapeBatch);
        spriteBatch.End();
        shapeBatch.End();

        canvas.Draw(spriteBatch);

        base.Draw(gameTime);
    }

    private void SetResolution(int width, int height)
    {
        graphics.PreferredBackBufferWidth = width;
        graphics.PreferredBackBufferHeight = height;
        graphics.ApplyChanges();
    }

    private void UpdateCanvasRenderSize(object sender, EventArgs e) =>
        canvas.UpdateRenderRectangle();

    private void SwitchGameState(GameState nextGameState)
    {
        if (currentGameState != null)
        {
            currentGameState.OnGameStateSwitch -= SwitchGameState;
            currentGameState.UnloadContent();
        }

        currentGameState = nextGameState;
        currentGameState.Initialize(this);
        currentGameState.LoadContent();
        currentGameState.OnGameStateSwitch += SwitchGameState;
        inputManager.SetActionMap(currentGameState.ActionMap);
    }

    protected override void UnloadContent()
    {
        currentGameState?.UnloadContent();
        base.UnloadContent();
    }
}