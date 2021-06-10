using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Session.World.MissionManagment.Objectives
{
    [Serializable]
    class PlayerIsLiveObjective : MissionObjective
    {
        public ObjectiveStatus StatusOnDeath;

        public PlayerIsLiveObjective(ObjectiveStatus statusOnDeath = ObjectiveStatus.Ongoing)
        {
            StatusOnDeath = statusOnDeath;            
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            if(scene.PlayerAgent != null)                
            {
                if (scene.PlayerAgent.IsActive)
                    Status = ObjectiveStatus.Completed;
                else
                    Status = StatusOnDeath;
            }
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
