using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils.Graphics;
using System.Diagnostics;
using SolarConflict.Framework;

namespace SolarConflict.Session.World.MissionManagment.Objectives
{
    /// <summary>
    /// A goal to pass a vertical limit on screen
    /// </summary>
    [Serializable]
    public class VerticalGoalObjective : MissionObjective
    {

        public float VerticalPosition;
        public ObjectiveStatus StatusAbove;
        public ObjectiveStatus StatusBelow;
    
        private Vector2 _goalMarkerPosition;

        public VerticalGoalObjective(float verticalPosition)
        {
            VerticalPosition = verticalPosition;
            StatusAbove = ObjectiveStatus.Completed;
            StatusBelow = ObjectiveStatus.Ongoing;
        }

        public override string GetObjectiveText()
        {
            return Text;
        }

        public override Vector2? GetPosition()
        {
            return _goalMarkerPosition;
        }

        public override float GetRadius()
        {
            return 0;
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            Status = ObjectiveStatus.Ongoing;
            var player = scene.PlayerAgent;
            if (player != null && player.IsActive)
            {
                Text = "Distance:" + (Math.Abs(player.Position.Y - VerticalPosition) * Consts.PixelsToUinits).ToString("0.##");
                _goalMarkerPosition = new Vector2(player.Position.X, VerticalPosition);
                if (player.Position.Y < VerticalPosition)
                    Status = StatusAbove;
                else
                    Status = StatusBelow;
            }
            return Status;
        }
    }
}
