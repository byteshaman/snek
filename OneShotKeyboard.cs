using Microsoft.Xna.Framework.Input;

namespace snek {
  class OneShotKeyboard {
    static KeyboardState currentKeyState;
    static KeyboardState previousKeyState;
    static Keys lastKeyPressed;

    public static KeyboardState GetState() {
      previousKeyState = currentKeyState;
      currentKeyState = Keyboard.GetState();

      return currentKeyState;
    }

    public static void SetLastKeyPressed(Keys k) {
      lastKeyPressed = k;
    }   
    
    public static Keys GetLastKeyPressed() {
      return lastKeyPressed;
    }

    /// <summary>
    /// Check that the currently pressed key is different from the previous one
    /// </summary>
    public static bool NewKeyPressed(Keys key) {
      return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
    }
  }
}
