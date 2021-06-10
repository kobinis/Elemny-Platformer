using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;
using XnaUtils.XnaUtils.RichText;

namespace SolarConflict {
    /// <remarks>Mostly pointlessly copypasted from the internal XnaUtils ImageCommand</remarks>
    [Serializable]
    class ItemIconCommand : ITextElement {
        public const string Command = "itemicon";

        Sprite _secondarySprite;
        Sprite _sprite;

        public void Draw(SpriteBatch spriteBatch, RichTextParser parser, Vector2 position, Color? color) {
            if (_sprite == null)
                return;

            color = color ?? Color.White;

            var height = Math.Max(parser.LineHeight, parser.CurrentHeight);
            if (height == 0) {
                var defaultCharacter = parser.CurrentFont.DefaultCharacter.HasValue ? parser.CurrentFont.DefaultCharacter.Value : ' ';
                height = parser.CurrentFont.MeasureString(defaultCharacter.ToString()).Y; // default height                                
            }

            var rectangle = new Rectangle((int)(parser.CurrentPosition.X + position.X), (int)(parser.CurrentPosition.Y + position.Y), _sprite.Width, (int)height);

            var size = Sprite.DrawIcon(spriteBatch, rectangle, color.Value, false, 1f, _sprite, _secondarySprite);

            parser.CurrentHeight = Math.Max(parser.CurrentHeight, size.Y);
            parser.CurrentPosition += new Vector2(size.X, 0);
        }

        /// <remarks>The current size, which is based on previously-parsed stuff. If nothing parsed or value is otherwise zero, defaults to a value
        /// based on the size of the current font's default character.</remarks>        
        public Vector2 GetSize(RichTextParser parser) {
            if (_sprite == null)
                return Vector2.Zero;

            var sprites = _secondarySprite == null ? new Sprite[] { _sprite } : new Sprite[] { _sprite, _secondarySprite };

            var height = Math.Max(parser.LineHeight, parser.CurrentHeight);
            if (height == 0) {
                var defaultCharacter = parser.CurrentFont.DefaultCharacter.HasValue ? parser.CurrentFont.DefaultCharacter.Value : ' ';
                height = parser.CurrentFont.MeasureString(defaultCharacter.ToString()).Y; // default height                                
            }

            Vector2 size = FMath.FitSize(new Vector2(sprites.First().Width, sprites.First().Height), new Vector2(sprites.First().Width, height));
            parser.CurrentHeight = Math.Max(parser.CurrentHeight, size.Y);
            parser.CurrentPosition += new Vector2(size.X, 0);
            return size;
        }

        public void ParseParameters(string parameters) {
            try {
                var item = ContentBank.Inst.GetItem(parameters, false);

                _sprite = item.Sprite;
                _secondarySprite = item.SecondarySprite;
            } catch {
                // Item not found, do nothing
            }
        }
    }
}
