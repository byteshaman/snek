using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using snek.Controls;
using snek.Helpers;
using System.Collections.Generic;

namespace snek.States {
  public class MenuState : State {
    private readonly List<Component> components;
    public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager contentManager, MenuType menu) : base(game, graphicsDevice, contentManager) {
      //Texture2D buttonTexture = contentManager.Load<Texture2D>("Controls/Button");
      SpriteFont buttonFont = contentManager.Load<SpriteFont>("Fonts/Font");

      Vector2 centerXY = new(Globals.WINDOW_SIZE / 2); //center of the window
      Vector2 marginY = new(0, 130); //y offset btween buttons

      // Negative margin because upper button
      Button newGameButton = new(buttonFont) {
        Position = centerXY - marginY,
        Text = "Start"
      };      

      newGameButton.Click += (sender, e) => {
        game.ChangeState(new GameState(game, graphicsDevice, contentManager));
      };

      // No margin because middle button
      Button highscoresButton = new(buttonFont) {
        Position = centerXY,
        Text = "Highscores",
      };

      //highscoresButton.Click += (sender, e) => {
      //  game.ChangeState(new HighscoreState(game, graphicsDevice, contentManager));
      //};

      // Positive margin because lower button
      Button quitGameButton = new(buttonFont) {
        Position = centerXY + marginY,
        Text = "Exit",
      };

      quitGameButton.Click += (sender, e) => {
        game.Exit();
      };

      components = new List<Component>() {
        newGameButton,
        highscoresButton,
        quitGameButton,
      };
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      graphicsDevice.Clear(Globals.MENU_BG_COLOR);

      spriteBatch.Begin();

      foreach (Component component in components) { 
        component.Draw(gameTime, spriteBatch);
      }

      spriteBatch.End();
    }

    public override void Update(GameTime gameTime) {
      foreach (Component component in components) {
        component.Update(gameTime);
      }
    }
  }
}
