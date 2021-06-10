//using SolarConflict.XnaUtils;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.Framework.Utils
//{
//    public class PreferencesManager
//    {
//        private const string _prefsFileName = "preferences"; // TODO: move to gamedata
//        private const string _prefsFileExtension = ".save";
//        private readonly string _prefsSaveDir = Consts.GAME_DATA_PATH;
//        private PreferencesStorage _prefsStorage;

//        private PreferencesManager()
//        {
//            //_prefsSaveDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), System.Diagnostics.Process.GetCurrentProcess().ProcessName);//System.AppDomain.CurrentDomain.FriendlyName);
//            //if (!Directory.Exists(_prefsSaveDir))
//            //{
//            //    Directory.CreateDirectory(_prefsSaveDir);
//            //}

//            //try
//            //{//TODO: fix extension issue - we shouldn't have to add it here manually
//            //    _prefsStorage = (PreferencesStorage)SaveLoadManager.Instance().Load(Path.Combine(_prefsSaveDir, _prefsFileName + _prefsFileExtension));
//            //}
//            //catch (FileNotFoundException)
//            //{
//            //    _prefsStorage = new PreferencesStorage();
//            //    LoadDefaults();
//            //}

//            _prefsStorage = new PreferencesStorage();
//            LoadDefaults();
//            _prefsStorage.MusicVolume = 0.5f;

//        }

//        private void LoadDefaults()
//        {
//            _prefsStorage.LoadDefaults();
//        }

//        private static PreferencesManager _instance;

//        public PreferencesStorage PrefsStorage
//        {
//            get
//            {
//                return _prefsStorage;
//            }

//            set
//            {
//                _prefsStorage = value;
//            }
//        }

//        public static PreferencesManager Inst 
//        {
//            get
//            {
//                if (_instance == null)
//                    _instance = new PreferencesManager();
//                return _instance;
//            }
//        }
        
//        public void SavePreferences()
//        {
//            //SaveLoadManager.Instance().Save(_prefsSaveDir, _prefsFileName, _prefsStorage);
//        }
//    }

//    public class PreferencesStorage
//    {
//        public void LoadDefaults()
//        {
//            MusicVolume = 0.5f;
//            IsFullScreen = false;
//        }

//        public float MusicVolume { get; set; }
//        public bool IsFullScreen { get; set; }
//    }
//}
