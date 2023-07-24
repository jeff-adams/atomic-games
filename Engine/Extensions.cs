using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine;

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

    public static void InputMappingToAction<T>(this Dictionary<T, Action> mappings, Predicate<T> isInputPressed)
    {
        foreach (var mapping in mappings)
        {
            if (isInputPressed(mapping.Key))
            {
                mapping.Value();
            }
        }
    }

    public static Vector2 ToVector2(this Point point) =>
        new Vector2(point.X, point.Y);
        
    public static Vector2 PositionToVector2(this Rectangle rect) =>
        new Vector2(rect.X, rect.Y);
}

public static class AtomicMath
{
    public static float Min(params float[] values) =>
        values.Min();

    public static float Max(params float[] values) =>
        values.Max();
}