using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AtomicGames.Engine.Graphics;

namespace AtomicGames.Engine.Components;

public class ShapeRectangle : GameObject
{
    public Rectangle Rectangle { get; set; }
    public Color Color { get; set; }
    public float LineThickness { get; set; }

    private float width;
    private float height;
    private bool isFloatingPointRectangle = false;

    public ShapeRectangle(Rectangle rect, Color color) 
        : this(rect, 0f, color) { }

    public ShapeRectangle(float width, float height, Color color)
        : this(width, height, 0, color) { }

    public ShapeRectangle(float width, float height, float outlineThickness, Color color)
        : this(new Rectangle(0, 0, (int)width, (int)height), outlineThickness, color)
    {
        this.width = width;
        this.height = height;
        this.isFloatingPointRectangle = true;
    }

    public ShapeRectangle(Rectangle rect, float outlineThickness, Color color)
    {
        rect.X = 0;
        rect.Y = 0;
        this.Rectangle = rect;
        this.LineThickness = outlineThickness;
        this.Color = color;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, ShapeBatch shapeBatch)
    {
        if (LineThickness > 0)
        {
            if (isFloatingPointRectangle)
            {
                shapeBatch.Rectangle(
                    Position.X, Position.Y,
                    Position.X + width, Position.Y,
                    Position.X + width, Position.Y + height,
                    Position.X, Position.Y + height,
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
                shapeBatch.RectangleFill(Position.X, Position.Y, width, height, Color);
            }
            else
            {
                shapeBatch.RectangleFill(Rectangle, Color);
            }
        }

        base.Draw(gameTime, spriteBatch, shapeBatch);
    }

    protected override void SetBounds()
    {
        Bounds = new Rectangle((int)Position.X, (int)Position.Y, Rectangle.Width, Rectangle.Height);
    }

    public override string ToString() =>
        $"{base.ToString()}, Rectangle: {Rectangle}";
}