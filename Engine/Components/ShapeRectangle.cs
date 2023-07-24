using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AtomicGames.Engine.Graphics;

namespace AtomicGames.Engine.Components;

public class ShapeRectangle : GameObject
{
    public Rectangle Rectangle { get; set; }
    public Color Color { get; set; }
    public float LineThickness { get; set; }

    private float x;
    private float y;
    private float width;
    private float height;
    private bool isFloatingPointRectangle = false;

    public ShapeRectangle(Rectangle rect, Color color) 
        : this(rect, 0f, color) { }

    public ShapeRectangle(float x, float y, float width, float height, Color color)
        : this(x, y, width, height, 0, color) { }

    public ShapeRectangle(float x, float y, float width, float height, float outlineThickness, Color color)
        : this(new Rectangle((int)x, (int)y, (int)width, (int)height), outlineThickness, color)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
        this.isFloatingPointRectangle = true;
    }

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
            if (isFloatingPointRectangle)
            {
                shapeBatch.Rectangle(
                    x, y,
                    x + width, y,
                    x + width, y + height,
                    x, y + height,
                    LineThickness, Color);
            }
            else
            {
                shapeBatch.Rectangle(Rectangle, LineThickness, Color);
            }
        }
        else
        {
            if (isFloatingPointRectangle)
            {
                shapeBatch.RectangleFill(x, y, width, height, Color);
            }
            else
            {
                shapeBatch.RectangleFill(Rectangle, Color);
            }
        }

        base.DrawContent(gameTime, spriteBatch, shapeBatch);
    }
}