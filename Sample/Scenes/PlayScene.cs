using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AtomicGames.Engine;
using AtomicGames.Engine.Input;
using AtomicGames.Engine.Graphics;
using AtomicGames.Engine.Components;

namespace AtomicGames.Sample;

public class PlayScene : Scene
{
    private SpriteObject ship;
    private SpriteObject alert;
    private ShapeRectangle square;
    private Debugger debug;
    SpriteFont smallFont;
    SpriteFont largeFont;

    private float deltaTime;

    public override IActionMap ActionMap => input;
    private ActionMapPlay input;

    public PlayScene()
    {
        input = new ActionMapPlay();
        BackgroundColor = Color.DimGray;
    }

    public override void LoadContent()
    {
        smallFont = Load<SpriteFont>("fonts/MajorMonoDisplay_small");
        largeFont = Load<SpriteFont>("fonts/MajorMonoDisplay_large");

        square = new ShapeRectangle(10f, 10f, Color.CadetBlue);
        AddGameObject(square);

        var meteorTypes = new Texture2D[]{
            Load<Texture2D>("objects/meteor_a"),
            Load<Texture2D>("objects/meteor_b"),
            Load<Texture2D>("objects/meteor_c"),
        };
        Random rng = new ();
        int numOfMeteors = 20;
        for (int i = 0; i < numOfMeteors; i++)
        {
            int meteorType = rng.Next(0, 3);
            var meteorPos = new Vector2(rng.Next(-2000, 2000), rng.Next(-2000, 2000));
            SpriteObject meteor = new (meteorTypes[meteorType]);
            meteor.MoveTo(meteorPos);
            AddGameObject(meteor);
        }

        ship = new SpriteObject(Load<Texture2D>("player/player"));
        ship.MoveTo(new Vector2(0f, 0f));
        AddGameObject(ship);
        Camera.Follow(ship, 0.15f);

        alert = new SpriteObject(Load<Texture2D>("player/alert"), 0.4f);
        alert.MoveTo(new Vector2(75f, 35f));
        ship.AddChildObject(alert); 
        
        debug = new Debugger(smallFont);
        UI.AddChildObject(debug);

        SubscribeToActions();
    }

    public override void Update(GameTime gameTime)
    {
        deltaTime = gameTime.ElapsedGameTime.Milliseconds;
        debug.AddDebugConsoleMessage("ship", ship.ToString());
        debug.AddDebugConsoleMessage("alert", alert.ToString());
        debug.AddDebugConsoleMessage("square", square.ToString());
        debug.AddDebugConsoleMessage("camera", Camera.ToString());
        debug.AddDebugConsoleMessage("canvas", Canvas.ToString());
    }

    private void MousePosition(Vector2 position)
    {
        Vector2 mouseWorldPosition = Camera.GetWorldPosition(position);
        debug.AddDebugMessage("mouse pos", debug.ConvertPositionToDebugMessage(position));
        debug.AddDebugMessage("mouse world pos", debug.ConvertPositionToDebugMessage(mouseWorldPosition));
    }

    private void MouseDirection(Vector2 mouseScreenPosition)
    {
        //TODO: Refactor and figure out how to use matrix translations to do this
        var renderLocation = new Vector2(Canvas.RenderRectangle.Location.X, Canvas.RenderRectangle.Location.Y);
        var mouseRenderPosition = mouseScreenPosition - renderLocation;
        // mouseRenderPosition.X = mouseRenderPosition.X / Canvas.RenderRectangle.Width * Canvas.Width;
        // mouseRenderPosition.Y = mouseRenderPosition.Y / Canvas.RenderRectangle.Height * Canvas.Height;

        var mouseWorldPosition = Vector2.Transform(mouseRenderPosition, Matrix.Invert(Camera.ViewMatrix));
        var mouseDirection = Vector2.Normalize(ship.Position - mouseWorldPosition);
        RotateShip(mouseDirection);
        
        //DEBUG 
        //debug.AddDebugMessage("mouse screen pos", $"x {mouseScreenPosition.X}, y {mouseScreenPosition.Y}");
        //debug.AddDebugMessage("mouse render pos", $"x {mouseRenderPosition.X}, y {mouseRenderPosition.Y}");
        //debug.AddDebugMessage("mouse world pos", $"x {mouseWorldPosition.X}, y {mouseWorldPosition.Y}");
        //debug.AddDebugMessage("ship position", $"x {ship.Transform.Position.X}, y {ship.Transform.Position.Y}");
        //debug.AddDebugMessage("mouse direction", $"x {mouseDirection.X}, y {mouseDirection.Y}");
    }

    private void RotateShip(Vector2 dir)
    {
        if (dir != Vector2.Zero)
        {
            dir.Y *= -1;
            ship.RotateToDirection(dir);
        }
        
        debug.AddDebugMessage("ship direction", debug.ConvertPositionToDebugMessage(ship.Direction));
    }

    private void MoveShip(Vector2 dir)
    {
        float speed = 1f;
        ship.Move(dir * deltaTime * speed);
    }

    private void MoveCamera(Vector2 input)
    {
        float moveSpeed = 6f;
        Camera.Pan(input * moveSpeed);
    }

    private void ZoomCamera(float direction)
    {
        float zoomSpeed = 0.02f;
        Camera.AdjustZoom(direction * zoomSpeed);
    }

    private void ResetCamera()
    {
        Camera.Reset();
    }

    private void CameraFollowShip()
    {
        Camera.Follow(ship);
    }

    private void ToggleDebug()
    {
        if(debug.IsActive)
        {
            debug.Disable();
        }
        else
        {
            debug.Enable();
        }
    }

    private void PrintDebug() => 
        debug.PrintDebugMessagesToConsole();

    private void DebugInputPresses(InputState state)
    {
        string stateType = state.Pressed ? "pressed" : "held";
        debug.AddDebugMessage(stateType, state.Name.ToLower());
    }

    private void Quit() =>
        System.Environment.Exit(0);

    private void ChangeScreenResolution()
    {
        
    }

    private void SubscribeToActions()
    {
        input.OnQuitPressed += Quit;
        input.OnDirectionInput += MoveShip;
        input.OnCameraPan += MoveCamera;
        input.OnCameraZoom += ZoomCamera;
        input.OnMousePositionInput += MousePosition;
        input.OnResetCameraPressed += ResetCamera;
        input.OnCameraTarget += CameraFollowShip;
        //Debug
        input.OnToggleDebugPressed += ToggleDebug;
        input.OnPrintDebugPressed += PrintDebug;
        input.OnDebugInputPressed += DebugInputPresses;
    }

    public override void Dispose()
    {
        //TODO: Unsub to all events hered
        base.Dispose();
    }
}