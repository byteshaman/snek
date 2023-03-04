using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Xml.Schema;



namespace snek {
  public class Game1 : Game {
    #region Attributes
    readonly GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    readonly int windowSize;

    // Mouse
    Texture2D mouseTexture;
    Vector2 mousePosition;
    float mouseTimer;
    readonly float respawnMouseTimer;

    // Snake
    Texture2D snakeHeadTexture;
    Vector2 snakeHeadPosition;
    float snakeTimer;
    readonly float snakeSpeed;


    // Keyboard
    KeyboardState keyboardState;

    // Map
    readonly Map map;
    readonly int tilesNumber;
    readonly int tileSize;

    // Timer
    #endregion

    // Constructor, used to initialize the starting variables
    public Game1() {
      windowSize = 800;

      graphics = new GraphicsDeviceManager(this);
      graphics.PreferredBackBufferWidth = graphics.PreferredBackBufferHeight = windowSize;
      graphics.ApplyChanges();

      tilesNumber = 20;
      tileSize = 40;
      map = new(tilesNumber, tileSize);

      keyboardState = new KeyboardState();
      snakeTimer = 0f;
      snakeSpeed = 0.1f;

      Content.RootDirectory = "Content";
      IsMouseVisible = true;
      respawnMouseTimer = 5f;
      mouseTimer = 0f;
    }

    #region BaseFunctions
    // Called after the constructor, used to query any required services and load any non-graphic related content
    protected override void Initialize() {
      Console.WriteLine($"Width: {graphics.PreferredBackBufferWidth}, Height: {graphics.PreferredBackBufferHeight}");

      base.Initialize();

      mousePosition = GetXYPosition(mouseTexture.Width, mouseTexture.Height);
      snakeHeadPosition = new Vector2(0, 0);
      OneShotKeyboard.SetLastKeyPressed(Keys.Right);
    }

    // Called within the Initialize method, used to load game content 
    protected override void LoadContent() {
      spriteBatch = new SpriteBatch(GraphicsDevice);

      mouseTexture = Content.Load<Texture2D>("mouse");
      snakeHeadTexture = Content.Load<Texture2D>("snakeHead");
    }

    // Called multiple times per second, draws content to the screen
    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(Color.Black);
      map.Draw(spriteBatch);

      spriteBatch.Begin();
      spriteBatch.Draw(mouseTexture, mousePosition, Color.White);
      spriteBatch.Draw(snakeHeadTexture, snakeHeadPosition, Color.White);
      spriteBatch.End();


      base.Draw(gameTime);
    }

    // Called multiple times per second, updates the game state
    protected override void Update(GameTime gameTime) {
      if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
        Exit();
      }

      // Detect collision
      if (snakeHeadPosition == mousePosition) {
      mouseTimer = 0f;
        mousePosition = GetXYPosition(mouseTexture.Width, mouseTexture.Height);
      }

      #region timer actions
      // Calculate time passed since last Update() 
      mouseTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

      if (mouseTimer >= respawnMouseTimer) {
        mouseTimer -= respawnMouseTimer; // reset timer
        mousePosition = GetXYPosition(mouseTexture.Width, mouseTexture.Height); //update mouse position
        Console.WriteLine($"mousePosition.X: {mousePosition.X}, mousePosition.Y: {mousePosition.Y}");
      }
      #endregion

      #region keyboard
      keyboardState = OneShotKeyboard.GetState();

      if (this.keyboardState.GetPressedKeyCount() > 0) {
        HandleInput();
      }

      snakeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
      if (snakeTimer >= snakeSpeed) {
        snakeTimer -= snakeSpeed;
        switch (OneShotKeyboard.GetLastKeyPressed()) {
          case Keys.Left:
            snakeHeadPosition.X -= tileSize;
            break;
          case Keys.Right:
            snakeHeadPosition.X += tileSize;
            break;
          case Keys.Up:
            snakeHeadPosition.Y -= tileSize;
            break;
          case Keys.Down:
            snakeHeadPosition.Y += tileSize;
            break;
        }
      }

      HandleBorderCollision();

      #endregion
      base.Update(gameTime);
    }
    #endregion

    #region MyFunctions
    Vector2 GetXYPosition(int width, int height) {
      int lowerBound = 1;
      int upperBound = tilesNumber;
      Random random = new();
      int rndX = random.Next(lowerBound, upperBound);
      int rndY = random.Next(lowerBound, upperBound);
      int posX = rndX * tileSize;
      int posY = rndY * tileSize;

      Console.WriteLine($"TileX: {rndX}, TileY: {rndY} - PosX: {posX}, PosY: {posY}");

      return new Vector2(posX, posY);
    }

    protected void HandleBorderCollision() {
      // Respawn right
      if (snakeHeadPosition.X >= windowSize) {
        snakeHeadPosition.X -= windowSize;
      }
      // Respawn left
      if (snakeHeadPosition.X < 0) {
        snakeHeadPosition.X += windowSize;
      }
      // Respawn down
      if (snakeHeadPosition.Y >= windowSize) {
        snakeHeadPosition.Y -= windowSize;
      }
      // Respawn up
      if (snakeHeadPosition.Y < 0) {
        snakeHeadPosition.Y += windowSize;
      }
    }

    protected void HandleInput() {
      // Right
      if (keyboardState.IsKeyDown(Keys.Right)) {
        if (OneShotKeyboard.HasNotBeenPressed(Keys.Right)) {
          //snakeHeadPosition.X += tileSize;
          OneShotKeyboard.SetLastKeyPressed(Keys.Right);
        }
      }
      // Left
      if (keyboardState.IsKeyDown(Keys.Left)) {
        if (OneShotKeyboard.HasNotBeenPressed(Keys.Left)) {
          //snakeHeadPosition.X -= tileSize;
          OneShotKeyboard.SetLastKeyPressed(Keys.Left);
        }
      }
      // Up
      if (keyboardState.IsKeyDown(Keys.Up)) {
        if (OneShotKeyboard.HasNotBeenPressed(Keys.Up)) {
          //snakeHeadPosition.Y -= tileSize;
          OneShotKeyboard.SetLastKeyPressed(Keys.Up);
        }
      }
      // Down
      if (keyboardState.IsKeyDown(Keys.Down)) {
        if (OneShotKeyboard.HasNotBeenPressed(Keys.Down)) {
          //snakeHeadPosition.Y += tileSize;
          OneShotKeyboard.SetLastKeyPressed(Keys.Down);
        }
      }
    }
    #endregion
  }
}