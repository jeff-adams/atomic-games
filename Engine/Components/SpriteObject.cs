using System.Linq;
using AtomicGames.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;

namespace AtomicGames.Engine.Components;

public class SpriteEntity : Entity
{
    public Sprite Sprite { get; init; }

    private readonly SpriteBatch spriteBatch;
    private readonly ShapeBatch shapeBatch;

    public SpriteEntity(Sprite sprite, SpriteBatch spriteBatch, ShapeBatch shapeBatch)  : base()
    {
        Sprite = sprite;
        this.spriteBatch = spriteBatch;
        this.shapeBatch = shapeBatch;
        Transform.Origin = new Vector2(Sprite.Width * 0.5f, Sprite.Height * 0.5f);
    }

    public SpriteEntity(Texture2D texture, SpriteBatch spriteBatch, ShapeBatch shapeBatch) 
        : this(texture, 1.0f, spriteBatch, shapeBatch) { }

    public SpriteEntity(Texture2D texture, float scale, SpriteBatch spriteBatch, ShapeBatch shapeBatch) 
        : this(new Sprite(texture.Name, texture) { Scale = new Vector2(scale, scale)}, spriteBatch, shapeBatch) { }

    public void Draw(GameTime gameTime)
    {
        //spriteBatch.Draw(texture, Transform.Position, null, Color.White, Transform.Rotation, origin, scale, flip, 0f);
        Sprite.Draw(spriteBatch, Transform.Position);
    }

    private Rectangle CalculateSelfBounds()
    {
        Vector2 topLeft = Vector2.Transform(new Vector2(0, 0), Transform.WorldMatrix);
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