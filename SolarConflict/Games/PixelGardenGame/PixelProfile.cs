using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Activities.Games;
using SolarConflict.Games.PixelGardenGame;

namespace PaintPlay
{
    public abstract class PixelProfile
    {
        
        public abstract void Logic(int x, int y, GPixel[,] grid);

        public virtual void LogicEdge(int x, int y, PixelGardenEngine logicGrid)
        {

        }

        public virtual void Init(int lastX, int lastY, int x, int y, Color color, GPixel[,] grid)
        {
        }

        public virtual void Draw(int x, int y, GPixel pixel, MCGA mcga)
        {
            mcga.Putpixel(x, y, pixel.color);
        }

        public virtual void Draw(int x, int y, GPixel[,] grid, MCGA mcga)
        {
            mcga.Putpixel(x, y, grid[x, y].color);
        }
        
        // DrawNormalColors

        public virtual void Explode(int burnValue, ref GPixel sourcePixel, ref GPixel data)
        {
            data.type = PixelType.ColoredFire;
            data.value = burnValue >> 1;
            data.color = sourcePixel.color; //?
        }

        public virtual void Burn(int burnValue,ref GPixel sourcePixel, ref GPixel data)
        {
            data.type = PixelType.Fire;
            data.value = burnValue >> 1;
        }

        public virtual void ColorBurn(int burnValue, ref GPixel sourcePixel, ref GPixel data)
        {
            Burn(burnValue, ref sourcePixel, ref data);
            /*data.type = PixelType.ColoredFire;
            data.color = sourcePixel.color;
            data.value = burnValue >> 1;*/
        }

        public virtual bool IsSolid(GPixel data)
        {
            return false;
        }

        public virtual void Collepse(ref GPixel data)
        {

        }

    }
}
