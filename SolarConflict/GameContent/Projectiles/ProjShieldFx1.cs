//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
//using XnaUtils.Graphics;

//namespace SolarConflict
//{
//    class ProjShieldFx1 : ProjectileContent
//    {
//        public static ProjectileContent Get()
//        {
//            return new ProjShieldFx1();
//        }

//        public override void Make()
//        {
//            base.Make();
//            projectileProfile.CollisionType = CollisionType.Effects;
//            projectileProfile.DrawType = DrawType.Additive;
//            projectileProfile.ColorLogic = ColorUpdater.FadeInOut;
//            projectileProfile.TextureID = "shockwave2";
//            projectileProfile.ScaleMult = 1f / (float)(projectileProfile.Sprite.Width - 6)*2.2f;
//            projectileProfile.InitSize = new InitFloatParentSize(); // change name to parent
//            projectileProfile.UpdateSize = null;
//            projectileProfile.InitMaxLifetime = new InitFloatConst(20);//texture from param
//            projectileProfile.MovementLogic = new MoveWithParent();
//            projectileProfile.InitColor = new InitColorConst(Color.White);
//        }
//    }
//}