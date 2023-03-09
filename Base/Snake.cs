using Microsoft.Xna.Framework;
using MonoGame.Extended.Collections;
using snek.Helpers;
using System;
using System.Collections.Generic;

namespace snek.Base
{
    internal class Snake
    {

        private LinkedList<Cell> snakePartList = new();
        private float speed;
        private Direction direction;
        private Cell snakePart;

        public Snake()
        {
            SetRndDirection();

            // Generate snake cells
            for (int i = 0; i < Globals.INITIAL_SNAKE_SIZE; i++)
            {
                IncreaseSize();
            }

            speed = Globals.INITIAL_SNAKE_SPEED;
        }

        // Size
        public void IncreaseSize()
        {
            Console.WriteLine($"Inside IncreaseSize(), size: {GetSize()}");
            int row; int col;
            if (GetSize() == 0)
            {
                (row, col) = Globals.GenerateCellArrayPosition(true); // Generate a new cell in a more closed range
            }
            else
            {
                (row, col) = snakePartList.Last.Value.GetPosition();
                switch (direction)
                {
                    case Direction.Left:
                        col++;
                        break;
                    case Direction.Right:
                        col--;
                        break;
                    case Direction.Up:
                        row++;
                        break;
                    case Direction.Down:
                        row--;
                        break;
                }
            }
            snakePart = new Cell(row, col, CellType.SNAKE_NODE);
            // Debug
            Console.WriteLine($"Added snake {(GetSize() == 0 ? "head" : "part")} at: {snakePart.GetCoordinates()}");

            snakePartList.AddLast(snakePart);
        }

        public void DecreaseSize() { snakePartList.RemoveLast(); }
        public int GetSize() { return snakePartList.Count; }

        // Speed
        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }
        public float GetSpeed()
        {
            return speed;
        }

        // Direction
        public Direction GetDirection() { return direction; }
        public DirectionAxis GetDirectionAxis()
        {
            return direction == Direction.Left || direction == Direction.Right ? DirectionAxis.X : DirectionAxis.Y;
        }
        public void SetDirection(Direction direction)
        {
            this.direction = direction;
        }

        /// <summary>
        /// Set a random direction when game starts
        /// </summary>
        public void SetRndDirection()
        {
            Array directions = Enum.GetValues(typeof(Direction));
            Random random = new();
            int index = random.Next(directions.Length);
            Direction direction = (Direction)directions.GetValue(index);

            // Debug
            Console.WriteLine($"Random direction generated: {direction}");
            this.direction = direction;
        }

        // Movement
        public void Move(Cell nextCell)
        {
            // Remove tail
            Cell tail = snakePartList.Last.Value;
            snakePartList.RemoveLast();
            tail.SetType(CellType.EMPTY);

            // Move snakePart
            snakePart = nextCell;
            snakePart.SetType(CellType.SNAKE_NODE);
            snakePartList.AddFirst(snakePart);
        }

        // Others
        public LinkedList<Cell> GetSnakePartList() { return snakePartList; }
        public Cell GetHead()
        {
            return snakePartList.First.Value;
        }
    }
}
