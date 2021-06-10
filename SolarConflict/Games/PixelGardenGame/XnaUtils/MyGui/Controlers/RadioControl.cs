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
    class RadioControl : GuiControl
    {
        private int value;
        List<ToggleControl> controls;

        public bool HasValueChanged { set; get; }


        public RadioControl() //gets norma
        {
            controls = new List<ToggleControl>();
        }

        public void AddControl(ToggleControl control)
        {
            controls.Add(control);
        }

        public override void Update(Gui gui, List<TouchState> inputs)
        {
            isPressed = false;
            HasValueChanged = false;
                           
            for (int i = 0; i < controls.Count; i++)
            {
                controls[i].Update(gui, inputs);
                if (controls[i].IsInputOn && controls[i].InputState.OnPress)
                {
                    value = i;
                    HasValueChanged = true;
                }                                                    
            }
                
            

            for (int i = 0; i < controls.Count; i++)
            {
                if (i == value)
                {
                    controls[i].Press();
                }
                else
                    controls[i].UnPress();
            }

            foreach (TouchState state in inputs)
            {
                for (int i = 0; i < controls.Count; i++)
                {
                    if (controls[i].IsPositionOnControl(state.Position - gui.Position))
                    {
                    }
                }
            }

        }

        public override void Draw(Gui gui)
        {
            foreach (GuiControl control in controls)
            {
                control.Draw(gui);
            }
        }

        public override int GetValue()
        {
            return value;
        }

        public void SetValue(int value)
        {
            this.value = value;
        }


    }
}

