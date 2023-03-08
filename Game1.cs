using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;



namespace snek {
  public class Game1 : Game {
    #region Attributes
    readonly GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    // Mouse
    Texture2D mouseTexture;
    Vector2 mousePosition;
    float mouseTimer;
    readonly float respawnMouseTimer;

    // Snake
    Snake snake;
    Texture2D snakePartTexture;
    Vector2 snakeHeadPosition;
    float snakeTimer;
    readonly float snakeSpeed;


    // Keyboard
    KeyboardState keyboardState;

    // GameBoard
    readonly GameBoard board;

    // Timer
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
      snakeSpeed = 0.5f;

      respawnMouseTimer = 500f;
      mouseTimer = 0f;

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
      

      // Detect collision
      if (snakeHeadPosition == mousePosition) {
        // Reset timer
        mouseTimer = 0f;
        mousePosition = board.GetNextMousePosition();
      }

      #region timer actions
      // Calculate time passed since last Update() 
      mouseTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

      if (mouseTimer >= respawnMouseTimer) {
        mouseTimer -= respawnMouseTimer; // reset timer
        mousePosition = board.GetNextMousePosition(); //update mouse position
        Console.WriteLine($"mouse coord: ({mousePosition.X}, {mousePosition.Y})");
      }
      #endregion

      #region keyboard
      keyboardState = OneShotKeyboard.GetState();

      if (this.keyboardState.GetPressedKeyCount() > 0) {
        HandleInput();
      }

      snakeTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
      if (snakeTimer >= snakeSpeed) {
        snakeTimer -= snakeSpeed;
        switch (snake.GetDirection()) {
          case Direction.LEFT:
            snakeHeadPosition.X -= Globals.CELL_SIZE;
            break;
          case Direction.RIGHT:
            snakeHeadPosition.X += Globals.CELL_SIZE;
            break;
          case Direction.UP:
            snakeHeadPosition.Y -= Globals.CELL_SIZE;
            break;
          case Direction.DOWN:
            snakeHeadPosition.Y += Globals.CELL_SIZE;
            break;
        }
      }

      HandleBorderCollision();

      #endregion
      base.Update(gameTime);
    }
    #endregion

    #region MyFunctions
    protected void HandleBorderCollision() {
      // Respawn right
      if (snakeHeadPosition.X >= Globals.WINDOW_SIZE) {
        snakeHeadPosition.X -= Globals.WINDOW_SIZE;
      }
      // Respawn left
      if (snakeHeadPosition.X < 0) {
        snakeHeadPosition.X += Globals.WINDOW_SIZE;
      }
      // Respawn down
      if (snakeHeadPosition.Y >= Globals.WINDOW_SIZE) {
        snakeHeadPosition.Y -= Globals.WINDOW_SIZE;
      }
      // Respawn up
      if (snakeHeadPosition.Y < 0) {
        snakeHeadPosition.Y += Globals.WINDOW_SIZE;
      }
    }

    protected void HandleInput() {
      // Quit game
      if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
        Exit();
      }

      // Right
      if (keyboardState.IsKeyDown(Keys.Right)) {
        if (snake.GetDirectionAxis() != DirectionAxis.X) {
          snake.SetDirection(Direction.RIGHT);
          OneShotKeyboard.SetLastKeyPressed(Keys.Right);
        }
      }
      // Left
      if (keyboardState.IsKeyDown(Keys.Left)) {
        if (snake.GetDirectionAxis() != DirectionAxis.X) {
          snake.SetDirection(Direction.LEFT);
          OneShotKeyboard.SetLastKeyPressed(Keys.Left);
        }
      }
      // Up
      if (keyboardState.IsKeyDown(Keys.Up)) {
        //if (OneShotKeyboard.HasNotBeenPressed(Keys.Up)) {
        //  OneShotKeyboard.SetLastKeyPressed(Keys.Up);
        //}
        if (snake.GetDirectionAxis() != DirectionAxis.Y) {
          snake.SetDirection(Direction.UP);
          OneShotKeyboard.SetLastKeyPressed(Keys.Up);
        }
      }
      // Down
      if (snake.GetDirectionAxis() != DirectionAxis.Y) {
        if (snake.GetDirectionAxis() != DirectionAxis.Y) { 
          snake.SetDirection(Direction.DOWN);
          OneShotKeyboard.SetLastKeyPressed(Keys.Down);
        }
      }
    }
    #endregion
  }
}