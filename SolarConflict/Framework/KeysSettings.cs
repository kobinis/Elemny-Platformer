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
using SolarConflict.Framework.Scenes;
using SolarConflict.XnaUtils;

namespace SolarConflict
{

    public class KeyBindingsData
    {
        public Dictionary<PlayerCommand, GKeys> CommandBindings;
        public Dictionary<ControlSignals, GKeys> KeyBindings;
        public Dictionary<SceneComponentType, GKeys> MenuBindings;
        public Keys LockRotation;

        public KeyBindingsData()
        {
            CommandBindings = new Dictionary<PlayerCommand, GKeys>();
            KeyBindings = new Dictionary<ControlSignals, GKeys>();
            MenuBindings = new Dictionary<SceneComponentType, GKeys>();
            
            KeyBindings.Add(ControlSignals.Up, new GKeys(Keys.W));
            KeyBindings.Add(ControlSignals.Down, new GKeys(Keys.S));
            KeyBindings.Add(ControlSignals.Left, new GKeys(Keys.A));
            KeyBindings.Add(ControlSignals.Right, new GKeys(Keys.D));
            KeyBindings.Add(ControlSignals.Action1, new GKeys(MouseButtons.LeftButton));
            KeyBindings.Add(ControlSignals.Action2, new GKeys(MouseButtons.RightButton));
            KeyBindings.Add(ControlSignals.Action3, new GKeys(Keys.Q));
            KeyBindings.Add(ControlSignals.Action4, new GKeys(Keys.E));
            KeyBindings.Add(ControlSignals.MoveToCursor, Keys.Space);

            KeyBindings.Add(ControlSignals.QuickUse1, new GKeys(Keys.D1));
            KeyBindings.Add(ControlSignals.QuickUse2, new GKeys(Keys.D2));
            KeyBindings.Add(ControlSignals.QuickUse3, new GKeys(Keys.D3));
            KeyBindings.Add(ControlSignals.QuickUse4, new GKeys(Keys.D4));
            KeyBindings.Add(ControlSignals.Brake, new GKeys(Keys.X));

            CommandBindings = new Dictionary<PlayerCommand, GKeys>();
            CommandBindings.Add(PlayerCommand.Use, Keys.F);
            CommandBindings.Add(PlayerCommand.SwapUp, Keys.Tab);
            CommandBindings.Add(PlayerCommand.CallHelp, Keys.R);

            MenuBindings = new Dictionary<SceneComponentType, GKeys>();
            MenuBindings[SceneComponentType.EscapeMenu] = Keys.Escape;
            MenuBindings[SceneComponentType.MissionLog] = Keys.F1;
            MenuBindings[SceneComponentType.Inventory] = Keys.F2;
            MenuBindings[SceneComponentType.Hangar] = Keys.F3;
            MenuBindings[SceneComponentType.TacticalMap] = Keys.M;
            MenuBindings[SceneComponentType.GalaxyMap] = Keys.F5;
            MenuBindings[SceneComponentType.Codex] = Keys.C;
            MenuBindings[SceneComponentType.Imbue] = Keys.L;
            LockRotation = Keys.LeftShift;
        }
    }

    //public 
    public static class KeysSettings
    {        
        public const string LOCK_ROTATION = "LockRotation";
        public static Dictionary<GKeys, string> KeyIconIDs;        
        private static string fullPath;

        public static KeyBindingsData Data;

        public static GKeys GetSceneComponentKey(SceneComponentType type)
        {
            if(Data.MenuBindings.ContainsKey(type))
            {
                return Data.MenuBindings[type];
            }
            return new GKeys();
        }


        static KeysSettings()
        {
            fullPath = Path.Combine(Consts.GAME_DATA_PATH, "Keys.json");
            KeyIconIDs = new Dictionary<GKeys, string>() { { MouseButtons.LeftButton, "lmbV2" }, { MouseButtons.RightButton, "rmbV2" }, { MouseButtons.MiddleButton, "mousescroll" } }; //Add MMB
            Data = new KeyBindingsData();
            LoadOrDefaultBindings(true);
        }



        public static void LoadOrDefaultBindings(bool isLoad)
        {
            try
            {
            
                if (isLoad)
                {
                    Data = SaveLoadManager.Instance().Load<KeyBindingsData>(fullPath);                    
                }

            }
            catch (Exception)
            {
                SaveKeys();
                
            }
        }

        public static void SaveKeys()
        {
            SaveLoadManager.Instance().Save(fullPath, Data);
        }

        public static string GetCommandString(PlayerCommand command)
        {
            //Maybe add if contains
            return Data.CommandBindings[command].ToString();
        }

        public static string GetControlSignalString(ControlSignals controlSignal)
        {
            //Maybe add if contains
            if (Data.KeyBindings.ContainsKey(controlSignal))
            {
                return Data.KeyBindings[controlSignal].ToString();
            }
            else
                return string.Empty;
        }


        public static string _GetTag(PlayerCommand command)
        {
            if (Data.CommandBindings.ContainsKey(command))
                return GetTag(Data.CommandBindings.Get(command));

            return string.Empty;
        }

        public static string GetTag(GKeys key)
        {
            if (KeyIconIDs.ContainsKey(key))
            {
                return Sprite.Get(KeyIconIDs[key]).ToTag();
            }
            else
            {
                return key.ToString();
            }
        }

        public static GKeys PrimaryKey(ControlSignals signal)
        {
            return Data.KeyBindings.Get(signal);
        }

        public static string GetTag(ControlSignals controlSignals)
        {
            if (KeysSettings.Data.KeyBindings.ContainsKey(controlSignals))
            {
                return GetTag(KeysSettings.Data.KeyBindings[controlSignals]);
            }
            return string.Empty;
        }
    }
}