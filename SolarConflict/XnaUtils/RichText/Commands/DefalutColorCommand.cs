using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaUtils.XnaUtils.RichText.Commands
{
    [Serializable]
    class DefalutColorCommand : ITextElement
    {
        public const string Command = "dcolor";

        public void ParseParameters(string paramaters) 
        {
        }

        public void Draw(SpriteBatch spriteBatch, RichTextParser parser, Vector2 position, Color? color)
        {
            parser.CurrentColor = parser.DefaultColor;
        }

        public Vector2 GetSize(RichTextParser parser)
        {
            return Vector2.Zero;
        }
        
    }
}
