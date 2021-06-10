using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.Utils.QuickStart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Items.AATestedItems
{
    class EmpImmunity
    {
        public static Item Make()
        {
            KitData data = new KitData("EMP Immunity kit", "EmpRecovery1", MeterType.StunTime, 0, Utility.Frames(20), ControlSignals.OnStun);
            data.ItemData.Level = 4;
            data.ImpactType = ImpactType.Min;
            data.Value = 0;
            data.ItemData.BuyPrice = 1200;
            data.ActiveTime = 60 * 5;
            //data.ActivationEmitterID = "Missing";
            var kit = KitQuickStart.Make(data);
            kit.Profile.IsActivatable = false;
            kit.Profile.DescriptionText = Palette.Highlight.ToTag("Clears stuns from your ship");
            kit.Profile.IsConsumed = false;
            EmitterCallerSystem system = new EmitterCallerSystem();
            system.ActiveTime = 1;
            system.CooldownTime = 0;
            system.ActivationCheck = new ActivationCheck(ControlSignals.OnStun);
            system.SelfImpactSpec = new CollisionSpec();
            system.SelfImpactSpec.AddEntry(MeterType.StunTime, 0, ImpactType.Min);
            kit.System = system;
            return kit;
        }
    }
}
