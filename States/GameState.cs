using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using snek.Base;
using snek.Helpers;
using G = snek.Helpers.Globals;
using System;

namespace snek.States {
  public class GameState : State {
    #region Fields
    readonly Board board;
    readonly Food food;
    readonly Snake snake;
    KeyboardState keyboardState;

    Timer scoreTimer;
    #endregion

    #region Properties
    public int Score { get; set; }
    #endregion

    #region Methods
    public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager contentManager) : base(game, graphicsDevice, contentManager) {
      board = new Board();
      food = new Food(); // set Type = FOOD
      snake = new Snake();
      keyboardState = new KeyboardState();

      //Score
      scoreTimer = new(1);
      Score = 0;


      // Disable cursor
      game.IsMouseVisible = false;

      // Inizialize functions
      food.LoadContent(contentManager);
      snake.LoadContent(contentManager);
      GenerateFood();
    }

    // Called multiple times per second, draws content to the screen
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      game.GraphicsDevice.Clear(G.GAME_BG_COLOR);

      board.Draw(spriteBatch);
      food.Draw(spriteBatch);
      snake.Draw(spriteBatch);
    }

    // Called multiple times per second, updates the game state
    public override void Update(GameTime gameTime) {
      // Food management
      food.Timer.Update(gameTime);
      if (food.Timer.NeedsReset()) {
        GenerateFood();
        food.Timer.Reset();
        // Reduce score for not eating food in time
        //UpdateScore(G.FOOD_TIMER_MALUS);
      }

      // Snake movement and shrink management
      snake.MovementTimer.Update(gameTime);
      if (snake.MovementTimer.NeedsReset()) {
        snake.MovementTimer.Reset();
        MoveSnake();
      }

      snake.ShrinkTimer.Update(gameTime);
      if (snake.ShrinkTimer.NeedsReset()) {
        snake.DecreaseSize();
        snake.ShrinkTimer.Reset();
      }

      // Score management
      //scoreTimer.Update(gameTime);
      //if (scoreTimer.NeedsReset()) {
      //  scoreTimer.Reset();
      //  Score++;
      //}

      // KBD management
      keyboardState = OneShotKbd.GetState();
      if (this.keyboardState.GetPressedKeyCount() > 0) {
        HandleInput();
      }
    }

    public void GenerateFood() {
      Cell emptyCell = board.GetEmptyCell();

      board.EmptyCellAtPos(food.Position); //set current food position as empty
      food.Position = emptyCell.Position;
      food.Coordinates = emptyCell.Coordinates;
      emptyCell.Type = CellType.FOOD;//set new food position as food
    }

    private Cell GetNextCell(Cell snakeHead) {
      Point pos = snakeHead.Position;

      // Change position based on direction, respawn snake at proper position
      switch (snake.Direction) {
        case Direction.Left:
          pos.X--;
          if (pos.X < 0) {
            pos.X = G.GetMaxMapCellValue();
          }
          break;
        case Direction.Right:
          pos.X++;
          if (pos.X > G.GetMaxMapCellValue()) {
            pos.X = 0;
          }
          break;
        case Direction.Up:
          pos.Y--;
          if (pos.Y < 0) {
            pos.Y = G.GetMaxMapCellValue();
          }
          break;
        case Direction.Down:
          pos.Y++;
          if (pos.Y > G.GetMaxMapCellValue()) {
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

      // Debug
      //Console.WriteLine($"Current snake direction {snake.Direction}");

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


    private void HandleDifficulty() {
      foreach (DifficultyLevel level in G.DifficultyLevels) {
        if (Score >= level.scoreThreshold) {
          Console.WriteLine($"level.scoreThreshold {level.scoreThreshold}");

          snake.MovementTimer.TimerProp = level.snakeSpeed;
          snake.ShrinkTimer.TimerProp = level.snakeShrink;
          food.Timer.TimerProp = level.foodTimer;
        }
      }
    }

    private void MoveSnake() {
      Cell nextCell = GetNextCell(snake.GetHead());
      if (nextCell.Type == CellType.FOOD) {
        Console.WriteLine($"Collided with {nextCell.Type}, score: {Score}, size: {snake.GetSize()}");
        nextCell.Type = CellType.EMPTY;
        UpdateScore(G.FOOD_BONUS);

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

    private void UpdateScore (sbyte amount) {
      Score += amount;
      HandleDifficulty();
    }

    #endregion
  }
}
