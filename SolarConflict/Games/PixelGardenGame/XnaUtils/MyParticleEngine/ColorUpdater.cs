using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PaintPlay.XnaUtils
{
    abstract class ColorUpdater
    {
        public abstract void Update(ref Color color, float nLifetime);
    }
}
