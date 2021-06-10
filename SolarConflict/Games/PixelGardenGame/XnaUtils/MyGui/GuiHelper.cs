using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PaintPlay;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PaintPlay.XnaUtils.MyGui
{
    enum FrameDesgin { Fade, Normal, Centerd, None }   

    struct GuiControlDesign 
    {
        public int frameNumber;
        public float colorGradient; //britnessGardinet 
        public float color; // britness
        public FrameDesgin frameDesign;

        public GuiControlDesign(int frameNumber, FrameDesgin design)
        {
            this.frameNumber = frameNumber;
            colorGradient= 1f; //britnessGardinet 
            color = 1f; // britness
            frameDesign = design;
        }
    }


    class GuiHelper
    {
       

        public static float FrameShade(float x, float y, FrameDesgin desgin) //recive an enum
        {
            double deg;
            switch (desgin)
            {

                case FrameDesgin.Fade:
                    deg = Math.Atan2(y, x) - Math.PI / 4.0;
                    return (float)Math.Cos(deg);   
                case FrameDesgin.Normal:                   
                    return (float)MyMath.DegSign( Math.Atan2(y, x) + Math.PI/4);                                                
                case FrameDesgin.Centerd:
                    return 1;
                case FrameDesgin.None:                   
                    return 0; 
                default:
                   deg = Math.Atan2(y, x) - Math.PI/4.0;
                   return (float)Math.Cos(deg);
            }
                 
        }
 
        //public static float FrameShade(float x, float y,

        public static Texture2D GenerateTexture(int radX, int radY, GuiControlDesign design, Norma shapeNorma) // gets a delgate of the norm function
        {
            MCGA mcga;
            Texture2D texture;
            mcga = new MCGA(MyGraphics.gdm.GraphicsDevice, MyGraphics.sb, radX*2+1, radY*2+1);
                             
            for (int x = -radX; x <= radX; x++) //flipOrder will be faster
            {
                for (int y = -radY; y <= radY; y++)
                {
                    float norma = shapeNorma((float)x / radX, (float)y / radY);

                    if (norma < 1) 
                    {
                        Color color;
                        int scaleFactor = Math.Min(radX, radY);
                        //float scaleFactor = (radX + radY) / 2f;
                        if (norma > 1 - (float)design.frameNumber / scaleFactor)
                        {
                            float shader = FrameShade(x, y, design.frameDesign);
                            float col = design.color -  (norma - 1 + (float)design.frameNumber / scaleFactor)*design.colorGradient*shader;
                            //color = new Color(col, col, col, 1 - (norma - 1 + (float)design.frameNumber / scaleFactor)*2);
                            color = new Color(col, col, col);
                        }
                        else
                        {
                            color = new Color(design.color, design.color, design.color);
                        }                                                                                               
                        mcga.Putpixel(x + radX, y + radY, color);      
                    }
                }
            }
            mcga.SetData();
            texture = mcga.mcgaTexture;
            return texture;
        }
    }
}
