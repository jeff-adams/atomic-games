using System.Collections.Generic;
using AtomicGames.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Sample
{
    public class Debugger : GameObject
    {
        private readonly SpriteFont font;
        private readonly int verticalSpacing;
        private Dictionary<string, string> messages;

        public Debugger(SpriteFont font) : base()
        {
            this.font = font;
            verticalSpacing = (int)System.Math.Ceiling((font.MeasureString("A")).Y * 1.2);
            IsActive = true;
            IsVisible = true;
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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int i = 0;
            foreach (var message in messages)
            {
                spriteBatch.DrawString(font, $"{message.Key}: {message.Value}", Transform.Position + new Vector2(0, i * verticalSpacing), Color.LimeGreen);
                i++;
            }
        }
    }
}