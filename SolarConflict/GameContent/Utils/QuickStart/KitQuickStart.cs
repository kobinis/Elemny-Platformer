using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Utils.QuickStart
{
    /// <summary>
    /// Hold the data you need to create a Kit - A Kit is an consumable item used to augment a Meter, automatically or when activated
    /// For example: 
    /// AutoRepair Kit - repairs your hull when low on hitpoints
    /// AutoShlied Kit - Repairs your shield when low shield
    /// EmpRecoveryKit - Removes stun when stunned
    /// AutoEnergyKit  - Replenish your energy when low
    /// </summary>
    public struct KitData
    {
        public ItemData ItemData;
        public MeterType MeterType;
        public float Value;
        public ImpactType ImpactType;
        public MeterType SecoundMeterType;
        public float SecoundValue;
        public int ActiveTime;
        public ImpactType SecoundImpactType;
        public int Cooldown;
        public string ActivationEmitterID;
        public ControlSignalsMask ActivationControlSignal;
        public bool IsGlobalCooldown;
        public MeterType GlobalCooldownMeter;        

        public KitData(string name, string textureID, MeterType meterType, float value, int cooldown, ControlSignalsMask activationControlSignal, bool isGlobalCooldown = false)
        {
            ItemData = new ItemData(name);
            ItemData.IconID = textureID;
            ItemData.MaxStack = 20;
            ItemData.SlotType = SlotType.Consumable;
            MeterType = meterType;
            Value = value;
            ImpactType = ImpactType.Additive;
            SecoundMeterType = MeterType.None;
            SecoundValue = 0;
            SecoundImpactType = ImpactType.Additive;
            Cooldown = cooldown;
            ActivationEmitterID = "EmitterPickupFx";
            ActivationControlSignal = activationControlSignal;
            IsGlobalCooldown = isGlobalCooldown;
            GlobalCooldownMeter = MeterType.GlobalRepairCooldown;
            ActiveTime = 1;
        }

    }

    public class KitQuickStart
    {
        public static Item Make(KitData data)
        {
            ItemProfile profile = ItemQuickStart.Profile(data.ItemData);
            profile.StatsText = ExtendedDescription(data);
            profile.IsShownOnHUD = true;
            profile.IsActivatable = true; //??
            profile.IsWorkingInInventory = true;
            profile.IsConsumed = true;
            profile.Category = ItemCategory.Consumable;

            Item item = new Item(profile);
            //TODO: change it from AgentEmitter
            EmitterCallerSystem kit = new EmitterCallerSystem();            
            kit.EmitterID = data.ActivationEmitterID;
            kit.ActivationCheck.controlMask = data.ActivationControlSignal;
            kit.CooldownTime = data.Cooldown;
            kit.SelfImpactSpec = new CollisionSpec();
            kit.SelfImpactSpec.AddEntry(data.MeterType, data.Value, data.ImpactType);
            if (data.ActiveTime > 0)
                kit.ActiveTime = data.ActiveTime;
            if(data.SecoundMeterType != MeterType.None)
            {
                kit.SelfImpactSpec.AddEntry(data.SecoundMeterType, data.SecoundValue, data.SecoundImpactType);
            }
            if (data.IsGlobalCooldown)
            {
              //  kit.ActivationCheck.AddCost(data.GlobalCooldownMeter, data.Cooldown);                                
              //  kit.SelfImpactSpec.AddEntry(data.GlobalCooldownMeter, 0, ImpactType.Min);
            }
                        
            item.System = kit;
            
            return item;
        }

        public static string ExtendedDescription(KitData data)
        {            
            string ans = $"Restores: {data.Value} {data.MeterType}";
            if(data.SecoundMeterType != MeterType.None)
            {
                ans +=$"\nRestores: {data.SecoundValue} {data.SecoundMeterType.ToString()}";
            }
            return ans;
        }
    }
}
