using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using snek.Helpers;
using System;

namespace snek.Base {
  internal class Board {
    private Cell[,] cells; //2D array
    private readonly int cellSize;
    private readonly int cellCount;

    public Board() {
      cellCount = Globals.CELL_COUNT;
      cellSize = Globals.CELL_SIZE;
      // 2D Cell array
      cells = new Cell[cellCount, cellCount];
      for (int row = 0; row < cellCount; row++) {
        for (int col = 0; col < cellCount; col++) {
          cells[row, col] = new Cell(row, col); //set Position, Coordinates and Type = EMPTY
          // Debug
          //Console.WriteLine($"Created cell: [{row},{col}] at coordinates: {cells[row, col].Coordinates}");
        }
      }
    }

    public Cell[,] GetCells() {
      return cells;
    }

    /// <summary>
    /// Get the cell at the specified position
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Cell GetCellAtPos(Point pos) {
      return cells[pos.X,pos.Y];
    }

    /// <summary>
    /// Mark the cell at the specified position as EMPTY
    /// </summary>
    /// <param name="pos"></param>
    public void EmptyCellAtPos(Point pos) {
      cells[pos.X, pos.Y].Type = CellType.EMPTY;
    }

    public Cell GetEmptyCell() {
      Point p;
      // Generate a random position until an empty cell is found
      while (true) {
        p = Globals.GenerateRndCellPosition();
        if (cells[p.X, p.Y].Type == CellType.EMPTY) {
          Console.WriteLine($"Empty cell found at [{p.X},{p.Y}]");
          return cells[p.X, p.Y];
        }
      }
    }

    public void Draw(SpriteBatch spriteBatch) {
      Vector2 tilePosition = Vector2.Zero;

      spriteBatch.Begin();
      for (int row = 0; row < cellCount; row++) {
        for (int column = 0; column < cellCount; column++) {
          // Draw first rectangle
          spriteBatch.FillRectangle(tilePosition, new Size2(cellSize, cellSize), Globals.CELL_BORDER_COLOR);
          // Draw second rectangle to give the border effect
          spriteBatch.FillRectangle(tilePosition + new Vector2(1, 1), new Size2(cellSize - 2, cellSize - 2), Globals.CELL_COLOR);
          // Draw column
          tilePosition.Y += cellSize;
        }
        // Reset column
        tilePosition.Y = 0;
        // Move to the next row
        tilePosition.X += cellSize;
      }
      spriteBatch.End();
    }
  }
}
