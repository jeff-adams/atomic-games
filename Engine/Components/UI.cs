using AtomicGames.Engine.Graphics;
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

        public override void UpdateContent(GameTime gameTime)
        {
            foreach (GameObject element in Children)
            {
                if (element.IsActive) element.UpdateContent(gameTime);
            }
        }

        public override void DrawContent(GameTime gameTime, SpriteBatch spriteBatch, ShapeBatch shapeBatch)
        {
            foreach (GameObject element in Children)
            {
                if (element.IsVisible) element.DrawContent(gameTime, spriteBatch, shapeBatch);
            }
        }
    }
}