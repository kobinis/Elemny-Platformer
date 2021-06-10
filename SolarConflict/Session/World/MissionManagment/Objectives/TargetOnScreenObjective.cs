using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Session.World.MissionManagment.Objectives
{
    [Serializable]
    class TargetOnScreenObjective : MissionObjective
    {
        public GameObject target; //TODO: maybe add fail if object is not alive

        public TargetOnScreenObjective(GameObject target)
        {
            Text = "OnScreen";
            this.target = target;
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            if (scene.Camera.IsOnScreen(target, -1))
            {
                Status = ObjectiveStatus.Completed;
            }
            else
                Status = ObjectiveStatus.Ongoing;
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
