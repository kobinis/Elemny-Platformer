using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.Agents.Systems.Misc
{
    /// <summary>
    /// Deploys a spesific object (Not a copy) to be used
    /// </summary>
    class DeployObjectSystem : AgentSystem
    {
        
        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            throw new NotImplementedException();
        }

        public override AgentSystem GetWorkingCopy()
        {
            throw new NotImplementedException();
        }
    }
}
