//using SolarConflict.Framework.Emitters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Emitters.Loot
//{
//    class CargoShipLoot
//    {
//        public static IEmitter Make()
//        {
//            //#all{MatB1,MatC1,MatA1,#pickone{RepairKit1:2, EnergyKit1:2,AsteroidPullMine:1,HomeBeaconKit:2}}"

//            GroupEmitter emitter = new GroupEmitter();
//            LootEmitter baseLoot = new LootEmitter();
//            baseLoot.AddEmitter("MatC1", 1, 1, 3);
//            baseLoot.AddEmitter("MatA1", 1, 1, 3);
//            baseLoot.AddEmitter("MatB1", 1, 1, 3);

//            emitter.AddEmitter(baseLoot);
            
//            LevelEmitter level = new LevelEmitter();
//            GroupEmitter gear = new GroupEmitter();            
//            gear.EmitType = GroupEmitter.EmitterType.RandomOne;

//            level.
//            emitter.AddEmitter(level);


//            return emitter;
//        }
//    }
//}
