//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
//using XnaUtils.Graphics;

//namespace SolarConflict
//{
//    class ProjShot1 : ProjectileContent
//    {
//        public static ProjShot1 Get()
//        {
//            return new ProjShot1();
//        }

//        public override void Make()
//        {
//            base.Make();
//            projectileProfile.CollisionType = CollisionType.Collide1;
//            projectileProfile.DrawType = DrawType.Additive;
//            projectileProfile.ColorLogic = ColorUpdater.FadeOutSlow;//ProjectileProfile.ColorUpdate.FadeInOut;
//            projectileProfile.TextureID = "lightGlow";
//            projectileProfile.ScaleMult = 1f / (float)(projectileProfile.Sprite.Width - 6)*3;
//            projectileProfile.InitSize = new InitFloatConst(10);
//            projectileProfile.UpdateSize = null;
//            projectileProfile.InitMaxLifetime = new InitFloatConst(100);//texture from param
//            projectileProfile.CollisionSpec = CollisionSpec.SpecForce;
//            projectileProfile.IsDestroyedOnCollision = false;
//            projectileProfile.IsEffectedByForce = false;
//            //projectileProfile.updateRotation = new UpdateRotationForward(MathHelper.PiOver2);
//            projectileProfile.CollisionSpec = new CollisionSpec(20, 0.1f);
//            projectileProfile.Mass = 0.1f;
//           // projectileProfile.ImpactEmitter = ContentBank.GetBank().GetEmitter("EmitterFxSmokeSmall1");
//            projectileProfile.CollisionSpec.IsDamaging = true;
//            projectileProfile.IsDestroyedOnCollision = true;
//            projectileProfile.UpdateEmitter = ContentBank.Inst.GetEmitter("EmitterFxTrail1");
//            projectileProfile.RotationLogic = new UpdateRotationHoming();
//         //   projectileProfile.UpdateMovement = new UpdateMovementForward();

//        }
//    }
//}
