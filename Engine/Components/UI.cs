using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine
{
    public class UI : GameObject
    {
        public UI(int width, int height)
        {
            Bounds = new Rectangle(0, 0, width, height);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameObject element in Children)
            {
                if (element.IsActive) element.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (GameObject element in Children)
            {
                if (element.IsVisible) element.Draw(gameTime, spriteBatch);
            }
        }
    }
}