using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Agents
{
    class SmallShop1
    {
        public static GameObject Make()
        {
            Agent ship = ShipQuickStart.Make("SmallShop1", 8000);
            ship.Name = "Shop";
            //ship.control.controlAi = new ShopAI();
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Generator, (SizeType)1);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Shield, (SizeType)1);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Rotation, (SizeType)1);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, (SizeType)1);
            //ship.collideWithMask = GameObjectType.None;
            ship.gameObjectType &= ~GameObjectType.PotentialTarget;
            return ship;
        }
    }
}
