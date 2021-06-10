using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items
{
    class CargoEmitter
    {
        public static Item Make()
        {
            ItemProfile profile = ItemQuickStart.Profile("Cargo Emitter", "When activated drops your cargo",
                1, "CargoEmitter", "HarpoonGun"); //TODO: change icon
            profile.SlotType = SlotType.Utility;            

            Item item = new Item(profile);

            InventoryEmitter emitter = new InventoryEmitter(ItemCategory.Material, true);            

            EmitterCallerSystem mainGun = new EmitterCallerSystem(); 
            mainGun.Emitter = emitter;
            mainGun.CooldownTime = 10;
            mainGun.EmitterSpeed = 50;
            mainGun.refVelocityMult = 1;
            mainGun.velocity =  Vector2.UnitX*50;
            mainGun.SelfImpactSpec = new CollisionSpec();
            mainGun.SelfImpactSpec.Force = -0.05f;
            item.System = mainGun;

            return item;
        }
    }
}
