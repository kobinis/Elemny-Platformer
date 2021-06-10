using SolarConflict.Framework.World.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.GameWorld;
using XnaUtils;
using SolarConflict.Session.World.MissionManagment;
using SolarConflict.Session.World.MissionManagment.Objectives;
using SolarConflict.NodeGeneration.NodeProcesess;
using SolarConflict.Framework;

namespace SolarConflict.NodeGeneration.Features
{
    class StarportFeature : AgentFeature
    {
        public bool AddMission;
        public StarportFeature(bool addMisson = false) //TODO: add roads to starport
        {
            AddMission = addMisson;
            AgenEmitterID = "Starport";
            ActivitySwitcherSystem switcher = new ActivitySwitcherSystem("StarportActivity");
            AddAgentSystem(switcher);
        }
        public override GameObject GenerationLogic(Scene scene, SceneGenerator generator)
        {     
            var starport = base.GenerationLogic(scene, generator) as Agent;
            starport.ID = "starport";
            starport.gameObjectType |= GameObjectType.Starport;
            starport.Name = "Starport";
      //      scene.GameEngine.AddGameProcces(new GenerateFetchMissionProcess(starport));
      //      scene.GameEngine.AddGameProcces(new GenerateFetchMissionProcess(starport));
      //      scene.GameEngine.AddGameProcces(new GenerateFetchMissionProcess(starport));
            //if (AddMission)
            //{
            //    var m = new Mission("Go to the Starport", new TextAsset("Dock with the starport to get missions"));
            //    m.ID = "GotoStarport";
            //    m.IsDismissable = true;
            //    m.Objective = new GoToTargetObjective(starport);
            //    scene.AddMission(m);
            //}  
            return starport;
        }
    }
}
