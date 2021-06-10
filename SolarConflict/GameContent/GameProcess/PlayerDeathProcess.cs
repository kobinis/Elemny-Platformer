using SolarConflict.Session.World.MissionManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.Framework
{
    /// <summary>
    /// Add death location mission
    /// </summary>
    
    [Serializable]
    class PlayerDeathProcess : GameProcess
    {        
        public override void Update(GameEngine gameEngine)
        {           
            if (gameEngine.Scene != null && gameEngine.Scene.PlayerAgent != null && gameEngine.Scene.PlayerAgent.IsNotActive && gameEngine.Scene.IsPlayerInScene)// (PlayerAgent.ControlSignal & ControlSignals.OnDestroyed) >0)
            {
                gameEngine.Scene.AddMission(MissionFactory.PlayerDeathLocationMission(gameEngine.Scene.PlayerAgent));
            }
        }

        public override GameProcess GetWorkingCopy()
        {
            return this;
        }
    }
}
