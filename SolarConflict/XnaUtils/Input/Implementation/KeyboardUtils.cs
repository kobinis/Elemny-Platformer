using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace SolarConflict.XnaUtils
{
    public class KeyboardUtils
    {
        private static KeyboardUtils instance = null;

        public static KeyboardUtils Inst
        {
            get
            {
                if (instance == null)
                {
                    instance = new KeyboardUtils();
                }
                return instance;
            }
        }        


        KeyboardState lastKeyboardState, keyboardState;

        public void Update()
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// Returens true when the key is pressed (is down and was up)
        /// </summary>
        public bool IsKeyPressed(Keys key)
        {
            return (keyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key));
        }

        public List<Keys> GetPressedKeys()
        {
            /*foreach (var item in collection)
            {
                
            }*/

            return null;
        }

    }
}
