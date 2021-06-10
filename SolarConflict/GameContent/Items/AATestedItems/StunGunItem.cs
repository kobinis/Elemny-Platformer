using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Emitters;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items
{
    class StunGunItem
    {
        public static Item Make()
        {
            ItemProfile profile = ItemQuickStart.Profile("EMP Device", "Stuns enemy ship around you  ", 4, "EMPItem", null);
            profile.SlotType = SlotType.Weapon | SlotType.Turret;                        
            profile.BuyPrice = ScalingUtils.ScaleCost(profile.Level);
            
            profile.MaxStack = 1;
            profile.IsShownOnHUD = true;

            Item item = new Item(profile);
            EmitterCallerSystem system = new EmitterCallerSystem(typeof(AoeStun2).Name);
            system.CooldownTime = 900;
            system.EmitterSpeed = 0;
            system.SecondaryEmitterID = typeof(GunFlashFx).Name;
            system.secondaryVelocityMult = 0.1f;
            system.ThirdEmitterID = "sound_shot1";
            system.MaxLifetime = 100;

            TurretSystemHolder holder = new TurretSystemHolder(system, Vector2.Zero, "item3");

            item.System = holder;
            return item;
        }
    }
}
