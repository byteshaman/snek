using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace snek {
	public class Game1 : Game	{
		#region Attributes
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;
		private Texture2D fruitTexture;
		private Texture2D snakeHeadTexture;
		private Vector2 fruitPosition;

		// Timer
		readonly float countDuration; 
		float currentTime;
		#endregion

		// Constructor, used to initialize the starting variables
		public Game1() {
			this.graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			this.IsMouseVisible = true;
			this.countDuration = 5f;
			this.currentTime = 0f;
		}

		#region BaseFunctions
		// Called after the constructor, used to query any required services and load any non-graphic related content
		protected override void Initialize() {
			Debug.WriteLine($"Width: {this.graphics.PreferredBackBufferHeight}, Height: {this.graphics.PreferredBackBufferWidth}");

			base.Initialize();
		}

		// Called within the Initialize method, used to load game content 
		protected override void LoadContent() {
			this.spriteBatch = new SpriteBatch(GraphicsDevice);

			this.fruitTexture = Content.Load<Texture2D>("fruit");
			this.fruitPosition = this.GetXYPosition(fruitTexture.Width, fruitTexture.Height);
			this.snakeHeadTexture = Content.Load<Texture2D>("snakeHead");
		}

		// Called multiple times per second, draws content to the screen
		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);

			this.spriteBatch.Begin();
			this.spriteBatch.Draw(fruitTexture, fruitPosition, Color.White);
			this.spriteBatch.End();

			base.Draw(gameTime);
		}

		// Called multiple times per second, updates the game state
		protected override void Update(GameTime gameTime)	{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			#region timer actions
			// Calculate time passed since last Update() 
			this.currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; 

			if (this.currentTime >= this.countDuration)	{
				this.currentTime -= this.countDuration; // reset timer
				this.fruitPosition = GetXYPosition(fruitTexture.Width, fruitTexture.Height); //update fruit position
			}
			#endregion

			base.Update(gameTime);
		}
		#endregion

		#region MyFunctions
		private Vector2 GetXYPosition(int width, int height) {
			int lowerBound = 0;
			int upperBoundX = this.graphics.PreferredBackBufferWidth - width;
			int upperBoundY = this.graphics.PreferredBackBufferHeight - height;
			Random random = new();
			int posX = random.Next(lowerBound, upperBoundX);
			int posY = random.Next(lowerBound, upperBoundY);

			return new Vector2(posX, posY);
		}
		#endregion
	}
}