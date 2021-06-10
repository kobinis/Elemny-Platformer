using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict
{    
    public interface IAgentControl
    {
        int ID { get; set; }
        ControlSignals Update(Agent agent, GameEngine gameEngine, ref Vector2[] analogDirections);        
        IAgentControl GetWorkingCopy();        
    }
}
