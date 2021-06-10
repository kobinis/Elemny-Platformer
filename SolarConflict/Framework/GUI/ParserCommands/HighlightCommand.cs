using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaUtils;
using XnaUtils.XnaUtils.RichText;

namespace SolarConflict.Framework.GUI.ParserCommands
{
    [Serializable]
    class HighlightCommand : ITextElement
    {
        public const string Command = "hcolor";

        public void ParseParameters(string paramaters)
        {
        }

        public void Draw(SpriteBatch spriteBatch, RichTextParser parser, Vector2 position, Color? color)
        {
            parser.CurrentColor = Palette.Highlight;
        }

        public Vector2 GetSize(RichTextParser parser)
        {
            return Vector2.Zero;
        }

    }
}
