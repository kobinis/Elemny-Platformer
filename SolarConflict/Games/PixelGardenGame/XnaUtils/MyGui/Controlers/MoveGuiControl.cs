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
    class MoveGuiControl : GuiControl
    {
        private Vector2 origin;
        private Vector2 basePosition;
        private bool isMoving;
        private int inputID;

        private Vector2 speed;//momentom;


        public MoveGuiControl(Vector2 position, int radX, int radY, GuiControlDesign design, Norma norma) //gets norma
        {
            this.radX = radX;
            this.radY = radY;
            Position = position;
            //GuiControlDesgin design = new GuiControlDesgin();
            ControlColor = new Color(1f, 1f, 1f, 0.8f);
            this.norma = norma;
            texture = GuiHelper.GenerateTexture(radX, radY, design, norma);
            origin = new Vector2(radX, radY);
            isPressed = false;
        }

        public override void Update(Gui gui, List<TouchState> inputs)
        {
            base.Update(gui, inputs);

            if (IsInputOn)
            {
                if (InputState.OnPress)
                {
                    isMoving = true;
                    inputID = InputState.ID;
                    basePosition = gui.Position;
                }
            }

            if (isMoving)
            {
                //inputs.Clear(); not good
                foreach (TouchState state in gui.GetAllInputs())
                {
                    if (state.ID == inputID)
                    {
                        gui.Position = basePosition - state.FirstPosition + state.Position;
                        inputs.Remove(state);
                        if (state.OnRelease)
                        {
                            isMoving = false;                            
                        }
                        else
                            speed = state.GetSpeed();
                        break;
                    }
                }
            }
            else
            {
                speed *= 0.9f;//momentom
                gui.Position += speed;
            }


        }


    }
}
