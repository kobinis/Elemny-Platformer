using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.Framework.MetaGame.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Utils
{
    public struct ShipData
    {
        public string Name;
        public int Hitpoints;
        public string SpriteID;
        public float? Size;
        public float? Mass;
        public float RotationMass;
        public SizeType? SizeType;
        public int? InventorySize;
        public CraftingStationType CraftingStationType;
        public float VelocityInertia;
        public bool IsAddCommonSystems;
        public bool IsNonRotating;
        public int ShieldSlotNum;
        public int GeneratorSlotNum;
        public int UtilSlotNum;
        //public FactionType


        [XmlArray("ItemSlotList"), XmlArrayItem(typeof(ItemSlot), ElementName = "ItemSlot")]
        public List<ItemSlot> ItemSlotList { get; set; }
               
        public ShipData(string spriteID, int hitpoints, bool isNonRotating = false, float? size = null, float? mass = null, float rotationMass = 0, SizeType? sizeType = null, int? inventorySize = null)
        {
            Name = null;
            IsNonRotating = isNonRotating;
            Hitpoints = hitpoints;
            SpriteID = spriteID;
            Size = null;
            Mass = mass;
            RotationMass = rotationMass;
            SizeType = sizeType;
            InventorySize = inventorySize;
            CraftingStationType = CraftingStationType.Basic;
            VelocityInertia = Consts.DefaultVelocityInertia;
            IsAddCommonSystems = true;
            ShieldSlotNum = 1;
            GeneratorSlotNum = 1;
            UtilSlotNum = 1;

            ItemSlotList = new List<SolarConflict.ItemSlot>();
        }

        public ShipData(Agent agent, bool isAddCommonSystems =  true)
        {
            Name = agent.Name;
            IsNonRotating = ((agent.gameObjectType & GameObjectType.NonRotating) > 0);
            SpriteID = agent.Sprite.ID;
            Hitpoints = (int)agent.MaxHitpoints;
            Size = agent.Size;
            Mass = agent.Mass;
            RotationMass = agent.RotationMass;
            SizeType = agent.SizeType;
            InventorySize = agent.GetInventory().Size;
            CraftingStationType = agent.CraftingStationType;
            VelocityInertia = agent.VelocityInertia;
            IsAddCommonSystems = isAddCommonSystems;

            ShieldSlotNum = 1; //TODO: get from agent
            GeneratorSlotNum = 1;
            UtilSlotNum = 1;

            ItemSlotList = new List<ItemSlot>();
            if (agent.ItemSlotsContainer != null)
                ItemSlotList = agent.ItemSlotsContainer.GetItemSlotsList();
        }
    }

    class ShipQuickStart
    {
        public static List<string> ShipNames = GalaxyGenerator.LoadNamesListFromFile();
        private const int repairCooldownTime = 60 * 10;
        private static int[] SIZE_THRESHOLD = { 0, 110, 201, 301, 401 };

        public static Agent Make(string spriteID, int hitpoints, bool isNonRotating = false, int? inventorySize = null)
        {
            ShipData data = new ShipData(spriteID, hitpoints, isNonRotating);            
            data.InventorySize = inventorySize;
            return Make(data);
        }

        public static string GenerateShipName(Agent agent)
        {
            if (agent != null && agent.Sprite != null && agent.Sprite.ID != null)
            {
                int index = Math.Abs( agent.Sprite.ID.GetHashCode() + 300000) % ShipQuickStart.ShipNames.Count;
                string name = ShipQuickStart.ShipNames[index];
                return name;
            }
            return string.Empty;
        }

        public static Agent Make(ShipData data, bool addBasicSlots = false)
        {
            Agent ship = new Agent();
            ship.Name = data.Name;            
            ship.DrawType = DrawType.Lit;
            ship.CraftingStationType = data.CraftingStationType;
            ship.gameObjectType |= GameObjectType.Ship | GameObjectType.PotentialTarget;
            if (data.IsNonRotating)
            {
                ship.gameObjectType |= GameObjectType.NonRotating;
                ship.AddAfterSystem(new RotationFixer(0));
                //ship.control.controlAi = new RoundShipAI();
                ship.control.controlAi = null;
            }
            else
            {
                ship.control.controlAi = null;//new SmartAI();
            }
            ship.Sprite = Sprite.Get(data.SpriteID);
            float size = data.Size.HasValue ? data.Size.Value : Math.Max(ship.Sprite.Width / 2f, ship.Sprite.Height / 2f);
            ship.Size = size;
            ship.SizeType = data.SizeType.HasValue ? data.SizeType.Value : CalculateSizeType(size);
            var mass = data.Mass ?? DefaultMass(data);
            ship.Mass = mass;
            ship.RotationMass = mass * ship.Size;
            int inventorySize = data.InventorySize.HasValue ? data.InventorySize.Value : ((int)ship.SizeType + 2) * 9;
            ship.Inventory = new Inventory(inventorySize); //TODO: maybe only if >0
            int mult = 10;
            switch (ship.SizeType)
            {
                case SizeType.Small:
                    mult = 10;
                    break;
                case SizeType.Medium:
                    mult = 20;
                    break;
                case SizeType.Large:
                    mult = 30;
                    break;
                case SizeType.Huge:
                    mult = 30;
                    break;
                case SizeType.Gigantic:
                    mult = 30;
                    break;
                default:
                    break;
            }
            if (data.Hitpoints == 0)
                data.Hitpoints = (int)(ship.Size * mult);
            ship.SetMeter(MeterType.Hitpoints, new Meter(data.Hitpoints));
            ship.targetSelector = new TargetSelector(); //??
            ship.ItemSlotsContainer = new ItemSlotsContainer();
            ship.VelocityInertia = data.VelocityInertia;
            ship.analogDiractions[0] = FMath.ToCartesian(1, -MathHelper.PiOver2);
            ship.analogDiractions[1] = ship.analogDiractions[0];

            if (data.IsAddCommonSystems)
                AddCommonSystems(ship);

            ship.ItemSlotsContainer = new ItemSlotsContainer();
            foreach (var item in data.ItemSlotList)
            {
                ship.ItemSlotsContainer.AddAgentSlot(item);
            }

            if(addBasicSlots)
            {
                ShipQuickStart.AddBasicGearSlots(ship, !data.IsNonRotating, data.GeneratorSlotNum, data.ShieldSlotNum, data.UtilSlotNum);
                ShipQuickStart.FinalizeShip(ship);
            }
            if (string.IsNullOrEmpty(ship.Name))
                ship.Name = GenerateShipName(ship);
          //  ship.DrawType = DrawType.Alpha;
            return ship;
        }

        public static void AddBasicGearSlots(Agent ship, bool isRotation = true, int generatorNum = 1, int shieldNum = 1, int utilityNum = 1, SizeType? size = null)
        {
            SizeType sizeType = size.HasValue ? size.Value : ship.SizeType;
            for (int i = 0; i < generatorNum; i++)
            {
                ship.ItemSlotsContainer.AddBasicSlot(SlotType.Generator, sizeType);
            }
            for (int i = 0; i < shieldNum; i++)
            {
                ship.ItemSlotsContainer.AddBasicSlot(SlotType.Shield, sizeType);
            }
            if (isRotation)
                ship.ItemSlotsContainer.AddBasicSlot(SlotType.Rotation, sizeType);
            if (utilityNum > 0)
            {
                ControlSignals usedSignals = ControlSignals.None;
                for (int i = 0; i < ship.ItemSlotsContainer.Count - ship.ItemSlotsContainer.BasicSlotsCount; i++)
                {
                    usedSignals |= ship.ItemSlotsContainer[i].ActivationSignal;
                }
                for (int i = 0; i < utilityNum; i++)
                {
                    ControlSignals activationSignal = ControlSignals.None;
                    if((usedSignals & ControlSignals.Action3) == 0)
                    {
                        activationSignal = ControlSignals.Action3;
                        usedSignals |= activationSignal;
                    }
                    else
                    {
                        if ((usedSignals & ControlSignals.Action4) == 0)
                        {
                            activationSignal = ControlSignals.Action4;
                            usedSignals |= activationSignal;
                        }
                    }
                    ship.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, sizeType, activationSignal);
                }
            }
            if(isRotation)
            {
                ship.CraftingStationType |= CraftingStationType.Rotating;
            }
            else            
            {
                ship.gameObjectType |= GameObjectType.NonRotating;
            }
        }

        public static void FinalizeShip(Agent agent)
        {
            agent.HullCost = AgentUtils.CalculateAgentHullCost(agent);
        }

        private static SizeType CalculateSizeType(float size)
        {
            SizeType sizeType = SizeType.Small;
            for (int i = 0; i < SIZE_THRESHOLD.Length; i++)
            {
                if (size >= SIZE_THRESHOLD[i])
                {
                    sizeType = (SizeType)i;
                }
            }
            return sizeType;
        }

        public static float DefaultMass(ShipData data)
        {
            var sprite = Sprite.Get(data.SpriteID);
            var size = data.Size ?? Math.Max(sprite.Width, sprite.Height) / 2f;
            return (int)Math.Round( data.Mass ?? 0.1f * size);
        }

        private static void AddCommonSystems(Agent ship)
        {
            ship.AddAfterSystem(new MeterMaxValueSetter(MeterType.Energy, MeterType.EnergyMaxValue));
            ship.AddAfterSystem(new MeterMaxValueSetter(MeterType.Shield, MeterType.ShieldMaxValue));
            ship.AddSystem(new FactionMeterBinder(MeterType.FactionKills));
            ship.AddSystem(new FactionMeterBinder(MeterType.Money)); //??                                                                     
            //ship.AddSystem(new FactionMeterBinder(MeterType.Reputation)); //??
            ship.AddSystem(new FactionMeterBinder(MeterType.ControlPoints)); //??
            float delta = -0.5f;
            switch (ship.SizeType)
            {
                case SizeType.Small:
                    delta = -0.1f;
                    break;
                case SizeType.Medium:
                    delta = -0.3f;
                    break;
                case SizeType.Large:
                    delta = -0.5f;
                    break;                
                default:
                    break;
            }
            ship.AddSystem(new ReputationUpdateSystem(delta));

            //ship.AddSystem(new ShowTargetSystem());

            //Damage Text
            // ship.AddSystem(new DamageTextEmitter());//TODO: remove after demo
            //Destroyed Explosion //TODO: add random exp //if size is above 100 use a more impressive explosion
            ship.AddSystem(new BasicEmitterCallerSystem(ControlSignals.OnDestroyed, "FullExplosionFx1"));
            //BasicEmitterCallerSystem dropBlueprintsSystem = new BasicEmitterCallerSystem(ControlSignals.OnDestroyed, ship.ID + "BlueprintPart");
            //ship.AddSystem(dropBlueprintsSystem);
            //Stun Fx
            ship.AddSystem(new EmitterCallerSystem(ControlSignals.OnStun, 10, "StunFx"));
            ////Shield Fx //remove //No need comes with Shield            
            //AgentEmitter shieldFx = new AgentEmitter(ControlSignals.OnTakingDamage, 5, "ShieldFx1");
            //ship.AddSystem(shieldFx);
            ship.AddSystem(new DamageTextEmitter());//TODO: fix

            //Cargo Emitter
            ship.AddAfterSystem(new CargoEmitterSystem());

            ////Hull damage FX (debris)
            //AgentEmitter onDamageToHull = new AgentEmitter(ControlSignals.OnStun, 10, "ProjDebris1"); ;
            //ship.AddSystem(onDamageToHull);

            //ship.SetMeter(MeterType.GlobalRepairCooldown, new Meter(repairCooldownTime)); //TODO: remove and change kit activation 
            //MeterGenerator repairCooldown = new MeterGenerator(); //maybe change
            //repairCooldown.MeterType = MeterType.GlobalRepairCooldown;
            //repairCooldown.GenerationAmountPerSec = 60;
            //repairCooldown.MaxValue = repairCooldownTime;
            //ship.AddSystem(repairCooldown);

            // Loot            
            //ship.AddSystem(new LootSystem()); //Move to


            //debris

            //low hitpoints improve
            LowHitPointsFlames lowHP = new LowHitPointsFlames();
            ship.AddAfterSystem(lowHP);
        }

        public static Agent LoadFromXML(string fullpath)
        {
           
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ShipData));
            StreamReader file = new StreamReader(fullpath);
            ShipData shipdata = (ShipData)xmlSerializer.Deserialize(file);
            file.Close();

            var ship = ShipQuickStart.Make(shipdata, true);
            ship.ID = Path.GetFileNameWithoutExtension(fullpath);
            return ship;
        }
    }
}
