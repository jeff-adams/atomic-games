using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtomicGames.Graphics
{
    public class Display
    {
        private Game game;
        
        private float prefferedAspectRatio = 1f;
        private float currentAspectRatio = 1f;
        public Display(Game game)
        {
            this.game = game;
        }
        public void UpdateScreenSize(object sender, EventArgs e)
        {
            float currentAspectRatio = game.window.ClientBounds.Width / (float) game.window.ClientBounds.Height;
            windowScale = 1f / currentAspectRatio;
            renderRect = GetRenderTargetPositionAsRectangle(currentAspectRatio);
        }

        private Rectangle GetRenderTargetPositionAsRectangle(float currentAspectRatio)
        {
            if (currentAspectRatio <= prefferedAspectRatio)
            {
                // output is taller than it is wider, bars on top/bottom
                int presentHeight = (int)((game.window.ClientBounds.Width / prefferedAspectRatio) + 0.5f);
                int barHeight = (game.window.ClientBounds.Height - presentHeight) / 2;
                return new Rectangle(0, barHeight, game.window.ClientBounds.Width, presentHeight);
            }
            else
            {
                // output is wider than it is tall, bars left/right
                int presentWidth = (int)((game.window.ClientBounds.Height * prefferedAspectRatio) + 0.5f);
                int barWidth = (game.window.ClientBounds.Width - presentWidth) / 2;
                return new Rectangle(barWidth, 0, presentWidth, game.window.ClientBounds.Height);
            }
        }
    }
}