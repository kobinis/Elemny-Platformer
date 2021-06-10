using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent
{

    public struct GeneratorData
    {
        public ItemData ItemData;
        public float Capacity { get; set; }
        public float GenerationRatePerSec { get; set; }
        public float InCombatGenerationRatePerSec { get; set; }
        public float RechargeDelayInSec { get; set; } //Possibly change name to Recovery Delay

       // public string EffectEmitterID { get; set; }
       // public Color EffectColor { get; set; }

        public string AbilityEmitterID
        {
            set { AbilityEmitter = ContentBank.Inst.GetEmitter(value); }
            get { return AbilityEmitter?.ID; }
        }
        public IEmitter AbilityEmitter { get; set; }
        public int AblitiyCooldown { get; set; }
        public ControlSignals AbilitiyActivation { get; set; }

        public GeneratorData(string name)
        {
            ItemData = new ItemData(name);
            ItemData.SlotType = SlotType.Shield;
            Capacity = 0;
            GenerationRatePerSec = 0;
            InCombatGenerationRatePerSec = 0;
            RechargeDelayInSec = 0;
           // EffectEmitterID = "ShieldAuraFx";
            //EffectColor = Color.White;
            AbilityEmitter = null;
            AblitiyCooldown = 30;
            AbilitiyActivation = ControlSignals.OnLowHitpoints;

        }
    }

    public class GeneratorQuickStart
    {
        public static string ExtendedDescription(string description, GeneratorData data)
        {
            //if (description == null)
            //    description = "Provides hitpoints;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Capacity: " + Palette.Shield.ToTag(Math.Round(data.Capacity).ToString()) + "");
            if (data.GenerationRatePerSec == data.InCombatGenerationRatePerSec)
                sb.AppendLine("Generation: " + Palette.Energy.ToTag(Math.Round(data.GenerationRatePerSec).ToString()) + " / s");
            else
            {
                if (data.GenerationRatePerSec > 0)
                    sb.AppendLine("Generation: " + Palette.Energy.ToTag(Math.Round(data.GenerationRatePerSec).ToString()) + " / s");
                if (data.InCombatGenerationRatePerSec > 0)
                    sb.AppendLine("Disrupted Generation: " + Palette.Energy.ToTag(Math.Round(data.InCombatGenerationRatePerSec).ToString()) + " / s");
            }

            if (data.RechargeDelayInSec > 0)
                sb.AppendLine("Recovery Time: " + Palette.Damage.ToTag(data.RechargeDelayInSec.ToString()) + " s");
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public static Item Make(GeneratorData data)
        {
            ItemProfile profile = ItemQuickStart.Profile(data.ItemData);
            profile.SlotType = SlotType.Generator;            
            profile.StatsText =  ExtendedDescription(string.Empty, data);
            profile.Category = ItemCategory.Generator;

            SystemGroup shieldSystemGroup = new SystemGroup();
            MeterGenerator shieldSystem = new MeterGenerator();
            shieldSystem.MeterType = MeterType.Energy;
            shieldSystem.MeterToTakeMaxValue = MeterType.EnergyMaxValue;
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

            Item item = new Item(profile);
            item.System = shieldSystemGroup;
            return item;
        }
    }
}
