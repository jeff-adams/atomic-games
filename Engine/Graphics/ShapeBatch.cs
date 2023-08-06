using System;
using AtomicGames.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine.Graphics;

public class ShapeBatch : IDisposable
{
    private readonly GraphicsDevice graphics;
    private readonly Camera camera;
    private readonly BasicEffect effect;

    private readonly VertexPositionColor[] vertices;
    private readonly short[] indices;

    private short vertexCount;
    private short indexCount;
    private int shapeCount;

    private bool isStarted;

    public ShapeBatch(GraphicsDevice graphics, Camera camera)
    {
        this.graphics = graphics ?? throw new ArgumentNullException(nameof(graphics));
        this.camera = camera ?? throw new ArgumentException(nameof(camera));
        // Vector3 origin = new Vector3(graphics.Viewport.Width * 0.5f, graphics.Viewport.Height * 0.5f, 0);

        effect = new BasicEffect(graphics)
        {
            VertexColorEnabled = true,
            // View = camera.ViewMatrix,
            Projection = Matrix.CreateOrthographicOffCenter(0f, graphics.Viewport.Width, graphics.Viewport.Height, 0f, 0f, 1f)
        };

        const int maxIndexCount = short.MaxValue;
        vertices = new VertexPositionColor[maxIndexCount / 3];
        indices = new short[maxIndexCount];

        Initialize();
    }

    private void Initialize()
    {
        vertexCount = 0;
        indexCount = 0;
        shapeCount = 0;
    }

    public void Begin()
    {
        if (isStarted)
            throw new Exception("Batching is already been started.");

        isStarted = true;
    }

    public void End()
    {
        EnsureBatchingIsStarted();
        Flush();
        Initialize();
        isStarted = false;
    }

    private void Flush()
    {
        EnsureBatchingIsStarted();

        if (shapeCount == 0) return;

        foreach (var pass in effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            graphics.DrawUserIndexedPrimitives(
                PrimitiveType.TriangleList,
                vertices,
                0,
                vertexCount,
                indices,
                0,
                indexCount / 3);
        }
    }

    private void EnsureBatchingIsStarted()
    {
        if (!isStarted)
            throw new Exception("Batching has not been started.");
    }

    private void EnsureShapeSpace(int shapeVertexCount, int shapeIndexCount)
    {
        if (shapeVertexCount > vertices.Length)
            throw new ArgumentOutOfRangeException(nameof(shapeVertexCount), vertices.Length, "Shape vertex count is greater than the alloted shape space");

        if (shapeIndexCount > indices.Length)
            throw new ArgumentOutOfRangeException(nameof(shapeIndexCount), indices.Length, "Shape index count is greater than the alloted shape space");

        if (vertexCount + shapeVertexCount > vertices.Length ||
            indexCount + shapeIndexCount > indices.Length)
        {
            Flush();
            Initialize();
        }
    }

    public void RectangleFill(Rectangle rect, Color color) =>
        RectangleFill(
            rect.X, rect.Y,
            rect.Width, rect.Height,
            color);

    public void RectangleFill(float x, float y, float width, float height, Color color)
    {
        EnsureBatchingIsStarted();

        const int rectangleVertexCount = 4;
        const int rectangleIndexCount = 6;

        EnsureShapeSpace(rectangleVertexCount, rectangleIndexCount);

        float left = x;
        float right = x + width;
        float top = y;
        float bottom = y + height;

        var topLeft = new Vector3(left, top, 0f);
        var topRight = new Vector3(right, top, 0f);
        var bottomRight = new Vector3(right, bottom, 0f);
        var bottomLeft = new Vector3(left, bottom, 0f);

        indices[indexCount++] = (short)(0 + vertexCount);
        indices[indexCount++] = (short)(1 + vertexCount);
        indices[indexCount++] = (short)(2 + vertexCount);
        indices[indexCount++] = (short)(0 + vertexCount);
        indices[indexCount++] = (short)(2 + vertexCount);
        indices[indexCount++] = (short)(3 + vertexCount);

        vertices[vertexCount++] = new VertexPositionColor(topLeft, color);
        vertices[vertexCount++] = new VertexPositionColor(bottomLeft, color);
        vertices[vertexCount++] = new VertexPositionColor(bottomRight, color);
        vertices[vertexCount++] = new VertexPositionColor(topRight, color);

        shapeCount++;
    }

    public void Rectangle(Rectangle rect, float thickness, Color color) =>
        Rectangle(
            rect.Left, rect.Top,
            rect.Left, rect.Bottom,
            rect.Right, rect.Bottom,
            rect.Right, rect.Top,
            thickness, color);

    public void Rectangle(
        float ax, float ay,
        float bx, float by,
        float cx, float cy,
        float dx, float dy,
        float thickness,
        Color color)
    {
        Line(ax, ay, bx, by, thickness, color);
        Line(bx, by, cx, cy, thickness, color);
        Line(cx, cy, dx, dy, thickness, color);
        Line(dx, dy, ax, ay, thickness, color);
    }

    public void Line(Vector2 a, Vector2 b, float thickness, Color color) =>
        Line(a.X, a.Y, b.X, b.Y, thickness, color);

    public void Line(float ax, float ay, float bx, float by, float thickness, Color color)
    {
        EnsureBatchingIsStarted();

        const int lineIndexCount = 4;
        const int lineVertexCount = 6;

        EnsureShapeSpace(lineIndexCount, lineVertexCount);

        float minThickness = 1f;
        float maxThickness = 10f;
        thickness = MathHelper.Clamp(thickness, minThickness, maxThickness);
        float lineCenter = thickness * 0.5f;

        //Calculate the edges with half thickness
        float e1x = bx - ax;
        float e1y = by - ay;
        (e1x, e1y) = Normalize(e1x, e1y);
        e1x *= lineCenter;
        e1y *= lineCenter;
        float e2x = -e1x;
        float e2y = -e1y;

        float n1x = -e1y;
        float n1y = e1x;
        float n2x = -n1x;
        float n2y = -n1y;

        //Calculate corners of the "line"
        Vector3 topLeft = new(ax + n1x + e2x, ay + n1y + e2y, 0f);
        Vector3 topRight = new(bx + n1x + e1x, by + n1y + e1y, 0f);
        Vector3 bottomLeft = new(ax + n2x + e2x, ay + n2y + e2y, 0f);
        Vector3 bottomRight = new(bx + n2x + e1x, by + n2y + e1y, 0f);

        indices[indexCount++] = (short)(0 + vertexCount);
        indices[indexCount++] = (short)(1 + vertexCount);
        indices[indexCount++] = (short)(2 + vertexCount);
        indices[indexCount++] = (short)(0 + vertexCount);
        indices[indexCount++] = (short)(2 + vertexCount);
        indices[indexCount++] = (short)(3 + vertexCount);

        vertices[vertexCount++] = new VertexPositionColor(topLeft, color);
        vertices[vertexCount++] = new VertexPositionColor(bottomLeft, color);
        vertices[vertexCount++] = new VertexPositionColor(bottomRight, color);
        vertices[vertexCount++] = new VertexPositionColor(topRight, color);

        shapeCount++;
    }

    private (float x, float y) Normalize(float x, float y)
    {
        float inverseLength = 1f / (float)Math.Sqrt(x * x + y * y);

        x *= inverseLength;
        y *= inverseLength;

        return (x, y);
    }

    public void Dispose() => effect?.Dispose();
}