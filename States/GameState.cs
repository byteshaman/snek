using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using snek.Base;
using snek.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace snek.States {
  public class GameState : State {
    #region Fields
    readonly Board board;
    readonly Food food;
    readonly Snake snake;
    KeyboardState keyboardState;

    #endregion

    public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager contentManager) : base(game, graphicsDevice, contentManager) {
      //this.graphicsDevice = graphicsDevice;
      //this.contentManager = contentManager;

      board = new Board();
      food = new Food(); // set Type = FOOD
      snake = new Snake();
      keyboardState = new KeyboardState();

      GenerateFood();

      food.LoadContent(contentManager);
      snake.LoadContent(contentManager);
    }

    #region BaseFunctions
    // Called multiple times per second, draws content to the screen
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      game.GraphicsDevice.Clear(Globals.GAME_BG_COLOR);

      board.Draw(spriteBatch);
      food.Draw(spriteBatch);
      snake.Draw(spriteBatch);
    }

    // Called multiple times per second, updates the game state
    public override void Update(GameTime gameTime) {
      food.Timer.Update(gameTime);
      if (food.Timer.NeedsReset()) {
        GenerateFood();
        food.Timer.Reset();
      }

      snake.ShrinkTimer.Update(gameTime);
      if (snake.ShrinkTimer.NeedsReset()) {
        snake.ShrinkTimer.Reset();
        snake.DecreaseSize();
      }

      snake.MovementTimer.Update(gameTime);
      if (snake.MovementTimer.NeedsReset()) {
        snake.MovementTimer.Reset();
        MoveSnake();
      }


      keyboardState = OneShotKbd.GetState();
      if (this.keyboardState.GetPressedKeyCount() > 0) {
        HandleInput();
      }
    }
    #endregion

    #region MyFunctions
    public void GenerateFood() {
      Cell emptyCell = board.GetEmptyCell();

      board.EmptyCellAtPos(food.Position);
      food.Position = emptyCell.Position;
      food.Coordinates = emptyCell.Coordinates;
      emptyCell.Type = CellType.FOOD;
    }

    private Cell GetNextCell(Cell snakeHead) {
      Point pos = snakeHead.Position;

      // Change position based on direction, respawn snake at proper position
      switch (snake.Direction) {
        case Direction.Left:
          pos.X--;
          if (pos.X < 0) {
            pos.X = Globals.GetMaxMapCellValue();
          }
          break;
        case Direction.Right:
          pos.X++;
          if (pos.X > Globals.GetMaxMapCellValue()) {
            pos.X = 0;
          }
          break;
        case Direction.Up:
          pos.Y--;
          if (pos.Y < 0) {
            pos.Y = Globals.GetMaxMapCellValue();
          }
          break;
        case Direction.Down:
          pos.Y++;
          if (pos.Y > Globals.GetMaxMapCellValue()) {
            pos.Y = 0;
          }
          break;
      }

      // Access the next cell
      Cell nextCell = board.GetCellAtPos(pos);
      return nextCell;
    }

    private void HandleInput() {
      // Quit game
      if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
        game.Exit();
      }

      Console.WriteLine($"Current snake direction {snake.Direction}");

      if (snake.GetDirectionAxis() == DirectionAxis.X) {
        // Up
        if (keyboardState.IsKeyDown(Keys.Up) && OneShotKbd.IsNewKeyPress(Keys.Up)) {
          snake.Direction = Direction.Up;
        }
        // Down
        if (keyboardState.IsKeyDown(Keys.Down) && OneShotKbd.IsNewKeyPress(Keys.Down)) {
          snake.Direction = Direction.Down;
        }
      } else {
        // Right
        if (keyboardState.IsKeyDown(Keys.Right) && OneShotKbd.IsNewKeyPress(Keys.Right)) {
          snake.Direction = Direction.Right;
        }
        // Left
        if (keyboardState.IsKeyDown(Keys.Left) && OneShotKbd.IsNewKeyPress(Keys.Left)) {
          snake.Direction = Direction.Left;
        }
      }
    }

    private void MoveSnake() {
      Cell nextCell = GetNextCell(snake.GetHead());
      if (nextCell.Type == CellType.FOOD) {
        Console.WriteLine($"Collided with {nextCell.Type}, increase score, size and re-generate food");
        nextCell.Type = CellType.EMPTY;
        food.Timer.Reset();
        GenerateFood();
        snake.IncreaseSize();
      } else if (nextCell.Type == CellType.SNAKE || nextCell.Type == CellType.POISON) {
        snake.Direction = Direction.None;
        Console.WriteLine($"Collided with {nextCell.GetType()}, game over :(");
        // Set direction to none to stop moving?
      }
      snake.Move(nextCell);
    }

    #endregion
  }
}
