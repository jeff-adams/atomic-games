using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine
{
    public abstract class GameObject
    {

        public virtual GameObject Parent { get; }
        public virtual bool IsActive { get; private set; }
        public virtual Transform Transform { get; }
        
        private GameObject[] children;

        public GameObject(GameObject parent)
        {
            Parent = parent;
            Transform = new Transform(parent.Transform.ObjectMatrix);
        }

        public void AttachChildGameObjects(params GameObject[] childrenGameObjects)
        {
            children = childrenGameObjects;
        }

        public void Enable() => IsActive = true;
        public void Disable() => IsActive = false;
    }
}