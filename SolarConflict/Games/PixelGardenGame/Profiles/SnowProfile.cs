using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PaintPlay
{
    class SnowProfile : PixelProfile
    {
        int initValue = 0;
        MCGA tile;
        public SnowProfile()
        {
            Texture2D tileTexture = MyGraphics.GetTexture("snowtex");
            tile = new MCGA(tileTexture.Width, tileTexture.Height);
            tile.SetData(tileTexture); 
        }

        public override void Init(int lastX, int lastY, int x, int y, Color color, GPixel[,] grid)
        {
          
            GPixel pixel = new GPixel();
            pixel.type = PixelType.Snow;
            pixel.value = initValue + MyMath.rand.Next(5);
           // pixel.color = color;

            /*
            float colRand = (float)MyMath.rand.NextDouble()*0.5f;
            pixel.color = (new Color(1f - colRand, 1f - colRand, 0.1f)).PackedValue;
            */

            

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
            if (grid[x, y + 1].type == PixelType.Empty && MyMath.rand.Next(2) > 0) // Down
            {
                grid[x, y].value = Math.Min(grid[x, y].value + 1, 255);
                grid[x, y + 1] = grid[x, y];
                grid[x, y].type = PixelType.Empty;
            }
            else
            {
                int dx = MyMath.rand.Next(2) * 2 - 1; //First Diagonal
                if (grid[x + dx, y + 1].IsEmpty() && grid[x, y].value > 1)
                {
                    grid[x, y].value -= 1;
                    grid[x + dx, y + 1] = grid[x, y];
                    grid[x, y].type = PixelType.Empty;
                }
                else
                {
                    dx = dx * -1;
                    if (grid[x + dx, y + 1].IsEmpty() && grid[x, y].value > 1) //Secound Diagonal
                    {
                        grid[x, y].value -= 1;
                        grid[x + dx, y + 1] = grid[x, y];
                        grid[x, y].type = PixelType.Empty;
                    }
                    else
                    {
                        if (grid[x, y].value > 2 && MyMath.rand.Next(5) == 0) //Vertical
                        {
                            dx = MyMath.rand.Next(2) * 2 - 1;
                            if (grid[x + dx, y].IsEmpty())
                            {
                                grid[x, y].value -= 2;
                                grid[x + dx, y] = grid[x, y];
                                grid[x, y].type = PixelType.Empty;
                            }
                        }
                    }

                }
            }



        }

        //public override void Draw(int x, int y, GPixel[,] grid, MCGA mcga)
        //{
        //    //mcga.Putpixel(x, y, grid[x, y].color);
        //   // byte col = (byte)(grid[x, y].value / 2);
            
        //   // mcga.Putpixel(x, y, grid[x,y].color);
        //    mcga.Putpixel(x, y, tile.Getpixel(x % tile.Width, y % tile.Height));
        //}
      

        public override bool IsSolid(GPixel data)
        {
            return true;
        }
    }
}
