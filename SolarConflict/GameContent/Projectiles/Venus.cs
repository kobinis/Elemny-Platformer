//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using XnaUtils.Graphics;

//namespace SolarConflict
//{
//    class Venus : ProjectileContent
//    {
//        public static ProjectileContent Get()
//        {
//            return new Venus();
//        }

//        public override void Make()
//        {
//            base.Make();
//            projectileProfile.TextureID = "venus";
//            MoveWithParent movement = new MoveWithParent();
//            projectileProfile.CollisionWidth = 260;
//            movement.RefRotationMult = 0;
//            movement.RotationSpeed = 200f;
//            projectileProfile.CollisionType = CollisionType.CollideAll;
//            projectileProfile.DrawType = DrawType.Alpha;
//            projectileProfile.MovementLogic = movement;
//            projectileProfile.InitSizeID = "180";
//            projectileProfile.IsDestroyedOnCollision = false;
//            projectileProfile.IsEffectedByForce = false;
//            projectileProfile.CollisionSpec = CollisionSpec.SpecForce;
//            projectileProfile.Draw = new DrawRotationTarget(215);
//        }

//    }
//}
