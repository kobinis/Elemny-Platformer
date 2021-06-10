//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent
//{
//    using global::XnaUtils.Graphics;
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Text;


//    class InstituteCommon
//    {

//        public static Institute MakeInstitute(string texture, float hitpoints, int inventorySize, float size = 0)
//        {
//            Institute institute = new Institute();
//            institute.gameObjectType |= GameObjectType.Ship;
//            institute.Sprite = Sprite.Get(texture);
//            if (size == 0)
//            {
//                size = Math.Max(institute.Sprite.Width / 2f, institute.Sprite.Height / 2f);
//            }

//            institute.Size = size;
//            institute.Mass = 0.1f * size; //maybe clamp
//            institute.RotationMass = 0.01f * institute.Size * institute.Size; //?? //clamp

//            institute.SetMeter(MeterType.Hitpoints, new Meter(hitpoints));

//            institute.targetSelector = new TargetSelector();
//            institute.Inventory = new Inventory(inventorySize);
//            institute.ItemSlotsContainer = new ItemSlotsContainer();
//            return institute;
//        }


//        public static void AddCommonSystems(Agent ship)
//        {
//            ship.AddSystem(new FactionMeterBinder(MeterType.FactionKills));
//            ship.AddSystem(new FactionMeterBinder(MeterType.Money));
//            ship.AddSystem(new FactionMeterBinder(MeterType.Reputation)); //??

//            //Destroyed Explosion //TODO: add random exp //if size is above 100 use a more impresive explotion
//            AgentEmitter destroyedExplosion = new AgentEmitter(ControlSignals.OnDestroyed, "FullExplosionFx1");
//            ship.AddSystem(destroyedExplosion);

//            //Stun Fx
//            AgentEmitter stunFx = new AgentEmitter(ControlSignals.OnStun, 10, "StunFx"); ;
//            ship.AddSystem(stunFx);
            
//            //Cargo Emitter
//            ship.AddAfterSystem(new CargoEmitterSystem());
          
//            //Hull damage FX (debris)
//            //debris

//            //low hitpoints improve
//            AgentEmitter fxOnLowHitpoints = new AgentEmitter(ControlSignals.OnLowHitpoints, 4, "EmitterFxSmoke");
//            ship.AddSystem(fxOnLowHitpoints);
//        }

//        //public static Institute InstituteQuickStart(string texture, float hitpoints, int inventorySize, float size = 0)
//        //{
//        //    Institute institute = InstituteCommon.MakeInstitute(texture, hitpoints, inventorySize, size);
//        //    ShipCommon.AddCommonMeters(institute);
//        //    ShipCommon.AddCommonSystems(institute);
//        //    return institute;
//        //}
//    }
//}
