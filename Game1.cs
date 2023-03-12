using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using snek.Base;
using snek.Helpers;
using System;
using Food = snek.Base.Food;

namespace snek
{
    public class Game1 : Game {
    #region Attributes
    readonly GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    readonly Board board;
    Food food;
    Snake snake;
    Texture2D snakePartTexture;

    // Keyboard
    KeyboardState keyboardState;

    // GameBoard
    #endregion

    // Constructor, used to initialize the starting variables
    public Game1() {
      graphics = new GraphicsDeviceManager(this) {
        PreferredBackBufferWidth = Globals.WINDOW_SIZE,
        PreferredBackBufferHeight = Globals.WINDOW_SIZE
      };
      graphics.ApplyChanges();

      board = new Board();
      food = new Food(); // set Type = FOOD
      snake = new Snake();

      keyboardState = new KeyboardState();

      Content.RootDirectory = "Content";
      IsMouseVisible = true;
    }

    #region BaseFunctions
    // Called after the constructor, used to query any required services and load any non-graphic related content
    protected override void Initialize() {
      Console.WriteLine($"Width: {graphics.PreferredBackBufferWidth}, Height: {graphics.PreferredBackBufferHeight}");

      base.Initialize();

      GenerateFood();
    }

    // Called within the Initialize method, used to load game content 
    protected override void LoadContent() {
      spriteBatch = new SpriteBatch(GraphicsDevice);

      food.LoadContent(Content);
      snakePartTexture = Content.Load<Texture2D>("snakeHead");
    }

    // Called multiple times per second, draws content to the screen
    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(Color.Black);

      board.Draw(spriteBatch);
      food.Draw(spriteBatch);

      spriteBatch.Begin();
      foreach (Cell snakePart in snake.GetSnakePartList()) {
        spriteBatch.Draw(snakePartTexture, snakePart.Coordinates, Color.White);
      }
      spriteBatch.End();


      base.Draw(gameTime);
    }

    // Called multiple times per second, updates the game state
    protected override void Update(GameTime gameTime) { 
      food.Timer.Update(gameTime);
      if (food.Timer.NeedsReset()) {
        GenerateFood();
        food.Timer.Reset();
      }

      snake.Timer.Update(gameTime);
      if (snake.Timer.NeedsReset()) {
        snake.Timer.Reset();
        MoveSnake();
      }


      keyboardState = OneShotKbd.GetState();
      if (this.keyboardState.GetPressedKeyCount() > 0) {
        HandleInput();
      }

      base.Update(gameTime);
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
        Exit();
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
      } else if (nextCell.Type == CellType.SNAKE_NODE || nextCell.Type == CellType.POISON) {
        Console.WriteLine($"Collided with {nextCell.GetType()}, game over :(");
        // Set direction to none to stop moving?
      }
      snake.Move(nextCell);
    }

    #endregion
  }
}