using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Graphics
{
    public class ShapeBatch : IDisposable
    {
        private Game game;
        private BasicEffect effect;

        private VertexPositionColor[] vertices;
        private int vertexCount;
        private int[] indices;
        private int indexCount;
        private int shapeCount;

        private bool isStarted;

        public ShapeBatch(Game game)
        {
            this.game = game ?? throw new ArgumentNullException("game");
            effect = new BasicEffect(game.GraphicsDevice);
            effect.VertexColorEnabled = true;
            //Needed?
            effect.LightingEnabled = false;
            effect.TextureEnabled = false;
            effect.FogEnabled = false;
            effect.Projection = Matrix.Identity;
            effect.World = Matrix.Identity;
            effect.View = Matrix.Identity;

            const int maxVertexCount = 1024;
            vertices = new VertexPositionColor[maxVertexCount];
            indices = new int[maxVertexCount * 3];
            Initialize();
        }

        private void Initialize()
        {
            isStarted = false;
            vertexCount = 0;
            indexCount = 0;
            shapeCount = 0;
        }

        public void Begin()
        {
            if (isStarted)
            {
                throw new Exception("Batching is already been started.");
            }

            isStarted = true;
        }

        public void End()
        {
            EnsureBatchingIsStarted();
            Draw();
        }

        private void Draw()
        {
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, vertexCount, indices, 0, indexCount / 3);
            }
            Initialize();
        }

        private void EnsureBatchingIsStarted()
        {
            if (!isStarted)
            {
                throw new Exception("Batching has not been started.");
            }
        }

        private void EnsureShapeSpace(int shapeVertexCount, int shapeIndexCount)
        {
            if (shapeVertexCount > vertices.Length)
            {
                throw new ArgumentOutOfRangeException("shapeVertexCount", vertices.Length, "Shape vertex count is greater than the alloted shape space");
            }

            if (shapeIndexCount > indices.Length)
            {
                throw new ArgumentOutOfRangeException("shapeIndexCount", indices.Length, "Shape index count is greater than the alloted shape space");
            }

            if (vertexCount + shapeVertexCount > vertices.Length ||
                indexCount + shapeIndexCount > indices.Length)
            {
                //TODO: start a new batch???
            }
        }

        public void Rectangle(float x, float y, float width, float height, Color color)
        {
            EnsureBatchingIsStarted();

            const int rectangleVertexCount = 4;
            const int rectangleIndexCount = 6;

            EnsureShapeSpace(rectangleVertexCount, rectangleIndexCount);

            float left = x;
            float right = x + width;
            float top = y;
            float bottom = y + height;

            var a = new Vector3(left, top, 0f);
            var b = new Vector3(right, top, 0f);
            var c = new Vector3(right, bottom, 0f);
            var d = new Vector3(left, bottom, 0f);

            indices[indexCount++] = 0 + vertexCount;
            indices[indexCount++] = 1 + vertexCount;
            indices[indexCount++] = 2 + vertexCount;
            indices[indexCount++] = 0 + vertexCount;
            indices[indexCount++] = 2 + vertexCount;
            indices[indexCount++] = 3 + vertexCount;

            vertices[vertexCount++] = new VertexPositionColor(a, color);
            vertices[vertexCount++] = new VertexPositionColor(b, color);
            vertices[vertexCount++] = new VertexPositionColor(c, color);
            vertices[vertexCount++] = new VertexPositionColor(d, color);

            shapeCount++;
        }

        public void Dispose()
        {
            effect?.Dispose();
        }
    }
}