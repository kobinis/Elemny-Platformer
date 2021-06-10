//using Microsoft.Xna.Framework;
//using SolarConflict.GameContent.Utils;
//using SolarConflict.NewContent.Projectiles;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items.AATestedItems
//{
//    class SpeedBoost
//    {
//        public static Item Make()
//        {
//            ItemData itemData = new ItemData("Speed Boost", iconID:"SpeedBoostItem");
//            itemData.SlotType = SlotType.Utility;
//            itemData.Level = 2;
//            var item = ItemQuickStart.Make(itemData);


//            EmitterCallerSystem nitroBoost = new EmitterCallerSystem(ControlSignals.None, 360, null);
//            nitroBoost.refVelocityMult = 0;
//            nitroBoost.ActiveTime = 120;
//            nitroBoost.MidCooldownTime = 2;
//            nitroBoost.velocity = -Vector2.UnitX;
//            nitroBoost.SelfImpactSpec = new CollisionSpec();
//            nitroBoost.SelfImpactSpec.Force = -7f ;
//            nitroBoost.SelfImpactSpec.ForceType = ForceType.FromCenter;
//            nitroBoost.SelfSpeedLimit = 80;

//            TurretSystemHolder holder = new TurretSystemHolder(nitroBoost, Vector2.Zero, null);
//            holder.AnalogIndex = 0;
//            holder.FixToCenter = true;

//            item.System = holder;

//            return item;
//        }
//    }
//}
