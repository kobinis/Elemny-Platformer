using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Emitters;
using SolarConflict.NewContent.Projectiles;
using SolarConflict.NewContent.Emitters;
using SolarConflict.GameContent.Utils;

namespace SolarConflict.GameContent.Items
{
    class DevastationCoreDetonator
    {
        public static Item Make() ///Add warmup effect
        {
            //, "Emits a large ring of fire."
            WeaponData data = new WeaponData("Devastation Core Detonator", 4, "DevastationCoreDetonator");
            
            data.ShotEmitterID = "DevastationEmitter";
            data.CooldownSec = 15;
            data.ItemCostID = "DevastationCore"; //TODO: cahnge to ammoType
            data.IsFixedToCenter = true;
            data.ItemData.BuyPrice = 10000;
            //data.AmmoType = ItemCategory.Core;
            data.Range = 2500;
            data.IsAutoExtendedDescription = false;
            Item item = WeaponQuickStart.Make(data);
            item.Profile.Category |= ItemCategory.NonAI;

            (item.System as EmitterCallerSystem).ActivationCheck.AddItemCost("DevastationCore", 1);
            
            return item;
        }
    }
}
