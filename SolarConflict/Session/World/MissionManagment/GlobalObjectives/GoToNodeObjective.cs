using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.Session.World.MissionManagment
{
    /// <summary>
    /// Go to a spasific node
    /// </summary>
    [Serializable]
    class GoToNodeObjective:MissionObjective
    {
        public int NodeIndex;

        public GoToNodeObjective(int nodeIndex)
        {
            NodeIndex = nodeIndex;
        }

        public override string GetObjectiveText()
        {
            var node = GameSession.Inst.GalaxyMap.Nodes[NodeIndex];

            return GetStatusTag() + " Warp to " + node.Name + node.GetNodeSprite().ToTag();
        }

        public override Vector2? GetPosition()
        {
            return null;
        }

        public override float GetRadius()
        {
            return 0;
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            Status = ObjectiveStatus.Ongoing;
            if (GameSession.Inst.GalaxyMap.CurrentNodeIndex == NodeIndex)
            {
                Status = ObjectiveStatus.Completed;
            }
            return Status;
        }
    }
}
