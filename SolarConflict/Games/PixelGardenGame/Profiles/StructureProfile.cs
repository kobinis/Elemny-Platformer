using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace PaintPlay
{
    class StructureProfile : PixelProfile
    {
        public override void Logic(int x, int y, GPixel[,] grid)
        {
            //grid[0,0].value 
            
            //energy
            //grid[x, y + 1].IsSolid() || grid[x - 1, y + 1].IsSolid() || grid[x + 1, y + 1].IsSolid()
            if (grid[x, y].value == 0)
            {
                if (GPixel.profiles[(int)grid[x - 1, y + 1].type].IsSolid(grid[x - 1, y + 1])
                  || GPixel.profiles[(int)grid[x + 1, y + 1].type].IsSolid(grid[x + 2, y + 1]))
                {

                }
                else
                {
                    if (!GPixel.profiles[(int)grid[x, y + 1].type].IsSolid(grid[x, y + 1]))
                    {
                        grid[x, y].value = grid[x, y].value + 1; ;
                        grid[x, y + 1] = grid[x, y];
                        grid[x, y].type = PixelType.Empty;
                    }
                }
            }
            else
            {
                if (!GPixel.profiles[(int)grid[x, y + 1].type].IsSolid(grid[x, y + 1]))
                {
                    if (MyMath.rand.Next(5) == 0)
                    {
                        GPixel.profiles[(int)grid[x, y - 2].type].Collepse(ref grid[x, y - 2]);   
                    }
                    grid[x, y].value = grid[x, y].value + 1;
                    grid[x, y].type = PixelType.Snow; 
                    grid[x, y + 1] = grid[x, y];
                    grid[x, y].type = PixelType.Empty;
                }
            }
        }

        public override bool IsSolid(GPixel data)
        {
            return true;
        }

        public override void Collepse(ref GPixel data)
        {
            data.type = PixelType.Snow;
        }

        public override void Init(int lastX, int lastY, int x, int y, Color color, GPixel[,] grid)
        {
            GPixel pixel = new GPixel();
            pixel.type = PixelType.Structure;
            pixel.color = color;
            pixel.value = 0;

           // pixel.color = color;

            for (int xx = -2; xx <= 2; xx++) // replace with 
            {
                for (int yy = -2; yy <= 2; yy++)
                {
                    float colRand = 0.5f + (float)MyMath.rand.NextDouble() * 0.5f;
                    Color col = new Color();
                    col = color;
                    Vector3 colVec = col.ToVector3() * colRand;
                    col = new Color(colVec);
                   // pixel.color = col;
                    
                    GardenHelper.Line(lastX + xx, lastY + yy, x + xx, y + yy, pixel, grid);
                }

            }

        }
    }
}
