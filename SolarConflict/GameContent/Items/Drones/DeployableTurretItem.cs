using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items
{
    class DeployableTurretItem
    {
        public static Item Make()
        {
            ItemProfile profile = ItemQuickStart.Profile("Deployable Turret", "Deploys turret, activate from the inventory.", 2, "deployable turret", null);
            profile.SlotType = SlotType.Consumable | SlotType.Utility;
            profile.Category |= ItemCategory.Hotbar;
            profile.IsActivatable = true;

            profile.MaxStack = 10;
            profile.IsConsumed = true;
            profile.BuyPrice = 400;
            profile.SellPrice = 250;
            profile.IsWorkingInInventory = true;

            Item item = new Item(profile);
            item.Profile.MaximalRange = 1600;     
            
            EmitterCallerSystem gun = new EmitterCallerSystem();
            gun.EmitterID = "DeployableTurret1";   
            gun.CooldownTime = 30;
            gun.EmitterSpeed = 10;                        
            item.System = gun;

            return item;
        }
    }
}
