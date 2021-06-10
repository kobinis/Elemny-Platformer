using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.Achievements
{

    public class AchievementsManager
    {
        #region Singelton
        private static AchievementsManager instanse = null;
        public static AchievementsManager Inst
        {
            get
            {
                if (instanse == null)
                {
                    instanse = new AchievementsManager();
                }
                return instanse;
            }
        }
        #endregion
    }
}
