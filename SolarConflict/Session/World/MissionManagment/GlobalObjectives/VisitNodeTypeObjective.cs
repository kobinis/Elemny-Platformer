using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.MetaGame.World;
using SolarConflict.Framework.World.MetaGame;
using XnaUtils;

namespace SolarConflict.Session.World.MissionManagment.GlobalObjectives
{
    /// <summary>
    /// Completed once node of a spesific type is visited
    /// </summary>
    [Serializable]
    class VisitNodeTypeObjective : MissionObjective
    {
        private NodeType nodeType;

        public VisitNodeTypeObjective(NodeType type)
        {
            
            nodeType = type;
            Text = "Visit " + type.GetUserName() + " type sector";
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
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

        public override void OnEnterNode()
        {
            NodeInfo node = GalaxyMap.Inst.GetCurrentNodeInfo();
            if (node != null && node.Type == nodeType)
                Status = ObjectiveStatus.Completed;
        }

    }
}
