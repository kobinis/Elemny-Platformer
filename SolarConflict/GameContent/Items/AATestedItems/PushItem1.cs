using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.AATestedItems
{
    class PushItem1
    {
        public static Item Make()
        {
            ItemProfile profile = ItemQuickStart.Profile("Shockwave emitter", "Emits a pushing shockwave  ", 3, "EMPItem", null);
            profile.SlotType = SlotType.Utility;
            profile.BuyPrice = 6000;
            profile.SellPrice = 3000;
            profile.MaxStack = 1;
            profile.IsShownOnHUD = true;

            Item item = new Item(profile);
            EmitterCallerSystem system = new EmitterCallerSystem(typeof(AoePush1).Name);
            system.CooldownTime = 60*15;
            system.EmitterSpeed = 0;
            system.SecondaryEmitter = null;
            system.secondaryVelocityMult = 0; 
            system.ThirdEmitterID = "sound_pulse";
            system.MaxLifetime = 100;

            TurretSystemHolder holder = new TurretSystemHolder(system, Vector2.Zero, "item3");

            item.System = holder;
            return item;
        }
    }
}
