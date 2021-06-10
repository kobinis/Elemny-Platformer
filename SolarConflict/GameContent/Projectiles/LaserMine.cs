using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.NewContent.Emitters;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class LaserMine
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            //profile.InitColor = new InitColorConst(new Color(255,255,100));
            profile.DrawType = DrawType.AlphaFront;
            //profile.InitColor = new InitColorFaction();
            //profile.UpdateColor = ProjectileProfile.ColorUpdate.FadeOutSlow;
            profile.TextureID = "attention";
            profile.CollisionWidth = profile.Sprite.Width - 2;
            profile.InitSizeID = "24";
            profile.UpdateSize = null; // new UpdateSizeGrow(1.1f);
            profile.InitMaxLifetimeID = "2000";  // 1/60 of a second
            profile.Mass = 0.3f;
            //profile.ImpactEmitterId = "EmitterImpactFx1";
            profile.CollisionSpec = new CollisionSpec();            
            profile.CollisionSpec.AddEntry(MeterType.Damage, 1, ImpactType.Additive);
            profile.InitHitPointsID = "5000"; //check it

            profile.IsDestroyedWhenParentDestroyed = false; 
            profile.CollisionType = CollisionType.CollideAll;
                     
            profile.IsDestroyedOnCollision = false; //projectile is terminated on impact
            profile.IsEffectedByForce = true;
            //profile.Draw = new ProjectileDrawRotateWithTime(0.1f, 0.1f);            
            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Enemy, 0.05f, 3);
            profile.VelocityInertia = 0.99f;

            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = typeof(LaserBit).Name;
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Const;
            emitter.PosRadType = ParamEmitter.EmitterPosRad.Range;
            emitter.PosRadMin = 0;
            emitter.PosRadRange = 50;

            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.PosAngle;

            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
            emitter.VelocityMagMin = 0;

            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Const;
            emitter.RotationSpeedRange = 0;

            emitter.MinNumberOfGameObjects = 10;

            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Const;
            emitter.LifetimeMin = 1;

            ParamEmitter laserEmitter = new ParamEmitter();
            laserEmitter.Emitter = emitter;//typeof(LaserEmitter).Name;
            laserEmitter.PosRadType = ParamEmitter.EmitterPosRad.Const;
            laserEmitter.PosRadMin = 35;
            laserEmitter.PosAngleType = ParamEmitter.EmitterPosAngle.Range;
            laserEmitter.PosAngleRange = 360;

            laserEmitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.PosAngle;
            laserEmitter.MinNumberOfGameObjects = 4;

            laserEmitter.RotationType = ParamEmitter.EmitterRotation.PosAngle;
            
            profile.UpdateEmitter = laserEmitter;

            profile.TimeOutEmitterID = "LaserMineItem";
            profile.Mass = 2;

            //profile.TimeOutEmitterId = "BoomerangItem";
            return profile;
        }
    }
}
