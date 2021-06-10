using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems;
using SolarConflict.GameContent.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameWorld;
using SolarConflict.Session.World.MissionManagment;
using XnaUtils.Graphics;
using SolarConflict.Framework.InGameEvent;
using SolarConflict.Framework.InGameEvent.Activations;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.Framework.Emitters;

namespace SolarConflict.NodeGeneration.Features.Missions
{
    class MinerBossFeature:AgentFeature
    {
        public MinerBossFeature()
        {
            AgenEmitterID = "MinerBoss";
            LootSystem loot = new LootSystem();
            loot.LootEmitter = ContentBank.Inst.GetEmitter(typeof(DevastationCore).Name);
            AddAgentSystem(loot);
            this.SetFaction(Framework.FactionType.MinerGuild);

            // Create event for respawning the boss
            var respawnEvent = new SpawnAgents();
            respawnEvent.SpawnPositionRad = 5000f;

            var proximityCheck = new DistanceFromPoint();
            proximityCheck.Point = Position;
            proximityCheck.Distance = 10000f;

            var timeCheck = new TimeCheck();
            timeCheck.TimeInSeconds = 60;
            timeCheck.Condition = proximityCheck;
            timeCheck.OnFail = TimeCheck.BehaviorOnFail.Pause;

            respawnEvent.ActivationCheck = timeCheck;
            respawnEvent.ActivationProbability = 0.5f;
            respawnEvent.IsOneTime = true;

            // Then install a system on the agent for triggering that event (the event'll reinstall said system on whatever it spawns)
            var respawnSystem = new BasicEmitterCallerSystem();
            respawnSystem.Activation = ControlSignals.OnDestroyed;
            respawnSystem.Emitter = respawnEvent;

            var systemsEmitter = new AddSystemsEmitter();
            systemsEmitter.SystemsToAdd.Add(respawnSystem);
            systemsEmitter.Emitter = AgentEmitter;

            respawnEvent.Emitter = systemsEmitter;

            AddAgentSystem(respawnSystem);
        }

        public override GameObject GenerationLogic(Scene scene, SceneGenerator generator)
        {
            string missiondescription = "Ancient Miners are huge mining drones\n"
                + "They were used in the golden age of man, before the great war\n" +
                "Despite their age they are formidable\nThey use the surrounding asteroids to generate ammo for their Devastator weapon\n" +
                "Destroy them to obtain Devastation Cores" + Sprite.Get("DevastationCore").ToTag();
            int fromLevel = 6;
            if (generator.Level >= fromLevel)
                AgenEmitterID = "MinerBossElite";
            else
                AgenEmitterID = "MinerBoss";
            GameObject boss = base.GenerationLogic(scene, generator);
            if (generator.Level >= fromLevel)
            {
                boss.Name = "Elite Ancient Miner";
                var agent = boss as Agent;
                agent.AddSystem(new Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers.BlueprintEmitterSystem(agent.ID + "BlueprintPart"));

            }
            else
                boss.Name = "Ancient Miner";

          

            var mission = MissionFactory.DestroyTargetObjective(boss, "Destroy the Ancient Miner", missiondescription);
            //scene.MissionManager.GeneratorBank.Add(mission);                 
            mission.IsDismissable = true;
            scene.AddMissionGenerator(mission);
            boss.SetAggroRange(2000, 22000, TargetType.Enemy);

            return boss;
        }
    }
}
