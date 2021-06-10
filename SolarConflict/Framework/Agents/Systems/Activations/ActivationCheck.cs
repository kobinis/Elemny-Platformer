using SolarConflict.Framework.Agents.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict
{


    //TODO: add indication and energy (or other need)
    [Serializable]
    public struct ItemCost//
    {
        public string itemId; //maybe change to item/item profile
        public int amount;
        public bool consumed; 
    
        public ItemCost(string id, int amount, bool consumed)
        {
            this.itemId = id;
            this.amount = amount;
            this.consumed = consumed;            
        }
    }

    [Serializable]
    public struct ControlSignalsMask
    {
        public ControlSignals Signals;
        public ControlSignals NotMask; //use Xor to apply not on input

        public ControlSignalsMask(ControlSignals controlMask, ControlSignals notMask)
        {
            this.Signals = controlMask;
            this.NotMask = notMask;
        }

        public bool CheckOrMask(ControlSignals controlSignals)
        {
            return (controlSignals & (Signals ^ NotMask)) != 0;            
        }

        public bool CheckAndMask(ControlSignals controlSignals)
        {
           return (controlSignals & (Signals ^ NotMask)) == controlSignals;         
        }

        public static implicit operator ControlSignalsMask(ControlSignals controlSignals)
        {
            ControlSignalsMask o = new ControlSignalsMask(controlSignals, ControlSignals.None);
            return o;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ActivationCheck:SystemActivationCheck
    {
        public ControlSignalsMask controlMask; // moveOut??
        public List<ControlSignalsMask> controlMaskAndList;
 
        public Dictionary<MeterType, float> costList; // can be array or list of stuct;

        public List<ItemCost> itemCostList; //maybe needs to be a dictionery       

        public void AddItemCost(string id, int amount, bool consumed = true)
        {                         
              itemCostList.Add(new ItemCost(id, amount, consumed));             
        }

        public ActivationCheck(ControlSignals controlMask, ControlSignals notMask = 0)
        {
            this.controlMask = new ControlSignalsMask(controlMask, notMask);;
            controlMaskAndList = new List<ControlSignalsMask>();
            costList = new Dictionary<MeterType, float>();
            itemCostList = new List<ItemCost>();
        }

        public ActivationCheck():this(0,0)
        {
        }
               

        /// <summary>
        /// Checks activation conditions... needs to be tested
        /// Dose not check cooldown
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool Check(Agent agent, bool tryActivate)
        {
            if (!tryActivate)
            {               
                if (controlMask.CheckOrMask(agent.ControlSignal))
                {
                    tryActivate = true;
                }
                else
                {
                    foreach (var maskAnd in controlMaskAndList)
                    {
                        if (maskAnd.CheckAndMask(agent.ControlSignal))
                        {
                            tryActivate = true;
                            break;
                        }
                    }
                }
            }
            
            if (tryActivate)
            {
                foreach (var cost in costList)
                {                    
                    if (agent.GetMeterValue(cost.Key) < cost.Value)
                        return false;
                }

                Inventory inventory = agent.GetInventory();
                if (inventory != null)
                {
                    foreach (var itemCost in itemCostList)
                    {
                        if (!inventory.CheckForItem(itemCost.itemId, itemCost.amount))
                            return false;
                    }
                }
              

                return true; //add Item activation cost
            }

            return false;
        }

        
        public override void DrainCost(Agent agent)
        {
            foreach (var cost in costList)
            {
                Meter meter = agent.GetMeter(cost.Key);
                if (meter != null)
                {
                    meter.AddValue(-cost.Value);
                }
            }

            Inventory inventory = agent.GetInventory();
            if (inventory != null)
            {
                foreach (var itemCost in itemCostList)
                {
                    if (itemCost.consumed)
                        inventory.RemoveItem(itemCost.itemId, itemCost.amount);
                }
            }
        }

        public void AddCost(MeterType type, float amount)
        {
            costList[type] = amount;
        }

        public void SetSignalBind(ControlSignals signal, ControlSignals notSignal = 0)
        {
            controlMask = new ControlSignalsMask(signal, notSignal);
        }

        public void AddAndBind(ControlSignals controlMask, ControlSignals notMask = 0)
        {
            controlMaskAndList.Add(new ControlSignalsMask(controlMask, notMask));
        }       

        public override SystemActivationCheck GetWorkingCopy() 
        {
            return this;
            //return (ActivationCheck)MemberwiseClone();
        }        
    }
}
