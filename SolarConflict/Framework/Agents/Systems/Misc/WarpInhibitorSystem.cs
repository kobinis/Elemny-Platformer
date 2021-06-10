using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.Agents.Systems.Misc
{
    [Serializable]
    class WarpInhibitorSystem : AgentSystem
    {
        public bool AlwaysOn;
        public ControlSignals Activation;

        public WarpInhibitorSystem(bool alwaysOn = true )
        {
            AlwaysOn = alwaysOn;
            Activation = ControlSignals.None;
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            // Note that system only works if our faction is hostile to the player
            if((AlwaysOn || tryActivate || (agent.ControlSignal & Activation) > 0) && (gameEngine.Scene != null)
                && (gameEngine.GetFaction(agent.FactionType).IsHostile(FactionType.Player)))
            {
                gameEngine.Scene.IsWarpDisabled = true;
                return true;
            }
            return false;
        }

        public override AgentSystem GetWorkingCopy()
        {
            return this;
        }
    }
}
