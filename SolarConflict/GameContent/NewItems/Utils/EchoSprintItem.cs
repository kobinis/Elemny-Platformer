using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.NewContent.Projectiles;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.NewItems.Utils
{
    class EchoSprintItem
    {
        public static Item Make()
        {
            ItemProfile profile = ItemCommon.CommonProfile();
            profile.IsShownOnHUD = true;
            profile.Name = "Echo Sprint";
            profile.DescriptionText = "Gives you a short #color{255,255,0}speed boost#dcolor{}";
            profile.IconTextureID = "EchoSprint"; 
            profile.IsConsumed = false;            
            profile.IsActivatable = true;
            profile.MaxStack = 1;
            profile.SlotType = SlotType.Utility;            
            profile.BuyPrice = 850;
            profile.SellPrice = 350;
            profile.Level = 2;
            profile.IconSecondarySprite = Sprite.Get("lv2");

            Item item = new Item(profile);


            EmitterCallerSystem nitroBoost = new EmitterCallerSystem(ControlSignals.None, 360, typeof(ProjShipwreck1).Name);
            nitroBoost.refVelocityMult = 0;
            nitroBoost.ActiveTime = 50;
            nitroBoost.MidCooldownTime = 2;
            nitroBoost.velocity = -Vector2.UnitX;
            nitroBoost.SelfImpactSpec = new CollisionSpec();
            nitroBoost.SelfImpactSpec.Force = -70;
            nitroBoost.SelfImpactSpec.ForceType = ForceType.DirectionOfMovment;
            nitroBoost.SelfSpeedLimit = 80;

            TurretSystemHolder holder = new TurretSystemHolder(nitroBoost, Vector2.Zero, null);
            holder.AnalogIndex = 0;
            holder.FixToCenter = true;

            item.System = holder ;

            return item;
        }
    }
}
