using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Items {
    class LightBulb
    {
        public static Item Make()
        {
            int lightDuration = 10 * 60 * 60;
            // Projectile
            var projectile = new ProjectileProfile();
            
            projectile.CollisionType = CollisionType.Effects;            
            projectile.InitMaxLifetime = new InitFloatConst(lightDuration);
            projectile.Light = Lights.HugeLight(new Color(255, 159, 150));
            projectile.MovementLogic = new MoveWithParent();

            // System
            var system = new BasicEmitterCallerSystem();
            system.CooldownTime = lightDuration;
            system.Emitter = projectile;

            // Item
            var profile = ItemQuickStart.Profile("Light Bulb", null, 3,
                "lightbulub", null);
            profile.SlotType = SlotType.Consumable;            
            profile.IsActivatable = true;
            profile.MaxStack = 100;
            profile.BuyPrice = 150;
            profile.SellPrice = 20;
            profile.IsConsumed = true;            
            profile.IsWorkingInInventory = true;                  

            var item = new Item(profile);
            item.System = system;

            return item;
        }
    }
}
