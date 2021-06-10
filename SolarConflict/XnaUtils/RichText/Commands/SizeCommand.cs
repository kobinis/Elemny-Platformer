using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using XnaUtils.XnaUtils.RichText;

namespace XnaUtils.XnaUtils.RichText.Commands
{
    [Serializable]
    internal class SizeCommand: ITextElement
    {
        public const string Command = "s";

        private Vector2 size;

        public void ParseParameters(string paramaters)
        {
            string[] values = paramaters.Split(',');
            if(values.Length >= 2)
            {
                size = new Vector2(ParserUtils.ParseFloat(values[0]), ParserUtils.ParseFloat(values[1]));
            }
            else
            {
                if (values.Length == 1)
                {
                    size = new Vector2(0,ParserUtils.ParseFloat(values[0]));
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, RichTextParser parser, Vector2 position, Color? color)
        {
            GetSize(parser);
        }

        public Vector2 GetSize(RichTextParser parser)
        {            
            parser.CurrentHeight = Math.Max(parser.CurrentHeight, size.Y);
            parser.CurrentPosition += new Vector2(size.X, 0);
            return size; 
        }

        
    }
}