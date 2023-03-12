using Microsoft.Xna.Framework;
using snek.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace snek.Base {
  internal class Snake {

    private LinkedList<Cell> snakePartList = new();
    public Direction Direction { get; set; }
    public float Speed { get; set; }
    public Timer Timer { get; set; }

    private Cell snakePart;

    public Snake() {
      Speed = 1f;
      Timer = new Timer(0.1f/Speed);
      Direction = GetRndDirection();

      // Debug
      Console.WriteLine($"Initial snake direction: {Direction}");

      // Generate snake cells
      for (int i = 0; i < Globals.INITIAL_SNAKE_SIZE; i++) {
        IncreaseSize();
      }
    }    
    
    public Snake(int row, int col) {
      Speed = 1f;
      Timer = new Timer(0.1f/Speed);
      Direction = Direction.Up;

      // Debug
      Console.WriteLine($"Initial snake direction: {Direction}");

      // Generate snake cells
      snakePartList.AddLast(new Cell(row, col, CellType.SNAKE_NODE));
      switch (Direction) {
        case Direction.Left:
          row++;
          break;
        case Direction.Right:
          row--;
          break;
        case Direction.Up:
          col++;
          break;
        case Direction.Down:
          col--;
          break;
      }
      snakePartList.AddLast(new Cell(row, col, CellType.SNAKE_NODE));
    }

    // Size
    public void IncreaseSize() {
      Point p;
      if (GetSize() == 0) {
        p = Globals.GenerateRndCellPosition(true); // Generate a new cell in a more closed range
      } else {
        p = snakePartList.Last.Value.Position;
        switch (Direction) {
          case Direction.Left:
            p.X++;
            break;
          case Direction.Right:
            p.X--;
            break;
          case Direction.Up:
            p.Y--;
            break;
          case Direction.Down:
            p.Y++;
            break;
        }
      }
      snakePart = new Cell(p.X, p.Y, CellType.SNAKE_NODE);
      snakePartList.AddLast(snakePart);
      Console.WriteLine($"IncreaseSize(), cell n.{GetSize()} added at {snakePart.Position}");
    }

    public void DecreaseSize() { snakePartList.RemoveLast(); }
    public int GetSize() { return snakePartList.Count; }


    // Direction
    public DirectionAxis GetDirectionAxis() {

      return Direction == Direction.Left || Direction == Direction.Right ? DirectionAxis.X : DirectionAxis.Y;
    }

    /// <summary>
    /// Set a random direction when game starts
    /// </summary>
    public Direction GetRndDirection() {
      Direction[] directions = Enum.GetValues(typeof(Direction)).Cast<Direction>()
                                    .Where(d => d != Direction.None)
                                    .ToArray(); //todo: filter out None
      Random random = new();
      int index = random.Next(directions.Length);
      return (Direction)directions.GetValue(index);
    }

    // Movement
    public void Move(Cell nextCell) {
      // Remove tail
      Cell tail = snakePartList.Last.Value;
      snakePartList.RemoveLast();
      tail.Type = CellType.EMPTY;

      // Move snakePart
      snakePart = nextCell;
      snakePart.Type = CellType.SNAKE_NODE;
      snakePartList.AddFirst(snakePart);
    }

    // Others
    public LinkedList<Cell> GetSnakePartList() { return snakePartList; }
    public Cell GetHead() {
      return snakePartList.First.Value;
    }
  }
}
