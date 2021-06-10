using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PaintPlay.XnaUtils.Input;

namespace PaintPlay.XnaUtils.MyGui
{
    class ClickedControl : GuiControl
    {
        private Vector2 origin;


        public ClickedControl(Vector2 position, int radX, int radY, GuiControlDesign design, Norma norma) //gets norma
        {
            this.radX = radX;
            this.radY = radY;
            Position = position;
            //GuiControlDesgin design = new GuiControlDesgin();
            ControlColor = new Color(1f, 1f, 1f, 0.6f);
            PressedControlColor = new Color(0.95f, 0.95f, 0.95f, 0.6f);
            this.norma = norma;
            texture = GuiHelper.GenerateTexture(radX, radY, design, norma);
            origin = new Vector2(radX, radY);
            InputState = new TouchState();
            isPressed = false;
        }

        public override void Update(Gui gui, List<TouchState> inputs)
        {
            base.Update(gui, inputs);

            if (IsInputOn)
            {
                if (InputState.OnPress)
                {
                    isPressed = true;
                }
            }
            else
            {
                isPressed = false;
            }

            
        }


    }
}
