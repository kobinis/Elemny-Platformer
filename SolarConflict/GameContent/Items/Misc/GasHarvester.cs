using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items
{
    public class GasHarvester
    {
        public static Item Make()
        {
            ItemProfile profile = ItemQuickStart.Profile("Gas Harvester", "Used to mine gas from nebulas.", 5,"gasharvester");
            profile.SlotType = SlotType.Utility;
            profile.BuyPrice = 5000;
            profile.SellPrice = 2700;
            profile.IsActivatable = true;

            Item item = new Item(profile);

            EmitterCallerSystem system = new EmitterCallerSystem(ControlSignals.None, 60, "NebulaGas");
            system.ActivationCheck.AddCost(MeterType.NebulaGas, 10);
            system.SelfImpactSpec = new CollisionSpec();
            system.SelfImpactSpec.AddEntry(MeterType.NebulaGas, 0, ImpactType.Min);                                                                             
            item.System = system;

            return item;
        }
    }
}
