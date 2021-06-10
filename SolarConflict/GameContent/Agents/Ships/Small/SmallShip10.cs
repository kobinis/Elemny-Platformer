using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Ships
{
    class SmallShip10
    {
        public static GameObject Make()
        {
            Agent ship = ShipQuickStart.Make("SmallShip10", 0, false, 9 * 3);
            ship.Name = "Disarray";
            ship.FactionType = Framework.FactionType.Pirates1;
            //var ai = new SmartAI();
            //ai.MinimalDistance = 0;
            //ai.OptimalDistance = 0;
            //ship.control.controlAi = ai;

            DecorationSystem skullEyes = new DecorationSystem();
            skullEyes.Scale = 0.5f;
            skullEyes.Sprite = Sprite.Get("redeye");
            skullEyes.AddDecoration(new Vector2(3, 3), 0);
            skullEyes.AddDecoration(new Vector2(3, -3), 0);
            ship.AddSystem(skullEyes);
            //DecorationSystem wingLights = new DecorationSystem();
            //wingLights.DecorationColor = new Color(250, 250, 255, 150);
            //wingLights.AddDecoration(new Vector2(-51, 53), 0);
            //wingLights.AddDecoration(new Vector2(-51, -53), MathHelper.Pi);
            //ship.AddSystem(wingLights);

            BasicEmitterCallerSystem rammingFront = new BasicEmitterCallerSystem(ControlSignals.AlwaysOn, typeof(RammingFront).Name);
            ship.AddAfterSystem(new SystemHolder(rammingFront, new Vector2(56, 0), 0));

            //AgentEmitter impactSound = new AgentEmitter();
            //impactSound.ActivationCheck.controlMask.ControlMask = ControlSignals.OnColision; //TODO: fix
            //ship.AddSystem(new SystemHolder(impactSound, new Vector2(42, 0), 0));

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, (SizeType)0, new Vector2(-52, 0), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, (SizeType)0, new Vector2(-34, -33), 0, ControlSignals.Action3);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, (SizeType)0, new Vector2(-34, 33), 0, ControlSignals.Action4);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, (SizeType)0, new Vector2(27, -54), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, (SizeType)0, new Vector2(27, 54), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Engine, (SizeType)0, new Vector2(23, 0), 0, ControlSignals.Action1);

            // ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon, (SizeType)0, new Vector2(-16, 0), 0, ControlSignals.None);

            ShipQuickStart.AddBasicGearSlots(ship);
            ShipQuickStart.FinalizeShip(ship);
            return ship;
        }
    }
}
