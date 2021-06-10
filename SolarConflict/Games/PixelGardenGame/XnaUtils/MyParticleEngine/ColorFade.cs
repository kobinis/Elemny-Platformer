using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PaintPlay.XnaUtils
{
    class ColorFade : ColorUpdater
    {
        public override void Update(ref Color color, float nLifetime)
        {
            color.A = (byte)MathHelper.Lerp(100,0, nLifetime);
        }

    }
}
