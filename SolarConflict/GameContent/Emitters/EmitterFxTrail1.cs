//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;

//namespace SolarConflict
//{
//    class EmitterFxTrail1
//    {
//        public static void Make()
//        {
//            ParamEmitter emitter = new ParamEmitter(ContentBank.Inst.GetEmitter("FxExp1"));
//            emitter.Id = "EmitterFxTrail1";
//           /* emitter.speedDegType = EmitterSpeedDeg.Range;
//            emitter.speedDegRange = 0.05f;

//            emitter.speedMagType = EmitterSpeedMag.Random;
//            emitter.speedMagMin = 0.1f;
//            emitter.speedMagRange = 1f;*/

//            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
//            emitter.RotationSpeedRange = MathHelper.ToDegrees(0.1f);

//            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
//            emitter.RotationRange = MathHelper.ToDegrees(MathHelper.TwoPi);


//           // emitter.minNumberOfGameObjects = 20;*/

//            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Const;
//            emitter.LifetimeMin = 20;
//            //emitter.lifetimeRange = 5;

//            emitter.RefVelocityMult = 0.0f;

//            ContentBank.Inst.AddContent(emitter);
//        }
//    }
//}

//namespace SolarConflict.GameContent.Emitters
//{
//    class EmitterFxTrail1
//    {
//    }
//}
