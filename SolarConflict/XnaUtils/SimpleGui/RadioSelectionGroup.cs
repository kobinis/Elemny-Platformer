using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils.Input;

namespace XnaUtils.SimpleGui
{
    /// <summary>
    /// Used to make a group of control act as a radio selection (only one can be selected)
    /// </summary>
    [Serializable]
    public class RadioSelectionGroup
    {
        int selectedControlIndex = 0;
        List<GuiControl> controls;

        public int SelectedControlIndex { get { return selectedControlIndex; } set { selectedControlIndex = value; Update(); } }
        public GuiControl SelectedControl {
            get { return controls[selectedControlIndex]; }
            set
            {
                for (int i = 0; i < controls.Count; i++)
                {
                    if (value == controls[i])
                    {
                        selectedControlIndex = i;
                        return;
                    }                    
                }                
            }
        }

        public RadioSelectionGroup()
        {
            controls = new List<GuiControl>();
        }

        public void AddControl(GuiControl control)
        {
            controls.Add(control);
            control.Action += ControlSelectedHandler;
            Update();
        }

        private void ControlSelectedHandler(GuiControl source, CursorInfo cursorLocation)
        {
            selectedControlIndex = source.Index;
            Update();
            //fire event
        }

        public void Update()
        {
            for (int i = 0; i < controls.Count; i++)
            {
                controls[i].Index = i; //???
                if(selectedControlIndex == i)
                {
                    controls[i].IsPressed = true;
                }
                else
                {
                    controls[i].IsPressed = false;
                }
            }
        }
        
    }
}
