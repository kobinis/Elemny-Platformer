using Microsoft.Xna.Framework;
using SolarConflict.GameContent;
using SolarConflict.GameContent.Utils.QuickStart;
using SolarConflict.NewContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class WormAsteroid1
    {
        public static ProjectileProfile Make()
        {

            var profile = AsteroidQuickStart.MakeEmptyAsteroid(Color.LightGreen);
            profile.DrawType = DrawType.Lit;
            //GroupEmitter loot = new GroupEmitter();
            //loot.RandomVelocityBase = 4;
            //loot.RandomVelocityRange = 3;
            //loot.EmitType = GroupEmitter.EmitterType.RandomOne;
            //loot.AddEmitter("MatA1", 0.1f);
            //loot.AddEmitter("MatA1", 1);
            //loot.AddEmitter("MatC1", 1);


            //ParamEmitter lootParam = new ParamEmitter(loot);
            //lootParam.RotationType = ParamEmitter.EmitterRotation.Random;
            //lootParam.RotationRange = 360;
            //lootParam.MinNumberOfGameObjects = 6;
            //lootParam.RotationType = ParamEmitter.EmitterRotation.Random;
            //lootParam.RotationRange = 360;

            GroupEmitter emitterGroup = new GroupEmitter();
            emitterGroup.EmitType = GroupEmitter.EmitterType.All;
            emitterGroup.AddEmitter("FxEmitterRockExp"); //maybe change the effects to bio expli
            emitterGroup.AddEmitter("FxEmitterRockExp");
           // emitterGroup.AddEmitter(lootParam);
            var param = ParamEmitter.MakeSpreadParam(2, 1);
            param.EmitterID = "Worm1";
            param.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
            emitterGroup.AddEmitter(param);

                   //    GroupEmitter emitterGroup = new GroupEmitter();
            //            emitterGroup.EmitType = GroupEmitter.EmitterType.All;
            //            emitterGroup.AddEmitter("FxEmitterRockExp");
            //            emitterGroup.AddEmitter("FxEmitterRockExp");
            //            emitterGroup.AddEmitter(lootParam);

            profile.HitPointZeroEmiiter = emitterGroup;           
            profile.CollisionSpec.Flags |= CollisionSpecFlags.AffectsAllies;
            profile.CollisionType = CollisionType.UpdateOnlyOnScreen;
            profile.Light = Lights.LargeLight(new Color(230, 255, 230));
            profile.Name = "Vile Asteroids";
            return profile;
        }
    }
}


//using Microsoft.Xna.Framework;
//using SolarConflict.NewContent.Projectiles;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using XnaUtils.Graphics;

//namespace SolarConflict.GameContent.Projectiles.Asteroids
//{
//    //Temp
//    class WormAsteroid
//    {
//        public static ProjectileProfile Make()
//        {
//            ProjectileProfile profile = Asteroid.Make();
//            profile.DrawType = DrawType.Lit;
//            profile.TextureID = "spacerock10000";

//            ProjectileDrawAni draw = new ProjectileDrawAni();

//            for (int i = 10000; i <= (10000) + 178; i += 2)
//            {
//                draw.AddTextureId("spacerock" + i.ToString());
//            }

//            GroupEmitter loot = new GroupEmitter();
//            loot.RandomVelocityBase = 4;
//            loot.RandomVelocityRange = 3;
//            loot.EmitType = GroupEmitter.EmitterType.All;
//            loot.AddEmitter("Biomass", 0.5f);            
//            loot.AddEmitter("Worm1", 0.5f);


//            ParamEmitter lootParam = new ParamEmitter(loot);
//            lootParam.PosRadType = ParamEmitter.EmitterPosRad.Const;
//            lootParam.PosRadMin = 30;
//            lootParam.PosAngleType = ParamEmitter.EmitterPosAngle.Random;
//            lootParam.PosAngleRange = 360;

//            lootParam.RotationType = ParamEmitter.EmitterRotation.PosAngle;
//            lootParam.RotationRange = 360;
//            lootParam.MinNumberOfGameObjects = 5;            


//            GroupEmitter emitterGroup = new GroupEmitter();
//            emitterGroup.EmitType = GroupEmitter.EmitterType.All;
//            emitterGroup.AddEmitter("FxEmitterRockExp");
//            emitterGroup.AddEmitter("FxEmitterRockExp");
//            emitterGroup.AddEmitter(lootParam);            

//            profile.HitPointZeroEmiiter = emitterGroup;
//            profile.InitColor = new InitColorConst(new Color(90, 255, 80));

//            profile.CollisionSpec.h = true;

//            return profile;
//        }
//    }
//}
