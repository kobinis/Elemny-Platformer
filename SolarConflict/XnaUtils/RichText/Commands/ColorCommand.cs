using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaUtils.XnaUtils.RichText.Commands
{
    [Serializable]
    internal class ColorCommand: ITextElement //REFACTOR: maybe change to be a partial inner class of RichText and change current color to private
    {
        public const string Command = "color";

        private Color _color;

        public ColorCommand()
        {
        }

        public ColorCommand(Color color)
        {
            _color = color;
        }

        public void ParseParameters(string paramaters)
        {
            _color = ColorCommand.ColorParse(paramaters);
        }

        public void Draw(SpriteBatch spriteBatch, RichTextParser parser, Vector2 position, Color? color)
        {
            parser.CurrentColor = _color;
        }

        public Vector2 GetSize(RichTextParser parser)
        {
            return Vector2.Zero;
        }

        public static Color ColorParse(string colorString) //TODO: move out
        {
            Color color = Color.White;
            string[] colors = colorString.Split(',');
            if (colors.Length < 3)
            {
                colors = colorString.Split(' ');
                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] = colors[i].Substring(colors[i].LastIndexOf(':') + 1);
                }
            }
            byte value;
            byte.TryParse(colors[0], out value);
            color.R = value;
            byte.TryParse(colors[1], out value);
            color.G = value;
            byte.TryParse(colors[2], out value);
            color.B = value;
            color.A = 255;
            if (colors.Length > 3)
            {                
                if (byte.TryParse(colors[3], out value))
                {
                    color.A = value;
                }
                
            }
            return color;
        }
    }
}
