using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Session.World.MissionManagment.Objectives
{
    [Serializable]
    class TimeObjective : MissionObjective
    {
        int _timeLeft;
        public TimeObjective(float timeoutInSec)
        {
            _timeLeft = (int)Math.Round( timeoutInSec * 60);
        }

        public override string GetObjectiveText()
        {
            return GetStatusTag() + " Time Left: " + (Math.Max( _timeLeft / 60,0)).ToString();
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
            
            Status = ObjectiveStatus.Completed;
            if (_timeLeft <= 0)
                Status = ObjectiveStatus.Failed;           
            return Status;
        }
    }
}
