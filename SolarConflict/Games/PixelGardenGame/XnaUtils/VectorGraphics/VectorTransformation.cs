using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PaintPlay.XnaUtils.VectorGraphics
{
    class VectorTransformation
    {
        public VectorTransformation()
        {
            Rotation = 0;
            Translation = Vector2.Zero;
            Scale = Vector2.One;
        }

        public float Rotation { set; get; }
        public Vector2 Translation { set; get; }
        public Vector2 Scale { set; get; }

    }
}
