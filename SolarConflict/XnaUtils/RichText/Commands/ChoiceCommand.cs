using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;
using XnaUtils.XnaUtils.RichText;

namespace XnaUtils.XnaUtils.RichText.Commands {
    /// <remarks>Everything but the Command string and ParseParmeters copypasted (rather than inherited) from TextCommand. It was Kobi's idea</remarks>
    [Serializable]
    class ChoiceCommand : ITextElement {
        public const string Command = "choice";

        string _text;

        public void Draw(SpriteBatch spriteBatch, RichTextParser parser, Vector2 position, Color? color) {
            var drawColor = parser.CurrentColor;
            if (color != null) {
                drawColor = new Color(parser.CurrentColor.ToVector4() * color.Value.ToVector4());
            }
            spriteBatch.DrawString(parser.CurrentFont, _text, parser.CurrentPosition + position, drawColor);
            GetSize(parser);
        }

        public Vector2 GetSize(RichTextParser parser) {
            Vector2 size = parser.CurrentFont.MeasureString(_text);
            parser.CurrentHeight = Math.Max(parser.CurrentHeight, size.Y);
            parser.CurrentPosition += new Vector2(size.X, 0);
            return size;
        }

        public void ParseParameters(string parameters) {
            //_text = parameters.Split('|').Choice(FMath.Rand);
            var split = parameters.Split('|');
            _text = split[FMath.Rand.Next() % split.Length];
        }
    }
}