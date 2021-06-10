using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.Framework.Agents.Systems
{
    //Abstruct class to check if to activate an AgentSystem
    [Serializable]
    public abstract class SystemActivationCheck
    {
        public abstract bool Check(Agent agent, bool tryActivate);  //
        public abstract void DrainCost(Agent agent);
        public abstract SystemActivationCheck GetWorkingCopy();
    }
}
