//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.Utils;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using XnaUtils.Graphics;

//namespace SolarConflict.GameContent.Items
//{
//    class Firefly
//    {
//        public static Item Make()
//        {
//            // TODO: maybe make Light compatible with ColorUpdater, use one to have color oscillate between two values over time

//            var lifetime = Utility.Frames(10f);

//            // Projectile
//            var projectile = new ProjectileProfile();

//            projectile = new ProjectileProfile();

//            projectile.IsDestroyedWhenParentDestroyed = true;
//            projectile.CollideWithMask = GameObjectType.None;
//            //projectile.MovementLogic = new StaggerNearTarget(ProjectileTargetType.Parent, 400f, 20f, 10f);
//            projectile.MovementLogic = new StaggerNearTarget(ProjectileTargetType.Parent, 400f, 20f, 10f, true);
//            projectile.InitMaxLifetime = new InitFloatConst(lifetime);
//            projectile.InitSizeID = "10";
//            projectile.Sprite = Sprite.Get("add10");
//            projectile.CollisionWidth = projectile.Sprite.Width - 10f;

//            projectile.Light = Lights.MakeLight(Color.Yellow, 3f, distanceForHalfIntensity: 2000f, exponent: 1);

//            // System
//            var system = new EmitterCallerSystem();
//            system.Emitter = projectile;
//            system.CooldownTime = lifetime;

//            // Item
//            var profile = ItemQuickStart.Profile("Firefly", "All this little guy wants to do is brighten up your day", 2,
//                "Firefly", null);        
//            profile.SlotType = SlotType.Utility;
//            profile.IsActivatable = true;
//            profile.MaxStack = 1;
//            profile.BuyPrice = 10;
//            profile.SellPrice = 5;

//            var item = new Item(profile);
//            item.System = system;


//            return item;
//        }
//    }
//}