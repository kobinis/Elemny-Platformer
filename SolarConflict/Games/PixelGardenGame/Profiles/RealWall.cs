using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PaintPlay
{
    class RealWall:PixelProfile
    {

        public override void Init(int lastX, int lastY, int x, int y, Color color, GPixel[,] grid)
        {
            grid[x, y].type = PixelType.RealWall;
            grid[x, y].value = 6;
        }

        public override void Logic(int x, int y, GPixel[,] grid)
        {
            grid[x, y].value--;
            if (grid[x, y].value < 0)
                grid[x, y].type = PixelType.Empty;
        }


        public override void Burn(int burnValue, ref GPixel sourcePixel, ref GPixel data)
        {
            sourcePixel.value = 300;
            //base.Burn(burnValue, ref sourcePixel, ref data);
        }

        public override bool IsSolid(GPixel data)
        {
            return true;
        }
    }
}
