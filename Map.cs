using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace snek {
  internal class Map {
    private readonly int tileSize, tilesNumber;


    public Map(int tilesNumber, int tileSize) {
      this.tileSize = tileSize;
      this.tilesNumber = tilesNumber;
    }

    public void Draw(SpriteBatch spriteBatch) {
      Vector2 tilePosition = Vector2.Zero;

      spriteBatch.Begin();
      for (int x = 0; x < tilesNumber; x++) {
        for (int y = 0; y < tilesNumber; y++) {
          spriteBatch.FillRectangle(tilePosition, new Size2(tileSize, tileSize), Color.DarkGray);
          // Draw border
          spriteBatch.FillRectangle(tilePosition + new Vector2(1, 1), new Size2(tileSize - 2, tileSize - 2), Color.Black);
          // Draw column
          tilePosition.Y += tileSize;
        }
        // Reset column
        tilePosition.Y = 0;
        // Move to the next row
        tilePosition.X += tileSize;
      }
      spriteBatch.End();
    }
  }
}
