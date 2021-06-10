 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PaintPlay
{
    class SandProfile : PixelProfile
    {
        int initValue = 0;
        MCGA tile;

        public SandProfile()
        {
            Texture2D tileTexture = MyGraphics.GetTexture("sandtex");
            tile = new MCGA(tileTexture.Width, tileTexture.Height);
            tile.SetData(tileTexture); 
        }

        public override void Init(int lastX, int lastY, int x, int y, Color color, GPixel[,] grid)
        {
            GPixel pixel = new GPixel();
            pixel.type = PixelType.Sand;
            pixel.value = initValue + MyMath.rand.Next(5);
           // pixel.color = color;



            //mcga.Putpixel(x, y, tile.Getpixel(x % tile.Width, y % tile.Height));
            pixel.color = color;

            for (int xx = -2; xx <= 2; xx++) // replace with 
            {
                for (int yy = -2; yy <= 2; yy++)
                {                    
                    float colRand = 0.5f + (float)MyMath.rand.NextDouble() * 0.5f;
                    Color col = new Color();
                    col = color;
                    Vector3 colVec = col.ToVector3() * colRand;
                    col = new Color(colVec);                    
                    pixel.color = col;
                    GardenHelper.Line(lastX + xx, lastY + yy, x + xx, y + yy, pixel, grid);
                }

            }
                        
        }

        public override void Logic(int x, int y, GPixel[,] grid)
        {            
            if (!grid[x, y + 1].IsSolid() && MyMath.rand.Next(10)!=0) // Down
            {               
                grid[x, y].value = Math.Min( grid[x, y].value + 1 , 255);
                GPixel temp = grid[x, y + 1];
                grid[x, y + 1] = grid[x, y];
                grid[x, y] = temp;
            }
            else
            {                
                int dx = MyMath.rand.Next(2) * 2 - 1; //First Diagonal                
                if (grid[x + dx, y + 1].IsEmpty() && grid[x,y].value>1 )
                {
                    grid[x, y].value -= 1;
                    grid[x + dx, y + 1] = grid[x, y];
                    grid[x, y].type = PixelType.Empty;
                }
                else
                {
                    dx = dx * -1;
                    if (grid[x + dx, y + 1].IsEmpty() && grid[x,y].value>1 ) //Secound Diagonal
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

        public override void Burn(int burnValue, ref GPixel sourcePixel, ref GPixel data)
        {           
        }

        //public override void Draw(int x, int y, GPixel[,] grid, MCGA mcga)
        //{
            
        //    mcga.Putpixel(x, y, tile.Getpixel(x % tile.Width, y % tile.Height));                
        //}

        public void NormalDraw(int x, int y, GPixel[,] grid, MCGA mcga)
        {
            mcga.Putpixel(x, y, tile.Getpixel(x % tile.Width, y % tile.Height));
        }

        public override bool IsSolid(GPixel data)
        {
            return true;
        }
    }
}
