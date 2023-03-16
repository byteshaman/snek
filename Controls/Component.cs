using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace snek.Controls
{

    // abstract means that the class can't be instantiated but it's gonna only be used as a base class for classes that need to extend it
    public abstract class Component
    {
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
    }
}
