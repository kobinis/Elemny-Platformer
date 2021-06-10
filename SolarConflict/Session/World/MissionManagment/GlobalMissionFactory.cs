using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Session.World.MissionManagment.GlobalObjectives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.Session.World.MissionManagment
{
    [Serializable]
    class GlobalMissionFactory
    {
        public static Mission GoToNodeMission(int nodeIndex, string title = null, string description = null)
        {
            Mission mission = new Mission();
            mission.Title = title;
            mission.Description = new TextAsset(description);
            mission.Icon = Sprite.Get("GalaxyMapIcon");
            mission.Objective = new GoToNodeObjective(nodeIndex);
            mission.Color = Color.Crimson;
            mission.IsGlobal = true;
            mission.DestenationNode = nodeIndex;
            return mission;
        }
    }
}
