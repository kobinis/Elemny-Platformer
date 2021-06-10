using SolarConflict.Framework;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.Achievements
{
    public class Achievement
    {
        public string textAssetID;

        public string DisplayName;
        public string Description;
        public bool IsHidden;
        public string AchievedIcon;
        public string UnachievedIcon;

        public int ProgressStat;

        public virtual bool Evaluate()
        {
            return false;
        }

        public Achievement()
        {
            
        }
    }
}
