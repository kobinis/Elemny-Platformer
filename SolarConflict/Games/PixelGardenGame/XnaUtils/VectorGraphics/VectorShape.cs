using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PaintPlay.XnaUtils.VectorGraphics
{
    delegate void VectorPixel(float x, float y, float z, Color color);

    class VectorShape
    {
        Norma shape;
        Vector2 size;
        VectorColor vColor;
        
        VectorTransformation transformation;


        public VectorShape(Norma shape, Vector2 size, VectorTransformation transformation)
        {
            this.shape = shape;
            this.size = size;
            this.transformation = transformation;
            vColor = new VectorColor();
        }

        public VectorShape(Norma shape, Vector2 size):this(shape,size, new VectorTransformation())
        {
        }

        private int GetSizeX()
        {
            return (int)(size.X * transformation.Scale.X);
        }

        private int GetSizeY()
        {
            return (int)(size.Y * transformation.Scale.Y);
        } 

        private Color GetColor(float x, float y)
        {
            float value = shape(x / GetSizeX(), y / GetSizeY());
            float alpha = vColor.A;
            return new Color(value*vColor.R, value*vColor.G,vColor.B, alpha);
        }

        public void Render(VectorPixel pixelFunc)
        {

            for (int x = -GetSizeX(); x <= GetSizeX(); x++)
            {
                for (int y = -GetSizeX(); y <= GetSizeX(); y++)
                {
                    Color color = GetColor(x, y);
                    pixelFunc(x, y, 1f, Color.White);
                }                
            }
        }


        public Texture2D MakeTexture()
        {
            MCGA mcga = new MCGA(GetSizeX() * 2 + 1, GetSizeY() * 2 + 1);
            for (int x = -GetSizeX(); x <= GetSizeX(); x++)
            {
                for (int y = -GetSizeX(); y <= GetSizeX(); y++)
                {
                    Color color = GetColor(x, y);
                    mcga.PutpixelOn(x + GetSizeX(), y + GetSizeY(), color);
                }
            }

            return mcga.mcgaTexture;

        }

        
    }
}
