using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils.Graphics;

namespace SolarConflict.Session.World.MissionManagment.Objectives
{
    [Serializable]
    public class GoOutOfPositionObjective : MissionObjective
    {
        public Vector2 Position;
        public float Radius;
        public ObjectiveStatus Result;

        public GoOutOfPositionObjective(Vector2 position, float radius = 200)
        {
            Position = position;
            Radius = radius;
            Result = ObjectiveStatus.Completed;
        }

        public override string GetObjectiveText()
        {
            var text = " " + Sprite.Get(Status.ToString()).ToTag() + "Go to position " + Position.ToString();
            return text;
        }

        public override Vector2? GetPosition()
        {
            return null;
        }

        public override float GetRadius()
        {
            return Radius;
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            Status = ObjectiveStatus.Ongoing;
            var player = scene.PlayerAgent;
            if (player != null && player.IsActive && GameObject.DistanceFromEdge(player.Position, Position, player.Size, Radius) >= 0)
            {
                Status = Result;
            }
            return Status;
        }
    }
}

