using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SolarConflict.XnaUtils
{
    public class SaveLoadManager
    {
       // private ISaverLoader _saverLoader;
        private SaveLoadManager()
        {
            //if (_saverLoader == null)
            //    _saverLoader = new JsonSaverLoader();
                 

        }
        
        public void Save(string directory, string fileName, object o)
        {
            Directory.CreateDirectory(directory);
            Save(Path.Combine(directory, fileName), o);       
        }

        public void Save(string path, object o)
        {
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter(new CamelCaseNamingStrategy()) }
                , Formatting = Formatting.Indented
            };
            string json = JsonConvert.SerializeObject(o, settings);
            System.IO.File.WriteAllText(path, json);
        }

        public T Load<T>(string path)
        {
            var text = File.ReadAllText(path);
            return   JsonConvert.DeserializeObject<T>(text);
            //return _saverLoader.Load(path);
        }

        private static SaveLoadManager _instance;
        public static SaveLoadManager Instance()
        {
            if (_instance == null)
                _instance = new SaveLoadManager();
            return _instance;
        }
    }
}
