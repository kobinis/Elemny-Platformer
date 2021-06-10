using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.NodeGeneration.Features
{
    class ArenaFeature: AgentFeature
    {
        public ArenaFeature():base()
        {
            AgenEmitterID = "Arena";
            ActivitySwitcherSystem activitySwitcherSystem = new ActivitySwitcherSystem("ChallengeSelectionActivity");
           // activitySwitcherSystem.ActivityName = "ChallengeSelectionActivity";
            AddAgentSystem(activitySwitcherSystem);

        }        
    }
}
