//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.Emitters;
//using SolarConflict.Framework.GameObjects;
//using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SolarConflict.GameContent.Projectiles.Effects
//{
//    class FireDebris
//    {
//        public static ProjectileProfile Make()
//        {
//            ProjectileProfile projectileProfile = new ProjectileProfile();
//            projectileProfile.CollisionType = CollisionType.Effects;
//            projectileProfile.DrawType = DrawType.Alpha;
//            projectileProfile.TextureID = "smallLight";
//           // projectileProfile.InitSize = new InitFloatConst(20);
//            projectileProfile.InitMaxLifetime = new InitFloatConst(60);
//           // projectileProfile.MovementLogic = new MoveWithParent();
//            projectileProfile.InitColor = new InitColorConst(Color.Transparent);
//            var emitter = new ParticleSysDynamicEmitter(null, PARTICLE_MANAGER.Get("SmokePS"), new ValueUpdader(12000, 0, 0), new ValueUpdader(10));
//            projectileProfile.UpdateEmitter =  ContentBank.Inst.GetEmitter("SmokePS");
//            projectileProfile.Flags = GameObjectFlags.AddOnlyOnScreen;
//            return projectileProfile;
//        }
//    }
//}
