using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace SolarConflict.XnaUtils.Input
{
    public enum MouseButtons //can be flags ?
    {
        None,
        LeftButton,
        MiddleButton,
        RightButton,
        XButton1,
        XButton2,
    }

    public class MouseUtils
    {
 

        private static MouseUtils instance = null;

        public static MouseUtils Inst
        {
            get
            {
                if (instance == null)
                {
                    instance = new MouseUtils();                    
                }
                return instance;
            }
        }

        public MouseState lastMouseState, mouseState;  //change
        //maybe add gameTime;
        
        public static bool IsMouseButtonsDown(MouseState mouseState, MouseButtons mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButtons.None:
                    return false;                    
                case MouseButtons.LeftButton:
                    return mouseState.LeftButton == ButtonState.Pressed;
                case MouseButtons.MiddleButton:
                    return mouseState.MiddleButton == ButtonState.Pressed;
                case MouseButtons.RightButton:
                    return mouseState.RightButton == ButtonState.Pressed;
                case MouseButtons.XButton1:
                    return mouseState.XButton1 == ButtonState.Pressed;
                case MouseButtons.XButton2:
                    return mouseState.XButton2 == ButtonState.Pressed;                
            }
            return false;
        }

        public void Update()
        {
            lastMouseState = mouseState;
            mouseState =  Mouse.GetState();
        } 
       
        public int GetDScroolWheel()
        {
            return mouseState.ScrollWheelValue - lastMouseState.ScrollWheelValue;
        }

    }
}
