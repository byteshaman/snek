using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using snek.Helpers;

namespace snek.Base {
  internal class Food : Cell {
    public Timer Timer { get; set; }
    Texture2D Texture { get; set; }

    public Food() {
      Timer = new Timer(Globals.INITIAL_MOUSE_TIMER);
    }

    public void Draw(SpriteBatch spriteBatch) {
      spriteBatch.Begin();
      spriteBatch.Draw(Texture, Coordinates, Color.White);
      spriteBatch.End();
    }
    public void LoadContent(ContentManager Content) {
      Texture = Content.Load<Texture2D>("mouse");
    }
  }
}
