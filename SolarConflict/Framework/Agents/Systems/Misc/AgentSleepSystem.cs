using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc
{
    /// <summary>
    /// puts agents to sleep when
    /// </summary>
    [Serializable]
    public class AgentSleepSystem: AgentSystem
    {
        public int BackToSleepTime;
        private int _timer = 0;

        public AgentSleepSystem()
        {
            BackToSleepTime = 60 * 20;
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            if(agent.listType == CollisionType.CollideAll)
            {
                if(!gameEngine.Camera.IsOnScreen(agent.Position, agent.Size))
                {
                    _timer--;
                    if(_timer <= 0)
                    {
                        agent.listType = CollisionType.UpdateOnlyOnScreen;
                    }
                }
            }
            else
            {
                if(agent.listType == CollisionType.UpdateOnlyOnScreen)
                {
                    agent.ListType = CollisionType.CollideAll;
                    _timer = BackToSleepTime;
                }
            }

            return false;
        }

        public override AgentSystem GetWorkingCopy()
        {
            return (AgentSystem)MemberwiseClone();
        }
    }
}
