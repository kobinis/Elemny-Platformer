//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
//using XnaUtils.Graphics;

//namespace SolarConflict
//{
//    class ProjFlash : ProjectileContent
//    {
//        public static ProjectileContent Get()
//        {
//            return new ProjFlash();
//        }

//        public override void Make()
//        {
//            base.Make();
//            projectileProfile.CollisionType = CollisionType.Effects;
//            projectileProfile.DrawType = DrawType.Additive;
//            projectileProfile.ColorLogic = ColorUpdater.FadeOutSlow;//ProjectileProfile.ColorUpdate.FadeInOut;
//            projectileProfile.TextureID = "flash";//add9");
//            projectileProfile.ScaleMult = 1f / (float)(projectileProfile.Sprite.Width - 6); // -6
//            //projectileProfile.draw = new ProjectileDrawScale()

//            projectileProfile.InitSize = new InitFloatConst(10);
//            projectileProfile.UpdateSize = null;
//            projectileProfile.InitMaxLifetime = new InitFloatConst(10);//texture from param            


//        }
//    }
//}
