using SolarConflict.GameContent.Projectiles;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items
{
    class ShieldGenerator1
    {
        public static Item Make()
        {
            ItemProfile profile = ItemQuickStart.Profile("Shield Charger", "Regenerates 60 shield/sec in an area around you.\n Consumes 10 Energy/sec", 3,
                "ShieldCharger", "item3");
            profile.SlotType = SlotType.Utility;            
            profile.IsActivatable = true;
            profile.IsRetainedOnDeath = false;
            profile.MaxStack = 1;
            profile.BuyPrice = ScalingUtils.ScaleCost(profile.Level);
            Item item = new Item(profile);

            EmitterCallerSystem system = new EmitterCallerSystem();            
            system.EmitterID = typeof(AoeShield1).Name;
            system.MaxLifetime = 60;
            system.CooldownTime = 60;
            system.EmitterSpeed = 0;
            system.ActivationCheck.AddCost(MeterType.Energy, 10); //Energy Cost
            item.System = system;
            //item.MainSystem = new TurretSystemHolder(system, Vector2.Zero, 0, "turret1");
            return item;
        }
    }
}
