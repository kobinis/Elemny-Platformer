using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PaintPlay
{
    class TntProfile : PixelProfile
    {
        //value - explotion stage, 0 - not exploded, 1-start, 10- expoltion over;
        //param1 - delay time from burn to expotion

        float[] pX;
        float[] pY;        

        public TntProfile()
        {
            int pointNum = 9;
            pX = new float[pointNum];
            pY = new float[pointNum];
            for (int i = 0; i < pX.Length; i++)
            {
                pX[i] = (float)Math.Cos((double)i / pointNum * 2.0 * Math.PI);
                pY[i] = (float)Math.Sin((double)i / pointNum * 2.0 * Math.PI);
            }            
        }

        public override void Init(int lastX, int lastY, int x, int y, Color color, GPixel[,] grid)
        {
            GPixel pixel = new GPixel();
            pixel.type = PixelType.Tnt;
            pixel.value = 0;
            pixel.param1 = 0; //0 - explosive, 10 - fuse
            pixel.param2 = 20; //20
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
            
            if (grid[x, y].value > 0)
            {
                if (grid[x, y].value < grid[x,y].param2)
                {

                    if (grid[x, y].value > grid[x,y].param1)
                    {
                        double deg = MyMath.rand.NextDouble() * Math.PI * 2.0;
                        float cosVal = (float)Math.Cos(deg);
                        float sinVal = (float)Math.Sin(deg);
                        for (int i = 0; i < pX.Length; i++)
                        {
                            int posX = (int)(x + (grid[x, y].value-grid[x,y].param1)* (pX[i]*cosVal + pY[i]*sinVal) * 3.0);
                            int posY = (int)(y + (grid[x, y].value - grid[x, y].param1) * (pY[i] * cosVal - pX[i] * sinVal) * 3.0);
                            if (posX >= 0 && posX < grid.GetLength(0) && posY >= 0 && posY < grid.GetLength(1))
                                GPixel.profiles[(int)grid[posX, posY].type].Explode(300, ref grid[x,y], ref grid[posX, posY]); //200
                        }
                    }
                    grid[x, y].value++;
                }
                else
                {
                    grid[x, y].type = PixelType.ColoredFire;
                    grid[x, y].value = 200; //??
                }
            }            
        }

        //public override void Draw(int x, int y, GPixel[,] grid, MCGA mcga)
        //{
        //   mcga.Putpixel(x, y, grid[x,y].color);
        //}

        public override void Burn(int burnValue, ref GPixel sourcePixel, ref GPixel data)
        {
            data.value = Math.Max(data.value, 1);
        }

        public override void ColorBurn(int burnValue, ref GPixel sourcePixel, ref GPixel data)
        {
            data.value = Math.Max(data.value, 1);
        }

        public override bool IsSolid(GPixel data)
        {
            return true;
        }


    }
}
