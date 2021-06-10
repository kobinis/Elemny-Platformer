using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.NewContent.Emitters;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    /// <summary>
    /// a time bomb that explodes when timeout, if destoryed before it will not explode
    /// </summary>
    class TimeBomb
    {
        public static ProjectileProfile Make() //TODO: add sound,
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Alpha;
            profile.ColorLogic = new UpdateColorSwitch(Color.White, Color.Red); //TODO: maybe change to countdown animation
            profile.TextureID = "attention"; //TODO: maybe change to countdown animation
            profile.CollisionWidth = profile.Sprite.Width - 5; //adjusts colider size to display size
            profile.InitSizeID = "20";           
            profile.InitMaxLifetime = new InitFloatConst(60*7); //7 sec delay
            profile.Mass = 0.5f;
           // profile.ImpactEmitterId = typeof(EmitterImpactFx1).Name;
            profile.CollisionType = CollisionType.CollideAll;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = true;
           // profile.UpdateMovement = new MoveToTarget(ProjectileTargetType.Enemy, 0.2f, 5);
            profile.VelocityInertia = 0.99f;
            //profile.UpdateRotation = new UpdateRotationForward();
            profile.CollisionSpec = new CollisionSpec(0, 0.5f);
            profile.CollisionSpec.IsDamaging = true;
            profile.TimeOutEmitterID = typeof(DevastationEmitter).Name; //change explosion
            profile.HitPointZeroEmiiterID = typeof(DamageAoe).Name;
            profile.InitHitPointsID = "100";
            return profile;
        }
    }
}
