using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PaintPlay.XnaUtils.Input;

namespace PaintPlay.XnaUtils.MyGui
{
    class Gui
    {
        public List<GuiControl> controls;
        public Vector2 Position { set; get; } //guis postion
        private List<TouchState> allInputs;

        public Gui()
        {
            controls = new List<GuiControl>();
            Position = new Vector2(200,100);
            allInputs = new List<TouchState>();
            //add load constructor;
        }
         
        public void Update(List<TouchState> inputs)
        {
            allInputs = new List<TouchState>(inputs); //or clear and copy
            
            /*foreach (GuiControl control in controls) //diffrent solution
            {
                control.IsInputOn = false;
            }
            foreach (TouchState state in inputs)
            {
                for (int i = controls.Count-1; i < length; i++)
                {
                    
                }
            }*/

            for (int i = controls.Count-1; i >= 0; i--)
            {
                controls[i].Update(this, inputs); 
            }
            
        }

        public void Draw()
        {
            foreach (GuiControl control in controls)
            {
                control.Draw(this);
            }
        }

        public List<TouchState> GetAllInputs()
        {
            return allInputs;
        }
    }
}
