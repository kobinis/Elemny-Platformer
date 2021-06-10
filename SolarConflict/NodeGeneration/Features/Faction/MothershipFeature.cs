using SolarConflict.Framework.World.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameWorld;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc;
using SolarConflict.Framework;
using XnaUtils;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.Framework.Agents.Systems;
using System.Diagnostics;
using SolarConflict.Generation;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems;
using SolarConflict.NodeGeneration.NodeProcesess;

namespace SolarConflict.NodeGeneration.Features
{
    /// <summary>
    /// Mothership and including fleet
    /// </summary>
    class MothershipFeature: GenerationFeature
    {
        public string ID;
        public float AggroRange = 8000;
        public FleetCommandType FleetCommand;
        public IEmitter AgentEmitter;
        public string AgenEmitterID { set { AgentEmitter = ContentBank.Inst.GetEmitter(value); } }

        public MothershipFeature()
        {
            FleetCommand = FleetCommandType.Patrol;
            
        }
        public MothershipFeature(string agentEmitterID, FactionType? faction = null):this()
        {
            AgenEmitterID = agentEmitterID;
            localFaction = faction;
        }

        public override GameObject GenerationLogic(Scene scene, SceneGenerator generator)
        {            
            Agent mothership = AgentEmitter.Emit(scene.GameEngine, null, Faction, Position, Vector2.Zero, Rotation, param: Level) as Agent;

            //mothership.AddSystem(new ShowTargetSystem());
            if (ID != null)
                mothership.ID = ID;
            mothership.Name = GenerateName(scene);

            

            if (AggroRange > 0)
            {
                mothership.SetAggroRange(AggroRange, TargetType.Enemy);// AggroRange, TargetType.Enemy);                
                //mothership.SetAggroRange(float.MaxValue / 2, TargetType.Goal);
            }
            //Fleet and slots
            var factionGenData = MetaWorld.Inst.GetFaction(Faction).GenerationData;
            FleetSystem fleetSystem = new FleetSystem();
            fleetSystem.AddAgentSystem(new SlotItemDropSystem(ControlSignals.OnDestroyed, 0.9f, false));
            //fleetSystem.FleetSlots.Add(new FleetSystem.FleetSlot(SizeType.Medium));
            //fleetSystem.FleetSlots.Add(new FleetSystem.FleetSlot(SizeType.Medium));
            var loot = new LootSystem("FactionBaseLoot");
            mothership.AddSystem(loot);
            mothership.AddSystem(fleetSystem);
            //mothership.AddSystem(new ShowTargetSystem());
            var missionBank = new MissionBankSystem();
            missionBank.AddMissionFactory(new DestroyMissionFactory());
         //   scene.GameEngine.AddGameProcces(new GenerateFetchMissionProcess(mothership));
         //   scene.GameEngine.AddGameProcces(new GenerateFetchMissionProcess(mothership));
            mothership.AddSystem(missionBank);

            ActivitySwitcherSystem switcher = new ActivitySwitcherSystem("AgentMissionActivity");            
            mothership.InteractionSystem = switcher;

            var fleetShips = FleetGenerator.GenerateShips(scene.GameEngine, mothership, Faction, Level);

            fleetShips.Do(s => {
                fleetSystem.FleetSlots.Add(new FleetSystem.FleetSlot(SizeType.Large));

                fleetSystem.SetSlotShip(fleetSystem.FleetSlots.Count - 1, s);
            });
            
            fleetSystem.SetCommand(FleetCommand);

            return mothership;
        }

        private string GenerateName(Scene scene)
        {
            return MetaWorld.Inst.GetFaction(Faction).Name  + " Mothership";
        }

        

        

    }
}
