using SolarConflict.AI.GameAI;
using SolarConflict.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.Framework.Utils;
using SolarConflict.GameContent;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict
{
    class ShipCommon
    {
        public const int repairCooldownTime = 60 * 10;



        public static void AddBasicGear(Agent ship)
        {
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Generator, (SizeType)2);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Shield, (SizeType)2);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Rotation, (SizeType)2);
        }

        public static Agent MakeShip(string texture, float hitpoints, int inventorySize, float size = 0, float mass = -1f, float moment = -1f)
        {
            Agent ship = new Agent();
            ship.DrawType = DrawType.Lit;
            ship.CraftingStationType = Framework.CraftingStationType.Basic;
            ship.gameObjectType |= GameObjectType.Ship | GameObjectType.PotentialTarget;
            ship.Sprite = Sprite.Get(texture);
            if (size == 0)
            {
                size = Math.Max(ship.Sprite.Width / 2f, ship.Sprite.Height / 2f);
            }
            //if (ship.Sprite.NormalMap != null)
            //    // Default: use lighting if able
            //    ship.DrawType = DrawType.Lit;

            ship.Size = size;
            mass = 0.1f * size;

            if (moment < 0)
                moment = mass;
            ship.Mass = mass;
            ship.RotationMass = moment;
            ship.RotationMass *= ship.Size;

            ship.SetMeter(MeterType.Hitpoints, new Meter(hitpoints));

            ship.targetSelector = new TargetSelector();
            ship.Inventory = new Inventory(inventorySize);
            ship.ItemSlotsContainer = new ItemSlotsContainer();
            ship.VelocityInertia = 1;
            return ship;
        }

        //public static void AddCommonMeters(Agent ship)
        //{
        //    //ship.SetMeter(MeterType.Shield, new Meter(0)); //remove?
        //    //ship.SetMeter(MeterType.Energy, new Meter(0,0)); //remove?
        //    ship.SetMeter(MeterType.Electricity, new Meter(0));
        //    ship.SetMeter(MeterType.GlobalRepairCooldown, new Meter(repairCooldownTime));
        //}

        public static void AddCommonSystems(Agent ship)
        {
            // Loot            
            //ship.AddSystem(new LootSystem());

            ship.AddAfterSystem(new MeterMaxValueSetter(MeterType.Energy, MeterType.EnergyMaxValue));
            ship.AddAfterSystem(new MeterMaxValueSetter(MeterType.Shield, MeterType.ShieldMaxValue));

            ship.AddSystem(new FactionMeterBinder(MeterType.FactionKills));
            ship.AddSystem(new FactionMeterBinder(MeterType.Money)); //??
                                                                     //            ship.AddSystem(new FactionMeterBinder(MeterType.Reputation)); //??
            ship.AddSystem(new FactionMeterBinder(MeterType.ControlPoints)); //??

            ship.AddSystem(new ReputationUpdateSystem());

            //Damage Text
            ship.AddSystem(new DamageTextEmitter());//TODO: remove after demo

            //Destroyed Explosion //TODO: add random exp //if size is above 300 use a more impressive explosion
            BasicEmitterCallerSystem destroyedExplosion = new BasicEmitterCallerSystem(ControlSignals.OnDestroyed, "FullExplosionFx1");
            ship.AddSystem(destroyedExplosion);

            //BasicEmitterCallerSystem dropBlueprintsSystem = new BasicEmitterCallerSystem(ControlSignals.OnDestroyed, ship.ID + "BlueprintPart");
            //ship.AddSystem(dropBlueprintsSystem);

            //Stun Fx
            EmitterCallerSystem stunFx = new EmitterCallerSystem(ControlSignals.OnStun, 10, "StunFx");
            ship.AddSystem(stunFx);

            ////Shield Fx //remove //No need comes with Shield            
            //AgentEmitter shieldFx = new AgentEmitter(ControlSignals.OnTakingDamage, 5, "ShieldFx1");
            //ship.AddSystem(shieldFx);

            //Cargo Emitter
            ship.AddAfterSystem(new CargoEmitterSystem());

            ////Hull damage FX (debris)
            //AgentEmitter onDamageToHull = new AgentEmitter(ControlSignals.OnStun, 10, "ProjDebris1"); ;
            //ship.AddSystem(onDamageToHull);

            //MeterGenerator repairCooldown = new MeterGenerator(); //maybe change
            //repairCooldown.MeterType = MeterType.GlobalRepairCooldown;
            //repairCooldown.GenerationAmount = 1;
            //repairCooldown.MaxValue = repairCooldownTime;
            //ship.AddSystem(repairCooldown);



            //debris

            //low hitpoints improve
            LowHitPointsFlames lowHP = new LowHitPointsFlames();
            ship.AddAfterSystem(lowHP);
        }

        
    }
}
