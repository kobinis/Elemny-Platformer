using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaUtils.XnaUtils.RichText.Commands
{
    /// <summary>
    /// Example: #ctext{255,0,0,"Example text is red"}
    /// </summary>
    [Serializable]
    internal class CTextCommand : ISplittableTextElement
    {
        public const string Command = "ctext";

        private string _text;
        private Color _color;
        
        public CTextCommand()
        {
        }

        public CTextCommand(string text, Color color)
        {
            _text = text;
            _color = color;
        }

        public void Draw(SpriteBatch spriteBatch, RichTextParser parser, Vector2 position, Color? color)
        {
            var drawColor = _color;
            if (color != null)
            {
                drawColor = new Color(_color.ToVector4() * color.Value.ToVector4());
            }
            spriteBatch.DrawString(parser.CurrentFont, _text, parser.CurrentPosition + position, drawColor);
            GetSize(parser);
        }

        public Vector2 GetSize(RichTextParser parser)
        {
            Vector2 size = parser.CurrentFont.MeasureString(_text);
            parser.CurrentHeight = Math.Max(parser.CurrentHeight, size.Y);
            parser.CurrentPosition += new Vector2(size.X, 0);
            return size;
        }

        public void ParseParameters(string paramaters)
        {
            string[] colorAndText = paramaters.Split('"');
            if (colorAndText.Length < 2) {
                // Error parsing parmeters
                _text = "ColorCommand Error!";
                _color = Color.Red;

                return;
            }
            _color = ColorCommand.ColorParse(colorAndText[0]);
            _text = colorAndText[1];
        }

        public ITextElement[] Split(RichTextParser parser, float currentLineWidth, float lineWidth = -1f) {
            if (lineWidth < 0)
                lineWidth = currentLineWidth;

            return TextCommand.Split(parser, _text, currentLineWidth, lineWidth).Select(s => new CTextCommand() { _color = _color, _text = s }).ToArray();

        }
    }
}
