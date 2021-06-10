using Microsoft.Xna.Framework;
using SolarConflict.NewContent.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    /// <summary>
    /// a bomb that activaes when destoryed
    /// </summary>
    class DamageActivatedBomb
    {
        public static ProjectileProfile Make() //TODO: add sound, change exp
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Alpha;
            //profile.UpdateColor = new UpdateColorSwitch(Color.White, Color.Red); //TODO: maybe change to countdown animation
            profile.TextureID = "attention"; //TODO: maybe change to countdown animation
            profile.CollisionWidth = profile.Sprite.Width - 5; //adjusts colider size to display size
            profile.InitSizeID = "20";
            profile.InitMaxLifetime = new InitFloatConst(60 * 10); //7 sec delay
            profile.Mass = 2.5f;
            // profile.ImpactEmitterId = typeof(EmitterImpactFx1).Name;
            profile.CollisionType = CollisionType.CollideAll;
            profile.CollideWithMask = GameObjectType.All;
            //profile.ObjectType = GameObjectType.
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = true;
            // profile.UpdateMovement = new MoveToTarget(ProjectileTargetType.Enemy, 0.2f, 5);
            profile.VelocityInertia = 1;
            //profile.UpdateRotation = new UpdateRotationForward();
            profile.CollisionSpec = new CollisionSpec(0, 0.5f);
            profile.CollisionSpec.IsDamaging = true;
            profile.TimeOutEmitterID = typeof(DamageAoe).Name; //change explosion
            profile.HitPointZeroEmiiterID = typeof(DevastationEmitter).Name;
            profile.InitHitPointsID = "50";
            return profile;
        }
    }
}
