using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Projectiles {
    class NebulaLightningEmitter {
        public static ProjectileProfile Make() {
            // TODO: make less ugly, more menacing. Cool lightning effects and stuff
            var lightningEmitter = new ProjectileProfile();
            lightningEmitter.DrawType = DrawType.Additive;
            lightningEmitter.InitColor = new InitColorConst(Color.Yellow);
            lightningEmitter.ColorLogic = ColorUpdater.FadeOutSlow;
            lightningEmitter.TextureID = "disrupter";
            lightningEmitter.CollisionWidth = lightningEmitter.Sprite.Width - 10;
            lightningEmitter.CollideWithMask = GameObjectType.None;
            lightningEmitter.InitSizeID = "100";
            lightningEmitter.InitMaxLifetimeID = Utility.Frames(0.75f).ToString();

            var lightningBolt = new ProjectileProfile();
            lightningBolt.ID = "NebulaLightning";
            lightningBolt.DrawType = DrawType.Additive;
            lightningBolt.InitColor = new InitColorConst(Color.Yellow);
            lightningBolt.ColorLogic = ColorUpdater.FadeOutSlow;
            lightningBolt.TextureID = "add3";
            lightningBolt.CollisionWidth = lightningBolt.Sprite.Width - 10;
            lightningBolt.InitSizeID = "20".ToString();
            lightningBolt.MovementLogic = new MoveForward(100f, 10f);
            lightningBolt.InitMaxLifetime = new InitFloatConst(60f);
            lightningBolt.CollisionSpec = new CollisionSpec(10f, 2f);

            var rotatedLightningBolt = new ParamEmitter();
            rotatedLightningBolt.RotationType = ParamEmitter.EmitterRotation.Const;
            rotatedLightningBolt.RotationBase = 90;
            
            rotatedLightningBolt.Emitter = lightningBolt;

            lightningEmitter.TimeOutEmitter = rotatedLightningBolt;

            return lightningEmitter;
        }
    }
}
