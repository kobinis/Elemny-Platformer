using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Misc.Buildings
{
    class StorageContainer
    {
        public static IEmitter Make()
        {
            ShipData data = new ShipData("Container1", 600, false , inventorySize: 9 * 4);
            Agent ship = ShipQuickStart.Make(data);
            ship.Name = "Storage Container";
            ship.gameObjectType |= GameObjectType.NonRotating;
            //ship.control = null;
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Shield, ship.SizeType);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Utility , ship.SizeType);
            ship.HullCost = AgentUtils.CalculateAgentHullCost(ship);
            ship.impactSpec = new CollisionSpec(0, 6);
            ship.HullCost = AgentUtils.CalculateAgentHullCost(ship);
            ship.impactSpec.IsDamaging = false;
            


            GroupEmitter groupEmitter = new GroupEmitter();
            groupEmitter.AddEmitter("StorageContainerKit");
            groupEmitter.AddEmitter("CargoDropEmitter");
            groupEmitter.AddEmitter("AgentSlotDropEmitter");
            groupEmitter.AddEmitter("RemoveGameObjectEmitter");
            
            //Add remove cargo
            //Remove slot items items

            EmitterCallerSystem minedEmitter = new EmitterCallerSystem(ControlSignals.OnColision, string.Empty);
            minedEmitter.Emitter = groupEmitter;
            minedEmitter.CooldownTime = 1;
            minedEmitter.ActivationCheck.AddCost(MeterType.MiningSpeed, 10);
            ship.AddSystem(minedEmitter);
            ship.gameObjectType |= GameObjectType.Mineable;
            ship.gameObjectType |= GameObjectType.NonRotating;
            return ship;
        }
    }
}
