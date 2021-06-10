using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Emitters.ImbueAbilities
{
    class EmpAbility
    {
        public static ImbueSystem Make()
        {
            ImbueSystem imbueSystem = new ImbueSystem();
            imbueSystem.PrefixText = "EMP";

            EmitterCallerSystem system = new EmitterCallerSystem("AoeStun2");
            system.MaxLifetime = 90;
            system.ActivationCheck = new ActivationCheck(ControlSignals.OnLowHitpoints);
            //system.Color = Color;
            system.SelfImpactSpec = new CollisionSpec();
            system.CooldownTime = 60 * 60;
            
            imbueSystem.system = system;
            return imbueSystem;
        }
    }
}
