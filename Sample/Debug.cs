using System.Collections.Generic;
using AtomicGames.Engine;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Sample
{
    public class Debug : IGameObject
    {
        private readonly SpriteFontBase font;
        private Dictionary<string, string> messages;

        public bool IsActive { get; private set; }
        public Transform Transform { get; private set; }

        public Debug(SpriteFontBase font)
        {
            this.font = font;
            IsActive = true;
            Transform = new Transform(1f);
            Transform.Position = new Vector2(20f, 20f);
            messages = new Dictionary<string, string>();
        }

        public void AddDebugMessage(string title, string message)
        {
            if (messages.ContainsKey(title))
            {
                messages[title] = message;
            }
            else
            {
                messages.Add(title, message);
            }
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

        public void Enable() => IsActive = true;
        public void Disable() => IsActive = false;
    }
}