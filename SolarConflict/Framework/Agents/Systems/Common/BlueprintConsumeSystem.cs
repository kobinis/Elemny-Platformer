using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Utils;
using System.Diagnostics;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Common
{
    /// <summary>While in inventory, checks if its stack is big enough and if the inventory owner's faction
    /// doesn't know how to build the associated hull. If so, self-consumes, teaching the faction to build the hull.
    /// </summary>
    [Serializable]
    public class BlueprintConsumeSystem : AgentSystem
    {

        string _hullID;

        public BlueprintConsumeSystem(string hullID)
        {
            _hullID = hullID;
        }

        public override AgentSystem GetWorkingCopy()
        {
            return this;
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            if (agent.GetFactionType() == FactionType.Player && agent.Lifetime % 10 == 0)
            {
                Faction agentFaction = gameEngine.GetFaction(agent.GetFactionType());
                if (!agentFaction.CheckIfHullIsKnown(_hullID))
                {
                    gameEngine.GetFaction(agent.GetFactionType()).AddHullParts(_hullID);
                    return true;
                }
            }
            return false;
        }
    }
}
