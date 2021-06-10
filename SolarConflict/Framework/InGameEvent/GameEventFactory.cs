using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems;
using SolarConflict.Framework.InGameEvent;
using SolarConflict.Framework.InGameEvent.Activations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.InGameEvent.Content
{
    //TODO: move to Content    
    class GameEventFactory
    {
        public static InGameEventSpawner MakeGuild()
        {
            var spawner = new SpawnAgents();
            spawner.Faction = FactionType.TradingGuild;
            spawner.IsOneTime = true;
            spawner.Target = SpawnAgents.AgentTargetType.Player;
            //spwaner.SystemsToAdd() //add Loot Emitter
            spawner.ActivationCooldownTime = 60  * 10* 3;
            spawner.Text = "Guild";
            spawner.ActivationProbability = 0.1f;
            spawner.GoalPositionRad = 200000;
            spawner.SpawnPositionRad = 10000;

             spawner.EmitterList.Add(ContentBank.Inst.GetEmitter("SmallShip6A")); //Change according to player level
           // spawner.EmitterList.Add(ContentBank.Inst.GetEmitter("PirateRaider1"));

         //   GroupEmitter weaponLoot1 = new GroupEmitter();
       //     weaponLoot1.EmitType = GroupEmitter.EmitterType.RandomOne;
            //weaponLoot1.AddEmitter("Flamethrower", 1);
            //weaponLoot1.AddEmitter("MineSpreader", 2);
            //weaponLoot1.AddEmitter("MiniMissileLauncher", 1);
            //weaponLoot1.AddEmitter("LaserLance", 1);
            //weaponLoot1.AddEmitter("FlakGun", 1);
            //weaponLoot1.AddEmitter("Engine2", 1);

            LootEmitter loot = new LootEmitter();
            loot.AddEmitter("MatA3", 0.1f, 3, 10);
            loot.AddEmitter("MatB3", 1, 3, 10);
            loot.AddEmitter("MatC3", 1, 3, 10);
         //   loot.AddEmitter(weaponLoot1, 1, 1);
            var lootSystem = new LootSystem();
            lootSystem.LootEmitter = loot;

            spawner.SystemsToAdd.Add(lootSystem);
            var shopSystem = new ShopSystem();
            shopSystem.AddItem("WarpCell");
            shopSystem.AddItem("Flamethrower",0.5f);
            shopSystem.AddItem("LaserLance", 1);
            spawner.SystemsToAdd.Add(shopSystem);
            spawner.SystemsToAdd.Add(new GoalChanger());

            return spawner;
        }


        public static InGameEventSpawner MakeBlobSpawner() {
            var spawner = new SpawnAgents();            
            spawner.Faction = FactionType.Neutral;
            spawner.IsOneTime = false;
            spawner.Target = SpawnAgents.AgentTargetType.Player;
            //spwaner.SystemsToAdd() //add Loot Emitter
            spawner.ActivationCooldownTime =  4;
            spawner.Text = "Blob";
            spawner.ActivationProbability = 0.001f;
            spawner.GoalPositionRad = 200000;
            spawner.SpawnPositionRad = 10000;
            spawner.EmitterList.Add(ContentBank.Inst.GetEmitter("Blob1")); //Change according to player level            
            spawner.SystemsToAdd.Clear();
            return spawner;
        }


        public static InGameEventSpawner MakePirateRaidSpawner()
        {
            var spawner = new SpawnAgents();
            spawner.LevelOffset = -2;
            spawner.Faction = FactionType.Pirates1;
            spawner.IsOneTime = false;
            spawner.Target = SpawnAgents.AgentTargetType.Player;
            //spwaner.SystemsToAdd() //add Loot Emitter
            spawner.ActivationCooldownTime = 60 * 60 * 3;
            spawner.Text = "Pirate";
            spawner.ActivationProbability = 0.001f;
            spawner.GoalPositionRad = 200000;
            spawner.SpawnPositionRad = 10000;
            
            spawner.EmitterList.Add(ContentBank.Inst.GetEmitter("SmallPirateA_Gen"));
            spawner.EmitterList.Add(ContentBank.Inst.GetEmitter("SmallPirateA_Gen"));

            GroupEmitter weaponLoot1 = new GroupEmitter();
            //weaponLoot1.EmitType = GroupEmitter.EmitterType.RandomOne;
            //weaponLoot1.AddEmitter("KineticShotgun", 3);
            //weaponLoot1.AddEmitter("MineSpreader", 2);
            //weaponLoot1.AddEmitter("MiniMissileLauncher", 1);
            //weaponLoot1.AddEmitter("LaserLance", 3);
            //weaponLoot1.AddEmitter("FlakGun", 1);
            //weaponLoot1.AddEmitter("FireRing", 1);
            //weaponLoot1.AddEmitter("HeavyGun", 1);
            

            LootEmitter loot = new LootEmitter();
            loot.AddEmitter("MatA3", 0.1f, 3, 10); //TODO: change
            loot.AddEmitter("MatB2", 1, 3, 10);
            loot.AddEmitter("MatC1", 1, 3, 10);
           // loot.AddEmitter(weaponLoot1);
            var lootSystem = new LootSystem();
            lootSystem.LootEmitter = loot;

            spawner.SystemsToAdd.Add(lootSystem);
            return spawner;
        }

        public static InGameEventSpawner MakeCargoShipSpawner()
        {
            var spawner = new SpawnAgents();
            spawner.IsOneTime = false;
            //spwaner.SystemsToAdd() //add Loot Emitter
            spawner.ActivationCooldownTime = 60 * 45;
            spawner.ActivationProbability = 0.01f;
            spawner.GoalPositionRad = 900000;
            spawner.SpawnPositionRad = 13000;
            GroupEmitter cargoShipEmitter = new GroupEmitter();
            cargoShipEmitter.EmitType = GroupEmitter.EmitterType.RandomOne;
            cargoShipEmitter.AddEmitter("CargoShipA_Gen",2);
            cargoShipEmitter.AddEmitter("CargoShipB_Gen", 1);
            //cargoShipEmitter.AddEmitter("CargoShipC_Gen", 1);
            spawner.EmitterList.Add(cargoShipEmitter);            
            var lootSystem = new LootSystem();
            lootSystem.LootEmitter = ContentBank.Inst.GetEmitter("CargoShipLoot");
            spawner.SystemsToAdd.Add(lootSystem);
            return spawner;
        }


        public static SpawnAgents MakeAgentSpawnerProccess(string ships, float distance)
        {
            var spawner = new SpawnAgents();
            spawner.Faction = FactionType.Pirates1;
            spawner.IsOneTime = false;
            spawner.Target = SpawnAgents.AgentTargetType.Player;
            //spwaner.SystemsToAdd() //add Loot Emitter
            spawner.ActivationCooldownTime = 60 *10;
            spawner.Text = "Pirate";
            spawner.ActivationProbability = 0.01f;
            spawner.GoalPositionRad = 200000;
            spawner.SpawnPositionRad = 10000;
            spawner.ActivationCheck = new DistanceFromPoint(distance, Vector2.Zero, false);

            spawner.EmitterList.Add(ContentBank.Inst.GetEmitter("SmallPirateA_Gen"));
            spawner.EmitterList.Add(ContentBank.Inst.GetEmitter("SmallPirateA_Gen"));

            GroupEmitter weaponLoot1 = new GroupEmitter();
            //weaponLoot1.EmitType = GroupEmitter.EmitterType.RandomOne;
            //weaponLoot1.AddEmitter("KineticShotgun", 3);
            //weaponLoot1.AddEmitter("MineSpreader", 2);
            //weaponLoot1.AddEmitter("MiniMissileLauncher", 1);
            //weaponLoot1.AddEmitter("LaserLance", 3);
            //weaponLoot1.AddEmitter("FlakGun", 1);
            //weaponLoot1.AddEmitter("FireRing", 1);
            //weaponLoot1.AddEmitter("HeavyGun", 1);


            LootEmitter loot = new LootEmitter(); //Change
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
