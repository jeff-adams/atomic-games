using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Engine
{
    public abstract class GameObject : IGameObject
    {

        public GameObject Parent { get; }
        public bool IsActive { get; set; }
        public bool IsVisible { get; set; }
        public Transform Transform { get; }
        
        protected GameObject[] children;

        public GameObject() : this(new Transform()) 
        { }

        public GameObject(Transform transform)
        {
            Transform = transform;
        }

        public GameObject(GameObject parent)
        {
            Parent = parent;
            Transform = new Transform(parent.Transform.ObjectMatrix);
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }

        protected void AddChildGameObjects(params GameObject[] childrenGameObjects)
        {
            
            children = childrenGameObjects;
        }

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