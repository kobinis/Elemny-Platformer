 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PaintPlay
{
    class WaterProfile : PixelProfile
    {
        int[] dirX;
        int[] dirY;
        MCGA tile;

        public WaterProfile()
        {
            dirX = new int[4] { -1, 1, 0, 0 };
            dirY = new int[4] { 0, 0, -1, 1 };
            Texture2D tileTexture = MyGraphics.GetTexture("watertex");
            tile = new MCGA(tileTexture.Width, tileTexture.Height);
            tile.SetData(tileTexture); 


        }

        public override void Init(int lastX, int lastY, int x, int y, Color color, GPixel[,] grid)
        {
            GPixel pixel = new GPixel();
            pixel.type = PixelType.Water;
            pixel.value = 1;
            pixel.param1 = 0;
            pixel.param2 = 0;
           // pixel.color = color;

            for (int xx = -2; xx <= 2; xx++) // replace with 
            {
                for (int yy = -2; yy <= 2; yy++)
                {
                    GardenHelper.Line(lastX + xx, lastY + yy, x + xx, y + yy, pixel, grid);
                }

            }
        }

        public override void Burn(int burnValue, ref GPixel sourcePixel, ref GPixel data)
        {
            sourcePixel.type = PixelType.Empty;
            sourcePixel.value /= 2;
        }

        public override void Logic(int x, int y, GPixel[,] grid)
        {
            //param1 vx
            //param2 vy

            bool move = false;

            if (grid[x - 1, y].type != PixelType.Empty)
                grid[x, y].param2++;
            if (grid[x + 1, y].type != PixelType.Empty)
                grid[x, y].param2--;

            if (grid[x, y - 1].type == PixelType.Water)
            {
                if (GardenLogic.time % 3 == 0)
                    grid[x, y].value = grid[x, y - 1].value + 1;
            }

            if (grid[x, y - 1].type != PixelType.Empty)
            {
                grid[x, y].param1++;
            }
            else
            {

                if (GardenLogic.time % 6 == 0)
                    grid[x, y].value = 0;
            }

            if (grid[x, y + 1].type != PixelType.Empty)
                grid[x, y].param1--;

            if (grid[x, y + 1].type == PixelType.Empty)
                grid[x, y].param1++;

            grid[x, y].param1 = Math.Min(Math.Max(grid[x, y].param1, -5), 5);
            grid[x, y].param2 = Math.Min(Math.Max(grid[x, y].param2, -5), 5);


            int dx;
            int dy;

            int dir = MyMath.rand.Next(4);
            if (grid[x + dirX[dir], y + dirY[dir]].type == PixelType.Water && (grid[x + dirX[dir], y + dirY[dir]].value - grid[x, y].value) > 1)
            {
                grid[x + dirX[dir], y + dirY[dir]].value = grid[x, y].value;
               //  int val = (grid[x + dirX[dir], y + dirY[dir]].value = grid[x, y].value)/2;
             //    grid[x + dirX[dir], y + dirY[dir]].value = val;
               //  grid[x, y].value = val;
               /* grid[x, y].value += grid[x + dirX[dir], y + dirY[dir]].value;
                grid[x, y].param1 += dirX[dir];
                grid[x, y].param2 += dirY[dir];
                grid[x + dirX[dir], y + dirY[dir]] = grid[x, y];
                grid[x, y].type = PixelType.Empty;
                move = true;*/
            }
            
                dx = Math.Min(Math.Max((MyMath.rand.Next(Math.Abs(grid[x, y].param2))) * Math.Sign(grid[x, y].param2), -x), grid.GetLength(0) - x - 1);
                for (int i = Math.Abs(grid[x, y].param1); i > 0; i--)
                {
                    dy = Math.Min(Math.Max(i * Math.Sign(grid[x, y].param1), -y), grid.GetLength(1) - y - 1);
                    if (grid[x + dx, y + dy].type == PixelType.Empty) // Down
                    {
                        grid[x + dx, y + dy] = grid[x, y];
                        grid[x, y].type = PixelType.Empty;
                        move = true;
                        break;
                    }
                }

                if (!move)
                {
                    for (int i = Math.Abs(grid[x, y].param2); i > 0; i--)
                    {
                        dx = Math.Min(Math.Max(i * Math.Sign(grid[x, y].param2), -x), grid.GetLength(0) - x - 1);
                        if (grid[x + dx, y].type == PixelType.Empty) // Down
                        {
                            grid[x + dx, y] = grid[x, y];
                            grid[x, y].type = PixelType.Empty;
                            move = true;
                            break;
                        }
                    }
                }
            



        }

        public override void Draw(int x, int y, GPixel pixel, MCGA mcga)
        {
            
            //byte col = (byte)(Math.Min(grid[x, y].value*4,255));                       
            //mcga.Putpixel(x, y, new Color(0, col, 255, 100).PackedValue);
            int dx = (int)Math.Round( Math.Cos(GardenLogic.time*0.1+y*0.2) * 5 + 5);
            int dy = (int)Math.Round(Math.Sin(GardenLogic.time*0.09+x*0.2) * 5 + 5);
            Color col = tile.Getpixel((x + dx) % tile.Width, (y + dy) % tile.Height);
            col.A = 200;
            mcga.Putpixel(x, y, col);                        
        }
       

        public override bool IsSolid(GPixel data)
        {
            return false;
        }
    }
}

