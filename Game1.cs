using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace snek {
  public class Game1 : Game	{
		#region Attributes
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;
		private Texture2D mouseTexture;
		private Texture2D snakeHeadTexture;
		private Vector2 snakeHeadPosition;
		private Vector2 mousePosition;

    // Keyboard
    KeyboardState keyboardState;

    // Map
    private readonly Map map;
		private readonly int tilesNumber;
		private readonly int tileSize;

		// Timer
		readonly float countDuration; 
		float currentTime;
		#endregion

		// Constructor, used to initialize the starting variables
		public Game1() {
			this.graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = graphics.PreferredBackBufferHeight = 800;
			graphics.ApplyChanges();

			this.tilesNumber = 20;
			this.tileSize = 40;
			this.map = new(tilesNumber, tileSize);

      keyboardState = new KeyboardState();


      Content.RootDirectory = "Content";
			this.IsMouseVisible = true;
			this.countDuration = 100f;
			this.currentTime = 0f;
		}

		#region BaseFunctions
		// Called after the constructor, used to query any required services and load any non-graphic related content
		protected override void Initialize() {
			Console.WriteLine($"Width: {this.graphics.PreferredBackBufferWidth}, Height: {this.graphics.PreferredBackBufferHeight}");

			base.Initialize();

      this.mousePosition = this.GetXYPosition(mouseTexture.Width, mouseTexture.Height);
			this.snakeHeadPosition = new Vector2(0,0);
		}

		// Called within the Initialize method, used to load game content 
		protected override void LoadContent() {
			this.spriteBatch = new SpriteBatch(GraphicsDevice);

			this.mouseTexture = Content.Load<Texture2D>("mouse");
			this.snakeHeadTexture = Content.Load<Texture2D>("snakeHead");
		}

		// Called multiple times per second, draws content to the screen
		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);
			map.Draw(this.spriteBatch);

			this.spriteBatch.Begin();
			this.spriteBatch.Draw(mouseTexture, mousePosition, Color.White);
			this.spriteBatch.Draw(snakeHeadTexture, snakeHeadPosition, Color.White);
			this.spriteBatch.End();


			base.Draw(gameTime);
		}

		// Called multiple times per second, updates the game state
		protected override void Update(GameTime gameTime)	{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape)) { 
				Exit();
      }

      #region keyboard
      this.keyboardState = OneShotKeyboard.GetState();

			if (keyboardState.GetPressedKeyCount() > 0) {
				this.HandleInput();
			}
      #endregion

			// Detect collision
			if (snakeHeadPosition == mousePosition) {
        this.mousePosition = GetXYPosition(mouseTexture.Width, mouseTexture.Height);
      }

      #region timer actions
      // Calculate time passed since last Update() 
      this.currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; 

			if (this.currentTime >= this.countDuration)	{
				this.currentTime -= this.countDuration; // reset timer
				this.mousePosition = GetXYPosition(mouseTexture.Width, mouseTexture.Height); //update mouse position
				Console.WriteLine($"FWidth: {this.mousePosition.X}, FHeight: {this.mousePosition.Y}");
			}
			#endregion

			base.Update(gameTime);
		}
		#endregion

		#region MyFunctions
		private Vector2 GetXYPosition(int width, int height) {
			int lowerBound = 1;
			int upperBound = this.tilesNumber;
			Random random = new();
			int rndX = random.Next(lowerBound, upperBound);
			int rndY = random.Next(lowerBound, upperBound);
			int posX = rndX*tileSize;
			int posY = rndY*tileSize;

      Console.WriteLine($"TileX: {rndX}, TileY: {rndY} - PosX: {posX}, PosY: {posY}");

      return new Vector2(posX, posY);
		}

		protected void HandleInput() {
			// Right
      if (this.keyboardState.IsKeyDown(Keys.Right)) {
				if (OneShotKeyboard.HasNotBeenPressed(Keys.Right)) { 
					snakeHeadPosition.X += this.tileSize;
        }
      }
      // Left
      if (this.keyboardState.IsKeyDown(Keys.Left)) {
        if (OneShotKeyboard.HasNotBeenPressed(Keys.Left)) {
          snakeHeadPosition.X -= this.tileSize;
        }
      }
			// Up
      if (this.keyboardState.IsKeyDown(Keys.Up)) {
        if (OneShotKeyboard.HasNotBeenPressed(Keys.Up)) {
          snakeHeadPosition.Y -= this.tileSize;
        }
      }
			// Down
      if (this.keyboardState.IsKeyDown(Keys.Down)) {
        if (OneShotKeyboard.HasNotBeenPressed(Keys.Down)) {
          snakeHeadPosition.Y += this.tileSize;
        }
      }
    }
		#endregion
	}
}