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
    class BarControl : GuiControl
    {
        private Vector2 origin;
        private ClickedControl slider;
        float minValue, maxValue;
        float value;

        public bool HasValueChanged { get; set; }
        


        public BarControl(Vector2 position, int radX, int radY, float minValue, float maxValue, GuiControlDesign design) //gets norma
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.radX = radX;
            this.radY = radY;
            Position = position;            
            ControlColor = new Color(1f, 1f, 1f, 0.6f);
            PressedControlColor = new Color(0.95f, 0.95f, 0.95f, 0.6f);
            this.norma = new Norma(ShapeBank.RectangleNorma);
            texture = GuiHelper.GenerateTexture(radX, radY, design, norma);
            origin = new Vector2(radX, radY);
            InputState = new TouchState();
            isPressed = false;

            slider = new ClickedControl(Vector2.Zero, radX / 8, radY, new GuiControlDesign(5, FrameDesgin.Centerd),
              new Norma(ShapeBank.RoundedRectNorma));

            slider.ControlColor = ControlColor;
            slider.ControlColor = new Color(0.9f, 0.9f, 0.9f, 1f);
            value = (minValue+maxValue)/2;
            
        }

        public override void Update(Gui gui, List<TouchState> inputs)
        {
            HasValueChanged = false;

            base.Update(gui, inputs);
            slider.Update(gui, inputs);
            if (IsInputOn)
            {
                Vector2 newPos = new Vector2();
                newPos.X  = base.InputState.Position.X - gui.Position.X - Position.X;
                newPos.Y = slider.Position.Y;
                slider.Position = newPos;
                value = (slider.Position.X + radX) / (2 * radX) * (maxValue - minValue) + minValue;
                HasValueChanged = true;
            }

            

        }

        public override void Draw(Gui gui)
        {
            base.Draw(gui);
            slider.Draw(gui, gui.Position + Position + slider.Position);
        }

        public float GetFloatValue()
        {
            return value;
        }

        public override int GetValue()
        {
            return (int)value;
        }


    }
}
