//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using XnaUtils;

//namespace SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Draw
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    [Serializable]
//    class DrawParent : BaseDraw
//    {
//        public override void Draw(Camera camera, Projectile projectile)
//        {
//            if (projectile.Parent != null && projectile.Parent.GetSprite() != null)
//            {                
//                camera.CameraDraw(projectile.Parent.GetSprite(), projectile.Position, projectile.Rotation, 1f, projectile.ParticleColor);//mov
//            }

//        }
//    }
//}
