using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Session.World.MissionManagment.Objectives
{
    [Serializable]
    class MinimumTimeObjective:MissionObjective
    {
        int _timeLeft;
        public MinimumTimeObjective(float timeoutInSec)
        {
            _timeLeft = (int)Math.Round(timeoutInSec * 60);
        }

        public override string GetActiveText()
        {
            return GetObjectiveText();
        }

        public override string GetObjectiveText()
        {
            return GetStatusTag() + " Time left: " + Math.Max( (_timeLeft / 60),0).ToString();
        }

        public override Vector2? GetPosition()
        {
            return null;
        }

        public override float GetRadius()
        {
            return 0;
        }

        public override void Update(Mission mission, Scene scene)
        {
            _timeLeft--;
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {

            Status = ObjectiveStatus.Ongoing;
            if (_timeLeft <= 0)
                Status = ObjectiveStatus.Completed;
            return Status;
        }
    }
}
