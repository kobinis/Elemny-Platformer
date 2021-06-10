using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Session.World.MissionManagment.Objectives
{
    [Serializable]
    class ProtectTargetObjective : MissionObjective
    {
        public GameObject Target;
        public bool ShowTarget;

        public ProtectTargetObjective(GameObject target)
        {
            Target = target;
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            if (Target.IsActive)
                Status = ObjectiveStatus.Completed;
            else
                Status = ObjectiveStatus.Failed;
            return Status;
        }

        public override Vector2? GetPosition()
        {
            if (ShowTarget)
                return Target.Position;
            return null;
        }

        public override float GetRadius()
        {
            return Target.Size;
        }
    }
}
