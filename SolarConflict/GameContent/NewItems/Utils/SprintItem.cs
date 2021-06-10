using Microsoft.Xna.Framework;
using SolarConflict.NewContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.NewItems.Utils
{
    class SprintItem
    {
        public static Item Make()
        {
            ItemProfile profile = ItemCommon.CommonProfile();
            profile.IsShownOnHUD = true;
            profile.Name = "Speed Booster";
            profile.DescriptionText = "Gives you a short speed boost in the direction of movment";
            profile.IconTextureID = "EchoSprint";
            profile.IsConsumed = false;
            profile.IsActivatable = true;
            profile.MaxStack = 1;
            profile.SlotType = SlotType.Utility;
            profile.BuyPrice = 700;
            profile.SellPrice = 350;
            profile.Level = 2;

            Item item = new Item(profile);


            EmitterCallerSystem nitroBoost = new EmitterCallerSystem(ControlSignals.None, 360, typeof(ProjShipwreck1).Name);
            nitroBoost.refVelocityMult = 0;
            nitroBoost.ActiveTime = 50;
            nitroBoost.MidCooldownTime = 2;
            nitroBoost.velocity = -Vector2.UnitX;
            nitroBoost.SelfImpactSpec = new CollisionSpec();
            nitroBoost.SelfImpactSpec.Force = -70;
            nitroBoost.SelfSpeedLimit = 80;

            TurretSystemHolder holder = new TurretSystemHolder(nitroBoost, Vector2.Zero, null);
            holder.AnalogIndex = 0;
            holder.FixToCenter = true;

            item.System = holder;

            return item;
        }
    }
}
