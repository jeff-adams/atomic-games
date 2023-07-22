using AtomicGames.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine.Components;

public class SpriteObject : GameObject
{
    private Texture2D texture;
    private Vector2 origin;
    private SpriteEffects flip;
    private float scale;

    public SpriteObject(Texture2D texture, float scale = 1f) 
        : this(texture, new Vector2(texture.Width / 2, texture.Height / 2), scale) { }

    public SpriteObject(Texture2D texture, Vector2 origin, float scale = 1f) : base()
    {
        this.texture = texture;
        this.scale = scale;
        this.flip = SpriteEffects.None;
        this.origin = origin;
        IsActive = true;
        IsVisible = true;
        SetBounds();
    }

    public override void DrawContent(GameTime gameTime, SpriteBatch spriteBatch, ShapeBatch shapeBatch)
    {
        spriteBatch.Draw(texture, Transform.Position, null, Color.White, Transform.Rotation, origin, scale, flip, 0f);
        base.DrawContent(gameTime, spriteBatch, shapeBatch);
    }
    
    protected override void SetBounds()
    {
        int x = (int)(Transform.Position.X + origin.X);
        int y = (int)(Transform.Position.Y + origin.Y);
        Bounds = new Rectangle(x, y, texture.Width, texture.Height);
    }

    public override string ToString() =>
        $"{base.ToString()}, Texture: {texture.Name}";
}