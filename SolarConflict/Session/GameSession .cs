using SolarConflict.Framework;
using SolarConflict.Framework.MetaGame.World;
using SolarConflict.Framework.Utils;
using SolarConflict.GameContent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Session
{
    [Serializable]
    public class GameSession 
    {
        const string METADATA_FILEMANE = "\\metadata.bin";        
        #region Singelton
        static GameSession _instance;
        public static GameSession Inst
        {
            get
            {
                if (_instance == null)
                    _instance = new GameSession();
                    //throw new Exception("You must call init before accsessing GameSession");
                return _instance;
            }
        }
        #endregion
        public string SavePath { get; private set; }
        private MetaWorld _metaWorld;
        public MetaWorld MetaWorld { get { return _metaWorld; } }
        private GalaxyMap _galaxyMap; //TODO: move to meta world

        public GalaxyMap GalaxyMap {
            get { return _galaxyMap; }
        }
        
        public static void Init(string savePath)
        {
            _instance = new GameSession();
            FactionContent.LoadFactionData();
        }        

        private GameSession() {
            _metaWorld = new MetaWorld();
            _galaxyMap = new GalaxyMap();            
        }
        
        public void Save()
        {


            string path = SavePath;
            SaveMetadata(path);
            //Save current node
            //string worldName = @"\Node" + GalaxyMap.CurrentNodeIndex.ToString() + ".bin";
            //var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            //var file = new FileStream(path + worldName, FileMode.Create, FileAccess.Write, FileShare.None);
            //formatter.Serialize(file, GalaxyMap.CurrentScene);
            //file.Close();

            //Save global state
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.SurrogateSelector = SerializationUtils.MakeSurrogateSelector();
            var file = new FileStream(path + @"\MetaWorld.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(file, this);
            file.Close();              
        }
        
        private void SaveMetadata(string path)
        {
            SessionMetadata metaData = new SessionMetadata(path);
            metaData.SaveVersionInt = DebugUtils.SaveVersion;
            metaData.SaveTime = DateTime.Now;
            metaData.StarDate = MetaWorld.Stardate;
            metaData.SaveVersion = DebugUtils.Version;
            metaData.SpriteID = GalaxyMap.Nodes[GalaxyMap.CurrentNodeIndex].GetNodeSprite().ID;            

            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.SurrogateSelector = SerializationUtils.MakeSurrogateSelector();
            var file = new FileStream(path + METADATA_FILEMANE, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(file, metaData);
            file.Close();
        }

        public static SessionMetadata LoadMetaData(string path)
        {
            try
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.SurrogateSelector = SerializationUtils.MakeSurrogateSelector();
                var file = new FileStream(path + METADATA_FILEMANE, FileMode.Open, FileAccess.Read, FileShare.Read);
                var result = formatter.Deserialize(file) as SessionMetadata;
                result.Path = path;
                file.Close();
                if (result.SaveVersionInt < DebugUtils.SaveVersion)
                    return null;
                return result;
            }
            catch (Exception)
            {

                return null;
            }
        }        

        public void Continue()
        {            
            GalaxyMap.Continue();          
        }    

        public static void Load(string path)
        {            
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.SurrogateSelector = SerializationUtils.MakeSurrogateSelector();
            var file = new FileStream(path + @"\MetaWorld.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
            var session = formatter.Deserialize(file) as GameSession;
            file.Close();
            _instance = session;
            session.SavePath = path;
            session.Continue();
            _instance = session;
        }

        public static void NewSession(string path, string loadoutID)
        {
            _instance = new GameSession();
            FactionContent.LoadFactionData();
            if(loadoutID != null)
                _instance.MetaWorld.GetFaction(FactionType.Player).MothershipHanger.AddShipCopyToSlot(0, loadoutID);
            _instance.GalaxyMap.GenerateGalaxy();
            _instance.SavePath = path;
            //_instance.GalaxyMap.GenerateAllScenes();
        }

        //Generates a
        public static void GenerateNewWorld()
        {

        }

    }
}
