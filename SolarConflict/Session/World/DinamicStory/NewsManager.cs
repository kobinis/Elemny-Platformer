using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict
{
    [Serializable]
    public struct NewsEntry
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageID { get; set; }
        public StarDate Date { get; set; }    
        public int FactionID { get; set; }
        //TimeBeforeClear
    }

    [Serializable]
    public class NewsManager
    {
        private List<NewsEntry> _newsEnteryList;
        
        public NewsManager()
        {
            _newsEnteryList = new List<NewsEntry>();
        }
        

    }
}
