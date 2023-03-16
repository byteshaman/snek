using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using snek.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace snek.Base {
  internal class Snake {

    private LinkedList<Cell> snakePartList = new();
    public Direction Direction { get; set; }
    public float Speed { get; set; }
    public Timer MovementTimer { get; set; }
    public Timer ShrinkTimer { get; set; }

    private Cell snakePart;
    Texture2D HeadTexture { get; set; }
    Texture2D BodyTexture { get; set; }
    

    public void Draw(SpriteBatch spriteBatch) {
      Texture2D snakePartTexture = HeadTexture;
      spriteBatch.Begin();
      foreach (Cell snakePart in snakePartList) {
        if (snakePart != GetHead()) {
          snakePartTexture = BodyTexture;
        }
        spriteBatch.Draw(snakePartTexture, snakePart.Coordinates, Color.White);
      }
      spriteBatch.End();
    }
    public void LoadContent(ContentManager Content) {
      HeadTexture = Content.Load<Texture2D>("Sprites/snakeHead");
      BodyTexture = Content.Load<Texture2D>("Sprites/snakeBody");
    }


    public Snake() {
      Speed = 1f;
      MovementTimer = new Timer(0.1f/Speed);
      ShrinkTimer = new Timer(Globals.SNAKE_SHRINK_TIMER);
      Direction = GetRndDirection();

      // Debug
      Console.WriteLine($"Initial snake direction: {Direction}");

      // Generate snake cells
      for (int i = 0; i < Globals.INITIAL_SNAKE_SIZE; i++) {
        IncreaseSize();
      }
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
      snakePart = new Cell(p.X, p.Y, CellType.SNAKE);
      snakePartList.AddLast(snakePart);
      Console.WriteLine($"IncreaseSize(), cell n.{GetSize()} added at {snakePart.Position}");
    }
    public void DecreaseSize() {
      snakePartList.Last.Value.Type = CellType.EMPTY;
      snakePartList.RemoveLast(); 
    }
    public int GetSize() { return snakePartList.Count; }


    // Direction
    public DirectionAxis GetDirectionAxis() {

      return Direction == Direction.Left || Direction == Direction.Right ? DirectionAxis.X : DirectionAxis.Y;
    }

    /// <summary>
    /// Set a random direction when game starts
    /// </summary>
    public Direction GetRndDirection() {
      Direction[] directions = Enum.GetValues(typeof(Direction))
                                   .Cast<Direction>()
                                   .Where(d => d != Direction.None)
                                   .ToArray();
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
      snakePart.Type = CellType.SNAKE;
      snakePartList.AddFirst(snakePart);
    }

    // Others
    public LinkedList<Cell> GetSnakePartList() { return snakePartList; }
    public Cell GetHead() { return snakePartList.First.Value; }
  }
}
