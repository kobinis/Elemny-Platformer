//using SolarConflict.GameContent.Items;
//using SolarConflict.NewContent.Emitters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using XnaUtils.Graphics;

//namespace SolarConflict.NewContent.Projectiles
//{
//    class Asteroid
//    {
//        public static ProjectileProfile Make()
//        {
//            ProjectileProfile profile = new ProjectileProfile();
//            //display
//            profile.DrawType = DrawType.Lit;
//            profile.TextureID = "spacerock10000";

//            ProjectileDrawAni draw = new ProjectileDrawAni();
//            for (int i = 10000; i <= 10178; i += 2)
//            {
//                draw.AddTextureId("spacerock" + i.ToString());
//            }
//            draw.paramMult = 0.1f;

//            profile.Draw = draw;

//            profile.CollisionWidth = profile.Sprite.Width - 180;
//            profile.UpdateList.Add(new UpdateParamSumVelocity());

//            profile.InitParamID = "InitFloatRandom,0,10000";
//            //profile.InitParam = new InitFloatRandom(0, 10000);
//            profile.InitSizeID = "InitFloatRandom,50,15";
//            // profile.InitSize = new InitFloatRandom(100, 20);

//            //profile.ImpactEmitterId = "FxRock"; //change it



//            profile.CollisionSpec = new CollisionSpec(0.1f, ImpactType.Velocity, 1f);
//            profile.CollisionSpec.Flags = CollisionSpecFlags.AffectsAllies;

//            profile.Mass = 1;
//            profile.RotationMass = 10;
//            profile.InitHitPointsID = "150";
//            profile.IsDestroyedOnCollision = false;
//            profile.IsEffectedByForce = true;
//            profile.IsTurnedByForce = true;

//            profile.RotationInertia = 0.99f;
//            profile.VelocityInertia = 0.99f;

//            profile.CollisionType = CollisionType.UpdateOnlyOnScreen; //updateOnScreen

//            GroupEmitter lootGroupEmitter = new GroupEmitter();
//            lootGroupEmitter.EmitType = GroupEmitter.EmitterType.RandomOne;
//            lootGroupEmitter.AddEmitter("MatA0", 40);
//            lootGroupEmitter.AddEmitter("MatB0", 30);
//            lootGroupEmitter.AddEmitter("MatB1", 20);

//            ParamEmitter lootEmitter = new ParamEmitter(lootGroupEmitter);
//            lootEmitter.MinNumberOfGameObjects = 1;
//            lootEmitter.RangeNumberOfGameObject = 3;
//            lootEmitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
//            lootEmitter.VelocityAngleRange = 360;
//            lootEmitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
//            lootEmitter.VelocityMagRange = 4;
//            lootEmitter.VelocityMagMin = 1;
//            lootEmitter.RotationType = ParamEmitter.EmitterRotation.Random;
//            lootEmitter.RotationRange = 360;



//            GroupEmitter hitPointsZeroEmitter = new GroupEmitter();
//            hitPointsZeroEmitter.AddEmitter(typeof(FxEmitterRockExp).Name);
//            hitPointsZeroEmitter.AddEmitter(lootEmitter);

//            profile.ObjectType |= GameObjectType.Asteroid;

//            profile.HitPointZeroEmiiter = hitPointsZeroEmitter;// hitPointsZeroEmitter;


//            return profile;
//        }
//    }
//}
