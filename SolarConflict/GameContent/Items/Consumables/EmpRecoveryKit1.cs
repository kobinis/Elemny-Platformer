using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.Utils.QuickStart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items
{
    class EmpRecoveryKit1
    {
        public static Item Make()
        {

            KitData data = new KitData("EMP Recovery Kit", "EmpRecovery1", MeterType.StunTime, 0, Utility.Frames(20), ControlSignals.OnStun);
            data.ItemData.Level = 4;
            data.ImpactType = ImpactType.Min;
            data.Value = 0;
            data.ItemData.BuyPrice = 1200;
            data.ActiveTime = 60 * 5;
            data.ActivationEmitterID = "Missing";
            var kit = KitQuickStart.Make(data);
            kit.Profile.IsActivatable = false;
            kit.Profile.DescriptionText = Palette.Highlight.ToTag("Clears stuns from your ship");
            EmitterCallerSystem system = new EmitterCallerSystem("EmitterPickupFx");
            system.ActiveTime = 60 * 5;
           system.CooldownTime = system.ActiveTime;
            system.ActivationCheck = new ActivationCheck(ControlSignals.OnStun);
            system.SelfImpactSpec = new CollisionSpec();
            system.SelfImpactSpec.AddEntry(MeterType.StunTime, 0, ImpactType.Min);
            kit.System = system;
            return kit;
        }
    }
}
