using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AtomicGames.Graphics;

namespace AtomicGames.Sample
{
    public class SampleGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Display display;

        private const int gameWidth = 1440;
        private const int gameHeight = 900;
        private const float speed = 1.25f;

        private Sprite[] sprites;

        private KeyboardState keyState;

        public SampleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = gameWidth;
            graphics.PreferredBackBufferHeight = gameHeight;
            graphics.ApplyChanges();

            Window.AllowUserResizing = true;
            Window.Title = "AtomicGames";

            display = new Display(this, gameWidth, gameHeight);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            sprites = new Sprite[]
            {
                new Sprite(Content.Load<Texture2D>("bomb"), 3f)
            };
        }

        protected override void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var dirX = 0;
            var dirY = 0;

            var pressedKeys = keyState.GetPressedKeys();
            if (pressedKeys.Length > 0)
            {
                dirX += pressedKeys.Contains(Keys.Left) ? -1 : 0;
                dirX += pressedKeys.Contains(Keys.Right) ? 1 : 0;
                dirY += pressedKeys.Contains(Keys.Up) ? -1 : 0;
                dirY += pressedKeys.Contains(Keys.Down) ? 1 : 0;
            }

            if (dirX != 0 || dirY != 0)
            {
                var deltaTime = gameTime.ElapsedGameTime.Milliseconds;
                sprites[0].Position = new Vector2(sprites[0].Position.X + dirX * deltaTime * speed, 
                                                  sprites[0].Position.Y + dirY * deltaTime * speed);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            display.SetTarget();
            GraphicsDevice.Clear(Color.Black);

            display.BatchSprites(sprites);
            display.BatchShapes();

            display.UnSetTarget();
            display.Draw();

            base.Draw(gameTime);
        }
    }
}
