﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Items
{
    class FlollowEngine
    {
        public static Item Make()
        {
            ItemProfile profile = ItemQuickStart.Profile("Follow System", "Can push ship in any direction",
                9, "TurretEngineIcon", null);
            profile.SlotType = SlotType.Turret;


            Item item = new Item(profile);

            EmitterCallerSystem mainGun = new EmitterCallerSystem(ControlSignals.AlwaysOn, "ProjEngineTrail");
            mainGun.CooldownTime = 1;
            mainGun.EmitterSpeed = -3;
            mainGun.refVelocityMult = 1;
            mainGun.SelfImpactSpec = new CollisionSpec();
            mainGun.SelfImpactSpec.Force = -50f;
            mainGun.SelfSpeedLimit = 20;


            TurretSystemHolder holder = new TurretSystemHolder(mainGun, Vector2.Zero, null);
            holder.FixToCenter = true;
            holder.Sprite = Sprite.Get("turretEngine");
            item.System = new FollowSystem(10, 100);

            return item;
        }
    }
}

