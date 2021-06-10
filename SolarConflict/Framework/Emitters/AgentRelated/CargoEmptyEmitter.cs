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
    class CargoDropEmitter : IEmitter
    {
        public string ID { get { return "CargoDropEmitter"; }  set { } }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            Agent ship = parent as Agent;
            if (ship != null && ship.Inventory != null)
            {               
                Inventory cargo = ship.Inventory;

                for (int i = 0; i < cargo.Size; i++) //TODO: change it, you don't need to go over all the Capacity
                {
                    Item cargoItem = cargo.GetItem(i);
                    if (cargoItem != null && !cargoItem.Profile.IsRetainedOnDeath)
                    {
                        cargoItem = cargo.RemoveItem(i);
                        cargoItem.Position = ship.Position;
                        cargoItem.Velocity.X = (gameEngine.Rand.Next(21) - 10) / 2f; //change
                        cargoItem.Velocity.Y = (gameEngine.Rand.Next(21) - 10) / 2f;
                        cargoItem.Rotation = (float)gameEngine.Rand.NextDouble() * MathHelper.TwoPi;
                        gameEngine.AddList.Add(cargoItem);
                    }
                }               
            }
            return null;
        }

    }
}
