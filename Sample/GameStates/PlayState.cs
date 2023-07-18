using AtomicGames.Engine;
using AtomicGames.Engine.Input;
using AtomicGames.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Sample
{
    public class PlayState : GameState
    {
        private Sprite ship;
        private Debugger debug;
        SpriteFont font;

        private float deltaTime;
        private float speed = 1.25f;

        public override IActionMap ActionMap => input;
        private PlayActionMap input;

        public PlayState()
        {
            input = new PlayActionMap();
        }

        public override void LoadContent()
        {
            font = LoadFont("fonts/MajorMonoDisplay");

            ship = new Sprite(LoadTexture("player/player")); // Need rotation to be (float)(Math.PI / 2)?
            ship.Transform.Position = new Vector2(400, 400);
            debug = new Debugger(font);
            
            AddGameObject(ship);
            AddGameObject(debug);
            SubscribeToActions();
        }


        public override void Update(GameTime gameTime)
        {
            deltaTime = gameTime.ElapsedGameTime.Milliseconds;

            var button = Buttons.Back;
            var isButtonDown = GamePad.GetState(0).IsButtonDown(button);
            debug.AddDebugMessage(button.ToString().ToLower(), isButtonDown.ToString().ToLower());
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
            Direction(mouseDirection);
            
            //DEBUG 
            //debug.AddDebugMessage("mouse screen pos", $"x {mouseScreenPosition.X}, y {mouseScreenPosition.Y}");
            //debug.AddDebugMessage("mouse render pos", $"x {mouseRenderPosition.X}, y {mouseRenderPosition.Y}");
            //debug.AddDebugMessage("mouse world pos", $"x {mouseWorldPosition.X}, y {mouseWorldPosition.Y}");
            //debug.AddDebugMessage("ship position", $"x {ship.Transform.Position.X}, y {ship.Transform.Position.Y}");
            //debug.AddDebugMessage("mouse direction", $"x {mouseDirection.X}, y {mouseDirection.Y}");
        }

        private void Direction(Vector2 dir)
        {
            if (dir != Vector2.Zero)
            {
                dir.Y *= -1;
                ship.Transform.RotateToDirection(dir);
            }
            
            debug.AddDebugMessage("ship direction", $"x {ship.Transform.Direction.X}, y {ship.Transform.Direction.Y}");
        }

        private void MoveDirection(Vector2 dir) => ship.Transform.Position += dir * deltaTime * speed;

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
            input.GamePadLeftStickPositionAction += Direction;
            input.ToggleDebugAction += ToggleDebug;
            input.Quit += Quit;
            input.DirectionAction += MoveDirection;
            //input.MousePositionAction += MouseDirection;
            input.DebugInputPressed += DebugInputPresses;
            input.ChangeScreenResolution += ChangeScreenResolution;
        }

        public override void Dispose()
        {
            input.GamePadLeftStickPositionAction -= Direction;
            input.ToggleDebugAction -= ToggleDebug;
            input.Quit -= Quit;
            input.DirectionAction -= MoveDirection;
            input.MousePositionAction -= MouseDirection;
            input.ChangeScreenResolution -= ChangeScreenResolution;

            base.Dispose();
        }
    }
}