using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine
{
    public static class Extensions
    {
        public static Texture2D CreateTexture2D(this Texture2D texture, GraphicsDevice graphicsDevice, Rectangle spliceRect)
        {
            var splicedTexture = new Texture2D(graphicsDevice, spliceRect.Width, spliceRect.Height);
            int count = spliceRect.Width * spliceRect.Height;
            var data = new Color[count];
            texture.GetData(0, spliceRect, data, 0, count);
            splicedTexture.SetData(data);
            return splicedTexture;
        }
    }
}