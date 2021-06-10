using Microsoft.Xna.Framework;
using SolarConflict.Session;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework
{    
          
    public class PersistenceManager //Move to game session? as static
    {
        #region Singelton
        private static PersistenceManager _instance;
        public static PersistenceManager Inst
        {
            get
            {
                if (_instance == null)
                    _instance = new PersistenceManager();
                return _instance;
            }
        }
        #endregion

        private string _savesDirectory = Consts.GAME_DATA_PATH + @"\Saves"; //TODO: use path utils        

        public PersistenceManager()
        {            
        }

        public string GetSavePath(int index)
        {
            return _savesDirectory + @"\World" + index;
        }

        public bool NewSession(string loadoutID)
        {                        
            int freeIndex = FindFreeSlot();
            if (freeIndex == -1)
            {
                ActivityManager.Inst.AddToast("No free slot", 60 * 2);
            }

            GameSession.NewSession(GetSavePath(freeIndex), loadoutID);
            return true;
        }

        private int FindFreeSlot()
        {
            int freeIndex = -1;            
            for (int i = 0; i < 100; i++)
            {
                if (!Directory.Exists(GetSavePath(i)))
                {
                    freeIndex = i;
                    break;
                }
            }
            return freeIndex;
        }


        
        public void LoadSession(string path)
        {
            GameSession.Load(path);            
        }

        public void DeleteSession(string path)
        {
            try
            {
                Directory.Delete(path, true);
            }
            catch (Exception)
            {

                //throw;
            }
            
        }
        

        public void Save()
        {            
            if (!Directory.Exists(GameSession.Inst.SavePath))
            {
                Directory.CreateDirectory(GameSession.Inst.SavePath);
            }
            GameSession.Inst.Save();
            ActivityManager.Inst.AddToast("Game Saved", 60 * 2, Color.Green);
        }

        public List<SessionMetadata> GetAllSavesMetadata()//??
        {            
            string path = _savesDirectory;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var directorys = Directory.GetDirectories(path).ToList();
            List<SessionMetadata> sessionsMetadataList = new List<SessionMetadata>();
           
                foreach (var dir in directorys)
                {
                    try
                    {
                        var data = GameSession.LoadMetaData(dir);
                        if ( data != null)
                            sessionsMetadataList.Add(data);
                    }
                    catch
                    {
                        ActivityManager.Inst.AddToast("Some of the saves are deprecated", 100);
                    }
                }
           
            
            sessionsMetadataList = sessionsMetadataList.OrderByDescending(e => e.SaveTime).ToList();
            return sessionsMetadataList;
        }

        public void Continue()
        {
            try
            {
                var metadataList = PersistenceManager.Inst.GetAllSavesMetadata();
                if (metadataList.Count > 0)
                {
                    PersistenceManager.Inst.LoadSession(metadataList[0].Path);
                    GameSession.Inst.Continue();
                    return;
                }
                ActivityManager.Inst.AddToast(Color.Red.ToTag("No games to continue!"), 90);
            }
            catch (Exception)
            {
                ActivityManager.Inst.AddToast("Unable to load game", 100);
            }
        }
        

    }
}
