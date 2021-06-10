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
    class TextControl:GuiControl
    {
        public TextControl(string text, Color color)
        {
            helpText = text;
            ControlColor = color;
        }

        public override bool IsPositionOnControl(Vector2 inputPos)
        {
            return false;
        }

        public override void Update(Gui gui, List<TouchState> inputs)
        {            
        }

        public override void Draw(Gui gui)
        {
            Vector2 textSize = MyGraphics.font.MeasureString(this.helpText);

            MyGraphics.sb.DrawString(MyGraphics.font, this.helpText, gui.Position + Position - textSize/2f, ControlColor);
        }
    }
}
