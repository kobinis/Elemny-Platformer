//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.NewContent.Projectiles
//{
//    class NinjaKnife
//    {
//        public static ProjectileProfile Make()
//        {
//            ProjectileProfile profile = new ProjectileProfile();
//            profile.DrawType = DrawType.Additive;
//            profile.UpdateColor = ProjectileProfile.ColorUpdate.FadeOutSlow;
//            profile.TextureId = "NinjaKnifeItem";
//            profile.DisplayScale = profile.Texture.Width - 10;
//            profile.InitSizeId = "30";
//            profile.UpdateSize = null;
//            profile.UpdateRotation = new UpdateRotationForward();
//            profile.UpdateMovement = new MoveToTarget(ProjectileTargetType.Enemy, 0.05f, 7f);

//            profile.InitMaxLifetime = new InitFloatConst(200);
//            profile.Mass = 0.1f;
//            profile.ImpactEmitterId = "EmitterImpactFx1";
//            ParamEmitter emitter = new ParamEmitter();
//            emitter.EmitterId = "EmitterImpactFx1";
//            emitter.MinNumberOfGameObjects = 5;
//            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
//            emitter.VelocityAngleRange = 360;
//            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
//            emitter.VelocityMagMin = 5;

//            profile.ImpactEmitter = emitter;
//            profile.ImpactSpec = new ImpactInfo(50, 0.5f);
//            profile.EndOfLifeImpact = true;

//            profile.IsEffectedByForce = false;



//            return profile;


//        }
//    }
//}
