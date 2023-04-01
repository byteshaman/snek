using Microsoft.Xna.Framework;

namespace snek.Helpers {
  public class Timer {
    float timeElapsed;
    public float TimerProp;

    public Timer(float timer) {
      TimerProp = timer;
    }

    public bool NeedsReset() { return timeElapsed >= TimerProp; }

    public void Reset() {
      timeElapsed -= TimerProp;
    }

    // Called inside game's update using game's gametime
    public void Update(GameTime gameTime) {
      timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
  }
}
