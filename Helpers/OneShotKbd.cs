using Microsoft.Xna.Framework.Input;

namespace snek.Helpers {
  class OneShotKbd {
    static KeyboardState currentKeyState;
    static KeyboardState previousKeyState;

    public static KeyboardState GetState() {
      previousKeyState = currentKeyState;
      currentKeyState = Keyboard.GetState();

      return currentKeyState;
    }

    /// <summary>
    /// Check that the currently pressed key is different from the previous one
    /// </summary>
    public static bool IsNewKeyPress(Keys key) {
      return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
    }
  }
}
