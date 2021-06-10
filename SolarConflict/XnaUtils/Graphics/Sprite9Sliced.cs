using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace XnaUtils.Graphics
{
    [Serializable]
    public class Sprite9Sliced:Sprite
    {
        Rectangle[] _rectangles;
        int _margine = 5;

        public Sprite9Sliced(string id, int leftPadding, int rightPadding, int topPadding, int bottomPadding,  string normalID = null, int margine = 5) : base(id, normalID = null)
        {
            _margine = margine;
            _texture = TextureBank.Inst.GetTexture(_id);
            _rectangles = GraphicsUtils.Create9SlicePatches(new Rectangle(0, 0, _texture.Width, _texture.Height), leftPadding, rightPadding, topPadding, bottomPadding);            
        }

        public override void Draw(SpriteBatch batch, Rectangle destination, Color color)
        {
            destination.Inflate(_margine, _margine);
            GraphicsUtils.Draw9Slice(_texture, batch, destination, color, _rectangles);
        }


    }
}
