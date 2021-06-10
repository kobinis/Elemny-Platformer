using SolarConflict.Framework.Agents.Systems.Misc;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.AATestedItems
{
    class EvasiveSystem
    {
        public static Item Make()
        {
            ItemData data = new ItemData("Evasive System", 4, "EchoSprint1a");
            data.BuyPrice = 300;
            data.BreaksClocking = false;
            data.Category = ItemCategory.Evasive;
            data.SlotType = SlotType.Utility;
            data.MaxStack = 10;
            var item = ItemQuickStart.Make(data);
            Framework.Agents.Systems.Misc.EvasiveSystem system = new Framework.Agents.Systems.Misc.EvasiveSystem(8);
            item.System = system;
           
            return item;
        }
    }
}
