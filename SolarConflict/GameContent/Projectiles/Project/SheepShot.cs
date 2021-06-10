//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SolarConflict.GameContent.Projectiles;

//namespace SolarConflict.NewContent.Projectiles
//{
//    class SheepShot
//    {
//        public static ProjectileProfile Make()
//        {
//            ProjectileProfile profile = new ProjectileProfile();
//            profile.DrawType = DrawType.Additive;
//            profile.TextureId = "Sheep";
//            profile.DisplayScale = profile.Texture.Width +10;
//            profile.InitSizeId = "30";
//            profile.UpdateSize = null;
//            profile.InitMaxLifetime = new InitFloatConst(100);
//            profile.Mass = 0.1f;
//            profile.UpdateEmitter = ContentBank.Inst.GetEmitter("SheepWoolFx");
//            profile.UpdateEmitterCooldownTime = 3;
//            profile.ImpactEmitterId = typeof(DamageAoe).Name;
//            profile.TimeOutEmitterId = "SheepEndScatter";
//            profile.ImpactSpec = new ImpactInfo(5, 0.7f);
//            profile.EndOfLifeImpact = false;
//            profile.IsEffectedByForce = true;
//            return profile;
//        }
//    }
//}
