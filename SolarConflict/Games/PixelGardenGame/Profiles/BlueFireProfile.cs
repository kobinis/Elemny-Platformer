using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PaintPlay
{
    /// <summary>
    /// BlueFire - Blue fire burns all things, dose not call the burn function
    /// !Think of behavior when burned by red fire
    /// Params:
    /// value - burn value
    /// </summary>
    class BlueFireProfile:PixelProfile
    {        
        public static Color[] palette;
        public BlueFireProfile()
        {
           palette = new Color[256];
           byte r, g, b; //remove

           for (int i = 0; i < 256; i++)
           {
               b = (byte)Math.Min(i * 4, 255);
               g = (byte)Math.Max(Math.Min((i - 64) * 4, 255), 0);
               r = (byte)Math.Max(Math.Min((i - 64 * 2) * 4, 255), 0);
               palette[i] = new Color(r, g, b , Math.Min( i * 4, 255));               
           }
        }

        public override void Init(int lastX, int lastY, int x, int y, Color color, GPixel[,] grid)
        {
            GPixel pixel = new GPixel();
            pixel.type = PixelType.BlueFire;
            pixel.value = 300;
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


            if (value > 1) // or 0
            {
                int dirVec = MyMath.rand.Next(16);
                if ((dirVec & 1) > 0)
                {
                    if (grid[x + 1, y].type == PixelType.BlueFire || grid[x + 1, y].type == PixelType.Empty)
                    {
                        grid[x + 1, y].type = PixelType.BlueFire;
                        grid[x + 1, y].value = (grid[x + 1, y].value + value) >> 1;
                    }
                }

                if ((dirVec & 2) > 0)
                {
                    if (grid[x - 1, y].type == PixelType.BlueFire || grid[x - 1, y].type == PixelType.Empty)
                    {
                        grid[x - 1, y].type = PixelType.BlueFire;
                        grid[x - 1, y].value = (grid[x - 1, y].value + value) >> 1;
                    }                    
                }

                if ((dirVec & 4) > 0)
                {
                    if (grid[x, y + 1].type == PixelType.BlueFire || grid[x, y + 1].type == PixelType.Empty)
                    {
                        grid[x, y + 1].type = PixelType.BlueFire;
                        grid[x, y + 1].value = (grid[x, y + 1].value + value) >> 1;
                    }
                }

                if ((dirVec & 8) > 0)
                {
                    if (grid[x, y - 1].type == PixelType.BlueFire || grid[x, y - 1].type == PixelType.Empty)
                    {
                        grid[x, y - 1].type = PixelType.BlueFire;
                        grid[x, y - 1].value = (grid[x, y - 1].value + value) >> 1;
                    }
                }
            }
            else
            {
                grid[x, y].type = PixelType.Empty;
            }
            grid[x, y].value = value;
        }
       
        //public override void Draw(int x, int y, GPixel[,] grid, MCGA mcga)
        //{
        //   // mcga.Putpixel(x, y, palette[Math.Max(Math.Min(grid[x, y].value, 255),0)]);            
        //}

        public override void Burn(int burnValue,ref GPixel sourcePixel, ref GPixel data)
        {
            data.value = (burnValue + data.value) >> 1;
        }

        public override bool IsSolid(GPixel data)
        {
            return false;
        }

        
    }
}

