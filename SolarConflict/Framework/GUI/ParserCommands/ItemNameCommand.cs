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
    /// <remarks>Mostly pointlessly copypasted from the internal XnaUtils TextCommand</remarks>
    [Serializable]
    class ItemNameCommand : ITextElement {
        public const string Command = "itemname";

        private string _text;
        private Color _color;

        public void Draw(SpriteBatch spriteBatch, RichTextParser parser, Vector2 position, Color? color) {
            if (_text == null)
                return;

            var drawColor = _color;
            if (color != null) {
                drawColor = new Color(parser.CurrentColor.ToVector4() * color.Value.ToVector4());
            }
            spriteBatch.DrawString(parser.CurrentFont, _text, parser.CurrentPosition + position, drawColor);
            GetSize(parser);
        }

        public Vector2 GetSize(RichTextParser parser) {
            if (_text == null)
                return Vector2.Zero;

            Vector2 size = parser.CurrentFont.MeasureString(_text);
            parser.CurrentHeight = Math.Max(parser.CurrentHeight, size.Y);
            parser.CurrentPosition += new Vector2(size.X, 0);
            return size;
        }

        public void ParseParameters(string parameters) {            
            try {
                var item = ContentBank.Inst.GetItem(parameters, false);
                _text = item.Name;
                _color = ItemProfile.GetLevelColor(item.Level);
            } catch {
                // Item not found, do nothing
            }
        }        
    }
}
