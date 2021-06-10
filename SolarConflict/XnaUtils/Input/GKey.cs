using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.XnaUtils.Input
{
    public struct GKeys //maybe move
    {
       // public static Dictionary<Keys, string> _keyStrings = new Dictionary<Keys, string> { { Keys.D1, "1"}, }
        public Keys Key;
        public MouseButtons MouseButton;


        public GKeys(Keys key)
        {
            Key = key;
            MouseButton = MouseButtons.None;
        }

        public GKeys(MouseButtons mouseButtons)
        {
            Key = Keys.None;
            MouseButton = mouseButtons;
        }       
        
        public override string ToString()
        {
            string text;
            if (Key >= Keys.D1 && Key <= Keys.D9)
            {
                return ((int)Key - (int)Keys.D0).ToString();
            }
            if (Key != Keys.None)
            {
                text = Key.ToString();
                if (MouseButton != MouseButtons.None)
                {
                    text += "," + MouseButton.ToString();
                }
            }
            else
            {
                text = MouseButton.ToString();
            }
            return text;
        }        


        //public static bool operator== (GKeys left, GKeys right) {
        //    return (left.MouseButton == right.MouseButton) && (left.Key == right.Key);
        //}
        //public static bool operator!=(GKeys left, GKeys right) {
        //    return !(left == right);
        //}

        public static implicit operator GKeys(Keys key)
        {
            GKeys o = new GKeys(key);
            return o;
        }

        public static implicit operator GKeys(MouseButtons button) {
            GKeys o = new GKeys(button);
            return o;
        }

        public bool IsEmpty()
        {
            return Key == Keys.None && MouseButton == MouseButtons.None;
        }

        public static GKeys Parse(string s)
        {
            if (s == null)
                return new GKeys(Keys.None);
            GKeys gKey = new GKeys(Keys.None);
            gKey.MouseButton = ParserUtils.ParseEnum<MouseButtons>(s, MouseButtons.None);
            gKey.Key = ParserUtils.ParseEnum<Keys>(s, Keys.None);
            return gKey;
        }

        public string ToSaveString()
        {
            string res = Keys.None.ToString();
            if (MouseButton != MouseButtons.None)
                res = MouseButton.ToString();
            if (Key != Keys.None)
                res = Key.ToString();            
            return res;
        }
    }
}
