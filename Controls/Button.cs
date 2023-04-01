using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using MonoGame.Extended;
using snek.Helpers;

namespace snek.Controls
{
    public class Button {
    #region Fields

    private MouseState currentMouse;
    private SpriteFont font;
    private bool isHovering;
    private MouseState previousMouse;

    #endregion

    #region Properties

    public event EventHandler Click; //used to call a specific function when click happens
    public bool Clicked { get; private set; }
    public Color FontColour { get; set; }
    public Vector2 Position { get; set; }
    public Rectangle Rectangle {
      get {
        Vector2 textPadding = new(20, 0);
        Vector2 textSize = font.MeasureString(Text) + textPadding;
        Vector2 rectanglePos = Position - (textSize/2) + (textPadding/2);

        //Console.WriteLine($"Button '{Text}', that measures {textSize.X}x{textSize.Y} was created at coordinates: {Position}");

        return new Rectangle((int)rectanglePos.X, (int)rectanglePos.Y, (int)textSize.X, (int)textSize.Y);
      }
    }
    public string Text { get; set; }

    #endregion

    #region Methods

    public Button(SpriteFont font) {
      this.font = font;

      FontColour = Globals.FONT_COLOR;
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
      Color buttonColor = Globals.BUTTON_COLOR;

      // Change button color when it's hovered
      if (isHovering) {
        buttonColor = Globals.BUTTON_HOVER_COLOR;
      }

      // Draw button with border
      spriteBatch.FillRectangle(new Vector2(Rectangle.X, Rectangle.Y), new Size2(Rectangle.Width, Rectangle.Height), Globals.BUTTON_BORDER_COLOR);
      spriteBatch.FillRectangle(new Vector2(Rectangle.X, Rectangle.Y) + new Vector2(2), new Size2(Rectangle.Width-4, Rectangle.Height-4), buttonColor);
      

      if (!string.IsNullOrEmpty(Text)) {
        // Center string horizontally and vertically inside the button rectangle
        float x = (Rectangle.X + (Rectangle.Width / 2)) - (font.MeasureString(Text).X / 2);
        float y = (Rectangle.Y + (Rectangle.Height / 2)) - (font.MeasureString(Text).Y / 2);

        spriteBatch.DrawString(font, Text, new Vector2(x,y), FontColour);
      }
    }

    public void Update(GameTime gameTime) {
      previousMouse = currentMouse;
      currentMouse = Mouse.GetState();

      Rectangle mouseRectangle = new(currentMouse.X, currentMouse.Y, 1, 1);

      isHovering = false;

      // Detect mouse "collision" with button
      if (mouseRectangle.Intersects(Rectangle)) {
        isHovering = true;

        if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed) {
          Click?.Invoke(this, new EventArgs());
        }
      }
    }

    #endregion
  }
}
