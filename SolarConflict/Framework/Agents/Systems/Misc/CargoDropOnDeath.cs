using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict
{
    /// <summary>
    /// CargoEmitterSystem - Drops all cargo when active
    /// </summary>
    [Serializable]
    public class CargoEmitterSystem : AgentSystem //KOBI: add option to drop cargo in a cone
    {
        ActivationCheck activationCheck;
        //float angleRange;
        public CargoEmitterSystem()
        {
            activationCheck = new ActivationCheck(ControlSignals.OnDestroyed);
        }

        public override bool Update(Agent ship, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate)
        {
            bool wasActive = false;
            if (ship.Inventory != null && activationCheck.Check(ship, tryActivate)) //TODO: change it
            {
                
                wasActive = true;
                Inventory cargo = ship.Inventory;
                //int startIndex = ship.FactionType == Framework.FactionType.Player ? cargo.Size : 0;
                if (!GameplaySettings.KeepInventory || ship.FactionType != Framework.FactionType.Player)
                {
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
              
            }

            return wasActive;
        }        

        public override AgentSystem GetWorkingCopy()
        {
            return this;
           // return cargoEmitterCopy;
        }

        


    }
}
