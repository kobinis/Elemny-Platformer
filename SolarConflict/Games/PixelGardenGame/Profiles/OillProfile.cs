using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PaintPlay
{
    class OillProfile : PixelProfile
    {
        int[] dirX;
        int[] dirY;

        public OillProfile()
        {
            dirX = new int[4] { -1, 1, 0, 0 };
            dirY = new int[4] { 0, 0, -1, 1 };
        }

        public override void Init(int lastX, int lastY, int x, int y, Color color, GPixel[,] grid)        
        {
            GPixel pixel = new GPixel();
            pixel.type = PixelType.Oill;
            pixel.value = 35;
            pixel.param1 = 0;
            pixel.param2 = 0;
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
            //param1 vx
            //param2 vy

            bool move = false;

            if (grid[x - 1, y].type != PixelType.Empty)
                grid[x, y].param2++;
            if (grid[x + 1, y].type != PixelType.Empty)
                grid[x, y].param2--;

          /*  if (grid[x, y - 1].type == PixelType.Oill)
            {
                if (PixelGarden.time % 3 == 0)
                    grid[x, y].value = grid[x, y - 1].value + 1;
            }*/

            if (grid[x, y - 1].type != PixelType.Empty)
            {
                grid[x, y].param1++;
            }
            else
            {

              /*  if(PixelGarden.time % 6 ==0)
                    grid[x, y].value = 0;*/
            }

            if (grid[x, y + 1].type != PixelType.Empty)
                grid[x, y].param1--;

            if (grid[x, y + 1].type == PixelType.Empty)
                grid[x, y].param1++;

            grid[x, y].param1 = Math.Min(Math.Max(grid[x, y].param1, -5), 5);
            grid[x, y].param2 = Math.Min(Math.Max(grid[x, y].param2, -5), 5);

            
            int dx;
            int dy;
         


            /*  int dir = MyMath.rand.Next(4);
            if (grid[x + dirX[dir], y + dirY[dir]].type == PixelType.Water && (grid[x + dirX[dir], y + dirY[dir]].value - grid[x, y].value) > 1)
            {
                grid[x + dirX[dir], y + dirY[dir]].value = grid[x, y].value;
                // int val = (grid[x + dirX[dir], y + dirY[dir]].value = grid[x, y].value)/2;
                // grid[x + dirX[dir], y + dirY[dir]].value = val;
                // grid[x, y].value = val;
                /*grid[x, y].value += grid[x + dirX[dir], y + dirY[dir]].value;
                grid[x, y].param1 += dirX[dir];
                grid[x, y].param2 += dirY[dir];
                grid[x + dirX[dir], y + dirY[dir]] = grid[x, y];
                grid[x, y].type = PixelType.Empty;
                move = true;
            }*/

            //if(grid[x+dx,y].type == PixelType.Oill && Math.Abs(grid[x+dx,y
            /*
            if (grid[x + dirX[dir], y + dirY[dir]].type == PixelType.Oill && (grid[x + dirX[dir], y + dirY[dir]].value - grid[x, y].value)>1 )
            {
                grid[x + dirX[dir], y + dirY[dir]].value = grid[x, y].value;
                
            }*/

            if (!move)
            {
                dx = Math.Min(Math.Max((MyMath.rand.Next(Math.Abs(grid[x, y].param2))) * Math.Sign(grid[x, y].param2), -x), grid.GetLength(0) - x - 1);
                for (int i = Math.Abs(grid[x, y].param1); i > 0; i--)
                {
                    dy = Math.Min(Math.Max(i * Math.Sign(grid[x, y].param1), -y), grid.GetLength(1) - y - 1);
                    if (!grid[x + dx, y + dy].IsSolid() && grid[x + dx, y + dy].type != PixelType.Oill) // Down
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
        }

        public override void Burn(int burnValue, ref GPixel sourcePixel, ref GPixel data)
        {
            data.value--;
            if (data.value < 0)
            {
                data.type = PixelType.Fire;
                data.value = 500;
            }
            else
            {
                sourcePixel.value = 300;
            }
        }

      

        public override void Draw(int x, int y, GPixel pixel, MCGA mcga)
        {
            //mcga.Putpixel(x, y, grid[x, y].color);
            byte col = (byte)(pixel.value);
            mcga.Putpixel(x, y, new Color(pixel.value*3, pixel.value/2, 200, 200));
        }


        public override bool IsSolid(GPixel data)
        {
            return false;
        }
    }
}

