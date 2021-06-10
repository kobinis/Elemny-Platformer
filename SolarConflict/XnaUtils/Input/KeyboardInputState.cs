using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace XnaUtils.Input
{
    public struct KeyboardInputState
    {
        public KeyboardState LastKeyboardState, KeyboardState;

        /// <summary>
        /// Returens true when the key is pressed (is down and was up)
        /// </summary>
        public bool IsKeyPressed(Keys key)
        {
            return (KeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key));
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
