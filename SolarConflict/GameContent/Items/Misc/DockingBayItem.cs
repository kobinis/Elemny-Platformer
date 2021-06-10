using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items
{
    public class DockingBayItem
    {
        public static Item Make()
        {
            ItemProfile profile = ItemQuickStart.Profile("Docking Bay", "Store ship", 5,
                    "FactionDockingBay", "item3");
            profile.SlotType = SlotType.Utility;                        
            profile.BuyPrice = 5000;
            profile.SellPrice = 2700;
            profile.IsActivatable = true;

            Item item = new Item(profile);
         
            EmitterCallerSystem fighterEmitter = new EmitterCallerSystem(ControlSignals.None, 60 * 30, "Enemy1");
            fighterEmitter.velocity = Vector2.UnitX * 10;
            fighterEmitter.ActivationCheck.AddCost(MeterType.ControlPoints, 1); //add money cost
         //   fighterEmitter.ThirdEmitterID = "sound_launchFighter";
            item.System = fighterEmitter;

            return item;
        }   
    }
}
