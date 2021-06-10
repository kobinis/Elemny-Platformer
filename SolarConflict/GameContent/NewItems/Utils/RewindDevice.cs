using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc;
using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.NewItems.Utils {
    class RewindDevice {
        public static Item Make() {
            //, "Deploys a spacetime anchor. After a delay, warps the ship back to the anchor, restoring its hitpoints, shields, and energy to their values when the anchor was dropped\n\nOriginal content, do not steal"
            var data = new ItemData("Rewind Device", 0, "turret-engine");
            data.SlotType = SlotType.Utility;
            data.Level = 7;
            data.BuyPrice = 10000;
            data.BuyPrice = 2;           
            
            var profile = ItemQuickStart.Profile(data);


            var result = new Item(profile);

            var rewindSystem = new RewindSystem(new MeterType[] { MeterType.Hitpoints, MeterType.Shield, MeterType.Energy }, Utility.Frames(30f));
              
            result.System = rewindSystem;
            result.Profile.Level = 10;
            return result;            
        }
    }
}
