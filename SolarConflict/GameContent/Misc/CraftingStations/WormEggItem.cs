using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.GameContent.Agents;
using SolarConflict.GameContent.ContentGeneration.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.NewItems
{
    class WormEggItem
    {
        public static Item Make()
        {
            var kit = ShipConstructionKitGenerator.MakeItem(null, "Egg", 2000, "Suspicious Looking Egg", 5);
            kit.Profile.DescriptionText = "May cause terrible things to happen!";
            GroupEmitter em = new GroupEmitter();
            em.AddEmitter("WormEgg");
            em.AddEmitter("BloodSplashFx1");
            em.AddEmitter("sound_splat");
            em.RandomVelocityBase = 1;
            em.RotationSpeed = 0.05f;

            kit.System = new BasicEmitterCallerSystem(ControlSignals.AlwaysOn, em);
            kit.Profile.SlotType |= SlotType.None;
            return kit;
        }
    }
}
