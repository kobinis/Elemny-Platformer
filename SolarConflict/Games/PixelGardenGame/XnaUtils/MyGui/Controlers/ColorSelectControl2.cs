using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PaintPlay.XnaUtils.MyGui
{
    class ColorSelectControl2 : GuiControl
    {
        public Color[] pallete;
        Vector2 origin;
        ClickedControl Selector;

        BarControl alphaBar;

        public ColorSelectControl2(Vector2 position, int rad)
        {
            pallete = new Color[600]; //768
            for (int i = 0; i < 600; i++)
            {
                float t = (float)i / 100f; //128
                float r = MyMath.Trapz(t) + MyMath.Trapz(t - 6);
                float g = MyMath.Trapz(t - 2);
                float b = MyMath.Trapz(t - 4);
                pallete[i] = new Color(r, g, b);
            }

           

            this.radX = rad;
            this.radY = rad;
            Position = position;
            ControlColor = Color.White;
            PressedControlColor = new Color(0.95f, 0.95f, 0.95f);
            //norma = new Norma(ShapeBank.Norma1);
            norma = new Norma(ShapeBank.RectangleNorma);
            origin = new Vector2(rad, rad);
            isPressed = false;

            MCGA mcga; //move to gui helper
            mcga = new MCGA(MyGraphics.gdm.GraphicsDevice, MyGraphics.sb, rad * 2 + 1, rad * 2 + 1);
            for (int x = -rad; x <= rad; x++)
            {
                for (int y = -rad; y <= rad; y++)
                {
                    float pRad = norma(x,y);//(float)(Math.Abs(x) + Math.Abs(y));
                    if (pRad < rad)
                    {
                                                
                        Color col = VectorToColor(new Vector2(x, y));
                       
                        col.A = (byte)(Math.Min(1f, 4f - 4 * (float)pRad / rad) * 255);
                        
                        mcga.Putpixel(rad - x, rad - y, col);

                    }
                }
            }
            mcga.SetData();
            texture = mcga.mcgaTexture;
            alphaBar = new BarControl(position + new Vector2(0, radY + radY/8+6), radX, radY / 7, 0, 255, new GuiControlDesign(5, FrameDesgin.Centerd));
            Selector = new ClickedControl(new Vector2(0, 0), rad / 5, rad / 5, new GuiControlDesign(5, FrameDesgin.Centerd),
                new Norma(ShapeBank.CircleNorma));
            Selector.ControlColor = VectorToColor(Selector.Position);
        }

        private Color VectorToColor(Vector2 pos)
        {

            float pRad = radY -Math.Abs(pos.Y); //(float)Math.Sqrt(pos.X * pos.X + pos.Y * pos.Y);          
            //int ind = (int)((pos.X + radX - Math.Abs(pos.Y)) / (2 * radX + 1 - Math.Abs(2 * pos.Y)) * (pallete.Length - 1));
            int ind = (int)((pos.X + radX) / (2 * radX + 1) * (pallete.Length - 1));
            Color col = pallete[Math.Min(Math.Max(ind,0), pallete.Length-1)];
            Vector3 colVec = col.ToVector3();
            
            if(pos.Y<0) //can be done without if
                return new Color(1 - colVec.X * (pRad / radX), 1 - colVec.Y * (pRad / radX), 1 - colVec.Z * (pRad / radX));
            else
                return new Color((1 - colVec.X) * (pRad / radX), (1 - colVec.Y )* (pRad / radX), (1 - colVec.Z )* (pRad / radX));
        }

        public Color GetSelectedColor()
        {
            Color color = Selector.ControlColor;
            color.A = (byte)alphaBar.GetValue();
            return color;
        }

        public override void Update(Gui gui, List<Input.TouchState> inputs)
        {
            base.Update(gui, inputs);           
            if (IsInputOn)
            {
                Selector.Position = this.InputState.Position - gui.Position - Position;

                /* if (Selector.Position.Length() > radX)
                 {
                     Selector.Position.Normalize();
                    /* Selector.Position = radX;
                 }*/
                Selector.ControlColor = VectorToColor(Selector.Position);
            }

            alphaBar.Update(gui, inputs);
            // 
        }


        public override void Draw(Gui gui)
        {
            Vector2 origin = new Vector2(radX, radY);
            MyGraphics.sb.Draw(texture, Position + gui.Position, null, PressedControlColor, (float)Math.PI, origin, 1, SpriteEffects.None, 0);
            Selector.Draw(gui, gui.Position + Position + Selector.Position);

            alphaBar.Draw(gui);
        }


    }
}
