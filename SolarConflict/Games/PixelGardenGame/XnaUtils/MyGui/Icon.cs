using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PaintPlay.XnaUtils.Input;

namespace PaintPlay.XnaUtils.MyGui
{
    class Icon
    {
        Rectangle rect;
        Texture2D texture;
        int radX,radY;
        
        public Icon()
        {
            texture = null;
        }

        public Icon(Texture2D texture, int radX, int radY, float scale)
        {
            this.texture = texture;
            this.radX = (int)(radX*scale);
            this.radY = (int)(radY*scale);
        }

        public void Draw(Vector2 position)
        {
            MyGraphics.sb.Draw(texture, new Rectangle((int)position.X - radX + 1, (int)position.Y - radY, 2 * radX + 1, 2 * radY), Color.Black);
            MyGraphics.sb.Draw(texture, new Rectangle((int)position.X - radX-1, (int)position.Y - radY-1, 2 * radX, 2 * radY), Color.White);
            
        }
            
    }
}
