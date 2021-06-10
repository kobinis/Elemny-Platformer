using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SolarConflict.Framework.PlayersManagement;
using XnaUtils;
using SolarConflict.Framework;
using System.IO;
using System.Reflection;
using SolarConflict.XnaUtils.Input;
using XnaUtils.Graphics;

namespace SolarConflict
{
    public enum GameDifficulty { Easy = 0, Normal, Hard };

    public static class GameplaySettings
    {
        public static bool AutoSortInventory = true;
        public static bool IsFilter = true;
        public static bool AutoSave = true;
        public static bool KeepInventory { get { return Difficulty == GameDifficulty.Easy; } set { } }
        public static GameDifficulty Difficulty = GameDifficulty.Normal;
        public static bool SkipTutorial = false;

        public static bool DirectionalControl = true; //Reletive, Directional, Fly by Wire
    }
        
    
    public static class VolumeSettings
    {
        public static float EffectsVolume = 0.2f;
        public static float DialogVolume = 0f;
        public static float MusicVolume = 0.1f;
    }

    public static class GraphicsSettings
    {        
        public static int ResWidth = 0;
        public static int ResHeight = 0;
        public static bool IsFullscreen = true;
        public static bool IsBorderless = false;
        public static bool UseLighting = true;
        public static bool IsPostprocessing = false;
        public static bool IsVerticalSync = true;
        public static bool IsHardwareModeSwitch = true;
    }
    
    public class SettingsManager
    {                        
        private string _settingsPath;
        private List<PreferencesClassSerialization> _referencesList;        

        private SettingsManager()
        {
            _settingsPath = Consts.GAME_DATA_PATH;
            _referencesList = new List<PreferencesClassSerialization>();
            _referencesList.Add(new PreferencesClassSerialization("Volume.txt", typeof(VolumeSettings)));
            _referencesList.Add(new PreferencesClassSerialization("Graphics.txt", typeof(GraphicsSettings)));
            _referencesList.Add(new PreferencesClassSerialization("Gameplay.txt", typeof(GameplaySettings)));
        }

        private static SettingsManager _inst;
        public static SettingsManager Inst
        {
            get
            {
                if (_inst == null)
                    _inst = new SettingsManager();
                return _inst;
            }
        }
        
        public void Load()
        {            
            foreach (var item in _referencesList)
            {
                item.Load(_settingsPath);
            }
        }

        public void Save()
        {
            foreach (var item in _referencesList)
            {
                item.Save(_settingsPath);
            }
        }
               
        //public static bool ShowGoalIndicatorWhenOnScreen;
        //public static bool ShowDemageText = true;
        //public static bool ShowHealthBar = true;
        ////public bool FriendlyFire = false;
    }
}
