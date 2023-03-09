using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using snek.Helpers;

namespace snek.Base {
  internal class GameBoard {
    private Cell[,] cells; //2D array
    private readonly int cellSize;
    private readonly int cellCount;

    public GameBoard() {
      cellCount = Globals.CELL_COUNT;
      cellSize = Globals.CELL_SIZE;

      cells = new Cell[cellCount, cellCount];
      for (int row = 0; row < cellCount; row++) {
        for (int col = 0; col < cellCount; col++) {
          cells[row, col] = new Cell(row, col); //internally sets posX, posY and cellType
          // Debug
          // Console.WriteLine($"Created cell: [{row},{col}] at coordinates: {cells[row, col].GetCoordinates()}");
        }
      }
    }

    public Cell[,] GetCells() { return cells; }

    public Vector2 GetNextMousePosition() {
      int row, col;
      // Generate a random position until an empty cell is found
      while (true) {
        (row, col) = Globals.GenerateCellArrayPosition();
        if (cells[row, col].GetType() == CellType.EMPTY)
          break;
      }
      cells[row, col].SetType(CellType.FOOD);
      return cells[row, col].GetCoordinates();
    }

    public void Draw(SpriteBatch spriteBatch) {
      Vector2 tilePosition = Vector2.Zero;

      spriteBatch.Begin();
      for (int row = 0; row < cellCount; row++) {
        for (int column = 0; column < cellCount; column++) {
          // Draw first rectangle
          spriteBatch.FillRectangle(tilePosition, new Size2(cellSize, cellSize), Color.DarkGray);
          // Draw second rectangle to give the border effect
          spriteBatch.FillRectangle(tilePosition + new Vector2(1, 1), new Size2(cellSize - 2, cellSize - 2), Color.Black);
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
