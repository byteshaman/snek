using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using snek.Helpers;

namespace snek.Base {
  internal class Cell {
    // Position in the array
    private readonly int row;
    private readonly int col;

    // XY Coordinates
    private readonly int X;
    private readonly int Y;
    // Type (EMPTY, FOOD, POISON, SNAKE_NODE)
    private CellType cellType;

    public Cell(int row, int col, CellType cellType = CellType.EMPTY) {
      this.row = row;
      Y = row * Globals.CELL_SIZE;
      this.col = col;
      X = col * Globals.CELL_SIZE;
      // When the cell is created, its default type is empty
      if (cellType != CellType.EMPTY) {
        this.cellType = cellType;
      }
    }

    public Vector2 GetCoordinates() { return new Vector2(X, Y); }
    public (int, int) GetPosition() { return (row, col); }
    new public CellType GetType() { return cellType; }
    public void SetType(CellType cellType) { this.cellType = cellType; }
    public int GetRow() { return row; }
    public int GetCol() { return col; }
  }
}
