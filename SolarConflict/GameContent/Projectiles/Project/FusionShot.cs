using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.GameContent;
using XnaUtils.Graphics;
using Microsoft.Xna.Framework;

namespace SolarConflict.NewContent.Projectiles
{
    class FusionShot
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
           // profile.InitColor = new InitColorConst(new Color(255, 100, 0));
            profile.TextureID = "Fusion1";
            profile.CollisionWidth = profile.Sprite.Width;
            profile.InitSizeID = "65";
            profile.UpdateSize = null;
            profile.InitMaxLifetime = new InitFloatConst(150);
            profile.Mass = 0.1f;
            //profile.UpdateEmitterId = typeof(AoeDamage).Name;
         //   profile.UpdateEmitter = ContentBank.Inst.GetEmitter("EmitterFxSmoke");
            profile.UpdateEmitterCooldownTime = 1;

            profile.ImpactEmitter = MakeExplosion();

            profile.CollisionSpec = new CollisionSpec(50, 10f);            
            profile.CollisionSpec.ForceType = ForceType.FromCenter;
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;
            return profile;
        }

        static ProjectileProfile MakeExplosion() {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            //profile.InitColor = new InitColorConst(new Color(0.5f, 0.5f, 0.5f));
            profile.ColorLogic = ColorUpdater.FadeInOut;
            profile.TextureID = "add2";

            profile.CollisionWidth = profile.Sprite.Width - 5;
            profile.InitSize = new InitFloatConst(7.5f);
            profile.UpdateSize = new UpdateSizeGrow(2f);
            profile.InitMaxLifetime = new InitFloatConst(25);
            profile.Mass = 0.1f;
            profile.VelocityInertia = 0.8f;
            profile.CollisionSpec = new CollisionSpec(1f, 0f); //1
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            profile.Draw = new ProjectileDrawRotateWithTime(0f, 0f, "add2");

            return profile;
        }
    }
}
