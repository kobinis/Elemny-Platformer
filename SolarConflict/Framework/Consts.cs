using SolarConflict.Framework.MetaGame.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework
{
    public class Consts //?? maybe remove
    {
#if DEBUG
        public const string GAME_DATA_PATH = @"..\..\..\..\GameData";
#else
        public const string GAME_DATA_PATH = @"GameData1";
        //public const string GAME_DATA_PATH = @"..\..\..\..\GameData";
#endif
        public const string USER_LOADOUTS = "UserLoadouts";
        public const string SESSION_SAVE_PATH = GAME_DATA_PATH + @"\Sessions";
       // public const string AGENTS_SAVE_PATH = GAME_DATA_PATH + @"\Loadouts";
        public const string STAR_NAMES_PATH = GAME_DATA_PATH + @"\StarNames.txt";
        public const string STATISTICS_PATH = GAME_DATA_PATH + @"\Statistics";
        public const string SCREENSHOTS_PATH = GAME_DATA_PATH + @"\Screenshots\";
        public const string TEMPLATES_PATH = GAME_DATA_PATH + @"\Templates";
        public const string MENUS_PATH = GAME_DATA_PATH + @"\Menu";
        public const string AI_HELPER_TEXTURE_ID = "AI_Helper";
        public const string INFORMATION_TEXTURE_ID = null; //"information";
        public const string COMMUNICATION_CONSOLE_TEXTURE_ID = "greenconsole";

        public const string WARPIN_EFFECT = "HyperSpaceJumpFx";
        public const string WARPOUT_EFFECT = "HyperSpaceJumpFx";

        public static string GetLoadoutPath(string path = null)
        {
            if (path == null)
                path = "Loadouts";
            return Path.Combine(GAME_DATA_PATH, path);
        }

        //public float GLOBAL_INERTIA = 1;

        public const float CAMERA_MIN_ZOOM = 0.2f;
        public const float CAMERA_MAX_ZOOM = 1.5f;

        /// <remarks>Only for ships at the moment.</remarks>
        public static float DefaultVelocityInertia = 0.985f;

        public const float PixelsToUinits = 0.01f;

        public static int NeededHullPart = 3;

        //Content
        public const string GuildDrifter = "GuildDrifter";
        public const string DrifterItem = "InhibitorCore";
    }
}
