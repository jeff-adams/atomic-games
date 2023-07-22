using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine.Graphics;

public class Canvas : IDisposable
{
    public int VirtualWidth { get; }
    public int VirtualHeight { get; }
    public Rectangle RenderRectangle => renderRect;

    private readonly GraphicsDevice graphics;
    private readonly RenderTarget2D renderTarget;
    private Rectangle renderRect;
    
    public Canvas(GraphicsDevice graphics, int width, int height)
    {
        VirtualWidth = width;
        VirtualHeight = height;

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
        Rectangle screenSize = graphics.PresentationParameters.Bounds;
        
        float scaleX = (float)screenSize.Width / renderTarget.Width;
        float scaleY = (float)screenSize.Height / renderTarget.Height;
        float scale = Math.Min(scaleX, scaleY);

        int width = (int)(renderTarget.Width * scale);
        int height = (int)(renderTarget.Height * scale);

        int x = (screenSize.Width - width) / 2;
        int y = (screenSize.Height - height) / 2;

        renderRect = new Rectangle(x, y, width, height);
        return renderRect;
    }

    public override string ToString() =>
        $"RenderRect: {renderRect}, RenderTarget: {renderTarget.Bounds}";

    public void Dispose()
    {
        renderTarget?.Dispose();
    }
}