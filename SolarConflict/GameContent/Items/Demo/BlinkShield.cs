//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items
//{
//    /// <summary>
//    /// Teleports a short distance when hit
//    /// </summary>
//    class BlinkShield
//    {
//        public static Item Make()
//        {
//            ItemProfile profile = ItemQuickStart.Profile("Blink", "Shileds you and can be activated to shot an EMP shockwave.\ncosts 400 Shield to activate.\nCapacity: 1500\nRegen: 10",
//                0, "ShieldItem", "item3"); // add passive ablility
//            profile.Category = Item.Category.Shield;
//            profile.ItemSize = Item.SizeType.Small;
//            profile.IsWorkingOnlyInSlot = true;
//            profile.IsActivatable = true;

//            Item item = new Item(profile);


//            SystemGroup group = new SystemGroup();

//            MeterGenerator shiled = new MeterGenerator();
//            shiled.Meter = MeterType.Shield;
//            shiled.MaxValue = 1500;
//            shiled.GenerationAmount = 10f / 60f;
//            shiled.GenerationCooldownTime = 1;

//            group.AddSystem(shiled, false, false);

//            ParamEmitter emitter = new ParamEmitter();
//            emitter.EmitterId = "MoveParent";
//            emitter.PosAngleRange = 360;
//            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Random;
//            emitter.PosRadMin = 500;
//            emitter.PosRadType = ParamEmitter.EmitterPosRad.Const;

//            AgentEmitter empShockwave = new AgentEmitter(ControlSignals.OnTakingDamage, 5, Vector2.Zero, emitter);            
//            group.AddSystem(empShockwave, true, true);
//            empShockwave.SecondaryEmitterID = "TeleportFx";

//            item.MainSystem = group;
//            return item;
//        }
//    }
//}
