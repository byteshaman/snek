using Microsoft.Xna.Framework;
using System;

namespace snek {
  enum CellType {
    EMPTY,
    FOOD,
    POISON,
    SNAKE_NODE
  }

  enum Direction { 
    DOWN, 
    LEFT, 
    RIGHT, 
    UP
  } 
   
  enum DirectionAxis { 
    X,
    Y
  }

  public static class Globals {
    public const int CELL_SIZE = 40;
    public const int CELL_COUNT = 21;
    public const int WINDOW_SIZE = CELL_SIZE*CELL_COUNT;
    public const int INITIAL_SNAKE_SIZE = 3;
    public const int  INITIAL_SNAKE_SPEED = 1;

    public static int GetRandomCellIndex(bool considerSize) {
      int lowerBound = 0;
      int upperBound = Globals.CELL_COUNT; 

      // Limit the range of cells for when I create a new snake
      if (considerSize) {
        lowerBound = INITIAL_SNAKE_SIZE-1;
        upperBound -= INITIAL_SNAKE_SIZE;
        Console.WriteLine($"Considering the size of the snake, the bounds are: ({lowerBound}, {upperBound})");
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
    public static (int row, int col) GenerateCellArrayPosition(bool considerSize = false) {
      int row, col;
      row = Globals.GetRandomCellIndex(considerSize);
      col = Globals.GetRandomCellIndex(considerSize);
      return (row, col);
    }

    public static Vector2 GenerateCellCoordinates((int row, int col) tuple) {
      // Generate random coordinates
      int posX = tuple.row * Globals.CELL_SIZE;
      int posY = tuple.col * Globals.CELL_SIZE;
      // Debug
      Console.WriteLine($"Cell:[{tuple.row},{tuple.col}] - Position:({posX},{posY})");

      return new Vector2(posX, posY);
    }

  }
}
