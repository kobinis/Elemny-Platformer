using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Content;
using XnaUtils;

namespace Redux.Utilities.Managers
{
    /// <summary>
    /// Taken from http://www.dreamincode.net/forums/topic/257077-good-xna-keyboard-input/
    /// </summary>
    /// <remarks>TODO: remove, replace with something much simpler</remarks>
    [Serializable]
    public class KeyboardManager 
    {
        public bool IsBackspaceActive;
        //public variables
        public string Text { get; set; }
        /// <summary>
        /// This represents the amount of milliseconds for which keys must be held down before they'll repeat
        /// </summary>
        public int MustPass = 155;
        long _backspaceCooldown;
        long _backspaceCooldownDuration;   

        public KeyboardManager()             
        {            
            _backspaceCooldownDuration = (int)(0.4 * (new TimeSpan(0, 0, 0, 1).Ticks));
        }

        DateTime prevUpdate = DateTime.Now;

        public string Update(GameTime gameTime, Keys[] keysPressedThisFrame, Keys[] keysDown)
        {
            var shiftDown = keysDown.Contains(Keys.LeftShift) || keysDown.Contains(Keys.RightShift);
            var backspaceDown = keysDown.Contains(Keys.Back);

            Text = string.Empty;            
            string input = Convert(keysPressedThisFrame, shiftDown);
            //string prevInput = Convert(keysPreviouslyDown, shiftPressed);
            DateTime now = DateTime.Now;
            //make sure 100ms (with a few measurements) has passed

            IsBackspaceActive = false;

            if (backspaceDown) {
                // Backspace is pressed
                if (_backspaceCooldown <= gameTime.TotalGameTime.Ticks) {
                    IsBackspaceActive = true;
                    _backspaceCooldown = gameTime.TotalGameTime.Ticks + _backspaceCooldownDuration;
                }
            } else
                _backspaceCooldown = 0;
            
            //if (now.Subtract(prevUpdate).Milliseconds >= time)
            if (input.Length > 0)
            // so whenever we press a new key (not a key modifier), or simultaneously press one and release another
            {
                foreach (char x in input)
                {
                    if (x != '\b')
                        Text += x;
                }
                if (!string.IsNullOrEmpty(input))
                    prevUpdate = now;
            }

            return Text;
        }

        public string Convert(Keys[] keys, bool shiftDown)
        {
            string output = "";

            foreach (Keys key in keys)
            {               
                if (key >= Keys.A && key <= Keys.Z)
                    output += key.ToString();
                else if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
                    output += ((int)(key - Keys.NumPad0)).ToString();
                else if (key >= Keys.D0 && key <= Keys.D9)
                {
                    string num = ((int)(key - Keys.D0)).ToString();
                    #region special num chars
                    if (shiftDown)
                    {
                        switch (num)
                        {
                            case "1":
                                {
                                    num = "!";
                                }
                                break;
                            case "2":
                                {
                                    num = "@";
                                }
                                break;
                            case "3":
                                {
                                    num = "#";
                                }
                                break;
                            case "4":
                                {
                                    num = "$";
                                }
                                break;
                            case "5":
                                {
                                    num = "%";
                                }
                                break;
                            case "6":
                                {
                                    num = "^";
                                }
                                break;
                            case "7":
                                {
                                    num = "&";
                                }
                                break;
                            case "8":
                                {
                                    num = "*";
                                }
                                break;
                            case "9":
                                {
                                    num = "(";
                                }
                                break;
                            case "0":
                                {
                                    num = ")";
                                }
                                break;
                            default:
                                //wtf?
                                break;
                        }
                    }
                    #endregion
                    output += num;
                }
                else if (key == Keys.OemPeriod)
                    output += ".";
                else if (key == Keys.OemTilde)
                    output += "'";
                else if (key == Keys.Space)
                    output += " ";
                else if (key == Keys.OemMinus)
                    output += "-";
                else if (key == Keys.OemPlus)
                    output += "+";
                else if (key == Keys.OemQuestion && shiftDown)
                    output += "?";
                else if (key == Keys.Back) //backspace
                    output += "\b";

                if (shiftDown)
                    output = output.ToUpper();
                else
                    output = output.ToLower();
            }
            return output;
        }
    }
}


