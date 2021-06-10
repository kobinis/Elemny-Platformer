using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Session.World.MissionManagment.GlobalObjectives
{
    [Serializable]
    class ControlShipObjective : MissionObjective
    {
        private GameObject _objectToControl;
        public ControlShipObjective(GameObject objectToControl)
        {
            _objectToControl = objectToControl;
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            if (scene.PlayerAgent == _objectToControl)
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
