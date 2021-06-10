using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc;
using SolarConflict.GameContent.Utils;
using System.Text;

namespace SolarConflict.GameContent
{

    public struct CloakingDeviceData
    {
        public ItemData ItemData;

        public int ReCloakingCooldown { get; set; }

        public float Capacity { get; set; }
        public float GenerationRate { get; set; }
        public float InCommbatGenerationRate { get; set; }
        public int DepletionCooldown { get; set; }
        public string EffectEmitterID { get; set; }
        public Color EffectColor { get; set; }

        public string AbilityEmitterID
        {
            set { AbilityEmitter = ContentBank.Inst.GetEmitter(value); }
            get { return AbilityEmitter?.ID; }
        }
        public IEmitter AbilityEmitter { get; set; }
        public int AblitiyCooldown { get; set; }
        public ControlSignals AbilitiyActivation { get; set; }

        public CloakingDeviceData(string name)
        {
            ItemData = new ItemData(name);
            ItemData.SlotType = SlotType.Shield;
            Capacity = 0;
            GenerationRate = 0;
            InCommbatGenerationRate = 0;
            DepletionCooldown = 0;
            EffectEmitterID = null;
            EffectColor = Color.White;
            AbilityEmitter = null;
            AblitiyCooldown = 30;
            AbilitiyActivation = ControlSignals.OnLowHitpoints;
            ReCloakingCooldown = 0;
        }
    }

    public class CloakingDeviceQuickStart
    {
        public static string ExtendedDescription(string description, CloakingDeviceData data)
        {
            if (description == null)
                description = string.Empty;
            StringBuilder sb = new StringBuilder(description);
            sb.AppendLine();
            sb.AppendLine("ReCloakingCooldown: " + data.ReCloakingCooldown.ToString());
            sb.AppendLine("Capacity: " + data.Capacity.ToString() + "#image{Shield}");
            sb.AppendLine("Generation: " + (data.GenerationRate * 60).ToString() + "#image{Shield}/sec");
            return sb.ToString();
        }

        public static Item Make(CloakingDeviceData data)
        {
            ItemProfile profile = ItemQuickStart.Profile(data.ItemData);            
            profile.StatsText = ExtendedDescription(string.Empty, data);
            profile.BreaksCloaking = false;
            profile.Category = ItemCategory.Cloaking;            

            SystemGroup systemGroup = new SystemGroup();

            CloakingSystem cloakingSystem = new CloakingSystem();            
            cloakingSystem.ActivationEmitterID = "FxEmitterCloak";
            cloakingSystem.Cooldown = data.ReCloakingCooldown;
            systemGroup.AddSystem(cloakingSystem, true);
            

            if (data.Capacity > 0 || data.GenerationRate > 0)
            {
                MeterGenerator shieldSystem = new MeterGenerator();
                shieldSystem.MeterType = MeterType.Shield;
                shieldSystem.MeterToTakeMaxValue = MeterType.ShieldMaxValue;
                shieldSystem.MaxValue = data.Capacity;
                shieldSystem.GenerationAmountPerSec = data.GenerationRate * 60;
                //shieldSystem.DepletionCooldownTime = data.DepletionCooldown;
                shieldSystem.DisruptedGenerationAmountPerSec = data.InCommbatGenerationRate * 60;
                systemGroup.AddSystem(shieldSystem, false, false);
            }

            if (data.AbilityEmitter != null)
            {
                EmitterCallerSystem abilitySystem = new EmitterCallerSystem(data.AbilitiyActivation, data.AblitiyCooldown, Vector2.Zero, data.AbilityEmitter);
                systemGroup.AddSystem(abilitySystem, true, false);
            }

            // Shield visuals
            if (string.IsNullOrWhiteSpace(data.EffectEmitterID))
            {
                EmitterCallerSystem effectSystem = new EmitterCallerSystem();
                effectSystem.EmitterID = data.EffectEmitterID;
                effectSystem.ActivationCheck = new ActivationCheck(ControlSignals.OnDamageToShield);
                effectSystem.MaxLifetime = 30;
                effectSystem.CooldownTime = 30;
                effectSystem.EmitterSpeed = 0;
                effectSystem.Color = data.EffectColor;
                systemGroup.AddSystem(effectSystem, false, false);
            }

            Item item = new Item(profile);
            item.System = systemGroup;
            return item;
        }
    }
}

