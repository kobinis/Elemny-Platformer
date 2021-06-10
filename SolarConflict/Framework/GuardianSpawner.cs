using SolarConflict.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems;
using SolarConflict.Framework.InGameEvent;
using SolarConflict.Framework.InGameEvent.Activations;
using SolarConflict.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Events
{
    class GuardianSpawner
    {
        public static SpawnAgents MakeEvent(GameObject guardedObject)
        {
            var spawner = new SpawnAgents();

            var proximityCheck = new DistanceFromGameObject();
            proximityCheck.TargetGameObject = guardedObject;
            proximityCheck.Distance = 1500f;

            var timeCheck = new TimeCheck();
            timeCheck.TimeInSeconds = 30;
            timeCheck.Condition = proximityCheck;
            timeCheck.OnFail = TimeCheck.BehaviorOnFail.Pause;

            spawner.ActivationCheck = timeCheck;

            spawner.Faction = FactionType.Pirates1;
            spawner.IsOneTime = false;
            spawner.Target = SpawnAgents.AgentTargetType.Player;
            //spwaner.SystemsToAdd() //add Loot Emitter
            spawner.ActivationCooldownTime = 1;
            spawner.Text = "Pirate";
            spawner.ActivationProbability = 0.5f;
            spawner.SpawnPositionRad = 10000;

            spawner.EmitterList.Add(ContentBank.Inst.GetEmitter("Skill_Gen")); //Change according to player level
            spawner.EmitterList.Add(ContentBank.Inst.GetEmitter("Pirate4_Gen"));

            GroupEmitter weaponLoot1 = new GroupEmitter();

            LootEmitter loot = new LootEmitter();
            loot.AddEmitter("MatA3", 0.1f, 3, 10);
            loot.AddEmitter("MatB2", 1, 3, 10);
            loot.AddEmitter("MatC1", 1, 3, 10);
            // loot.AddEmitter(weaponLoot1);
            var lootSystem = new LootSystem();
            lootSystem.LootEmitter = loot;

            spawner.SystemsToAdd.Add(lootSystem);
            return spawner;
        }
    }
}
