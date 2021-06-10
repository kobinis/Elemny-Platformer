using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PaintPlay
{
    /// <summary>
    /// Fire - normal red fire, Burn some things according to thire burn function
    /// Params:
    /// value - burn value
    /// color - color
    /// </summary>
    class ColoredFireProfile : PixelProfile
    {
       
        public ColoredFireProfile()
        {
        
        }

        public override void Init(int lastX, int lastY, int x, int y, Color color, GPixel[,] grid)
        {
            GPixel pixel = new GPixel();
            pixel.type = PixelType.ColoredFire;
            pixel.value = 300;
            pixel.color = color;
            for (int xx = -2; xx <= 2; xx++) // replace with 
            {
                for (int yy = -2; yy <= 2; yy++)
                {
                    GardenHelper.Line(lastX + xx, lastY + yy, x + xx, y + yy, pixel, grid);
                }

            }

        }

        public override void Logic(int x, int y, GPixel[,] grid)
        {
            int value = Math.Max(grid[x, y].value - 1, 0);
            grid[x, y].value = value;
            if (value > 1) // or 0
            {
                int dirVec = MyMath.rand.Next(16);
                if ((dirVec & 1) > 0)
                {
                    GPixel.profiles[(int)grid[x + 1, y].type].ColorBurn(value, ref grid[x, y], ref grid[x + 1, y]);
                }

                if ((dirVec & 2) > 0)
                {
                    GPixel.profiles[(int)grid[x - 1, y].type].ColorBurn(value, ref grid[x, y], ref grid[x - 1, y]);
                }

                if ((dirVec & 4) > 0)
                {
                    GPixel.profiles[(int)grid[x, y + 1].type].ColorBurn(value, ref grid[x, y], ref grid[x, y + 1]);
                }

                if ((dirVec & 8) > 0)
                {
                    GPixel.profiles[(int)grid[x, y - 1].type].ColorBurn(value, ref grid[x, y], ref grid[x, y - 1]);
                }
            }
            else
            {
                grid[x, y].type = PixelType.Empty;
            }

        }

        public override void Draw(int x, int y, GPixel pixel, MCGA mcga)
        {
            float colMult = Math.Max(pixel.value, 0f) / 100f;
            Color color = pixel.color;
            Vector4 colVec = color.ToVector4();
            colVec *= colMult;
            color = new Color(colVec);


            mcga.Putpixel(x, y, color);
        }

        public override void Draw(int x, int y, GPixel[,] grid, MCGA mcga)
        {
             float colMult = Math.Max(grid[x, y].value, 0f) / 100f;
              Color color  = grid[x, y].color;                           
              Vector4 colVec = color.ToVector4();
              colVec *= colMult;
              color = new Color(colVec);

        
            mcga.Putpixel(x, y, color);
        }

        public override void ColorBurn(int burnValue, ref GPixel sourcePixel, ref GPixel data)
        {
            data.color = sourcePixel.color;
            data.value = (burnValue + data.value) >> 1;
        }

        public override void Burn(int burnValue, ref GPixel sourcePixel, ref GPixel data)
        {
            sourcePixel.type = PixelType.ColoredFire;
            sourcePixel.color = data.color;
            data.value = (burnValue + data.value) >> 1;
        }

        public override bool IsSolid(GPixel data)
        {
            return false;
        }


    }
}
