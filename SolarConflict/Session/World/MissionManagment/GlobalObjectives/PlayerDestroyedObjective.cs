using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Session.World.MissionManagment.GlobalObjectives
{
    [Serializable]
    class PlayerDestroyedObjective : MissionObjective
    {
        public ObjectiveStatus StatusOnDestroyed;
        private bool wasPlayerDestroyed;                
        public PlayerDestroyedObjective(ObjectiveStatus statusOnDestroyed)
        {
            wasPlayerDestroyed = false;
            StatusOnDestroyed = statusOnDestroyed;
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            if (scene.PlayerAgent != null && scene.PlayerAgent.GetHitpoints() <= 0)
                wasPlayerDestroyed = true;
            if (wasPlayerDestroyed)
                Status = StatusOnDestroyed;
            else
                Status = ObjectiveStatus.Ongoing;
            return Status;
        }

        public override string GetObjectiveText()
        {
            return GetStatusTag();
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
