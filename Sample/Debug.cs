using System.Collections.Generic;
using AtomicGames.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Sample
{
    public class Debug : IGameObject
    {
        private readonly SpriteFont font;
        private Dictionary<string, string> messages;

        public bool IsActive { get; private set; }
        public bool IsVisible { get; private set; }
        public Transform Transform { get; private set; }

        public Debug(SpriteFont font)
        {
            this.font = font;
            IsActive = true;
            IsVisible = true;
            Transform = new Transform();
            Transform.Position = new Vector2(20f, 20f);
            messages = new Dictionary<string, string>();
        }

        public void AddDebugMessage(string title, string message)
        {
            if (!messages.TryAdd(title, message))
                messages[title] = message;
        }

        public void ClearDebugMessages()
        {
            messages?.Clear();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int i = 0;
            foreach (var message in messages)
            {
                spriteBatch.DrawString(font, $"{message.Key}: {message.Value}", Transform.Position + new Vector2(0, i * 30), Color.LimeGreen);
                i++;
            }
        }

        public void Update(GameTime gameTime) { }

        public void Enable()
        {
            IsActive = true;
            IsVisible = true;
        }

        public void Disable()
        {
            IsActive = false;
            IsVisible = false;
        }
    }
}