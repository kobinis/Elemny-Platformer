using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles.Shots
{    
    class RicochetShot1 //TODO: add hit conter using param, hit 10 times
    {
        public static ProjectileProfile Make()
        {
            // TODO: increase damage with each bounce (up to a cap)
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "disrupter";
            profile.CollisionWidth = profile.Sprite.Height - 5;
            profile.InitSizeID = "15";
            profile.UpdateSize = null;
            profile.InitMaxLifetimeID = "200";
            profile.Mass = 0.1f;
            
            ParamEmitter bouncingEmitter = new ParamEmitter(); //TODO: replace with physical colission
            bouncingEmitter.EmitterID = "RicochetShot1";
            bouncingEmitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
            bouncingEmitter.VelocityAngleRange = 90;
            bouncingEmitter.VelocityAngleBase = 180;
            bouncingEmitter.VelocityMagMin = 25;
            bouncingEmitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
            bouncingEmitter.RotationType = ParamEmitter.EmitterRotation.VelocityAngle;
            bouncingEmitter.RefVelocityMult = 0;            
            GroupEmitter emitter = new GroupEmitter();
            emitter.AddEmitter(bouncingEmitter, 0.97f);
            profile.ImpactEmitter = emitter;
            profile.CollisionSpec = new CollisionSpec(10, 0.5f);
            profile.IsDestroyedOnCollision = true;
            profile.CollisionSpec.Flags |= CollisionSpecFlags.AffectsAllies;
            profile.IsEffectedByForce = true;
            profile.RotationLogic = new UpdateRotationForward();
            profile.CollusionUpdateList.Add( new ImpactChangeParent());
            return profile;
        }
    }
}
