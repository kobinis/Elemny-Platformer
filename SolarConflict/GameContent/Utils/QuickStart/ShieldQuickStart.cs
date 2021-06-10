using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent
{

    public struct ShieldData
    {
        public ItemData ItemData;
        public float Capacity { get; set; }        
        public float GenerationRatePerSec { get; set; }
        public float InCombatGenerationRatePerSec { get; set; }        
        public float RechargeDelayInSec { get; set; } //Possibly change name to Recovery Delay
        
        public string EffectEmitterID { get; set; }
        public Color EffectColor { get; set; }
        
        public string AbilityEmitterID {
            set { AbilityEmitter = ContentBank.Inst.GetEmitter(value); }
            get { return AbilityEmitter?.ID; }
        }
        public IEmitter AbilityEmitter { get; set; }
        public int AblitiyCooldown { get; set; }
        public ControlSignals AbilitiyActivation { get; set; }

        public ShieldData(string name)            
        {
            ItemData = new ItemData(name);            
            ItemData.SlotType = SlotType.Shield;
            Capacity = 0;
            GenerationRatePerSec = 0;
            InCombatGenerationRatePerSec = 0;
            RechargeDelayInSec = 0;
            EffectEmitterID = "ShieldAuraFx";
            EffectColor = Color.LightBlue;
            AbilityEmitter = null;
            AblitiyCooldown = 30;
            AbilitiyActivation = ControlSignals.OnLowHitpoints;
            
        }
    }

    public class ShieldQuickStart
    {   
        public static string ExtendedDescription(string description, ShieldData data)
        {
            //if (description == null)
            //    description = "Provides hitpoints;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Capacity: " + Palette.Shield.ToTag(Math.Round(data.Capacity).ToString()) + "");
            if(data.GenerationRatePerSec == data.InCombatGenerationRatePerSec)
                sb.AppendLine("Generation: " + Palette.Shield.ToTag(Math.Round(data.GenerationRatePerSec).ToString()) + " / s");
            else
            {
                if(data.GenerationRatePerSec > 0)
                    sb.AppendLine("Generation: " + Palette.Shield.ToTag(Math.Round(data.GenerationRatePerSec).ToString()) + " / s");
                if (data.InCombatGenerationRatePerSec > 0)
                    sb.AppendLine("Disrupted Generation: " + Palette.Shield.ToTag(Math.Round(data.InCombatGenerationRatePerSec).ToString()) + " / s");
            }

            if(data.RechargeDelayInSec >0)
                sb.AppendLine("Recovery Time: " + Palette.Damage.ToTag(data.RechargeDelayInSec.ToString()) + " s");
            sb.Remove(sb.Length-1, 1);
            return sb.ToString();
        }

        public static Item Make(ShieldData data)
        {
            ItemProfile profile = ItemQuickStart.Profile(data.ItemData);
            profile.SlotType = SlotType.Shield;
            profile.StatsText = ExtendedDescription(string.Empty, data);
            profile.Category = ItemCategory.Shield;

            SystemGroup shieldSystemGroup = new SystemGroup();
            MeterGenerator shieldSystem = new MeterGenerator();
            shieldSystem.MeterType = MeterType.Shield;
            shieldSystem.MeterToTakeMaxValue = MeterType.ShieldMaxValue;
            shieldSystem.MaxValue = data.Capacity;
            shieldSystem.GenerationAmountPerSec = data.GenerationRatePerSec;
            shieldSystem.DisruptedGenerationAmountPerSec = data.InCombatGenerationRatePerSec;
            shieldSystem.RechargeDelayInFrames = (int)Math.Round(data.RechargeDelayInSec * 60);            
            shieldSystemGroup.AddSystem(shieldSystem, false, false);

            if (data.AbilityEmitter != null)
            {                
                EmitterCallerSystem abilitySystem = new EmitterCallerSystem(data.AbilitiyActivation, data.AblitiyCooldown, Vector2.Zero, data.AbilityEmitter);
                shieldSystemGroup.AddSystem(abilitySystem, true, false);
            }

            // Shield visuals
            if (!string.IsNullOrWhiteSpace(data.EffectEmitterID))
            {
                EmitterCallerSystem effectSystem = new EmitterCallerSystem(); //TODO: change to basic emitter caller system
                effectSystem.EmitterID = data.EffectEmitterID;
                effectSystem.ActivationCheck = new ActivationCheck(ControlSignals.OnDamageToShield);
                effectSystem.MaxLifetime = 30;
                effectSystem.CooldownTime = 30;
                effectSystem.EmitterSpeed = 0;
                effectSystem.Color = data.EffectColor;
                shieldSystemGroup.AddSystem(effectSystem, false, false);
            }

            Item item = new Item(profile);            
            item.System = shieldSystemGroup;
            return item;
        }
    }
}
