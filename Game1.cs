﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using snek.Helpers;
using snek.States;
using System;

namespace snek {
  public class Game1 : Game {
    readonly GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    State currentState;
    State nextState;


    // Constructor, used to initialize the starting variables
    public Game1() {
      graphics = new GraphicsDeviceManager(this) {
        PreferredBackBufferWidth = Globals.WINDOW_SIZE,
        PreferredBackBufferHeight = Globals.WINDOW_SIZE
      };
      graphics.ApplyChanges();

      Content.RootDirectory = "Content";
    }

    #region BaseFunctions
    // Called after the constructor, used to query any required services and load any non-graphic related content
    protected override void Initialize() {
      // Load custom cursor
      Mouse.SetCursor(MouseCursor.FromTexture2D(Content.Load<Texture2D>("Sprites/mouseCursor"),0,0));
      
      IsMouseVisible = true;

      // Load menu
      currentState = new MenuState(this, graphics.GraphicsDevice, Content, MenuType.GameStart);

      // Debug
      //Console.WriteLine($"Width: {graphics.PreferredBackBufferWidth}, Height: {graphics.PreferredBackBufferHeight}");

      base.Initialize();
    }

    // Called within the Initialize method, used to load game content 
    protected override void LoadContent() {
      spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    // Called multiple times per second, draws content to the screen
    protected override void Draw(GameTime gameTime) {
      currentState.Draw(gameTime, spriteBatch);

      base.Draw(gameTime);
    }

    // Called multiple times per second, updates the game state
    protected override void Update(GameTime gameTime) {
      currentState.Update(gameTime);

      // Detect state change
      if (nextState != null) {
        currentState = nextState;
        nextState = null;
      }

      base.Update(gameTime);
    }
    #endregion

    public void ChangeState (State state) {
      nextState = state;
    }
  }
}