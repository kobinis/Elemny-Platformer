using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PaintPlay
{
    class PlantProfile : PixelProfile
    {
        PixelType type; //change

        int[] dxArray;
        FlowerProfile flowerProfile;

        public PlantProfile()
        {
            dxArray = new int[52] { 0, -1, 0, -1, 0, 0, -1, 0, 0, 0, 0, -1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1,
                                    0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, -1, 0, 0, 0, 0, -1, 0, 0, -1, 0, -1};           
            type = PixelType.Plant; // change

            flowerProfile = new FlowerProfile();
        }

        public override void Init(int lastX, int lastY, int x, int y, Color color, GPixel[,] grid)
        {
            if (x < 0 || y < 0 || x >= grid.GetLength(0) || y >= grid.GetLength(1))
                return;
            grid[x, y].type = type;
            grid[x, y].value = MyMath.rand.Next(dxArray.Length);
           // grid[x, y].color = color;
        }

        public override void Logic(int x, int y, GPixel[,] grid)
        {
            if ( grid[x, y + 1].IsSolid() || grid[x - 1, y + 1].IsSolid() || grid[x + 1, y + 1].IsSolid() )
            {

                int dx = dxArray[grid[x, y].value % dxArray.Length];
                int dy = -1;

                if (grid[x + dx, y + dy].IsEmpty() && MyMath.rand.Next(5) == 0)
                {
                    if (grid[x, y].value > 120 && MyMath.rand.Next(25) == 0)
                    {
                        flowerProfile.Init(0,0, x + dx, y + dy, Microsoft.Xna.Framework.Color.White, grid);
                    }
                    else
                    {


                        if (grid[x + dx, y + dy].type == PixelType.Empty) // Down
                        {
                            grid[x + dx, y + dy] = grid[x, y];
                            if (dx == 0)
                            {
                                grid[x + dx, y + dy].value += MyMath.rand.Next(2);
                            }
                            else
                            {
                                grid[x + dx, y + dy].value++;
                            }

                            
                            /*if (MyMath.rand.Next(35) == 0)
                            {
                                grid[x + 1, y].type = PixelType.Plant;
                                dx = MyMath.rand.Next(2) * 2 - 1;
                                grid[x + dx, y].value = grid[x, y].value + MyMath.rand.Next(dxArray.Length);
                            }*/

                        }
                    }
                }
                
            }
            else
            {
                grid[x, y + 1] = grid[x, y];
                grid[x, y].type = PixelType.Empty;                
            }

        }

        public override void Draw(int x, int y, GPixel pixel, MCGA mcga)
        {

            byte rCol = (byte)(Math.Abs((pixel.value % 100) - 50) * 4);

            mcga.Putpixel(x, y, new Color(rCol, 240, 0));

         //   mcga.Putpixel(x + 1, y, (new Color(100, 100, 100, 20)).PackedValue);
        //    mcga.Putpixel(x - 1, y, (new Color(100, 100, 100, 20)).PackedValue);

            /*mcga.Putpixel(x, y, (new Color(0, 240, 0, 255)).PackedValue);
           
            mcga.Putpixel(x + 1, y, (new Color(0,90,0,100)).PackedValue);
            mcga.Putpixel(x - 1, y, (new Color(0, 110, 0, 100)).PackedValue);*/
            /*
            for (int i = -2; i <= 2; i++)
            {
                byte col = (byte)(240 - Math.Abs(i) * 35);
                mcga.Putpixel(x + i, y, (new Color(0, col, 0, 255 - i * i * 50)).PackedValue);
            }*/
             //MCGA.Putpixel(x, y, grid[x,y].color);
        }

        public override void Burn(int burnValue, ref GPixel sourcePixel, ref GPixel data)
        {
            data.type = PixelType.Fire;
            data.value = (burnValue + 200)>> 1;
        } 
       

        public override bool IsSolid(GPixel data)
        {
            return true;
        }
    }
}
