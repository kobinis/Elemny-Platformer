using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameContent.Projectiles;

namespace SolarConflict.GameContent.NewItems      
{
    class AsteroidGrinderItem
    {
        public static Item Make()
        {
            ItemProfile profile = ItemQuickStart.Profile("Asteroid Grinder I", "Damages asteroids, good for mining and asteroid protection, works from the inventory."
                , 3, "AsteroidGrinder", "AsteroidGrinder");
            profile.SlotType = SlotType.Utility;                        
            profile.IsActivatable = false;
            profile.BuyPrice = 800;
            profile.SellPrice = 500;
            profile.MaxStack = 1;
            profile.Category = ItemCategory.Mining;

            Item item = new Item(profile);

            EmitterCallerSystem system = new EmitterCallerSystem();
            system.ActivationCheck = null;

            system.EmitterID = typeof(AsteroidDemageAoe).Name;
            system.MaxLifetime = 30;
            system.CooldownTime = 30;
            system.EmitterSpeed = 0;
            //system.ActivationCheck.AddCost(MeterType.Energy, 30); //Energy Cost
            item.System = system;
            item.Profile.IsWorkingInInventory = true;
            //item.MainSystem = new TurretSystemHolder(system, Vector2.Zero, 0, "turret1");
            return item;
        }     
    }
}
