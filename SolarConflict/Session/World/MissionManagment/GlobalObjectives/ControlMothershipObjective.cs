using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Session.World.MissionManagment.GlobalObjectives
{
    [Serializable]
    class ControlMothershipObjective : MissionObjective
    {        
        public ControlMothershipObjective()
        {
            Text = "Control Mothership";
        }
            
        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            if (scene.PlayerAgent != null && (scene.PlayerAgent.GetObjectType() & GameObjectType.Mothership) > 0)
                Status = ObjectiveStatus.Completed;            
            return Status;
        }
        
        public override Vector2? GetPosition()
        {
            return null;
        }

        public override float GetRadius()
        {
            return 0;
        }
    }
}
