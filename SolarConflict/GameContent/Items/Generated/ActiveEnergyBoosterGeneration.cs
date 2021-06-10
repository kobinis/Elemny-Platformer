using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.Generated
{
    /// <summary>
    /// Active energy boosters gives you a temporary boost in energy generation
    /// </summary>
    class ActiveEnergyBoosterGeneration
    {
        public static IEmitter Make()
        {
            int skipLevel = 3;
            var idNumber = 0;
            for (int i = 2; i < ScalingUtils.NumLevels; i+= skipLevel)
                ContentBank.Inst.AddContent(MakeBoosterItem(i, idNumber++));

            return null;
        }

        public static Item MakeBoosterItem(int level, int idNumber)
        {
            ItemData data = new ItemData($"EnergyBooster {idNumber}");
            data.BreaksClocking = false;
            data.IconID = "ActiveEnergyBooster";
            
            data.SlotType = SlotType.Utility;
            if (level > 0)
                data.SecounderyIconID = $"lvl{level}";
            data.Category = ItemCategory.Utility;
            data.Level = level;
            data.BuyPrice = ScalingUtils.ScaleCost(level);

            Item item = ItemQuickStart.Make(data);
            item.ID = $"ActiveEnergyBooster{idNumber}";
            item.Profile.IsShownOnHUD = true;

            EmitterCallerSystem system = new EmitterCallerSystem();
            int duration = 15;
            int cooldown = 60;
            system.CooldownTime = cooldown * 60;
            system.ActiveTime = duration * 60;
            system.SelfImpactSpec = new CollisionSpec();
            system.SelfImpactSpec.AddEntry(MeterType.Energy, EnergyGenerationRate(level));
            system.EmitterID = "RedLightFx";
            system.EmitterSpeed = 0;
            item.Profile.DescriptionText = "Boosts energy generation for a limited time";
            StringBuilder sb = new StringBuilder();            
            sb.AppendLine("Generation/Sec: " + Color.Yellow.ToTag(EnergyGenerationRate(level).ToString()));
            sb.AppendLine("Duration: " + Color.LightBlue.ToTag(duration.ToString()) + " Sec");
            sb.Append("Cooldown: " + Color.LightBlue.ToTag(cooldown.ToString()) + " Sec");

            item.Profile.StatsText = sb.ToString();
            item.System = system;
            return item;
        }

        
        
        public static string ExstendDiscription(EmitterCallerSystem system)
        {
            return null;
        }      

        public static float EnergyGenerationRate(int level)
        {
            return ScalingUtils.ScaleEnergyCostPerFrame(level) * 3;
        }
    }
}
