using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaUtils.XnaUtils.RichText.Commands
{
    [Serializable]
    internal class TextCommand : ISplittableTextElement {
        public const string Command = "text";

        private string _text;

        public TextCommand(string text) {
            _text = text;
        }

        public TextCommand() {
        }

        public void Draw(SpriteBatch spriteBatch, RichTextParser parser, Vector2 position, Color? color) {
            var drawColor = parser.CurrentColor;
            if (color != null) {
                drawColor = new Color(parser.CurrentColor.ToVector4() * color.Value.ToVector4());
            }
            if( parser.IsTextBorder)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if(Math.Abs(dx) + Math.Abs(dy) < 2)
                            spriteBatch.DrawString(parser.CurrentFont, _text, parser.CurrentPosition + position + new Vector2(dx,dy), Color.Black, 0, Vector2.Zero, parser.Scale, SpriteEffects.None, 0);
                    }
                }                
            }
            spriteBatch.DrawString(parser.CurrentFont, _text, parser.CurrentPosition + position, drawColor,0, Vector2.Zero,
                parser.Scale, SpriteEffects.None, 0);
            GetSize(parser);
        }

        public Vector2 GetSize(RichTextParser parser) {            
            Vector2 size = parser.CurrentFont.MeasureString(_text) * parser.Scale;
            parser.CurrentHeight = Math.Max(parser.CurrentHeight, size.Y);
            parser.CurrentPosition += new Vector2(size.X, 0);
            return size;
        }

        public void ParseParameters(string paramaters) {
            _text = paramaters;
        }

        public ITextElement[] Split(RichTextParser parser, float currentLineWidth, float lineWidth = -1f) {
            if (lineWidth < 0)
                lineWidth = currentLineWidth;

            return Split(parser, _text, currentLineWidth, lineWidth).Select(s => new TextCommand(s)).ToArray();

        }
        public static IEnumerable<string> Split(RichTextParser parser, string text, float currentLineWidth, float lineWidth) {
            if (parser.CurrentFont.MeasureString(text).X <= currentLineWidth) {
                // String's already short enough
                yield return text;
                yield break;
            }

            // Look for a good place to add a newline (the space closest to the end of line, from the left)
            var splitIndex = -1;
            for (int i = 0; i <  text.Length; ++i) {
                if (text[i] == ' ') {                    
                    var left = text.Substring(0, i);
                    if (parser.CurrentFont.MeasureString(left).X <= currentLineWidth)
                        // Can split here. Might be a better space up ahead, though. Keep searching
                        splitIndex = i;
                    else
                        break;
                }
            }

            if (splitIndex < 0) {
                // Can't split
                yield return text;
                yield break;
            }
            
            // Yield left side of split, recur for right
            if (splitIndex > 0)
                yield return text.Substring(0, splitIndex);
            foreach (var s in Split(parser, text.Substring(splitIndex + 1), lineWidth, lineWidth))
                yield return s;             
        }
    }
}
