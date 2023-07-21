using AtomicGames.Engine;
using AtomicGames.Engine.Input;
using AtomicGames.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AtomicGames.Sample
{
    public class PlayState : GameState
    {
        private Sprite ship;
        private Sprite alert;
        private ShapeRectangle square;
        private Debugger debug;
        SpriteFont font;

        private float deltaTime;
        private float speed = 1.25f;

        public override IActionMap ActionMap => input;
        private ActionMapPlay input;

        public PlayState()
        {
            input = new ActionMapPlay();
            BackgroundColor = Color.DimGray;
        }

        public override void LoadContent()
        {
            font = LoadFont("fonts/MajorMonoDisplay");

            ship = new Sprite(LoadTexture("player/player")); // Need rotation to be (float)(Math.PI / 2)?
            ship.Transform.Position = new Vector2(0, 0);
            AddGameObject(ship);
            Camera.Follow(ship, 0.15f);

            alert = new Sprite(LoadTexture("player/alert"));
            alert.Transform.Position = new Vector2(0, 0);
            ship.AddChildObject(alert);

            var meteorTypes = new Texture2D[]{
                LoadTexture("objects/meteor_a"),
                LoadTexture("objects/meteor_b"),
                LoadTexture("objects/meteor_c"),
            };
            Random rng = new ();
            int numOfMeteors = 20;
            for (int i = 0; i < numOfMeteors; i++)
            {
                int meteorType = rng.Next(0, 3);
                var meteorPos = new Vector2(rng.Next(-2000, 2000), rng.Next(-2000, 2000));
                Sprite meteor = new (meteorTypes[meteorType]);
                meteor.Transform.Position = meteorPos;
                AddGameObject(meteor);
            }

            // square = new ShapeRectangle(new Rectangle(0, 0, 1, 1), Color.CadetBlue);
            // AddGameObject(square);
            
            debug = new Debugger(font);
            UI.AddChildObject(debug);

            SubscribeToActions();
        }

        public override void Update(GameTime gameTime)
        {
            deltaTime = gameTime.ElapsedGameTime.Milliseconds;
            debug.AddDebugMessage("alert transform", alert.ToString());
        }

        private void MousePosition(Vector2 position)
        {
            debug.AddDebugMessage("mouse pos", debug.ConvertPositionToDebugMessage(position));
            Vector2 worldPosition = Camera.GetWorldPosition(position);
            debug.AddDebugMessage("mouse world pos", debug.ConvertPositionToDebugMessage(worldPosition));
        }

        private void MouseDirection(Vector2 mouseScreenPosition)
        {
            //TODO: Refactor and figure out how to use matrix translations to do this
            var renderLocation = new Vector2(Canvas.RenderRectangle.Location.X, Canvas.RenderRectangle.Location.Y);
            var mouseRenderPosition = mouseScreenPosition - renderLocation;
            mouseRenderPosition.X = mouseRenderPosition.X / Canvas.RenderRectangle.Width * Canvas.Width;
            mouseRenderPosition.Y = mouseRenderPosition.Y / Canvas.RenderRectangle.Height * Canvas.Height;

            var mouseWorldPosition = Vector2.Transform(mouseRenderPosition, Matrix.Invert(Camera.ViewMatrix));
            var mouseDirection = Vector2.Normalize(ship.Transform.Position - mouseWorldPosition);
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
                ship.Transform.RotateToDirection(dir);
            }
            
            debug.AddDebugMessage("ship direction", debug.ConvertPositionToDebugMessage(ship.Transform.Direction));
        }

        private void MoveShip(Vector2 dir) => 
            ship.Move(dir * deltaTime * speed);

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
}