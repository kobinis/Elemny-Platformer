using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Emitters.ImbueAbilities
{
    class LifelineAbility
    {
        public static ImbueSystem Make()
        {
            ImbueSystem imbueSystem = new ImbueSystem();
            imbueSystem.PrefixText = "Lifeline";

            EmitterCallerSystem system = new EmitterCallerSystem("EmitterPickupFx");
            system.Color = Color.LightGreen;
            system.ActiveTime = 3;
            system.ActivationCheck = new ActivationCheck(ControlSignals.OnLowHitpoints);
            system.SelfImpactSpec = new CollisionSpec();
            system.SelfImpactSpec.AddEntry(MeterType.Hitpoints, 600);
            system.SelfImpactSpec.AddEntry(MeterType.Shield, 1600);
            system.CooldownTime = 60*60;
            imbueSystem.system = system;
            return imbueSystem;
        }
    }
}
