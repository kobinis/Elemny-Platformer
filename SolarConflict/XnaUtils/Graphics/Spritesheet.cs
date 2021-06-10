using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace XnaUtils.Graphics
{
    /// <remarks>Assumes fixed sprite size and no padding</remarks>
    [Serializable]
    public class Spritesheet
    {

        public int NumSprites { get; private set; }
        int _spritesPerRow;
        public int SpriteWidth { get; private set; }
        public int SpriteHeight { get; private set; }

        public Sprite Sheet { get; private set; }

        public Spritesheet(string sheetId, string normalId, int spriteWidth, int spriteHeight, int numSprites)
        {
            // Could just compute spritesPerRow, but that involves assumptions about our sheet maker            
            SpriteWidth = spriteWidth;
            SpriteHeight = spriteHeight;
            NumSprites = numSprites;

            Sheet = new Sprite(sheetId, normalId);

            _spritesPerRow = Sheet.Width / spriteWidth;
        }

        public Spritesheet(string sheetId, int spriteWidth, int spriteHeight, int numSprites) : this(sheetId, null, spriteWidth, spriteHeight, numSprites) { }

        public Rectangle SourceRect(int spriteIndex)
        {
            Debug.Assert(spriteIndex >= 0 && spriteIndex < NumSprites, "Sprite index out of bounds");

            var x = spriteIndex % _spritesPerRow;
            var y = spriteIndex / _spritesPerRow;

            return new Rectangle(x * SpriteWidth, y * SpriteHeight, SpriteWidth, SpriteHeight);
        }
    }
}
