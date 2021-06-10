//using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using XnaUtils;
//using XnaUtils.Graphics;

//namespace SolarConflict {
//    [Serializable]
//    public class ProjectileDrawSpritesheet : BaseDraw {
//        Spritesheet _sheet;

//        public float lifeTimeMult, paramMult, globalTimeMult;//hitPointMult, normalizedLifeTime

//        public ProjectileDrawSpritesheet(Spritesheet sheet) {
//            _sheet = sheet;
//        }

//        public override void Draw(Camera camera, Projectile projectile) {

//            int index = ((int)(projectile.Lifetime * lifeTimeMult + projectile.Param * paramMult + Game1.time * globalTimeMult))
//                % _sheet.NumSprites;

//            camera.CameraDraw(_sheet, index, projectile.Position, projectile.Rotation, projectile.Size * projectile.profile.ScaleMult, projectile.color);
//        }

//    }
//}
