using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.Agents
{
    public interface IInteractionSystem
    {
        string GetInteractionText(Agent agent, GameEngine gameEngine, Agent playerAgent);
        bool Interact(Agent agent, GameEngine gameEngine, Agent playerAgent);
    }
}
