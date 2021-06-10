using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PaintPlay
{
    /// <summary>
    /// Wall - Dose not fall, blocks the fall of things
    /// Solid, Will not Burn,
    /// </summary>
    class WallProfile : PixelProfile
    {
        public override void Init(int lastX, int lastY, int x, int y, Color color, GPixel[,] grid)
        {
            for (int i = 0; i < 9; i++)
            {
                grid[x + i / 3, y + i % 3].type = PixelType.Wall;
                grid[x + i / 3, y + i % 3].value = 0;
                float colRand = (float)MyMath.rand.NextDouble() * 0.5f;
               // grid[x + i / 3, y + i % 3].color = color;
            }
        }

        public override void Logic(int x, int y, GPixel[,] grid)
        {

        }

        //public override void Draw(int x, int y, GPixel[,] grid, MCGA mcga)
        //{
        //    //mcga.Putpixel(x, y, grid[x,y].color);           
        //}
      

        public override bool IsSolid(GPixel data)
        {
            return true;
        }
        
    }
}
