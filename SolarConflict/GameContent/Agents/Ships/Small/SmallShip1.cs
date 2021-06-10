using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Reflection;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.GameContent.Utils;

namespace SolarConflict.GameContent.Ships
{
    class SmallShip1
    {

        public static GameObject Make()
        {
            Agent ship = ShipQuickStart.Make("SmallShip1", 0);
            ship.Name = "Insurgent";
            ship.FactionType = Framework.FactionType.Federation;
            ship.SetMeter(MeterType.Right, new Meter(4));
            ship.SetMeter(MeterType.Left, new Meter(0, 4));
            float speed = 30;

            var leftGun = new EmitterCallerSystem(ControlSignals.Action1, "EmitterGun1"); //TODO: fix gun
            leftGun.ActiveTime = 40;
            leftGun.CooldownTime = 50;
            leftGun.MidCooldownTime = 10;
            leftGun.velocity = Vector2.UnitX * speed;
            leftGun.ActivationCheck.AddCost(MeterType.Energy, 30);
            leftGun.ActivationCheck.AddCost(MeterType.Left, 4);
            leftGun.refVelocityMult = 1;
            leftGun.SelfImpactSpec = new CollisionSpec();
            leftGun.SelfImpactSpec.ImpactList.Add(new MeterCollisionSpec(MeterType.Right, 1));
            ship.AddSystem(new SystemHolder(leftGun, new Vector2(5, -23), 0));
            var rightGun = new EmitterCallerSystem(ControlSignals.Action1, "EmitterGun1");
            rightGun.ActiveTime = leftGun.ActiveTime;
            rightGun.CooldownTime = leftGun.CooldownTime;
            rightGun.MidCooldownTime = 10;
            rightGun.velocity = Vector2.UnitX * speed;
            rightGun.ActivationCheck.AddCost(MeterType.Energy, 30);
            rightGun.ActivationCheck.AddCost(MeterType.Right, 4);
            rightGun.refVelocityMult = 1;
            rightGun.SelfImpactSpec = new CollisionSpec();
            rightGun.SelfImpactSpec.ImpactList.Add(new MeterCollisionSpec(MeterType.Left, 1));
            ship.AddSystem(new SystemHolder(rightGun, new Vector2(5, 23), 0));
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, SizeType.Small, new Vector2(-26, 0), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, SizeType.Small, new Vector2(-34, -39), 270, ControlSignals.Right);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, SizeType.Small, new Vector2(-34, 39), 90, ControlSignals.Left);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Engine, SizeType.Small, new Vector2(7, 0), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, SizeType.Small, new Vector2(0, -52), 270, ControlSignals.Action3);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, SizeType.Small, new Vector2(0, 52), 90, ControlSignals.Action4);

            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Generator, SizeType.Small);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Shield, SizeType.Small);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Rotation, SizeType.Small);
            ship.HullCost = AgentUtils.CalculateAgentHullCost(ship);

            return ship;
        }
    }
}
