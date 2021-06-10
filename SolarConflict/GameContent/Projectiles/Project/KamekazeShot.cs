//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.NewContent.Projectiles
//{
//    class KamekazeShot
//    {
//        public static ProjectileProfile Make()
//        {
//            ProjectileProfile profile = new ProjectileProfile();
//            profile.DrawType = DrawType.Alpha;
//            profile.UpdateColor = ProjectileProfile.ColorUpdate.FadeOutSlow;
//            profile.TextureId = "blank";
//            profile.DisplayScale = profile.Texture.Width - 10;
//            profile.InitSizeId = "InitFloatParentSize";
//            profile.UpdateSize = null;
//            profile.UpdateRotation = new UpdateRotationForward();
//            profile.UpdateMovement = new MoveToTarget(ProjectileTargetType.Enemy, 0.05f, 7f);
//            //profile.Draw = new ProjectileDrawRotateWithTime(0.1f, -0.12f);
//            profile.InitMaxLifetime = new InitFloatConst(180);
//            profile.Mass = 0.1f;
//            profile.Draw = new ProjectileDrawParent();
//            profile.UpdateEmitter = ContentBank.Inst.GetEmitter("EmitterFxSmoke");
//            profile.UpdateEmitterCooldownTime = 2;
//            profile.ImpactEmitterId = "FullExplosionFx1";
//            profile.ImpactSpec = new ImpactInfo(100, 0.5f);
//            profile.EndOfLifeImpact = true;
//            profile.IsEffectedByForce = false;
//            return profile;
//        }
//    }
//}
