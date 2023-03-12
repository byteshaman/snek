using Microsoft.Xna.Framework;

namespace snek.Helpers {
  internal class Timer {
    float timeElapsed;
    float timer;
    float timerLowerBound;


    public Timer(float timer, float timerLowerBound = 1) {
      this.timer = timer;
      this.timerLowerBound = timerLowerBound;
    }

    public bool CanBeDecreased() {
      return timer > timerLowerBound;
    }

    public void DecreaseTimerBy(float decreaseUnit) {
      // I might be at 1.2 and decrease by 0.3 and the timerLowerBound is 1: the operation is possible since CanBeDecreased() will return true: reset timer to timerLowerBound
      if (timer - decreaseUnit < timerLowerBound) {
        timer = timerLowerBound;
      } else {
        timer -= decreaseUnit;
      }
    }

    public bool NeedsReset() {
      return timeElapsed >= timer;
    }

    public void Reset() {
      timeElapsed -= timer; //would timeElapsed = 0 work as well?
    }

    // Called inside game's update using game's gametime
    public void Update(GameTime gameTime) {
      timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
  }
}
