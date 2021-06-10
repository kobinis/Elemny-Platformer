using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.Session.World.MissionManagment.Objectives
{
    [Serializable]
    class GoToByIDObjective : MissionObjective
    {
        //public GameObject Target;
        public float RadiusOffset;
        public float RadiusMult;
        public bool WithSameShip;
        public bool TargetCanBeNonActive;
        private GameObject target;
        private Agent _originalAgent;
        public bool LookAtAll;
        private string id;

        /// <param name="withSameShip">If true, player must reach the objective using the Agent controlled when CheckStatus() first executes. Dying/respawning and switching Agents is permitted,
        /// but it must be the same Agent that reaches the objective (while under player control)</param>
        public GoToByIDObjective(string ID , float radiusOffset = 200, float radiusMult = 1, bool withSameShip = false, bool targetCanBeNonActive = false)
        {
            id = ID;
            //Target = target;
            RadiusOffset = radiusOffset;
            RadiusMult = radiusMult;
            WithSameShip = withSameShip;
            TargetCanBeNonActive = targetCanBeNonActive;
            LookAtAll = true;
        }

        public override Vector2? GetPosition()
        {
            return target?.Position;
        }

        public override float GetRadius()
        {
            if (target == null)
                return 0;
            else
                return target.Size * RadiusMult + RadiusOffset;
        }

        public override void Update(Mission mission, Scene scene)
        {
            if (scene.PlayerAgent == null)
                return;
            if (scene.GameEngine.FrameCounter % 20 == 0)
            {
                if(target == null)
                {
                    target = scene.FindClosestByID(id, scene.PlayerAgent.Position, LookAtAll);
                }
            }      
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            if (target == null || scene.PlayerAgent == null)
                return ObjectiveStatus.Ongoing;
            //if ( Target.IsNotActive && !TargetCanBeNonActive)
            //    return ObjectiveStatus.Failed;
            var player = scene.PlayerAgent;

            if (WithSameShip)
            {
                // Check that the player's controlling the right agent
                _originalAgent = _originalAgent ?? player;

                if (_originalAgent != player)
                    return ObjectiveStatus.Ongoing;
            }

            _originalAgent = _originalAgent ?? player;

            if (player != null && player.IsActive && GameObject.DistanceFromEdge(player.Position, target.Position, player.Size, GetRadius()) <= 0)
            {
                return ObjectiveStatus.Completed;
            }
            return ObjectiveStatus.Ongoing;
        }
    }
}

