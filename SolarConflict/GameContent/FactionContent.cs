using SolarConflict.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc;
using SolarConflict.NodeGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent
{
    class FactionContent //TODO: replace with loading from XML/JSON
    {
        /// <summary>
        /// Loads all Faction data to metaworld
        /// </summary>
        public static void LoadFactionData()
        {
            MetaWorld.Inst.SetFaction(CreateNeutralFaction());
            MetaWorld.Inst.SetFaction(CreatePlayerFaction());
            MetaWorld.Inst.SetFaction(CreateFederationFaction());
            MetaWorld.Inst.SetFaction(CreateEmpireFaction());
            MetaWorld.Inst.SetFaction(CreateGuildFaction());
            MetaWorld.Inst.SetFaction(CreatePirateFaction());
            MetaWorld.Inst.SetFaction(CreateVoidFaction());
            MetaWorld.Inst.SetFaction(CreateMinerFaction());
            MetaWorld.Inst.SetFaction(CreateVileFaction());
        }

        public static Faction CreateNeutralFaction()
        {
            var faction = new Faction(Framework.FactionType.Neutral);
            faction.Name = "Neutral";//"Freelancers"; //maybe can be selected by player
            faction.GetMeter(MeterType.Money).Value = 1000;
           // faction.Motto = "The world is your sandbox";
            faction.LogoTextureID = "PlayerLogo";
            faction.DefaultAttitude = 0.1f;            
            return faction;
        }

        public static Faction CreatePlayerFaction()
        {
            var faction = new Faction(Framework.FactionType.Player);
            faction.Name = "Player";//"Freelancers"; //maybe can be selected by player
            faction.GetMeter(MeterType.Money).Value = 100;
            faction.Motto = "The world is your sandbox";
            faction.LogoTextureID = "PlayerLogo";
            faction.DefaultAttitude = 0.1f;
            faction.ReflectRelations = true;
            //faction.OnTheSafeSide = true;
            faction.Mothership = ContentBank.Inst.GetGameObjectFactory("Mothership").MakeGameObject(null, null, Framework.FactionType.Player) as Agent;
            faction.Mothership.Name = "Mothership";
            faction.GetMeter(MeterType.WarpFuel).MaxValue = 5;
            MetaWorld.Inst.PlayerShip = ContentBank.Inst.GetGameObjectFactory("StartingShip1").MakeGameObject(null, null, Framework.FactionType.Player) as Agent;
            faction.AddValueToMeter(MeterType.ControlPoints, 5);
            FleetSystem dockingbay = new FleetSystem();                        
            dockingbay.FleetSlots.Add(new FleetSystem.FleetSlot(SizeType.Large));
            dockingbay.FleetSlots.Add(new FleetSystem.FleetSlot(SizeType.Large));
            dockingbay.FleetSlots.Add(new FleetSystem.FleetSlot(SizeType.Large));
            dockingbay.FleetSlots.Add(new FleetSystem.FleetSlot(SizeType.Large));

            faction.AddHull("RoundHull1"); //small round ship
            faction.AddHull("SmallShip3"); //small round ship            
            
            dockingbay.AddShipCopyToSlot(0, "StartingShip2");
            faction.Mothership.AddSystem(dockingbay);

            return faction;
        }

        public static Faction CreateFederationFaction()
        {
            var faction = new Faction(Framework.FactionType.Federation);
            //faction.Name = "The United Colonies";
            faction.Name = "Federation";
            faction.Motto = "With malice toward none and justice for all"; // todo: change moto //"Create new worlds"; 
            faction.LogoTextureID = "FederationLogo";
            faction.GetMeter(MeterType.Money).Value = 1000000;
            faction.DefaultAttitude = 0.1f;
            faction.Alliances[Framework.FactionType.Player] = 0.2f;
            faction.Alliances[Framework.FactionType.Empire] = -0.9f;
            faction.Alliances[Framework.FactionType.TradingGuild] = 0.8f;

            faction.GenerationData = new FactionGenerationData();
            faction.GenerationData.MothershipID = "FederationBase_Gen";
            faction.GenerationData.AddItems(ItemCategory.EnergyConsumingWeapon);
            faction.GenerationData.AddItems(ItemCategory.Utility);
            faction.GenerationData.AddItems(ItemCategory.All);

            faction.GenerationData.AddLoadout("Federation1");
            faction.GenerationData.AddLoadout("Federation2");
            faction.GenerationData.AddLoadout("Federation3");
            return faction;
        }

        public static Faction CreateEmpireFaction()
        {
            var faction = new Faction(Framework.FactionType.Empire);
            //faction.Name = "Terran Republic";
            faction.Name = "Nova Empire";
            faction.GetMeter(MeterType.Money).Value = 1000000;
            faction.Motto = "We are all equal";// "Freedom is slavery"; // todo: change moto //"Explore the horizon";
            faction.LogoTextureID = "HegemonyLogo";
            faction.DefaultAttitude = 0f;
            faction.Alliances[Framework.FactionType.Player] = 0.2f;            
            faction.Alliances[Framework.FactionType.Federation] = -0.9f;
            faction.Alliances[Framework.FactionType.TradingGuild] = 0.8f;
            //faction.AddLoadout("Skill");
            //faction.ItemRewards.Add("EnergyNetLauncher");
            //faction.ItemRewards.Add("DeployableTurretItem");
            //faction.ItemRewards.Add("PhoenixDeviceItem");
            //faction.ItemRewards.Add("AsteroidGrinderItem");
            faction.GenerationData = new FactionGenerationData();
            faction.GenerationData.MothershipID = "EmpireBase_Gen";
            faction.GenerationData.AddItems(ItemCategory.Shotgun);
            faction.GenerationData.AddItems(ItemCategory.Cloaking);
            faction.GenerationData.AddItem("AdvancedCraftingStationKit");
            faction.GenerationData.AddItem("ArmoryKit");
            faction.GenerationData.AddLoadout("Skill");
            faction.GenerationData.AddLoadout("Empire1");
            faction.GenerationData.AddLoadout("Empire2");
            faction.GenerationData.AddLoadout("Empire3");
            faction.GenerationData.AddLoadout("Empire4");
            //     faction.GenerationData.AddLoadout("MediumEmpire1");
            //     faction.GenerationData.AddLoadout("LargeEmpire1");
            // faction.GenerationData.AddLoadout("PentonLengon");
            return faction;
        }

        public static Faction CreateGuildFaction()
        {
            var faction = new Faction(Framework.FactionType.TradingGuild);
            faction.GetMeter(MeterType.Money).Value = 1000000;
            //faction.Name = "The Trade Alliance";
            faction.Name = "Trading Guild";
            faction.Motto = "Greed is eternal";
            faction.LogoTextureID = "GuildLogo";
            faction.DefaultAttitude = 0.1f;
            faction.Alliances[Framework.FactionType.Player] = 0.7f;
            faction.Alliances[Framework.FactionType.Federation] = 0.8f;
            faction.Alliances[Framework.FactionType.Empire] = 0.8f;
            faction.Alliances[Framework.FactionType.Pirates1] = 0f;
            faction.Alliances[Framework.FactionType.Pirates2] = 0f;
            faction.Alliances[Framework.FactionType.Pirates3] = 0.4f;

            faction.GenerationData = new FactionGenerationData();
            faction.GenerationData.MothershipID = "GuildBase_Gen";
            faction.GenerationData.AddItems(ItemCategory.All);
            faction.GenerationData.AddItem("AdvancedCraftingStationKit");
            faction.GenerationData.AddLoadout("LargeGuild1");
            faction.GenerationData.AddLoadout("MediumGuild1");
            faction.GenerationData.AddLoadout("Guild2");
            faction.GenerationData.AddLoadout("Guild3");

            return faction;
        }

        public static Faction CreatePirateFaction()
        {
            var faction = new Faction(FactionType.Pirates1);
            faction.GetMeter(MeterType.Money).Value = 300000;
            faction.Name = "Pirates";
            faction.LogoTextureID = "PirateLogo";
            faction.Motto = "Give me freedom or give me the rope";
            faction.DefaultAttitude = -0.2f;
            faction.Alliances[Framework.FactionType.Player] = -0.9f;
            faction.Alliances[Framework.FactionType.TradingGuild] = 0f;
            faction.Alliances[Framework.FactionType.Pirates2] = 0.8f;
            //faction.AddLoadout("Skill");

            faction.GenerationData = new FactionGenerationData();
            faction.GenerationData.MothershipID = "PirateBase_Gen";
           // faction.GenerationData.AddItems(ItemCategory.Shotgun);
            faction.GenerationData.AddItems(ItemCategory.Utility);
            faction.GenerationData.AddItem("HarpoonGun");
            //faction.GenerationData.AddItem("HarpoonGun");
            //faction.GenerationData.AddLoadout("SmallPirate1A");
            //faction.GenerationData.AddLoadout("SmallPirate1B");
            //faction.GenerationData.AddLoadout("MediumPirate2A");
            //faction.GenerationData.AddLoadout("MediumPirate2B");
            //faction.GenerationData.AddLoadout("SmallPirate3A");
            //faction.GenerationData.AddLoadout("SmallPirate3B");
            faction.GenerationData.AddLoadouts("MediumShip14_sg,MediumShip9_sg,SmallShp15_sg,SmallShip8_sg,SmallShip9_sg,SmallShip10_sg,SmallShip11_sg");


            //       faction.GenerationData.AddLoadout("SmallPirate2");
            //        faction.GenerationData.AddLoadout("SmallPirate3");
            //       faction.GenerationData.AddLoadout("MediumPirate1");
            //       faction.GenerationData.AddLoadout("MediumPirate2");

            //    faction.GenerationData.AddLoadout("Kemron");
            //  faction.GenerationData.AddLoadout("Firestarter");
            return faction;
        }

        public static Faction CreateVileFaction()
        {
            var faction = new Faction(FactionType.Vile);
            faction.GetMeter(MeterType.Money).Value = 300000;
            faction.Name = "Vile";
            faction.LogoTextureID = "PirateLogo";
            faction.Motto = "Give me freedom or give me the rope";
            faction.DefaultAttitude = -0.6f;
            faction.Alliances[Framework.FactionType.TradingGuild] = 0.8f;
            faction.Alliances[Framework.FactionType.Player] = -1;
            //faction.AddLoadout("Skill");

            faction.GenerationData = new FactionGenerationData();
            faction.GenerationData.MothershipID = "PirateBase_Gen";
            // faction.GenerationData.AddItems(ItemCategory.Shotgun);
            faction.GenerationData.AddItems(ItemCategory.Utility);
            return faction;
        }

        public static Faction CreateVoidFaction()
        {
            var faction = new Faction(Framework.FactionType.Void);
            faction.GetMeter(MeterType.Money).Value = 3000000;
            faction.Name = "Void";
            faction.LogoTextureID = "VoidLogo";
            faction.Motto = "Resistance is futile";
            faction.DefaultAttitude = -0.2f;
            faction.Alliances[Framework.FactionType.Player] = -0.6f;
            faction.Alliances[Framework.FactionType.Federation] = 0f;
            faction.Alliances[Framework.FactionType.Empire] = 0f;
            faction.Alliances[Framework.FactionType.MinerGuild] = 0.3f;

            faction.GenerationData = new FactionGenerationData();
            faction.GenerationData.MothershipID = "PirateBase_Gen";
            faction.GenerationData.AddItems(ItemCategory.All);

            faction.GenerationData.AddLoadout("Federation1");
            faction.GenerationData.AddLoadout("Federation2");
            faction.GenerationData.AddLoadout("Federation3");

            faction.GenerationData.AddLoadout("Empire1");
            faction.GenerationData.AddLoadout("Empire2");
            faction.GenerationData.AddLoadout("Empire3");
            faction.GenerationData.AddLoadout("Empire4");

            faction.GenerationData.AddLoadout("PirateShip1");
            faction.GenerationData.AddLoadout("PirateShip2");
            faction.GenerationData.AddLoadout("PirateShip3");
            faction.GenerationData.AddLoadout("PirateShip4");
            faction.GenerationData.AddLoadout("PirateShip5");

            faction.GenerationData.AddLoadout("LargeGuild1");
            faction.GenerationData.AddLoadout("MediumGuild1");
            faction.GenerationData.AddLoadout("Guild2");
            faction.GenerationData.AddLoadout("Guild3");

            //        faction.GenerationData.AddLoadout("PrologFirstEnemy");
            //       faction.GenerationData.AddLoadout("SmallPirate1");
            //        faction.GenerationData.AddLoadout("SmallPirate2");
            //        faction.GenerationData.AddLoadout("PrologShip1");
            //        faction.GenerationData.AddLoadout("PrologShip2");
            //        faction.GenerationData.AddLoadout("MediumPirate2");
            //        faction.GenerationData.AddLoadout("MediumShip8A");
            //"PrologueShip3", "Kemron", "PrologFirstEnemy", "PrologueEnemy1", "PrologueEnemy2" , "Skill" };
            return faction;
        }

        public static Faction CreateMinerFaction()
        {
            var faction = new Faction(Framework.FactionType.MinerGuild);
            faction.GetMeter(MeterType.Money).Value = 3000000;
            faction.Name = "Miners Guild";
            faction.LogoTextureID = "FederationLogo3";
            faction.Motto = "Rock hard";
            faction.DefaultAttitude = 1f;
            faction.Alliances[Framework.FactionType.Player] = -0.6f;
            faction.Alliances[Framework.FactionType.Pirates1] = 0f;
            faction.Alliances[Framework.FactionType.Void] = 0.3f;
            faction.GenerationData = new FactionGenerationData();
            faction.GenerationData.MothershipID = "PirateBase_Gen";
            faction.GenerationData.AddItems(ItemCategory.All);
     //       faction.GenerationData.AddLoadout("PrologFirstEnemy");
     //       faction.GenerationData.AddLoadout("SmallPirate1");
     //       faction.GenerationData.AddLoadout("SmallPirate2");
     //       faction.GenerationData.AddLoadout("PrologShip1");
    //        faction.GenerationData.AddLoadout("PrologShip2");
     //       faction.GenerationData.AddLoadout("MediumPirate2");
     //       faction.GenerationData.AddLoadout("MediumShip8A");
            //"PrologueShip3", "Kemron", "PrologFirstEnemy", "PrologueEnemy1", "PrologueEnemy2" , "Skill" };
            return faction;
        }


    }
}
