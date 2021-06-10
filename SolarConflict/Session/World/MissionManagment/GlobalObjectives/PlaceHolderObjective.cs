
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Scenes;

namespace SolarConflict.Session.World.MissionManagment.GlobalObjectives
{
    [Serializable]
    class PlaceHolderObjective : MissionObjective
    {
        public Vector2? Position;
        public float Radius;


        public PlaceHolderObjective()
        {
            
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            return Status;
        }

        public override string GetObjectiveText()
        {
            return Text;
        }

        public override Vector2? GetPosition()
        {
            return Position;
        }

        public override float GetRadius()
        {
            return Radius;
        }
    }
}
