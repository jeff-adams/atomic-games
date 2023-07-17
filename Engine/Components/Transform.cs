using System;
using Microsoft.Xna.Framework;

namespace AtomicGames
{
    public class Transform
    {
        private Matrix? parentObjectMatrix;
        private Vector2 position;
        private float rotation;
        
        public Vector2 Position 
        { 
            get => 
                parentObjectMatrix == null ? 
                position : 
                Vector2.Transform(position, (Matrix)parentObjectMatrix);
            set
            {
                position = value;
                ObjectMatrix = UpdateObjectMatrix();
            } 
        }

        public float Rotation 
        { 
            get => rotation;
            private set
            {
                rotation = value;
                ObjectMatrix = UpdateObjectMatrix();
            }
        }

        public Vector2 Direction { get; private set; }
        public Matrix ObjectMatrix { get; private set; }

        public Transform(Matrix? parentObjectMatrix = null)
        {
            if (parentObjectMatrix != null)
                this.parentObjectMatrix = parentObjectMatrix;
        }

        public void RotateToDirection(Vector2 direction)
        {
            Rotation = (float)(Math.Atan2(direction.Y, direction.X));
            Direction = direction;
        }

        private Matrix UpdateObjectMatrix() =>
            Matrix.CreateRotationZ(rotation) *
            Matrix.CreateTranslation(new Vector3(position, 0f));
    }
}