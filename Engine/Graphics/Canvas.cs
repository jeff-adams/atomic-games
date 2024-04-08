using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShapeTest2;

public class Canvas : IDisposable
{
    public int VirtualWidth { get; }
    public int VirtualHeight { get; }
    public Vector2 VirtualCenter { get; }
    public Matrix VirtualScaleMatrix { get; private set;}
    public Matrix ProjectionMatrix { get; }
    public Rectangle RenderRectangle => renderRect;

    private readonly GraphicsDevice graphics;
    private readonly RenderTarget2D renderTarget;
    private Rectangle renderRect;
    
    public Canvas(GraphicsDevice graphics, int width, int height)
    {
        VirtualWidth = width;
        VirtualHeight = height;
        ProjectionMatrix = Matrix.CreateOrthographicOffCenter(0f, width, height, 0f, 0f, 1f);
        VirtualCenter = new Vector2(width * 0.5f, height * 0.5f);
        VirtualScaleMatrix = Matrix.CreateScale(
            Math.Min((float)graphics.Viewport.Width / width, (float)graphics.Viewport.Height / height));

        this.graphics = graphics;

        renderTarget = new RenderTarget2D(this.graphics, width, height);
    }


    internal void Activate()
    {          
        graphics.SetRenderTarget(renderTarget);
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        graphics.SetRenderTarget(null);
        graphics.Clear(Color.Black);
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
        spriteBatch.Draw(renderTarget, renderRect, Color.White);
        spriteBatch.End();
    }

    internal Rectangle UpdateRenderRectangle()
    {
        Rectangle screenSize = graphics.Viewport.Bounds;
        
        float scaleX = (float)screenSize.Width / renderTarget.Width;
        float scaleY = (float)screenSize.Height / renderTarget.Height;
        float scale = Math.Min(scaleX, scaleY);

        int width = (int)(renderTarget.Width * scale);
        int height = (int)(renderTarget.Height * scale);

        int x = (screenSize.Width - width) / 2;
        int y = (screenSize.Height - height) / 2;

        VirtualScaleMatrix = Matrix.CreateScale(scale);
        renderRect = new Rectangle(x, y, width, height);
        return renderRect;
    }

    public Vector2 ConvertToWorldPosition(Point position) =>
        Vector2.Floor(Vector2.Transform((position - RenderRectangle.Location).ToVector2(), Matrix.Invert(VirtualScaleMatrix)));

    public override string ToString() =>
        $"RenderRect: {renderRect}, RenderTarget: {renderTarget.Bounds}";

    public void Dispose()
    {
        renderTarget?.Dispose();
    }
}