using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.Agents.Systems.Misc
{
    /// <summary>
    /// Overrides the deaflut target Selector, (TODO: make target selector almost empty or maybe in agent)
    /// </summary>
    [Serializable]
    class TargetSelectorSystem : AgentSystem
    {
        //will be used for AI core
        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            if(agent.Lifetime % 61 == 0)
            {
                var flagship = gameEngine.GetFaction(agent.GetFactionType()).MothershipHanger.flagship;
                if (flagship != null)
                {
                    agent.SetTarget(flagship, TargetType.Goal);
                    var enemy = flagship.GetTarget(gameEngine, TargetType.Enemy);
                    if(enemy != null && agent.GetTarget(gameEngine, TargetType.Enemy) == null)
                    {
                        agent.SetTarget(enemy, TargetType.Enemy);
                    }
                }
            }
            return false;
        }

        public override AgentSystem GetWorkingCopy()
        {
            return this;
        }
    }
}
