using System.Linq;
using AtomicGames.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;

namespace AtomicGames.Engine.Components;

public class SpriteObject : GameObject
{
    public Sprite Sprite { get; init; }

    public SpriteObject(Texture2D texture) 
        : this(texture, 1.0f) { }

    public SpriteObject(Texture2D texture, float scale) : base()
    {
        Sprite = new Sprite(texture.Name, texture)
        {
            Scale = new Vector2(scale, scale)
        };

        Origin = Sprite.Origin;

        SetBounds();
    }

    public override void DrawContent(GameTime gameTime, SpriteBatch spriteBatch, ShapeBatch shapeBatch)
    {
        //spriteBatch.Draw(texture, Transform.Position, null, Color.White, Transform.Rotation, origin, scale, flip, 0f);
        Sprite.Draw(spriteBatch, Position);
        base.DrawContent(gameTime, spriteBatch, shapeBatch);
    }
    
    protected override void SetBounds()
    {
        Rectangle selfBounds = CalculateSelfBounds();
        Bounds = selfBounds;
        // Not working ??? -> Children.Aggregate(selfBounds, (growingRect, child) => Rectangle.Intersect(growingRect, child.Bounds));
    }

    private Rectangle CalculateSelfBounds()
    {
        Vector2 topLeft = Vector2.Transform(new Vector2(0, 0), Translation);
        Vector2 topRight = new (Sprite.Width, 0);
        Vector2 bottomRight = new (Sprite.Width, Sprite.Height);
        Vector2 bottomLeft = new (0, Sprite.Height);

        Vector2 min = new Vector2(
            AtomicMath.Min(topLeft.X, topRight.X, bottomRight.X, bottomLeft.X),
            AtomicMath.Min(topLeft.Y, topRight.Y, bottomRight.Y, bottomLeft.Y));

        Vector2 max = new Vector2(
            AtomicMath.Max(topLeft.X, topRight.X, bottomRight.X, bottomLeft.X),
            AtomicMath.Max(topLeft.Y, topRight.Y, bottomRight.Y, bottomLeft.Y));

        return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
    }

    public override string ToString() =>
        $"{base.ToString()}, Sprite: {Sprite.Name}";
}