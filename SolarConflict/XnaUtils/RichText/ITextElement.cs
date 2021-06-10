using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaUtils.XnaUtils.RichText
{
    public interface ITextElement
    {
        void ParseParameters(string paramaters);
        Vector2 GetSize(RichTextParser parser);
        void Draw(SpriteBatch spriteBatch, RichTextParser parser, Vector2 position, Color? color);        
    }

    public interface ISplittableTextElement : ITextElement {
        ITextElement[] Split(RichTextParser parser, float currentLineWidth, float lineWidth = -1f);
    }
}
