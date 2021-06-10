//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;

//namespace SolarConflict.NewContent.Projectiles
//{
//    class FxSheepWool
//    {
//        public static ProjectileProfile Make()
//        {
//            ProjectileProfile profile = new ProjectileProfile();
//            profile.VelocityInertia = 1;
//            profile.RotationInertia = 1;
//            profile.TextureId = "woolFx";
//            profile.DisplayScale = 256;
//            //profile.InitColor = new InitColorConst(new Color(255, 255, 255, 100));
//            profile.InitSizeId = "InitFloatRandom,50,50";
//            profile.UpdateColorId = "FadeInOut";
//            profile.InitMaxLifetimeId = "20";
//            //profile.UpdateColor = new UpdateColorFade(0.2f, -0.2f);
//            profile.UpdateSize = new UpdateSizeGrow(1);
//            profile.ListType = ParticlesListType.FX;
//            profile.DrawType = DrawType.AlphaFront;
//            return profile;
//        }
//    }
//}
