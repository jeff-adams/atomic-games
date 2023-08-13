using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AtomicGames.Engine.Graphics;

namespace AtomicGames.Engine.Components;

public class UI : Entity
{
    public UI(int width, int height)
    {
        Bounds = new Rectangle(0, 0, width, height);
    }

    public override void Update(GameTime gameTime)
    {
        foreach (Entity element in children)
        {
            if (element.IsActive) element.Update(gameTime);
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, ShapeBatch shapeBatch)
    {
        foreach (Entity element in children)
        {
            if (element.IsVisible) element.Draw(gameTime, spriteBatch, shapeBatch);
        }
    }
}