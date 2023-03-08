using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;

namespace snek {
  internal class Cell {
    // Position in the array
    private readonly int row;
    private readonly int col;
    // XY Coordinates
    private readonly int posX;
    private readonly int posY;
    // Type (EMPTY, FOOD, POISON, SNAKE_NODE)
    private CellType cellType;

    public Cell(int row, int col, CellType cellType = CellType.EMPTY) {
      this.row = row;
      posY = row * Globals.CELL_SIZE;
      this.col = col;
      posX = col * Globals.CELL_SIZE;
      // When the cell is created, its default type is empty
      if (cellType != CellType.EMPTY) {
        this.cellType = cellType;
      }
    }

    public Vector2 GetCoordinates() { return new Vector2(posX, posY); }
    new public CellType GetType() { return cellType; }
    public void  SetType(CellType cellType) { this.cellType = cellType; }
    public int GetRow() { return row; }
    public int GetCol() { return col; }
  }
}
