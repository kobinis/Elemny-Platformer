using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils.XnaUtils.RichText;
using XnaUtils.Graphics;

namespace XnaUtils.XnaUtils.RichText.Commands
{
    [Serializable]
    class SeperatorCommand : ITextElement
    {
        public const string Command = "line";

        private Sprite sprite;
        private int height;

        public SeperatorCommand()
        {
            sprite = TextureBank.Inst.GetSprite("sperator");
            height = sprite.Height*2;
        }

        public Vector2 GetSize(RichTextParser parser)
        {
            parser.CurrentPosition = new Vector2(0, parser.CurrentPosition.Y + height);
           // parser.CurrentHeight = height;
            return Vector2.UnitY * height;
        }

        public void Draw(SpriteBatch spriteBatch, RichTextParser parser, Vector2 position, Color? color)
        {
            Rectangle rect = new Rectangle((int)position.X, (int)(position.Y + parser.CurrentPosition.Y), (int)parser.MaxWidth, height);
            spriteBatch.Draw(sprite, rect, new Color(255,255,255,100));
            parser.CurrentPosition = new Vector2(0, parser.CurrentPosition.Y + height);
            //parser.CurrentHeight = height;

        }

        public void ParseParameters(string paramaters)
        {
           
        }
    }
}
