using Microsoft.Xna.Framework;
using snek.Helpers;
using G = snek.Helpers.Globals;

namespace snek.Base {
  public class Cell {
    // Position in the array
    public Point Position { get; set; }

    // XY Coordinates
    public Vector2 Coordinates { get; set; }

    // Type (EMPTY, FOOD, POISON, SNAKE_NODE)
    public CellType Type {get; set; }

    public void SetPosAndCoord (Point p) {
      Position = p;
      Coordinates = new Vector2(p.X, p.Y) * G.CELL_SIZE;
    }

    public Cell() {
      Coordinates = Vector2.Zero;
      Type = CellType.EMPTY;
      Position = Point.Zero;
    }

    public Cell(int row, int col, CellType type = CellType.EMPTY) {
      Position = new Point(row, col);
      Coordinates = new Vector2(row, col)*G.CELL_SIZE;
      // Set specific type when provided
      if (type != CellType.EMPTY) {
        Type = type;
      }
    }
  }
}
