using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snek.States {
  public abstract class State {

    protected ContentManager contentManager;
    protected GraphicsDevice graphicsDevice;
    protected Game1 game;

    public State(Game1 game, GraphicsDevice graphicsDevice, ContentManager contentManager) {
      this.game = game;
      this.graphicsDevice = graphicsDevice;
      this.contentManager = contentManager;
    }

    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

    public abstract void Update(GameTime gameTime);  
  }
}
