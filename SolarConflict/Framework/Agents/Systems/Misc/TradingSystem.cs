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
    /// System that sells items from inventory
    /// </summary>
    [Serializable]
    public class TradingSystem : AgentSystem
    {
        public float SellRatio { get; set; }
        public ItemCategory ItemCategory { get; set; }
        public int CooldownTime { get; set; }
        private int _cooldown;

        public TradingSystem()
        {
            CooldownTime = 20;
            SellRatio = 0.5f;
            ItemCategory = ItemCategory.Material;
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            bool wasActivated = false;
            if (_cooldown <= 0 && tryActivate && agent.Inventory != null)
            {
                for (int i = 0; i < agent.Inventory.Size; i++) //TODO: go from the end of the inventory, only break one sell value is bigger then a param
                {
                    if (agent.Inventory.Items[i] != null && (agent.Inventory.Items[i].Profile.Category & ItemCategory) > 0)
                    {
                        float price = agent.Inventory.Items[i].GetStackSellPrice(SellRatio);
                        //agent.AddMeterValue(MeterType.Money, price); //TODO: maybe change to faction
                        var meter = gameEngine.GetFaction(agent.FactionType).GetMeter(MeterType.Money);
                        meter.AddValue(price);
                        agent.Inventory.Items[i] = null;
                        wasActivated = true;
                        break;
                    }
                }
            }
            _cooldown--;
            return wasActivated;
        }

        public override AgentSystem GetWorkingCopy()
        {
            return (AgentSystem)MemberwiseClone();
        }
    }
}
