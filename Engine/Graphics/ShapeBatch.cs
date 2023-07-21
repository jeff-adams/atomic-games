using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine.Graphics
{
    public class ShapeBatch : IDisposable
    {
        private GraphicsDevice graphics;
        private Camera camera;
        private BasicEffect effect;

        private VertexPositionColor[] vertices;
        private short vertexCount;
        private short[] indices;
        private short indexCount;
        private int shapeCount;

        private bool isStarted;

        public ShapeBatch(GraphicsDevice graphics, Camera camera)
        {
            this.graphics = graphics ?? throw new ArgumentNullException(nameof(graphics));
            this.camera = camera ?? throw new ArgumentException(nameof(camera));
            effect = new BasicEffect(graphics);
            effect.VertexColorEnabled = true;
            //effect.View = camera.TransformMatrix;
            //effect.Projection = camera.ProjectionMatrix;
            effect.World = camera.TransformMatrix;

            const int maxIndexCount = short.MaxValue;
            vertices = new VertexPositionColor[maxIndexCount / 3];
            indices = new short[maxIndexCount];
            
            Initialize();
        }

        private void Initialize()
        {
            isStarted = false;
            vertexCount = 0;
            indexCount = 0;
            shapeCount = 0;
        }

        public void Begin(Vector2 pos)
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
        }

        private void Flush()
        {
            EnsureBatchingIsStarted();

            if (shapeCount == 0) return;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawUserIndexedPrimitives<VertexPositionColor>(
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
            float bottom = y - height;

            var a = new Vector3(left, top, 0f);
            var b = new Vector3(right, top, 0f);
            var c = new Vector3(right, bottom, 0f);
            var d = new Vector3(left, bottom, 0f);

            indices[indexCount++] = (short) (0 + vertexCount);
            indices[indexCount++] = (short) (1 + vertexCount);
            indices[indexCount++] = (short) (2 + vertexCount);
            indices[indexCount++] = (short) (0 + vertexCount);
            indices[indexCount++] = (short) (2 + vertexCount);
            indices[indexCount++] = (short) (3 + vertexCount);

            vertices[vertexCount++] = new VertexPositionColor(a, color);
            vertices[vertexCount++] = new VertexPositionColor(b, color);
            vertices[vertexCount++] = new VertexPositionColor(c, color);
            vertices[vertexCount++] = new VertexPositionColor(d, color);

            shapeCount++;
        }

        public void Rectangle(Rectangle rect, float thickness, Color color) =>
            Rectangle(
                rect.X, rect.Y, 
                rect.Right, rect.Top, 
                rect.Right, rect.Bottom, 
                rect.Left, rect.Bottom,
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
            float lineCenter = thickness / 2f;

            float e1x = bx - ax;
            float e1y = by - ay;
            (e1x, e1y) = Normalize(e1x, e1y);
            e1x *= lineCenter;
            e1y *= lineCenter;

            float e2x = -e1x;
            float e2y = -e1y;
            float n1x = -e1y;
            float n1y = -e1x;
            float n2x = -n1x;
            float n2y = -n1y;

            float q1x = ax + n1x + e2x;
            float q1y = ay + n1y + e2y;
            float q2x = bx + n1x + e1x;
            float q2y = by + n1y + e1y;
            float q3x = ax + n2x + e1x;
            float q3y = ay + n2y + e1y;
            float q4x = bx + n2x + e2x;
            float q4y = by + n2y + e2y;

            indices[indexCount++] = (short) (0 + vertexCount);
            indices[indexCount++] = (short) (1 + vertexCount);
            indices[indexCount++] = (short) (2 + vertexCount);
            indices[indexCount++] = (short) (0 + vertexCount);
            indices[indexCount++] = (short) (2 + vertexCount);
            indices[indexCount++] = (short) (3 + vertexCount);

            vertices[vertexCount++] = new VertexPositionColor(new Vector3(q1x, q1y, 0), color);
            vertices[vertexCount++] = new VertexPositionColor(new Vector3(q2x, q2y, 0), color);
            vertices[vertexCount++] = new VertexPositionColor(new Vector3(q3x, q3y, 0), color);
            vertices[vertexCount++] = new VertexPositionColor(new Vector3(q4x, q4y, 0), color);

            shapeCount++;
        }

        private (float x, float y) Normalize(float x, float y)
        {
            float inverseLength = 1f / (float)Math.Sqrt(x * x + y * y);

            x *= inverseLength;
            y += inverseLength;

            return (x, y);
        }

        public void Dispose()
        {
            effect?.Dispose();
        }
    }
}