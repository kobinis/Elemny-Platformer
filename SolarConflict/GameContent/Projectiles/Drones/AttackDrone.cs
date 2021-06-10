using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.GameContent.Utils;
using SolarConflict.NewContent.Emitters;
using SolarConflict.NewContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class AttackDrone
    {
        public static ProjectileProfile Make()
        {            
            ProjectileProfile drone = new ProjectileProfile();
            drone.DrawType = DrawType.Alpha;
            drone.TextureID = "empire-ship-interceptor";
            drone.CollisionWidth = drone.Sprite.Width;
            drone.InitSizeID = "25";
            drone.InitMaxLifetimeID = "600";
            drone.Mass = 1f;
            drone.InitHitPointsID = "150";
            drone.UpdateEmitter = MakeShot();
            drone.UpdateEmitterCooldownTime = 40;            
            drone.HitPointZeroEmiiterID = "EmitterDebris1";
            drone.TimeOutEmitterID = "ProjShipwreck1";
            drone.MovementLogic = new MoveForward(0.5f, 15f);
            drone.RotationLogic = new UpdateRotationHoming(targetType: ProjectileTargetType.AncestorTarget);
            drone.VelocityInertia = 0.97f;
            drone.IsDestroyedOnCollision = false;
            drone.IsEffectedByForce = true;
            drone.CollisionType = CollisionType.CollideAll;
            drone.CollisionSpec = new CollisionSpec(0f,0.5f);
            return drone;
        }

        public static IEmitter MakeShot()
        {
            ParamEmitter paramEmitter = new ParamEmitter();
            paramEmitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
            paramEmitter.VelocityMagMin = 10f;
            paramEmitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
            paramEmitter.VelocityAngleBase = 0;
            paramEmitter.VelocityAngleRange = 30;


            ProjectileProfile shot = new ProjectileProfile();
            shot.DrawType = DrawType.Additive;
            shot.ColorLogic = ColorUpdater.FadeOutSlow;
            shot.TextureID = "lightGlow";
            shot.CollisionWidth = shot.Sprite.Width - 10;
            shot.InitSizeID = "20";
            shot.UpdateSize = null;
            shot.InitMaxLifetimeID = "100";
            shot.Mass = 0.1f;
            //profile.RotationLogic = new UpdateRotationForward();
            shot.ImpactEmitterID = typeof(EmitterImpactFx1).Name;
            shot.CollisionSpec = new CollisionSpec(30, 0.5f);
            shot.IsDestroyedOnCollision = true;
            shot.IsEffectedByForce = false;

            paramEmitter.Emitter = shot;

            return paramEmitter;
        }

        //public IEmitter MakeShotEmitter()
        //{

        //}
    }
}
