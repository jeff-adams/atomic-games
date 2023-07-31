using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AtomicGames.Engine;
using AtomicGames.Engine.Input;
using AtomicGames.Engine.Graphics;
using AtomicGames.Engine.Components;

namespace AtomicGames.Sample;

public class PlayScene : Scene
{
    private Player player;
    private Debugger debug;
    private SpriteFont font;

    private float deltaTime;

    public override IActionMap ActionMap => input;
    private ActionMapPlay input;

    public PlayScene()
    {
        input = new ActionMapPlay();
        BackgroundColor = Color.DimGray;
        player = new Player(this);
    }

    public override void LoadContent()
    {
        player.Initialize(input);

        font = Load<SpriteFont>("fonts/tiny");
        debug = new Debugger(font);
        UI.AddChildObject(debug);

        SubscribeToActions();
    }

    public override void Update(GameTime gameTime)
    {
        deltaTime = gameTime.ElapsedGameTime.Milliseconds;
        player.Update(gameTime);
        
        debug.AddDebugConsoleMessage("player", player.Object.ToString());
        debug.AddDebugMessage("player", debug.ConvertPositionToDebugMessage(player.Object.Position));
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
        // var renderLocation = new Vector2(Canvas.RenderRectangle.Location.X, Canvas.RenderRectangle.Location.Y);
        // var mouseRenderPosition = mouseScreenPosition - renderLocation;
        // mouseRenderPosition.X = mouseRenderPosition.X / Canvas.RenderRectangle.Width * Canvas.Width;
        // mouseRenderPosition.Y = mouseRenderPosition.Y / Canvas.RenderRectangle.Height * Canvas.Height;

        // var mouseWorldPosition = Vector2.Transform(mouseRenderPosition, Matrix.Invert(Camera.ViewMatrix));
        // var mouseDirection = Vector2.Normalize(player.Position - mouseWorldPosition);
        
        //DEBUG 
        //debug.AddDebugMessage("mouse screen pos", $"x {mouseScreenPosition.X}, y {mouseScreenPosition.Y}");
        //debug.AddDebugMessage("mouse render pos", $"x {mouseRenderPosition.X}, y {mouseRenderPosition.Y}");
        //debug.AddDebugMessage("mouse world pos", $"x {mouseWorldPosition.X}, y {mouseWorldPosition.Y}");
        //debug.AddDebugMessage("ship position", $"x {ship.Transform.Position.X}, y {ship.Transform.Position.Y}");
        //debug.AddDebugMessage("mouse direction", $"x {mouseDirection.X}, y {mouseDirection.Y}");
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

    private void CameraFollowPlayer()
    {
        Camera.Follow(player.Object);
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

    private void SubscribeToActions()
    {
        input.OnQuitPressed += Quit;
        input.OnCameraPan += MoveCamera;
        input.OnCameraZoom += ZoomCamera;
        input.OnMousePositionInput += MousePosition;
        input.OnResetCameraPressed += ResetCamera;
        input.OnCameraTarget += CameraFollowPlayer;
        //Debug
        input.OnToggleDebugPressed += ToggleDebug;
        input.OnPrintDebugPressed += PrintDebug;
        input.OnDebugInputPressed += DebugInputPresses;
    }

    public override void Dispose()
    {
        //TODO: Unsub to all events here
        base.Dispose();
    }
}