using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict;
using XnaUtils.Graphics;
using System.Diagnostics;

namespace XnaUtils.XnaUtils.RichText.Commands
{
    [Serializable]
    internal class SizedImage : ITextElement
    {
        public const string Command = "simage";
        private Sprite sprite;
        private int width;
        private int height;

        public void Draw(SpriteBatch spriteBatch, RichTextParser parser, Vector2 position, Color? color)
        {
            color = Color.White;
            Rectangle rectangle = new Rectangle((int)(parser.CurrentPosition.X + position.X), (int)(parser.CurrentPosition.Y + position.Y), (int)width, (int)height);
            var size = new Vector2(width, height);
            spriteBatch.Draw(sprite.Texture, rectangle, color.Value);
            parser.CurrentHeight = Math.Max(parser.CurrentHeight, size.Y);
            parser.CurrentPosition += new Vector2(size.X, 0);            
        }

        /// <remarks>The current size, which is based on previously-parsed stuff. If nothing parsed or value is otherwise zero, defaults to a value
        /// based on the size of the current font's default character.</remarks>        
        public Vector2 GetSize(RichTextParser parser)
        {
            Vector2 size = new Vector2(width, height);
            parser.CurrentHeight = Math.Max(parser.CurrentHeight, size.Y);
            parser.CurrentPosition += new Vector2(size.X, 0);
            return size;
        }

        public void ParseParameters(string parameters)
        {
            int maxWidth = 0;
            int maxHeight = 0;
            var split = parameters.Split(',').ToArray();            
            sprite = Sprite.Get(split[0]);            
            if (split.Length > 2)
            {
                maxWidth = int.Parse(split[1]);
                maxHeight = int.Parse(split[2]);
            }
            height = sprite.Height;
            width = sprite.Width;
            if (maxWidth > 0)
            {
                float scale = Math.Min(FMath.FindScale(new Vector2(sprite.Width, sprite.Height), new Vector2(maxWidth, maxHeight)), 1);
                width = (int)(sprite.Width * scale);
                height = (int)(sprite.Height * scale);
            }

        }
    }
}

