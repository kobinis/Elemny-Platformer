//using Microsoft.Xna.Framework;
//using SolarConflict.NewContent.Projectiles;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using XnaUtils.Graphics;

//namespace SolarConflict.GameContent.Projectiles.Asteroids
//{
//    class WormAsteroid0
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

//            profile.CollisionSpec.AffectsAllies = true;

//            return profile;
//        }
//    }
//}
