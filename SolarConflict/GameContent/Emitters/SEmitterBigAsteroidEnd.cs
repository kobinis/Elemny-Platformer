//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SolarConflict.NewContent.Projectiles;
//using SolarConflict.GameContent.Items;

//namespace SolarConflict.NewContent.Emitters
//{
//    class SEmitterBigAsteroidEnd
//    {
//        public static GroupEmitter Make()
//        {
//            var asteroEmitter = new ParamEmitter();
//            asteroEmitter.EmitterID = "Asteroid";
//            asteroEmitter.MinNumberOfGameObjects = 3;
//            asteroEmitter.RangeNumberOfGameObject = 2;
//            asteroEmitter.PosRadType = ParamEmitter.EmitterPosRad.Random;
//            asteroEmitter.PosRadMin = 10;
//            asteroEmitter.PosRadRange = 6;
//            asteroEmitter.PosAngleType = ParamEmitter.EmitterPosAngle.Random;
//            asteroEmitter.PosAngleRange = 360;

//            var emitter = new GroupEmitter();
//            emitter.EmitType = GroupEmitter.EmitterType.All;
//            emitter.AddEmitter("MatA3", 0.5f);           
//            emitter.AddEmitter("MatB0", 0.25f);            
//            emitter.AddEmitter("MatB1", 0.25f);            
//            emitter.AddEmitter(asteroEmitter);            
//            emitter.AddEmitter("FxEmitterRockExp");            
//            emitter.AddEmitter("FxEmitterRockExp");            
            

//            return emitter;
//        }
//    }
//}
