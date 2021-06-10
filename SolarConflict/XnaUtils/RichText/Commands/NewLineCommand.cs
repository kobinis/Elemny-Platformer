using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaUtils.XnaUtils.RichText.Commands
{
    [Serializable]
    class NewLineCommand : ITextElement
    {
        public const string Command = "nl";

        public void Draw(SpriteBatch spriteBatch, RichTextParser parser, Vector2 position, Color? color)
        {
            //parser.CurrentPosition = new Vector2(0, parser.CurrentPosition.Y + parser.CurrentHeight);
            GetSize(parser);
        }

        public Vector2 GetSize(RichTextParser parser)
        {
            parser.CurrentPosition = new Vector2(0, parser.CurrentPosition.Y + parser.CurrentHeight);
            parser.CurrentHeight = 0; //LineSpacing
            return Vector2.UnitY * parser.CurrentHeight;
        }

        public void ParseParameters(string paramaters)
        {            
        }
    }
}
