using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.Session.World.MissionManagment.Objectives
{
    [Serializable]
    class GoToTargetObjective : MissionObjective
    {
        public GameObject Target;
        public float RadiusOffset;
        public float RadiusMult;
        public bool WithSameShip;
        public bool TargetCanBeNonActive;

        [NonSerialized]
        Agent _originalAgent; //Save hash insted 

        /// <param name="withSameShip">If true, player must reach the objective using the Agent controlled when CheckStatus() first executes. Dying/respawning and switching Agents is permitted,
        /// but it must be the same Agent that reaches the objective (while under player control)</param>
        public GoToTargetObjective(GameObject target, float radiusOffset = 400, float radiusMult = 1, bool withSameShip = false, bool targetCanBeNonActive = false)
        {
            Target = target;
            RadiusOffset = radiusOffset;
            RadiusMult = radiusMult;
            WithSameShip = withSameShip;
            TargetCanBeNonActive = targetCanBeNonActive;
        }

        public override string GetObjectiveText()
        {
            //Distance to objective
            return Sprite.Get(Status.ToString()).ToTag() + " Go to " + Target.Name + Target.GetSprite().ToTag();
        }

        public override Vector2? GetPosition()
        {
            if (Target == null)
                return null;
            return Target.Position;
        }

        public override float GetRadius()
        {
            return Target.Size * RadiusMult + RadiusOffset;
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            if (Target.IsNotActive && !TargetCanBeNonActive)
                return ObjectiveStatus.Failed;
            var player = scene.PlayerAgent;

            if (WithSameShip)
            {
                // Check that the player's controlling the right agent
                _originalAgent = _originalAgent ?? player;

                if (_originalAgent != player)
                    return ObjectiveStatus.Ongoing;
            }

            _originalAgent = _originalAgent ?? player;

            if (player != null && player.IsActive && GameObject.DistanceFromEdge(player.Position, Target.Position, player.Size, GetRadius()) <= 0)
            {
                return ObjectiveStatus.Completed;
            }
            return ObjectiveStatus.Ongoing;
        }
    }
}

