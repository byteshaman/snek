using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using snek.Base;
using snek.Helpers;
using System;



namespace snek
{
    public class Game1 : Game {
    #region Attributes
    readonly GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    // Mouse
    Texture2D mouseTexture;
    Vector2 mousePosition;

    // Snake
    Snake snake;
    Texture2D snakePartTexture;
    readonly float snakeSpeed;
    float snakeTimer;

    // Keyboard
    KeyboardState keyboardState;

    // GameBoard
    readonly GameBoard board;

    // Timer
    Timer mouseRespawnTimer;
    #endregion

    // Constructor, used to initialize the starting variables
    public Game1() {
      graphics = new GraphicsDeviceManager(this) {
        PreferredBackBufferWidth = Globals.WINDOW_SIZE,
        PreferredBackBufferHeight = Globals.WINDOW_SIZE
      };
      graphics.ApplyChanges();

      board = new GameBoard();
      snake = new Snake();

      keyboardState = new KeyboardState();

      snakeTimer = 0f;
      snakeSpeed = 0.1f;

      // Timers
      mouseRespawnTimer = new Timer(Globals.INITIAL_MOUSE_TIMER);

      Content.RootDirectory = "Content";
      IsMouseVisible = true;
    }

    #region BaseFunctions
    // Called after the constructor, used to query any required services and load any non-graphic related content
    protected override void Initialize() {
      Console.WriteLine($"Width: {graphics.PreferredBackBufferWidth}, Height: {graphics.PreferredBackBufferHeight}");

      base.Initialize();

      mousePosition = board.GetNextMousePosition();
    }

    // Called within the Initialize method, used to load game content 
    protected override void LoadContent() {
      spriteBatch = new SpriteBatch(GraphicsDevice);

      mouseTexture = Content.Load<Texture2D>("mouse");
      snakePartTexture = Content.Load<Texture2D>("snakeHead");
    }

    // Called multiple times per second, draws content to the screen
    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(Color.Black);

      board.Draw(spriteBatch);

      spriteBatch.Begin();
      spriteBatch.Draw(mouseTexture, mousePosition, Color.White);
      foreach (Cell snakePart in snake.GetSnakePartList()) {
        spriteBatch.Draw(snakePartTexture, snakePart.GetCoordinates(), Color.White);
      }
      spriteBatch.End();


      base.Draw(gameTime);
    }

    // Called multiple times per second, updates the game state
    protected override void Update(GameTime gameTime) {
      

      //// Detect collision
      //if (snake.GetHeadCoordinates() == mousePosition) {
      //  // Reset timer
      //  mouseTimer = 0f;
      //  mousePosition = board.GetNextMousePosition();
      //}

      #region timer actions
      mouseRespawnTimer.Update(gameTime);
      if (mouseRespawnTimer.NeedsReset()) {
        mouseRespawnTimer.Reset();
        mousePosition = board.GetNextMousePosition(); //update mouse position
        Console.WriteLine($"mouse coord: ({mousePosition})");
      }
      #endregion

      #region keyboard
      keyboardState = OneShotKbd.GetState();

      if (this.keyboardState.GetPressedKeyCount() > 0) {
        HandleInput();
      }

      snakeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
      if (snakeTimer >= snakeSpeed) {
        snakeTimer -= snakeSpeed;
        Cell nextCell = GetNextCell(snake.GetHead());
        if (nextCell.GetType() == CellType.FOOD) {

          Console.WriteLine($"Collided with {nextCell.GetType()}, increase size and move mouse");
          snake.IncreaseSize();
          mousePosition = board.GetNextMousePosition();
        } else if (nextCell.GetType() == CellType.SNAKE_NODE || nextCell.GetType() == CellType.POISON) {
          Console.WriteLine($"Collided with {nextCell.GetType()}, game over :(");
          // Set direction to none to stop moving?
        }
        snake.Move(nextCell);
      }


      #endregion
      base.Update(gameTime);
    }
    #endregion

    #region MyFunctions
    private Cell GetNextCell(Cell snakeHead) {
      (int row, int col) = snakeHead.GetPosition();

      // Change position based on direction, respawn snake at proper position
      switch (snake.GetDirection()) {
        case Direction.Left:
          col--;
          if (col < 0) {
            col = Globals.GetMaxMapCellValue();
          }
          break;
        case Direction.Right:
          col++;
          if (col > Globals.GetMaxMapCellValue()) {
            col = 0;
          }
          break;
        case Direction.Up:
          row--;
          if (row < 0) {
            row = Globals.GetMaxMapCellValue();
          }
          break;
        case Direction.Down:
          row++;
          if (row > Globals.GetMaxMapCellValue()) {
            row = 0;
          }
          break;
      }

      // Access the next cell
      Cell nextCell = board.GetCells()[row,col];
      return nextCell;
    }

    protected void HandleInput() {
      // Quit game
      if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
        Exit();
      }

      if (snake.GetDirectionAxis() == DirectionAxis.X) {
        // Up
        if (keyboardState.IsKeyDown(Keys.Up) && OneShotKbd.IsNewKeyPress(Keys.Up)) {
          snake.SetDirection(Direction.Up);
        }
        // Down
        if (keyboardState.IsKeyDown(Keys.Down) && OneShotKbd.IsNewKeyPress(Keys.Down)) {
          snake.SetDirection(Direction.Down);
        }
      } else {
        // Right
        if (keyboardState.IsKeyDown(Keys.Right) && OneShotKbd.IsNewKeyPress(Keys.Right)) {
          snake.SetDirection(Direction.Right);
        }
        // Left
        if (keyboardState.IsKeyDown(Keys.Left) && OneShotKbd.IsNewKeyPress(Keys.Left)) {
          snake.SetDirection(Direction.Left);
        }
      }
    }
    #endregion
  }
}