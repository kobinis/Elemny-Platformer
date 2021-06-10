using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PaintPlay.XnaUtils.MyGui
{
    class GuiControler
    {
        public Vector2 position;  //change
        int radius;
        Vector2 origan;
        Texture2D texture;

        public static float CircleNorma(float x, float y)
        {
            //return Math.Max(Math.Abs(x), Math.Abs(y));//(float)Math.Sqrt( x * x + y * y);
           // return (float)Math.Sqrt(Math.Sqrt(x*x*x*x+y*y*y*y));//
            return (float)Math.Sqrt( x * x + y * y);
        }

        public GuiControler(Vector2 position, int size, Color[] pallete)
        {
            
            radius = size;
            this.position = position;

            /*MCGA mcga;
            mcga = new MCGA(MyGraphics.gdm.GraphicsDevice, MyGraphics.sb, size * 2 + 1, size * 2 + 1);
            for (int x = -size; x <= size; x++)
            {
                for (int y = -size; y <= size; y++)
                {
                    double rad = Math.Sqrt(x * x + y * y);
                    if (rad<size)
                    {
                        double deg = Math.Atan2(y, x)+Math.PI;
                        

                        int ind =  (int)Math.Floor( deg / Math.PI * 0.5 * (pallete.Length-1));
                        Color col = pallete[ind];
                        Vector3 colVec = col.ToVector3();
                        
                        mcga.Putpixel(x + size, y + size, col.PackedValue);
                    }
                }
            }
            mcga.SetData();
            texture = mcga.mcgaTexture;*/
            GuiControlDesign design = new GuiControlDesign();
            design.frameNumber = 10;
            design.colorGradient = 1f;
            design.color = 0.7f;

            texture = GuiHelper.GenerateTexture(size, size, design, new Norma(CircleNorma));
            //texture = GuiHelper.GenerateTexture(size, size, design, => x*x);
            origan = new Vector2(size, size);
        }

        public bool IsOnControler(Vector2 pos)
        {
            Vector2 relPos = pos - position;
            return relPos.Length() <= radius;
        }


        public void Draw()
        {
            MyGraphics.sb.Draw(texture, position - origan, Color.White);
        }
    }

}
