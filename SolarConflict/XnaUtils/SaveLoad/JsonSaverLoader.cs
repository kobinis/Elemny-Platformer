//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.XnaUtils
//{
//    class JsonSaverLoader : ISaverLoader
//    {
//        public object Load(string path)
//        {
//            string s = File.ReadAllText(path);
//            return JSON.ToObject(s);
//        }

//        public void Save (string path, object o)
//        {
//            path += ".save";
//            string json = JSON.ToJSON(o);
//            File.WriteAllText(path, json);
//        }
//    }
//}
