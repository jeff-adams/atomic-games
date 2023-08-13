using System.Linq;
using AtomicGames.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;

namespace AtomicGames.Engine.Components;

public class SpriteObject : Entity
{
    public Sprite Sprite { get; init; }

    public SpriteObject(Sprite sprite)  : base()
    {
        this.Sprite = sprite;
        Origin = new Vector2(Sprite.Width * 0.5f, Sprite.Height * 0.5f);
        SetBounds();
    }

    public SpriteObject(Texture2D texture) 
        : this(texture, 1.0f) { }

    public SpriteObject(Texture2D texture, float scale) 
        : this(new Sprite(texture.Name, texture) { Scale = new Vector2(scale, scale)}) { }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, ShapeBatch shapeBatch)
    {
        //spriteBatch.Draw(texture, Transform.Position, null, Color.White, Transform.Rotation, origin, scale, flip, 0f);
        Sprite.Draw(spriteBatch, Position);
        base.Draw(gameTime, spriteBatch, shapeBatch);
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
        Vector2 topRight = new (topLeft.X + Sprite.Width, topLeft.Y);
        Vector2 bottomRight = new (topLeft.X + Sprite.Width, topLeft.Y + Sprite.Height);
        Vector2 bottomLeft = new (topLeft.X, topLeft.Y + Sprite.Height);

        Vector2 min = new (
            AtomicMath.Min(topLeft.X, topRight.X, bottomRight.X, bottomLeft.X),
            AtomicMath.Min(topLeft.Y, topRight.Y, bottomRight.Y, bottomLeft.Y));

        Vector2 max = new (
            AtomicMath.Max(topLeft.X, topRight.X, bottomRight.X, bottomLeft.X),
            AtomicMath.Max(topLeft.Y, topRight.Y, bottomRight.Y, bottomLeft.Y));

        return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
    }

    public override string ToString() =>
        $"{base.ToString()}, Sprite: {Sprite.Name}";
}