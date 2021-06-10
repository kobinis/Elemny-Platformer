//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
//using XnaUtils.Graphics;

//namespace SolarConflict
//{
//    class ProjShot2 : ProjectileContent
//    {
//        public static ProjectileContent Get() 
//        {
//            return new ProjShot2();
//        }

//        public override void Make()
//        {
//            base.Make();
//            projectileProfile.CollisionType = CollisionType.Collide1;
//            projectileProfile.DrawType = DrawType.Additive;
//            projectileProfile.ColorLogic = ColorUpdater.FadeOutSlow;//ProjectileProfile.ColorUpdate.FadeInOut;
//            projectileProfile.TextureID = "lightGlow";//add9");
//            projectileProfile.ScaleMult = 1f / (float)(projectileProfile.Sprite.Width - 30);
//            //projectileProfile.draw = new ProjectileDrawScale()

//            projectileProfile.InitSize = new InitFloatConst(10);
//            projectileProfile.UpdateSize = null;
//            projectileProfile.InitMaxLifetime = new InitFloatConst(300);//texture from param            
            
//          //  projectileProfile.UpdateRotation = new UpdateRotationForward(MathHelper.PiOver2);      
            
//            projectileProfile.Mass = 0.1f;
//            projectileProfile.ImpactEmitter = ContentBank.Inst.GetEmitter("EmitterImpactFx1");

//            projectileProfile.CollisionSpec = new CollisionSpec(10, 0.5f);
//            projectileProfile.CollisionSpec.IsDamaging = true;
//            projectileProfile.IsDestroyedOnCollision = true;
//            projectileProfile.IsEffectedByForce = false;
//            projectileProfile.MovementLogic = new MoveToTarget(ProjectileTargetType.Enemy, 0.05f, 10f);
//           // projectileProfile.VelocityInertia = 0.98f;
//           /* projectileProfile.endOfLifeImpact = false;
//            projectileProfile.isEffectedByForce = true;*/

//        }
//    }
//}
