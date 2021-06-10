using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles.Shots
{
    class HeavyShot1
    {
        /// <summary>
        /// A Heavy shot
        /// </summary>
        public static ProjectileProfile Make()
        {
            ParamEmitter trail = new ParamEmitter();
            trail.EmitterID = typeof(HeavyShotTrail).Name;
            trail.VelocityAngleBase = 180;
            trail.VelocityMagMin = 6;
            //trail.RotationSpeedType =


            ProjectileProfile profile = new ProjectileProfile();
            profile.Light = Lights.MediumLight(new Color(250, 100, 0));
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "fireball1";
            profile.Draw = new ProjectileDrawRotateWithTime(-0.11f, 0.1f, "fireball1", "fireball1");
            profile.InitSizeID = "15";
            profile.UpdateSize = null;
            profile.InitMaxLifetimeID = "100";
            profile.Mass = 0.1f;
            profile.UpdateEmitter = trail;
            profile.UpdateEmitterCooldownTime = 2;
            GroupEmitter impactEmitter = new GroupEmitter();
            impactEmitter.AddEmitter("sound_exp2");
            impactEmitter.AddEmitter(typeof(DamageAoe).Name);
            profile.ImpactEmitter = impactEmitter;
            profile.CollisionSpec = new CollisionSpec(150, 12f);
            profile.CollisionSpec.ForceType = ForceType.FromCenter;
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;
            profile.CollideWithMask = GameObjectType.All;
            return profile;
        }
    }
}
