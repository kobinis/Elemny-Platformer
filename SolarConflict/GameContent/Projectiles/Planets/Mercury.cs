//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Projectiles.Planets
//{
//    class Mercury
//    {

//        public static IEmitter Make()
//        {
//            ProjectileProfile projectileProfile = new ProjectileProfile();
//            projectileProfile.TextureID = "merc";
//            MoveWithParent movement = new MoveWithParent();
//            projectileProfile.CollisionWidth = 390;
//            movement.RefRotationMult = 0;
//            movement.RotationSpeed = 200f;
//            projectileProfile.CollisionType = CollisionType.CollideAll;
//            projectileProfile.DrawType = DrawType.Alpha;
//            projectileProfile.MovementLogic = movement;
//            projectileProfile.InitSizeID = "100";
//            projectileProfile.IsDestroyedOnCollision = false;
//            projectileProfile.IsEffectedByForce = false;
//            projectileProfile.CollisionSpec = new CollisionSpec(0, 10);
//            projectileProfile.Draw = new DrawRotationParent(215 - 30);
//            return projectileProfile;
//        }

//    }
//}

