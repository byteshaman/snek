using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

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



  public static class Globals {
    public const sbyte CELL_SIZE = 40;
    public const sbyte CELL_COUNT = 21;
    public const int WINDOW_SIZE = CELL_SIZE * CELL_COUNT;
    public const sbyte INITIAL_SNAKE_SIZE = 4;
    public const float MOUSE_RESPAWN_TIMER = 8f;
    public const float SNAKE_SHRINK_TIMER = MOUSE_RESPAWN_TIMER - 0.5f;
    public const float SCORE_TIMER = 1f;
    public const sbyte FOOD_BONUS = 25;
    public const sbyte FOOD_TIMER_MALUS = 15;

    // My Colors
    public readonly static Color BUTTON_HOVER_COLOR = new(0, 69, 0);
    public readonly static Color CELL_BORDER_COLOR = Color.DarkGray;
    public readonly static Color CELL_COLOR = Color.Black;
    public readonly static Color FONT_COLOR = new(110, 246, 5);
    public readonly static Color BUTTON_BORDER_COLOR = FONT_COLOR;
    public readonly static Color MENU_BG_COLOR = Color.Black;
    public readonly static Color BUTTON_COLOR = MENU_BG_COLOR;
    public readonly static Color GAME_BG_COLOR = Color.Black;


    public static readonly List<DifficultyLevel> DifficultyLevels = new() {
      { new DifficultyLevel(0.1f, 6.9f, 7f, 100) },
      { new DifficultyLevel(0.085f, 5.875f, 6f, 300) },
      { new DifficultyLevel(0.065f, 4.85f, 5f, 600) },
      { new DifficultyLevel(0.05f, 4.425f, 4.5f, 750) },
      { new DifficultyLevel(0.03f, 3.8f, 4f, 1000) },
    };


    public static int GetMaxMapCellValue() {
      return CELL_COUNT - 1;
    }

    public static int GetRndCellIndex(bool considerSize) {
      sbyte lowerBound = 0;
      sbyte upperBound = CELL_COUNT;

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
      return new Point(row, col);
    }
  }
}
