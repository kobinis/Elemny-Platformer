using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaUtils.XnaUtils.RichText.Commands
{
    [Serializable]
    internal class MaxWidthCommand : ITextElement //REFACTOR: maybe change to be a partial inner class of RichText and change current color to private
    {
        public const string Command = "width";

        private int _witdh;

        public MaxWidthCommand() { }
        

        public void ParseParameters(string paramaters)
        {
            _witdh = ParserUtils.ParseInt(paramaters);
        }

        public void Draw(SpriteBatch spriteBatch, RichTextParser parser, Vector2 position, Color? color)
        {
            parser.MaxLineWidth = _witdh;            
        }

        public Vector2 GetSize(RichTextParser parser)
        {
            parser.MaxLineWidth = _witdh;
            return Vector2.Zero;
        }        
    }
}
