using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PaintPlay
{
    class FlowerProfile : PixelProfile 
    {
        //value - groth
        //param1 - maxSize
        //param2 - flower type (texture) & morph type (what to turn to?)
        // param3 - rotation , rodates when moves
        Texture2D[] textures;
        Vector2 origin;

        public FlowerProfile()
        {
            textures =  new Texture2D[7];
            textures[0] = MyGraphics.GetTexture("flower1");
            textures[1] = MyGraphics.GetTexture("flower2");
            textures[2] = MyGraphics.GetTexture("flower3");
            textures[3] = MyGraphics.GetTexture("flower4");
            textures[4] = MyGraphics.GetTexture("flower5");
            textures[5] = MyGraphics.GetTexture("flower6");
            textures[6] = MyGraphics.GetTexture("flower7");            
        }

        public override void Init(int lastX, int lastY, int x, int y, Color color, GPixel[,] grid)
        {
            grid[x, y].type = PixelType.Flower; //add try
            grid[x, y].value = 0;
            grid[x, y].param1 = 60 + MyMath.rand.Next(100);
            grid[x, y].param2 = MyMath.rand.Next(textures.Length);
            
        }

        public override void Logic(int x, int y, GPixel[,] grid)
        {

            if (grid[x, y + 1].IsSolid() || grid[x - 1, y + 1].IsSolid() || grid[x + 1, y + 1].IsSolid())
            {
                grid[x, y].value = Math.Min(grid[x, y].value + 1, grid[x, y].param1);                
            }
            else           
            {
                grid[x, y + 1] = grid[x, y];
                grid[x, y].type = PixelType.Empty;
            }
        }



        public override void Draw(int x, int y, GPixel pixel, MCGA mcga)
        {
            //can be improved in speed
            origin = new Vector2(textures[pixel.param2].Width / 2, textures[pixel.param2].Height / 2);                       
            float rotHelp = (float)y / 100f;
            float rotation =  (x % 2) * rotHelp - (1- x % 2) * rotHelp;
            Vector2 pos = mcga.McgaToScreen(x,y);
            Rectangle rect =  new Rectangle((int)pos.X, (int)pos.Y, (int)(pixel.value*0.8) , (int)(pixel.value*0.8)); 
            MyGraphics.sb.Draw(textures[pixel.param2], rect, null, Color.White, rotation, origin , SpriteEffects.None, 0);            
            //MyGraphics.sb.Draw(textures[grid[x, y].param2], mcga.McgaToScreen(x,y), null, Color.White, rotation, origin ,scale, SpriteEffects.None, 0);            
        }

        public override void Burn(int burnValue, ref GPixel sourcePixel, ref GPixel data)
        {            
        } 


        public override bool IsSolid(GPixel data)
        {
            return true;
        }



    }
}
