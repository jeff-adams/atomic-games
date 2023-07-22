using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AtomicGames.Engine.Graphics;

namespace AtomicGames.Engine.Components;

public class ShapeRectangle : GameObject
{
    public Rectangle Rectangle { get; set; }
    public Color Color { get; set; }
    public float LineThickness { get; set; }

    public ShapeRectangle(Rectangle rect, Color color) 
        : this(rect, 0f, color) { }

    public ShapeRectangle(Rectangle rect, float outlineThickness, Color color)
    {
        this.Rectangle = rect;
        this.LineThickness = outlineThickness;
        this.Color = color;
    }

    public override void DrawContent(GameTime gameTime, SpriteBatch spriteBatch, ShapeBatch shapeBatch)
    {
        if (LineThickness > 0)
        {
            shapeBatch.Rectangle(Rectangle, LineThickness, Color);
        }
        else
        {
            shapeBatch.RectangleFill(Rectangle, Color);
        }

        base.DrawContent(gameTime, spriteBatch, shapeBatch);
    }
}