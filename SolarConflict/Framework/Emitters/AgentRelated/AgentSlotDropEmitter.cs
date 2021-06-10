using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.Emitters
{
    //if parent is an agent it will emmit it's
    [Serializable]
    class AgentSlotDropEmitter : IEmitter
    {
        public string ID { get { return "AgentSlotDropEmitter"; } set { } }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            Agent ship = parent as Agent;
            if (ship != null && ship.ItemSlotsContainer != null)
            {
                var cargo = ship.ItemSlotsContainer.GetItemSlotsList();              
                for (int i = 0; i < cargo.Count; i++) //TODO: change it, you don't need to go over all the Capacity
                {
                    ItemSlot itemSlot = cargo[i];
                    if (itemSlot.Item != null)
                    {
                        Item item = itemSlot.Item;
                        itemSlot.Item = null;
                        item.Position = ship.Position;
                        item.Velocity.X = (gameEngine.Rand.Next(21) - 10) / 2f; //change
                        item.Velocity.Y = (gameEngine.Rand.Next(21) - 10) / 2f;
                        item.Rotation = (float)gameEngine.Rand.NextDouble() * MathHelper.TwoPi;
                        gameEngine.AddList.Add(item);
                    }
                }
            }
            return null;
        }

    }
}
