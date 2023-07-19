using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine
{
    public abstract class GameObject : IGameObject
    {

        public GameObject Parent { get; private set; }
        public HashSet<GameObject> Children { get; private set; }
        public Rectangle Bounds { get; protected set; }
        public Transform Transform { get; }

        public bool IsActive { get; set; } = true;
        public bool IsVisible { get; set; } = true;

        public GameObject() : this(new Transform()) 
        { }

        public GameObject(Transform transform)
        {
            Transform = transform;
            Children = new HashSet<GameObject>();
            Bounds = new Rectangle((int)Transform.Position.X, (int)Transform.Position.Y, 0, 0);
        }

        protected virtual void SetBounds() { }

        public void AddParentObject(GameObject parentGameObject)
        {
            Parent = parentGameObject;
            Parent.AddChildObject(this);
            Transform.AddParentMatrix(parentGameObject.Transform.ObjectMatrix);
            SetBounds();
        }

        public void AddChildObject(GameObject childGameObject)
        {
            Children.Add(childGameObject);
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }

        public virtual void Enable()
        {
            IsActive = true;
            IsVisible = true;
        }

        public virtual void Disable()
        {
            IsActive = false;
            IsVisible = false;
        }

    }
}