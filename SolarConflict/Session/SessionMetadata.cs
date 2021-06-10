using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Session
{
    /// <summary>Lightweight data about a session/save file, for previewing sessions without loading them entirely</summary>
    [Serializable]
    public class SessionMetadata
    {
        /// <remarks>We set path when loading a file, rather than asking the file where it is after we've found and loaded it</remarks>
        [NonSerialized]
        public string Path;
        public DateTime SaveTime;
        public string SaveVersion;
        public int SessionIndex;
        public StarDate StarDate;
        public string SpriteID;
        public int SaveVersionInt;
        
        public SessionMetadata(string path)
        {
            Path = path;
        }
    }
}
