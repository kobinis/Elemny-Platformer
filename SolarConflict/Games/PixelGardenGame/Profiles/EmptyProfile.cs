using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PaintPlay
{
    /// <summary>
    /// Empty - Empty Space, defalt burn defalt explode, solid
    /// Logic and draw are never called - can be implemented as the base class
    /// </summary>
    class EmptyProfile : PixelProfile
    {
        public override void Init(int lastX, int lastY, int x, int y, Color color, GPixel[,] grid)
        {         
        }        

        public override void Logic(int x, int y, GPixel[,] grid)
        {
            //throw exeption
        }

        public override void Draw(int x, int y, GPixel[,] grid, MCGA mcga)
        {
            //throw exeption
        }

        public override bool IsSolid(GPixel data)
        {
            return false;
        }

        public override void ColorBurn(int burnValue, ref GPixel sourcePixel, ref GPixel data)
        {
            data.type = PixelType.ColoredFire;
            data.color = sourcePixel.color;
            data.value = burnValue >> 1;
        }        
               
    }
}
