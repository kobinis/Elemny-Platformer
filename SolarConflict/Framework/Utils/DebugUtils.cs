using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace SolarConflict
{
    public enum ModeType { Debug, Release, Test }
    internal static class DebugUtils
    {
        
        public static ModeType Mode = ModeType.Release;

        public static Keys[] DebugKeys = new Keys[] { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0 };
        public static string Version = "0.5.1";
        public static int SaveVersion = 13;
        public static string BuildName = "Early Access: " + Version;

        public static bool UseDebugKeys = false;
        public static bool LoadWithThred = false;
        public static bool ShowItemID = false;
        public static bool ShowFPS = false;
        public static bool DebugMenu = false;
        public static bool TestContent = false;
        /// <summary>
        /// Shows number of gameobjects and colliders
        /// </summary>
        public static bool IsDebug = false;
       
        public static Color DebugColor = new Color(50, 200, 50, 150);

        public static bool IsReload = false;

        //Display
        public static bool HideHud = false;

        //public static string PathNotes = "";

    }
}
