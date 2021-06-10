using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.InGameEvent
{
    public interface IEventActivationCheck
    {
        bool CheckActivation(Agent agent, GameEngine gameEngine);

        IEventActivationCheck GetWorkingCopy();
    }
}
