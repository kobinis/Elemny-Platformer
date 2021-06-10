using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PaintPlay
{
    class WoodProfile : PixelProfile
    {
        MCGA tile;        

        public WoodProfile()
        {
            Texture2D tileTexture = MyGraphics.GetTexture("wood");
            tile = new MCGA(tileTexture.Width, tileTexture.Height);
            tile.SetData( tileTexture);            
        }

        public override void Init(int lastX, int lastY, int x, int y, Color color, GPixel[,] grid)
        {
            GPixel pixel = new GPixel();
            pixel.type = PixelType.WoodWall;
            pixel.value = 0; //Burn time
            pixel.param1 = 180;//Burn value 
            pixel.param2 = 280;//Burnout value
            pixel.color = tile.Getpixel(x % tile.Width, y % tile.Height);

            for (int xx = -2; xx <= 2; xx++) // replace with 
            {
                for (int yy = -2; yy <= 2; yy++)
                {
                    float colRand = 0.5f+(float)MyMath.rand.NextDouble() * 0.5f;
                    Color col = new Color();
                    col = color;
                    Vector3  colVec = col.ToVector3()*colRand;
                    col = new Color(colVec);
                    //pixel.color = col;
                    GardenHelper.Line(lastX + xx, lastY + yy, x + xx, y + yy, pixel, grid);
                }

            }
        }

        public override void Logic(int x, int y, GPixel[,] grid)
        {

        }

        public override void Burn(int burnValue, ref GPixel sourcePixel, ref GPixel data)
        {            
            data.value--;
            if (data.value < 0)
            {
                data.type = PixelType.Fire;
                data.value = data.param2;
            }
            else
            {
                sourcePixel.value = data.param1;
            }

        }

        //public override void Draw(int x, int y, GPixel pixel, MCGA mcga)
        //{
            
        //}

        //public override void Draw(int x, int y, GPixel[,] grid, MCGA mcga)
        //{
        //    //mcga.Putpixel(x, y, grid[x, y].color);
        //    mcga.Putpixel(x, y, tile.Getpixel(x % tile.Width, y % tile.Height));
        //}

        public override bool IsSolid(GPixel data)
        {
            return true;
        }

    }
}
