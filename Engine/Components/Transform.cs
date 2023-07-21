using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AtomicGames.Engine
{
    public class Transform
    {
        private Transform parentTransform;
        private HashSet<Transform> childrenTransforms;
        private Vector2 position;
        private float rotation;
        private float scale;
        
        public Vector2 Position 
        { 
            get => 
                parentTransform is null 
                ? position 
                : Vector2.Transform(position, parentTransform.WorldMatrix);
            set
            {
                position = value;
                UpdateMatrices();
            } 
        }

        public float Rotation 
        { 
            get => rotation;
            private set
            {
                rotation = value;
                UpdateMatrices();
            }
        }

        public float Scale
        {
            get => scale;
            private set
            {
                scale = value;
                UpdateMatrices();
            }
        }

        public Vector2 Direction { get; private set; }
        public Matrix LocalMatrix { get; private set; }
        public Matrix WorldMatrix { get; private set; }

        public Transform() : this(null) { }

        public Transform(Transform parentTransform)
        {
            if (parentTransform != null)
                this.parentTransform = parentTransform;
            
            Position = new Vector2(0, 0);
        }

        public Transform AddParentTransform(Transform parentTransform)
        {
            this.parentTransform = parentTransform
                .AddChildTransform(this);
            return this;
        }

        public Transform AddChildTransform(Transform childTransform)
        {
            if (childrenTransforms is null) 
                childrenTransforms = new HashSet<Transform>();

            childrenTransforms.Add(childTransform);
            return this;
        }

        public Transform RotateToDirection(Vector2 direction)
        {
            Rotation = (float)(Math.Atan2(direction.Y, direction.X));
            Direction = direction;
            return this;
        }

        public void UpdateMatrices()
        {
            LocalMatrix = UpdateLocalMatrix();
            WorldMatrix = UpdateWorldMatrix();

            if (childrenTransforms is null) return;
            
            foreach(Transform child in childrenTransforms)
            {
                child.UpdateMatrices();
            }
        }

        private Matrix UpdateWorldMatrix()
        {
            if (parentTransform is null) return LocalMatrix;

            return parentTransform.WorldMatrix * LocalMatrix;
        }

        private Matrix UpdateLocalMatrix() =>
            Matrix.CreateScale(scale) *
            Matrix.CreateRotationZ(rotation) *
            Matrix.CreateTranslation(new Vector3(position, 0));

        public override string ToString() =>
            $"Position: {Position}, Rotation: {Rotation}, Scale: {Scale}, HasParent: {parentTransform != null}";
    }
}