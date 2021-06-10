using Microsoft.Xna.Framework;
using PaintPlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.Games.PixelGardenGame.Profiles
{
    class ObjectProfile : PixelProfile
    {
        public override void Logic(int x, int y, GPixel[,] grid)
        {
            grid[x, y].value -= 1;
            if (grid[x, y].value == 0)
                grid[x, y].type = PixelType.Empty;
        }

        public override void Init(int lastX, int lastY, int x, int y, Color color, GPixel[,] grid)
        {
            GPixel pixel = new GPixel();
            pixel.type = PixelType.Object;
            pixel.value = 4;            
            pixel.color = color;

            for (int xx = -2; xx <= 2; xx++) // replace with 
            {
                for (int yy = -2; yy <= 2; yy++)
                {
                    GardenHelper.Line(lastX + xx, lastY + yy, x + xx, y + yy, pixel, grid);
                }

            }
        }


        public override bool IsSolid(GPixel data)
        {
            return false;
        }

        public override void Draw(int x, int y, GPixel pixel, MCGA mcga)
        {
            mcga.Putpixel(x, y, Color.Red);
        }

        public override void Burn(int burnValue, ref GPixel sourcePixel, ref GPixel data)
        {
            sourcePixel.value = 100;
        }


    }
}
