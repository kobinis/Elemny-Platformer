using Microsoft.Xna.Framework;
using SolarConflict.GameContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles {
    class OrbitingSunWithBackground {
        public static ProjectileProfile Make() {
            var profile = Sun.Make();
            profile.ID = "OrbitingSunWithBackground";
            profile.InitHitPoints = new InitFloatConst(100000);
            var movement = new MoveWithParent();
            movement.RotationSpeed = 800f;
            profile.IsEffectedByForce = false;
            profile.CollisionSpec.Flags = CollisionSpecFlags.AffectsAllies;
        

            profile.MovementLogic = movement;

            return profile;
        }
    }
}
