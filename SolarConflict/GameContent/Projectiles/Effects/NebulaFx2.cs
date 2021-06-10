//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Projectiles.Effects {
//    class NebulaFx2 {
//        public static ProjectileProfile Make() {
//            ProjectileProfile profile = new ProjectileProfile();

//            // Additive hideousness
//            profile.DrawType = DrawType.Additive;
//            profile.TextureID = "add2";
//            // AlphaFront hideousness            
//            profile.DrawType = DrawType.AlphaFront;
//            profile.TextureID = "smoke2";

//            profile.InitColor = new InitColorConst(new Color(0.3755f, 0.25f, 0.5f));
//            profile.CollisionWidth = profile.Sprite.Width - 28;
//            profile.CollideWithMask = GameObjectType.None;
//            profile.InitSizeID = "300";
//            profile.InitMaxLifetimeID = "300";

//            return profile;
//        }
//    }
//}
