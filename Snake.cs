using Microsoft.Xna.Framework;
using MonoGame.Extended.Collections;
using System;
using System.Collections.Generic;

namespace snek {
  internal class Snake {

    private LinkedList<Cell> snakePartList = new();
    private int speed;
    private Direction direction;
    private Cell snakePart;

    public Snake() {
      (int row, int col) = Globals.GenerateCellArrayPosition(true); // Generate a new cell in a more closed range
      this.SetRndDirection();

      // Set initial size
      for (int i = 0; i<Globals.INITIAL_SNAKE_SIZE; i++) {// Generate head position
        if (i == 0) {
          snakePart = new Cell(row, col, CellType.SNAKE_NODE);
        } else {
          switch (direction) {
            case Direction.LEFT: 
              col++;
              snakePart = new Cell(row, col, CellType.SNAKE_NODE);
              break;
           case Direction.RIGHT:
              col--;
              snakePart = new Cell(row, col, CellType.SNAKE_NODE);
              break;
           case Direction.UP:
              row++;
              snakePart = new Cell(row, col, CellType.SNAKE_NODE);
              break;
           case Direction.DOWN:
              row--;
              snakePart = new Cell(row, col, CellType.SNAKE_NODE);
              break;
          }
        }
        Console.WriteLine($"Snake {(i == 0 ? "head" : "part")} created: {snakePart.GetCoordinates()}");

        this.IncreaseSize();
      }

      this.speed = Globals.INITIAL_SNAKE_SPEED;
    }

    // Position
    public Vector2 GetHeadPosition() {
      return snakePartList.First.Value.GetCoordinates();
    }
    public void SetHeadPosition() {
      
    }

    // Size
    public void IncreaseSize() { snakePartList.AddLast(snakePart); }
    public void DecreaseSize() { snakePartList.RemoveLast(); }
    public int GetSize() { return snakePartList.Count; }

    // Speed
    public void SetSpeed(int speed) {
      this.speed = speed;
    }
    public int GetSpeed() {
      return speed;
    }

    // Direction
    public Direction GetDirection() { return direction; }
    public DirectionAxis GetDirectionAxis() { 
      return direction == Direction.LEFT || direction == Direction.RIGHT ? DirectionAxis.X : DirectionAxis.Y; 
    }
    public void SetDirection(Direction direction) { 
      this.direction = direction; 
    }

    public void SetRndDirection() {
      Array directions = Enum.GetValues(typeof(Direction));
      Random random = new();
      int index = random.Next(directions.Length);
      Direction direction = (Direction)directions.GetValue(index);


      Console.WriteLine($"Random direction generated: {direction}");
      this.direction = direction;
    }

    // Movement
    public void Move(Cell nextCell) {
      // Remove tail
      Cell tail = snakePartList.Last.Value;
      snakePartList.RemoveLast();
      tail.SetType(CellType.EMPTY);

      // Move snakePart
      snakePart = nextCell;
      snakePart.SetType(CellType.SNAKE_NODE);
      snakePartList.AddFirst(snakePart);
    }

    public LinkedList<Cell> GetSnakePartList() { return snakePartList; }
    public void SetSnakePartList(LinkedList<Cell> snakePartList) {
      this.snakePartList = snakePartList;
    }
  }
}
