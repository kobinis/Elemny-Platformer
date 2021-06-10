using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PaintPlay.XnaUtils.MyGui
{
    class ColorSelectControl : GuiControl
    {
        public Color[] pallete;
        Vector2 origin;
        ClickedControl Selector;

        public ColorSelectControl(Vector2 position, int rad)
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
            norma = new Norma(ShapeBank.CircleNorma);
            origin = new Vector2(rad, rad);
            isPressed = false;

            MCGA mcga; //move to gui helper
            mcga = new MCGA(MyGraphics.gdm.GraphicsDevice, MyGraphics.sb, rad * 2 + 1, rad * 2 + 1);
            for (int x = -rad; x <= rad; x++)
            {
                for (int y = -rad; y <= rad; y++)
                {
                    float pRad = norma(x, y);//(float)(Math.Abs(x) + Math.Abs(y));
                    if (pRad < rad)
                    {

                        Color col = VectorToColor(new Vector2(x, y));

                        col.A = (byte)(Math.Min(1f, 3f - 3 * (float)pRad / rad) * 255);

                        mcga.Putpixel(rad - x, rad - y, col);

                    }
                }
            }
            mcga.SetData();
            texture = mcga.mcgaTexture;

            Selector = new ClickedControl(new Vector2(0, -rad / 2), rad / 5, rad / 5, new GuiControlDesign(5, FrameDesgin.Centerd),
                new Norma(ShapeBank.CircleNorma));
            Selector.ControlColor = VectorToColor(Selector.Position);
        }

        private Color VectorToColor(Vector2 pos)
        {
            float pRad = (float)Math.Sqrt(pos.X * pos.X + pos.Y * pos.Y);
            double deg = Math.Atan2(pos.Y, pos.X) + Math.PI;
            int ind = (int)Math.Floor(deg / Math.PI * 0.5 * (pallete.Length - 1));
            Color col = pallete[ind];
            Vector3 colVec = col.ToVector3();
            //return new Color(colVec.X * (pRad / radX), colVec.Y * (pRad / radX), colVec.Z * (pRad / radX));
            return new Color(1 - colVec.X * (pRad / radX), 1 - colVec.Y * (pRad / radX), 1 - colVec.Z * (pRad / radX));
        }

        public Color GetSelectedColor()
        {
            return Selector.ControlColor;
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
            // 
        }


        public override void Draw(Gui gui)
        {
            Vector2 origin = new Vector2(radX, radY);
            MyGraphics.sb.Draw(texture, Position + gui.Position, null, PressedControlColor, (float)Math.PI, origin, 1, SpriteEffects.None, 0);
            Selector.Draw(gui, gui.Position + Position + Selector.Position);
        }


    }
}
