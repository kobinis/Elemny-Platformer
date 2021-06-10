using SolarConflict.Framework.World.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameWorld;
using SolarConflict.Framework.Agents.Systems.Misc;
using SolarConflict.Session.World.MissionManagment;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.Framework.Emitters.SceneRelated;
using SolarConflict.Framework.Agents.Systems.Engines;
using SolarConflict.Generation;
using XnaUtils;

namespace SolarConflict.NodeGeneration.Features
{
    class WarpInhibitorFeature:AgentFeature
    {
        public string description;
        public bool addMission;
        public WarpInhibitorFeature(bool addMission = true )
        {
            
            this.addMission = addMission;

            AgentEmitter = ContentBank.Inst.GetEmitter("Inhib0A");
            localFaction = FactionType.Void; //??
            var system = new WarpInhibitorSystem();
            //Add Loot
            //Add to scene Warp messege or to warp inhibitor agent
            AddAgentSystem(system);

            AddAgentSystem(new AnchorEngine());

            //SlotItemDropSystem dropSystem = new SlotItemDropSystem(ControlSignals.OnDestroyed);
            //AddAgentSystem(dropSystem);
            var loot = new LootSystem();
            //IEmitter lootEmitter = ContentBank.Inst.GetEmitter("WarpInhibLoot");
            IEmitter lootEmitter = ContentBank.Inst.GetEmitter("WarpInhibLoot");
            
            
            loot.LootEmitter = lootEmitter;
            AddAgentSystem(loot);
            
            StringBuilder sb = new StringBuilder();
            sb.Append("The Warp Inhibitor prevents the activations of warp drives in this sector.\n");
            sb.Append("You must " + Color.Red.ToTag("destroy the Warp Inhibitor!"));
            description = sb.ToString();
            localFaction = FactionType.Void;
        }

        public override GameObject GenerationLogic(Scene scene, SceneGenerator generator)
        {
            float rotation = (float)Math.Atan2(generator.PlayerStartingPoint.Y, generator.PlayerStartingPoint.X);

            var emmiters = TextBank.Inst.GetString($"Inhib{generator.Level}").Split(',');
            AgenEmitterID = emmiters[generator.Rand.Next(emmiters.Length)];
            Position = FMath.ToCartesian(generator.SunRadius * 2 + 5000, rotation);
            var agent = base.GenerationLogic(scene, generator) as Agent;            
            agent.Name = "Warp Inhibitor " + Level.ToString();                       
            AddEscortingShips(scene.GameEngine, agent, generator.Level);
            if (addMission)
            {

                Mission mission = MissionFactory.DestroyTargetObjective(agent, "Destroy the Warp Inhibitor", description);
                mission.MissionCompleteText = "Warp inhibitor destroyed!\nMission complete";
                mission.IsDismissable = true;
                //mission.MissionCompleteSoundEffect = "WarpInhibDestroyed";
                scene.AddMissionGenerator(mission); //.GeneratorBank.Add(mission
            }

            return agent;
        }

        private void AddEscortingShips(GameEngine gameEngine, GameObject parent, int level)
        {
            TextAsset asset = TextBank.Inst.GetTextAsset($"VoidShips{level}");
            int num = ParserUtils.ParseInt(asset.ImageID, 1);
            string ships = asset.Text;
            FleetGenerator.GenerateFleet(gameEngine, parent, ships, num, level);
            //FleetGenerator.GenerateShips(gameEngine, parent, Faction, gameEngine.Level);
        }
        

    }
}
