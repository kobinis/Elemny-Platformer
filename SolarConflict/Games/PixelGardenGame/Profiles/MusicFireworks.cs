using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace PaintPlay
{
    class MusicFireworks:PixelProfile
    {
         PixelType type; //change

         int count;
        FlowerProfile flowerProfile;
        SoundEffect[] effects;

        public MusicFireworks()
        {
                    
            type = PixelType.MusicFireworks; // change

            flowerProfile = new FlowerProfile();
            effects = new SoundEffect[16];
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i] = SoundEngine.GetSoundEffect((i + 1).ToString());
            }

            
            
        }

        public override void Init(int lastX, int lastY, int x, int y, Color color, GPixel[,] grid)
        {
            //add bounds check
            if (x < 0 || y < 0 || x >= grid.GetLength(0) || y >= grid.GetLength(1))
                return;
            count++;
           // color = Painter.palette[(count * 10) % Painter.palette.Length];
            grid[x, y].type = type;
            grid[x, y].value = 0; //from the colors
            grid[x, y].color = color;
            grid[x, y].param1 = 15 + (count % effects.Length) * 20;
        }

        public override void Logic(int x, int y, GPixel[,] grid)
        {

            if (grid[x, y].value > 0)
            {
                if (grid[x,y].value<grid[x,y].param1)
                {
                    if(MyMath.rand.Next(4)!=0)
                    {
                        if (!grid[x, y - 1].IsSolid())
                        {
                            grid[x, y].value++;
                            grid[x, y - 1] = grid[x, y];
                            grid[x, y].type = PixelType.Fire;
                            grid[x, y].value = 600;
                        }
                        else
                        {
                            grid[x, y].type = PixelType.ColoredFire;
                            grid[x, y].value = 200;
                        }
                    }
                }
                else
                {
                    grid[x, y].type = PixelType.Tnt;
                    grid[x, y].value = 1;
                    grid[x, y].param1 = 0; //0 - explosive, 10 - fuse
                    grid[x, y].param2 = 15; //20
                }

                

            }
            else
            {

                if (grid[x, y + 1].IsSolid() || grid[x - 1, y + 1].IsSolid() || grid[x + 1, y + 1].IsSolid())
                {

                }
                else
                {
                    grid[x, y + 1] = grid[x, y];
                    grid[x, y].type = PixelType.Empty;
                }

            }



        }

        

        public override void Draw(int x, int y, GPixel pixel, MCGA mcga)
        {
            if (pixel.value == 0)
            {
                mcga.Putpixel(x, y, pixel.color);
                mcga.Putpixel(x-1, y, pixel.color);
                mcga.Putpixel(x-1, y-1, pixel.color);
                mcga.Putpixel(x, y-1, pixel.color);                              
            }
            else
            {
                mcga.Putpixel(x, y, pixel.color);
            }
        }

        public override void Burn(int burnValue, ref GPixel sourcePixel, ref GPixel data)
        {
            if (data.value == 0)
            {
                int index =  Math.Max((data.param1-15)/20,0) % effects.Length;
                SoundEngine.AddSoundToQue(effects[index]);
                data.value = 1;
            }
        } 
       

        public override bool IsSolid(GPixel data)
        {
            return true;
        }
    }
    }

