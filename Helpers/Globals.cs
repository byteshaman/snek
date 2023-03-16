using Microsoft.Xna.Framework;
using System;

namespace snek.Helpers {
  public enum CellType {
    EMPTY,
    FOOD,
    POISON,
    SNAKE
  }

  public enum Direction {
    Down,
    Left,
    Right,
    Up,
    None
  }

  public enum DirectionAxis {
    X,
    Y
  }

  public enum MenuType {
    GameStart,
    GamePause,
    GameOver
  }

  public enum Difficulty {
    One = 1,
    Two,
    Three,
    Four,
    Five
  }



  public static class Globals {
    public const int CELL_SIZE = 40;
    public const int CELL_COUNT = 21;
    public const int WINDOW_SIZE = CELL_SIZE * CELL_COUNT;
    public const int INITIAL_SNAKE_SIZE = 4;
    public const float MOUSE_RESPAWN_TIMER = 5f;
    public const float SNAKE_SHRINK_TIMER = MOUSE_RESPAWN_TIMER - 0.5f;
    public const float SCORE_TIMER = 1f;

    // My Colors
    public readonly static Color BUTTON_HOVER_COLOR = new(0, 69, 0);
    public readonly static Color CELL_BORDER_COLOR = Color.DarkGray;
    public readonly static Color CELL_COLOR = Color.Black;
    public readonly static Color FONT_COLOR = new(110, 246, 5);
    public readonly static Color BUTTON_BORDER_COLOR = FONT_COLOR;
    public readonly static Color MENU_BG_COLOR = Color.Black;
    public readonly static Color BUTTON_COLOR = MENU_BG_COLOR;
    public readonly static Color GAME_BG_COLOR = Color.Black;



    public static int GetMaxMapCellValue() {
      return CELL_COUNT - 1;
    }

    public static int GetRndCellIndex(bool considerSize) {
      int lowerBound = 0;
      int upperBound = CELL_COUNT;

      // Limit the range of cells for when I create a new snake
      if (considerSize) {
        lowerBound = INITIAL_SNAKE_SIZE - 1;
        upperBound -= INITIAL_SNAKE_SIZE;
        //Console.WriteLine($"Considering the size of the snake, the bounds are: ({lowerBound}, {upperBound})");
      } else {
        upperBound -= 1; //array is 0-indexed
      }

      Random random = new();
      return random.Next(lowerBound, upperBound);
    }

    /// <summary>
    /// Generate the cell position inside the 2D array [row, col]
    /// </summary>
    /// <returns>(int row, int col)</returns>
    public static Point GenerateRndCellPosition(bool considerSize = false) {
      int row = GetRndCellIndex(considerSize);
      int col = GetRndCellIndex(considerSize);
      return new Point(row,col);
    }
  }
}
