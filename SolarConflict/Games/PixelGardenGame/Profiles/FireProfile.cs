using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Activities.Games;
using SolarConflict.Games.PixelGardenGame;

namespace PaintPlay
{
    /// <summary>
    /// Fire - normal red fire, Burn some things according to thire burn function
    /// Params:
    /// value - burn value
    /// </summary>
    class FireProfile:PixelProfile
    {
        public static Color[] palette;
        public FireProfile()
        {            
           palette = new Color[256];
           byte r, g, b;
           for (int i = 0; i < 256; i++)
           {
               r = (byte)Math.Min(i * 4, 255);
               g = (byte)Math.Max(Math.Min((i - 64) * 4, 255), 0);
               b = (byte)Math.Max(Math.Min((i - 64 * 2) * 4, 255), 0);
               palette[i] = new Color(r, g, b , Math.Min( i * 4, 255));
               //palette[i] = new Color(r, g, b).PackedValue;
           }
        }

        public override void Init(int lastX, int lastY, int x, int y, Color color, GPixel[,] grid)
        {
            GPixel pixel = new GPixel();
            pixel.type = PixelType.Fire;
            pixel.value = 300;            
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
            int value = Math.Max(grid[x, y].value - 1, 0);
            grid[x, y].value = value;
            if (value > 1) // or 0
            {
                int dirVec = MyMath.rand.Next(16);
                if ((dirVec & 1) > 0)
                {
                    GPixel.profiles[(int)grid[x + 1, y].type].Burn(value, ref grid[x, y], ref grid[x + 1, y]);                    
                }

                if ((dirVec & 2) > 0)
                {
                    GPixel.profiles[(int)grid[x - 1, y].type].Burn(value, ref grid[x, y], ref grid[x - 1, y]);                    
                }

                if ((dirVec & 4) > 0)
                {
                    GPixel.profiles[(int)grid[x, y + 1].type].Burn(value, ref grid[x, y], ref grid[x, y + 1]);                                        
                }

                if ((dirVec & 8) > 0)
                {
                    GPixel.profiles[(int)grid[x, y - 1].type].Burn(value, ref grid[x, y], ref grid[x, y - 1]);                                                            
                }
            }
            else
            {
                grid[x, y].type = PixelType.Empty;            
            }                        
        }


        public override void LogicEdge(int x, int y, PixelGardenEngine logicGrid)
        {
            GPixel pixel = logicGrid.GetPixelLim(x, y);
            int value = pixel.value - 1; //Math.Max(grid[x, y].value - 1, 0);
            pixel.value = value;
            if (value > 1) // or 0
            {
                int dirVec = MyMath.rand.Next(16);
                if ((dirVec & 1) > 0)
                {
                    var destPixel = logicGrid.GetPixelLim(x + 1, y);
                    GPixel.profiles[(int)destPixel.type].Burn(value, ref pixel, ref destPixel);
                }

                if ((dirVec & 2) > 0)
                {
                    var destPixel = logicGrid.GetPixelLim(x - 1, y);
                    GPixel.profiles[(int)destPixel.type].Burn(value, ref pixel, ref destPixel);
                }

                if ((dirVec & 4) > 0)
                {
                    var destPixel = logicGrid.GetPixelLim(x, y + 1);
                    GPixel.profiles[(int)destPixel.type].Burn(value, ref pixel, ref destPixel);
                }

                if ((dirVec & 8) > 0)
                {
                    var destPixel = logicGrid.GetPixelLim(x, y - 1);
                    GPixel.profiles[(int)destPixel.type].Burn(value, ref pixel, ref destPixel);
                }
            }
            else
            {
                pixel.type = PixelType.Empty;
            }

        }

        public override void Draw(int x, int y, GPixel pixel, MCGA mcga)
        {
            mcga.Putpixel(x, y, palette[Math.Max(Math.Min(pixel.value, 255), 0)]);
        }


        public override void Draw(int x, int y, GPixel[,] grid, MCGA mcga)
        {
            mcga.Putpixel(x, y, palette[Math.Max( Math.Min(grid[x, y].value, 255),0)]);            
        }

        public override void Burn(int burnValue, ref GPixel sourcePixel, ref GPixel data)
        {            
            data.value = (burnValue + data.value) >> 1;
        }       

        public override bool IsSolid(GPixel data)
        {
            return false;
        }

        
    }
}
