using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Scenes;

namespace SolarConflict.Session.World.MissionManagment.GlobalObjectives
{
    [Serializable]
    class ControlSignalObjective : MissionObjective
    {
        public ControlSignals _controlSignals;

        public ControlSignalObjective(ControlSignals controlSignals)
        {
            _controlSignals = controlSignals;
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            if (Status == ObjectiveStatus.Ongoing)
            {
                if (scene.PlayerAgent != null)
                {
                    if ((scene.PlayerAgent.ControlSignal & _controlSignals) > 0)
                        Status = ObjectiveStatus.Completed;
                }
            }
            return Status;
        }

        public override string GetObjectiveText()
        {
            return null;
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
