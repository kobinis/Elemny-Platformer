using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.Session.World.MissionManagment.Objectives
{
    [Serializable]
    class DestroyTargetObjective:MissionObjective
    {
        public GameObject Target;
        public float Offset;  

        public DestroyTargetObjective(GameObject target, float offset = 0)
        {
            Target = target;
            Offset = offset;           
        }
      
        public override string GetObjectiveText()
        {
            return GetStatusTag() + " Destroy Target " + Target?.GetSprite().ToTag();            
        }

        public override Vector2? GetPosition()
        {
            if (Target == null)
                return null;
            return Target.Position;
        }

        public override float GetRadius()
        {
            return Target.Size + Offset;
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            Status = ObjectiveStatus.Ongoing;
            if (Target== null || Target.IsNotActive)
            {
                Status = ObjectiveStatus.Completed;
            }
            return Status;
        }

    }
}
