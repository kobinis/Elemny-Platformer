using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.InGameEvent.Content
{
    /// <summary>
    /// 
    /// </summary>
    class AgentPatrolProcess:GameProcess
    {
        public struct AgentAndTarget
        {
            public Agent Actor;
            public List<GameObject> Targets;
        }

        private List<AgentAndTarget> _agentList;
        private IEmitter _warpEffect;

        public AgentPatrolProcess()
        {
            _agentList = new List<AgentAndTarget>();
        }
        
        public override void InitProcess(GameEngine gameEngine)
        { 
        }

        public override void Update(GameEngine gameEngine)
        {
        }

        public override GameProcess GetWorkingCopy()
        {
            return (GameProcess)MemberwiseClone();
        }


        public static List<GameObject> FindAllPotentialTargets(GameEngine gameEngine)
        {
            return null;
        }
        
    }
}
