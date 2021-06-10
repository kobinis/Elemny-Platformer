using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.Session.World.MissionManagment.Objectives
{
    [Serializable]
    class DefendTargetObjective : MissionObjective
    {
        public GameObject Target;

        public DefendTargetObjective(GameObject target)
        {
            Target = target;
        }

        public override string GetObjectiveText()
        {
            return GetStatusTag() + " Defand Target " + Target.GetSprite().ToTag();
        }

        public override Vector2? GetPosition()
        {
            return Target.Position;
        }

        public override float GetRadius()
        {
            return Target.Size;
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            Status = ObjectiveStatus.Completed;
            if (Target.IsNotActive)
            {
                Status = ObjectiveStatus.Failed;
            }
            return Status;
        }

    }
}

