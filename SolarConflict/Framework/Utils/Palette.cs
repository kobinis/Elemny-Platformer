using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict
{
    public class Palette
    {
        public static Color DialogTextColor = Color.White;
        public static Color GuiColor = new Color(32, 104, 104, 200);
        public static Color Textbox = Color.CadetBlue;
        public static Color Hitpoints = Color.Green;
        public static Color Shield = Color.Blue;
        public static Color Energy = Color.Yellow;
        public static Color Damage = Color.Red;

        public static Color HostileColor = Color.Red;
        public static Color NonHostileColor = Color.DarkBlue;
        public static Color SameFactionColor = Color.Blue;

        /// <summary>
        /// Used to mark important data in tooltips ans messeges
        /// </summary>
        public static Color Highlight = Color.Yellow;
        public static Color Highlight2 = Color.White;
        public static Color ItemDescription = Color.LightYellow;
        public static Color ItemNegetiveStat = Color.Crimson;

        //public static Color TextColor = new Color(0, 22, 26);
        public static Color GuiFrame = new Color(0, 224, 216);
        public static Color GuiBody = new Color(0, 22, 26);
        public static Color ArrowTextColor = Color.Beige;

        public static Color MainMissionColor = Color.Yellow;

        public static Color SlotColor = new Color(100, 100, 100, 200);
        public static Color SlotEnabledColor = new Color(0, 255, 0, 200);
        public static Color GuiHighlightFrame = new Color(255, 255, 50, 150);

        //public static Color TextBoxFont
        //{
        //    get { return new Color(167, 187, 174); }
        //}

        //public static Color GuiBlue
        //{
        //    get { return new Color(255,100,100,255); }
        //}
    }
}
