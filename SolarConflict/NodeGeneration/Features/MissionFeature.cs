using SolarConflict.Framework.World.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameWorld;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Session.World.MissionManagment;

namespace SolarConflict.NodeGeneration.Features
{
    /// <summary>
    /// adds a mission to mission bank ot to a spesific agent (child feature?)?
    /// </summary>
    class MissionFeature : GenerationFeature
    {
        public IMissionGenerator MissionToAdd { get; set; }

        

        public override GameObject GenerationLogic(Scene scene, SceneGenerator generator)
        {
            scene.AddMissionGenerator(MissionToAdd);
            return null;
        }

        //private string GenerateName(Agent agent, Scene scene)
        //{
        //    return null;
        //}
    }
}
