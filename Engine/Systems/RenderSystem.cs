using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using AtomicGames.Engine.Components;
using Microsoft.Xna.Framework.Graphics;
using AtomicGames.Engine.Graphics;

namespace AtomicGames.Engine.Systems;

public sealed class RenderSystem : ISystem<IRenderable>
{
    private SpriteBatch spriteBatch;
    private ShapeBatch shapeBatch;

    private HashSet<IRenderable> renderables;

    public void Initialize(AtomicGame game)
    {
        this.spriteBatch = game.SpriteBatch;
        this.shapeBatch = game.ShapeBatch;
    }

    public ISystem<IRenderable> Add(IRenderable renderable)
    {
        renderables.Add(renderable);
        return this;
    }

    public ISystem<IRenderable> Remove(IRenderable renderable)
    {
        renderables.Remove(renderable);
        return this;
    }

    public void Draw(GameTime gameTime)
    {
        foreach(IRenderable renderComponent in renderables.Where( r => r.Visible))
        {
            // TODO: do something with the spritebatch here
            renderComponent.Draw(gameTime);
        }
    }
}