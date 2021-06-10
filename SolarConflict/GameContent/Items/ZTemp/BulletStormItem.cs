//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using SolarConflict.NewContent.Projectiles;
//using SolarConflict.GameContent.Emitters;

//namespace SolarConflict.GameContent.Items
//{
//    class BulletStormItem
//    {
//        public static Item Make()
//        {
//            ItemProfile profile = ItemQuickStart.Profile("Bullet Storm", "Ahhhh!", 11,
//            "MissileTurretItem", "turret1");
//            profile.SlotType = SlotType.Weapon;                        
//            profile.IsRetainedOnDeath = true;

//            ParamEmitter emitter = new ParamEmitter();
//            emitter.EmitterId = typeof(HomingFusionShot).Name;
//            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
//            emitter.VelocityAngleRange = 360;
//            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
//            emitter.VelocityMagMin = 25;
//            emitter.RotationType = ParamEmitter.EmitterRotation.VelocityAngle;
//            emitter.MinNumberOfGameObjects = 5;            

//            Item item = new Item(profile);
//            EmitterCallerSystem system = new EmitterCallerSystem();
//            system.ActivationEmitterID = "sound_shot5";
//            system.Emitter = emitter;
//            system.CooldownTime = 60 * 30;
//            system.ActiveTime = 10;
//            system.MidCooldownTime = 0;
//            system.velocity = Vector2.Zero; // change to get a float
//            system.SecondaryEmitterID = typeof(GunFlashFx).Name;
//            system.SecondarySize = 40;
//            system.secondaryVelocityMult = 0.1f;
//            //system.ThirdEmitterId = ;
//            system.ActivationCheck.AddCost(MeterType.Energy, 75);

//            item.System = new TurretSystemHolder(system, Vector2.Zero, "turret1");
//            return item;
//        }
//    }
//}
