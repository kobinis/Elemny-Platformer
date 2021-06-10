//using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using XnaUtils.Graphics;

//namespace SolarConflict
//{
//    class ProjTest : ProjectileContent
//    {
//        public static ProjTest Get()
//        {
//            return new ProjTest();
//        }

//        public override void Make()
//        {
//            base.Make();

//            projectileProfile.CollisionType = CollisionType.CollideAll;
//            projectileProfile.DrawType = DrawType.Additive;
//            projectileProfile.ColorLogic = ColorUpdater.FadeOutSlow;
//            projectileProfile.TextureID = "shockwave1";
//            projectileProfile.ScaleMult = 1 / (projectileProfile.Sprite.Width-100f);
//            projectileProfile.InitSize = new InitFloatConst(100);
//            projectileProfile.UpdateSize = null;

//            projectileProfile.IsEffectedByForce = false;
//            projectileProfile.CollisionSpec = CollisionSpec.SpecForce;

//            projectileProfile.InitMaxLifetime = new InitFloatConst(float.MaxValue);
//            projectileProfile.IsDestroyedOnCollision = false;

            
//        }
//    }
//}
