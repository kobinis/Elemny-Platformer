using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework.Emitters.SceneRelated;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc;
using SolarConflict.Framework.InGameEvent;
using SolarConflict.Framework.InGameEvent.Content;
using SolarConflict.Framework.Scenes;
using SolarConflict.Framework.Scenes.DialogEngine;
using SolarConflict.Framework.World.MetaGame;
using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.Activities.SceneActivitys;
using SolarConflict.GameContent.Agents;
using SolarConflict.GameWorld;
using SolarConflict.Generation;
using SolarConflict.Session.World.Generation.Content;
using SolarConflict.Session.World.MissionManagment;
using SolarConflict.Session.World.MissionManagment.GlobalObjectives;
using SolarConflict.Session.World.MissionManagment.Objectives;
using System;
using System.Collections.Generic;
using XnaUtils;
using XnaUtils.Graphics;
using SolarConflict.NodeGeneration.NodeProcesess;
using SolarConflict.Framework.Agents.Systems.Misc;
using SolarConflict.Framework.GUI;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.Framework.InGameEvent.GenericProcess;

namespace SolarConflict.GameContent.Activities
{
    /// <summary>
    /// The first node the player starts in, to replace StartingNode
    /// </summary>
    [Serializable]
    class FirstNode : Scene
    {        

        Vector2 startingPosition = new Vector2(800,58000);
        bool _initDone;

        public FirstNode():base()
        {
            
        }        


        public override void InitScript(string parameters, ActivityParameters activityParameters = null)
        {
            GameEngine.Rand = new Random(32432);
            IsConfirmQuitNeeded = true;
            this.IsShipSwitchable = false; 
            this.SaveOnExit = true;
            SceneGenerator generator = FirstNodeGenerationInfo.Make();
            UpdatePlayerShip = true;
            generator.GenerateScene(this);
            background = new Background(0);
            GameEngine.Rand = new Random();

            SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.MissionLog);                    
            if (DebugUtils.Mode == ModeType.Debug)
            {
                SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.Hangar);
                SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.TacticalMap);
                SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.GalaxyMap);
            }

            //Player Starting point
            this.AddObjectRandomlyInLocalCircle("Asteroid1", 6, 1500, PlayerStartingPoint - Vector2.UnitX * 1500- Vector2.UnitY * 3000);
            this.AddObjectRandomlyInLocalCircle("SmallAsteroid1", 6, 1600, PlayerStartingPoint - Vector2.UnitX * 1500 - Vector2.UnitY * 3000);
            //Ring around system
            this.AddObjectRandomlyInLocalCircle("SmallAsteroid1", 1000, generator.SceneRadius*1.3f,null , generator.SceneRadius*1.4f);
            this.AddObjectRandomlyInLocalCircle("Asteroid2", 5, 1500, new Vector2(2800, 36500));
            this.AddObjectRandomlyInLocalCircle("SmallAsteroid2", 2, 1500, new Vector2(2800, 36500));

            GameEngine.Level = 0;
            CameraManager.MovmentType = CameraMovmentType.Custom;
            Camera.Position = startingPosition;
            SkipInteraction = true;
            HideRecycleBean = true;
            GameEngine.Update(InputState.EmptyState);            
        }

        private void AddMissionsAndInitGame()
        {
            CommonInit();
            if (GameplaySettings.SkipTutorial)
            {
                SkipTutorialInit();
            }
            else
            {
                TutorialInit();
            }
            AddGenericMissions();
        }

        GameObject starport;
        Agent shop;
        Agent gustav;
        Agent guildShop;

        string gustavLootId = "HumanRemains";
        public void CommonInit()
        {
            starport = FindGameObjectByID("starport") as Agent;
            AddGameObject("CraftingStation", starport.Position - Vector2.UnitX * 1000, 0);

            shop = AddGameObject("SmallShop1", PlayerStartingPoint + new Vector2(-20000, 3000), 90) as Agent;
            shop.ID = "shop";
            shop.collideWithMask = GameObjectType.None;

            // Create Gustav    
            gustav = AddGameObject("GustavShip", FactionType.Pirates1, shop.Position + new Vector2(-1000, -8000), -90) as Agent;
            
            // gustav.AddItemToInventory(gustavLootId);
            gustav.AddItemToInventory("AsteroidPullMine", 1);
            gustav.AddItemToInventory(gustavLootId, 1);
            //gustav.AddSystem(new LootSystem("GustavLoot"));
            // gustav.AddSystem(new SlotItemDropSystem(ControlSignals.OnDestroyed));//!!
            gustav.Name = "Gustav";
            gustav.SetAggroRange(500, 10000, TargetType.Enemy);

            guildShop = AddGameObject("SmallShop1", PlayerStartingPoint + new Vector2(-3000, -20000), 90, FactionType.TradingGuild) as Agent;
            guildShop.Name = "GuildShop";
            guildShop.ID = "guildShop";
            guildShop.collideWithMask = GameObjectType.None;
            var guildSystem = new ShopSystem();
            guildSystem.shopData.Portrait = Sprite.Get("flen");
            guildSystem.AddItemsFromAsset("GuildShopInv");
            guildShop.AddSystem(guildSystem);

            //this.MissionManager.AddMission(MissionFactory.EnteringInterStllerSpace(generator.SceneRadius * 1.3f));
            GameEngine.AddGameProcces(GameEventFactory.MakeCargoShipSpawner());
            GameEngine.AddGameProcces(GameEventFactory.MakeCargoShipSpawner());
            //GameEngine.AddGameProcces(new GenerateFetchMissionProcess(FactionType.Neutral));
            //GameEngine.AddGameProcces(new GenerateFetchMissionProcess(FactionType.Neutral));
            //GameEngine.AddGameProcces(new GenerateFetchMissionProcess(FactionType.Federation));
            //GameEngine.AddGameProcces(new GenerateFetchMissionProcess(FactionType.Empire));
            //GameEngine.AddGameProcces(new GenerateDestroyMissionProcess(FactionType.Federation));
            //GameEngine.AddGameProcces(new GenerateDestroyMissionProcess(FactionType.Empire));            
        }


        public void SkipTutorialInit()
        {
            IsShipSwitchable = true;
            SkipInteraction = false;
            SceneComponentSelector.AddComponent(SceneComponentType.Inventory);
            SceneComponentSelector.AddComponent(SceneComponentType.Hangar);
            SceneComponentSelector.AddComponent(SceneComponentType.TacticalMap);
            SceneComponentSelector.AddComponent(SceneComponentType.GalaxyMap);
            SceneComponentSelector.AddComponent(SceneComponentType.Codex);
            PlayerAgent.ItemSlotsContainer.ClearItems("PileOfJunk");
            Agent.EquipAgent(PlayerAgent, 0, true);
            PlayerAgent.SetMeterValue(MeterType.StunTime, 0);
            GetPlayerFaction().Mothership.AddItemToInventory("RepairKit1",2);
            PlayerAgent.AddItemToInventory("VacuumModulator1");
            PlayerAgent.AutoEquip(new List<Inventory> { PlayerAgent.Inventory }, false);
            //goto the mother ship

            MissionChainingHelper ch = new MissionChainingHelper(this);

            Mission meetTheMS = new Mission("Go to the Mothership");
            meetTheMS.Objective = new GoToTargetObjective(GetPlayerFaction().Mothership);
            ch.Add(meetTheMS);


            Mission smTutorialIntroduceActivations = new Mission("Use Repair Kit", new TextAsset( "Place the kit in the first slot of your inventory close it with escape and press #action{QuickUse1} to use it"));
            smTutorialIntroduceActivations.Objective = new AcquireItemObjective("RepairKit1", 1, false, new TutorialGoal("RepairKit1", false, false, false, false, 0));
            ch.Add(smTutorialIntroduceActivations);

            ch.Add(MakeSideMissions());            

            var shopSystem = new ShopSystem();
            shopSystem.shopData.Portrait = Sprite.Get("Gustav_Shop");
            shopSystem.AddItemsFromAsset("GustavShopInv");
            shop.AddSystem(shopSystem);
            PlayerAgent.AddMeterValue(MeterType.Money, 400); //??
        }

        public void TutorialInit()
        {                       
            MissionChainingHelper ch = new MissionChainingHelper(this);
            PlayerAgent.ItemSlotsContainer.ClearItems("PileOfJunk");            
            PlayerAgent.SetMeterValue(MeterType.StunTime, 0);

            //Intro
            Mission intro = MissionFactory.MissionQuickStart("smTutorialIntro");
            intro.Objective = new MinimumTimeObjective(0.1f);
            intro.OnMissionCompletion += (m, s) => { if (s.PlayerAgent != null) s.PlayerAgent.SetMeterValue(MeterType.StunTime, 0); };
            ch.Add(intro);

            Mission meetTheMS = MissionFactory.MissionQuickStart("smTutorialMeetMothership");
            meetTheMS.Objective = new GoToTargetObjective(GetPlayerFaction().Mothership);
            meetTheMS.OnMissionCompletion += (m, s) => { s.CameraManager.ManualZoom.ManualTargetZoom = 0.3f; };
            ch.Add(meetTheMS);


            Mission smTutorialIntroduceActivations = MissionFactory.MissionQuickStart("smTutorialIntroduceActivations");
            smTutorialIntroduceActivations.OnMissionStart += (m, s) => {
                s.SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.Inventory); 
                s.SkipInteraction = false;
                s.GetPlayerFaction().Mothership.AddItemToInventory("RepairKit1"); 
            };
            smTutorialIntroduceActivations.Objective = new AcquireItemObjective("RepairKit1", 1, false, new TutorialGoal("RepairKit1", false, false, false, false, 0));
            ch.Add(smTutorialIntroduceActivations);

            Mission smTutorialIntroduceUseConsumables = MissionFactory.MissionQuickStart("smTutorialIntroduceUseConsumables");
            smTutorialIntroduceUseConsumables.OnMissionStart += (m, s) => { s?.PlayerAgent.Inventory.Sort(); }; 
            smTutorialIntroduceUseConsumables.Objective = new ControlSignalObjective(ControlSignals.QuickUse1);
            ch.Add(smTutorialIntroduceUseConsumables);

            Mission nearMS = new Mission();
            nearMS.IsHidden = true;
            nearMS.Objective = new GoToTargetObjective(GetPlayerFaction().Mothership, 1000);
            ch.Add(nearMS);

            Mission smTutorialIntroduceBlueprints = MissionFactory.MissionQuickStart("smTutorialIntroduceBlueprints");
            smTutorialIntroduceBlueprints.EmitterOnStart = new SceneComponentEmitter(SceneComponentType.Inventory, true);
            smTutorialIntroduceBlueprints.Objective = new ObjectiveGroup();
            smTutorialIntroduceBlueprints.AddObjective(new AcquireItemObjective("Generator0", 1, true, new TutorialGoal("Generator0", false)));
            smTutorialIntroduceBlueprints.AddObjective(new AcquireItemObjective("Blaster0", 1, true, new TutorialGoal("Blaster0", false)));
            smTutorialIntroduceBlueprints.OnMissionCompletion += (m, s) => { Agent.EquipAgent(s.PlayerAgent, 0, true); };
            ch.Add(smTutorialIntroduceBlueprints);//TODO: install the other blasters, possibly say something about it


            Mission smTutorialIntroducePassiveEquipment = MissionFactory.MissionQuickStart("smTutorialIntroducePassiveEquipment");
            smTutorialIntroducePassiveEquipment.AgentType = AgentType.Player;
            smTutorialIntroducePassiveEquipment.OnMissionStart += (m, s) => { s.GetPlayerFaction().Mothership.AddItemToInventory("VacuumModulator1"); };     
            smTutorialIntroducePassiveEquipment.Objective = new AcquireItemObjective("VacuumModulator1", 1, true, new TutorialGoal("VacuumModulator1", true, true, false, false));
            ch.Add(smTutorialIntroducePassiveEquipment);

            Mission smTutorialIntroduceMothershipEquipment = MissionFactory.MissionQuickStart("smTutorialIntroduceAsteroids"); //TODO: get the shield blueprints      
            MissionFactory.ObtainMaterialsForItem("Shield1", smTutorialIntroduceMothershipEquipment, num: 2);
            smTutorialIntroduceMothershipEquipment.AddObjective(new GoToObjectTypeObjective(GameObjectType.Asteroid, level: 1), true);
            smTutorialIntroduceMothershipEquipment.OnMissionCompletion += (m, s) => { s.SkipInteraction = false; };
            ch.Add(smTutorialIntroduceMothershipEquipment);

            Mission backToTheMS = MissionFactory.MissionQuickStart("smBackToMothership");
            backToTheMS.Objective = new GoToTargetObjective(GetPlayerFaction().Mothership);
            ch.Add(backToTheMS);

            Mission craftShield = MissionFactory.MissionQuickStart("smTutorialIntroduceMothershipEquipment");
            craftShield.OnMissionStart += (m, s) => { s.SkipInteraction = false; };
            craftShield.Objective = new ObjectiveGroup();
            craftShield.AddObjective(new AcquireItemObjective("Shield1", 1, true, new TutorialGoal("Shield1", false, true, false, true)));
            craftShield.AddObjective(new AcquireItemObjective("Shield1", 1, true, new TutorialGoal("Shield1", false, true, true, true), agent:GetPlayerFaction().Mothership));                        
            ch.Add(craftShield);
            
            Mission smTutorialIntroduceCodex = MissionFactory.MissionQuickStart("smTutorialIntroduceCodex");
            smTutorialIntroduceCodex.Objective = new OpenSceneComponentObjective(SceneComponentType.Codex);
            smTutorialIntroduceCodex.EmitterOnStart = new SceneComponentEmitter(SceneComponentType.Codex, true);
            ch.Add(smTutorialIntroduceCodex);
           // ch.AddGroup(true, smTutorialIntroduceCodex);
            
            //smMainGoToStarport
            Mission smMainGoToStarport = MissionFactory.MissionQuickStart("smMainGoToStarport");
            smMainGoToStarport.Objective = new GoToTargetObjective(starport);
            ch.Add(smMainGoToStarport);

            
            //smMainGustavShop
            Mission smMainGustavShop = MissionFactory.MissionQuickStart("smMainGustavShop");
            smMainGustavShop.Objective = new GoToTargetObjective(shop);
            ch.Add(smMainGustavShop);


            //smMainConvinceGustav
            Mission smMainConvinceGustav = MissionFactory.MissionQuickStart("smMainConvinceGustav");
            smMainConvinceGustav.Objective = new GoToTargetObjective(gustav, 2000);
            smMainConvinceGustav.Data = gustav;
            smMainConvinceGustav.OnMissionCompletion += (m, s) => { (m.Data as Agent)?.SetAggroRange(10000, TargetType.Enemy); };
            ch.Add(smMainConvinceGustav);
            
            Mission smMainConvinceGustavHard = MissionFactory.MissionQuickStart("smMainConvinceGustavHard");
            smMainConvinceGustavHard.Color = Color.Red;
            smMainConvinceGustavHard.Objective = new DestroyTargetObjective(gustav);
            ch.Add(smMainConvinceGustavHard);

            //smMainLootGustav
            var smMainLootGustav = MissionFactory.MissionQuickStart("smMainLootGustav");            
            smMainLootGustav.Objective = new ObjectiveGroup();
            smMainLootGustav.AddObjective(new GoToByIDObjective(gustavLootId));
            smMainLootGustav.AddObjective(new AcquireItemObjective(gustavLootId, 1));            
            ch.Add(smMainLootGustav);


            //smMainBackToGustavShop
            Mission backToTheShop = MissionFactory.MissionQuickStart("smMainBackToGustavShop");
            backToTheShop.Objective = new GoToTargetObjective(shop, 300);
            backToTheShop.OnMissionCompletion += (Mission mission, Scene s) =>
            {
                var shopSystem = new ShopSystem();
                shopSystem.shopData.Portrait = Sprite.Get("Gustav_Shop");
                shopSystem.AddItemsFromAsset("GustavShopInv");
                Agent gustavShop = s.FindGameObjectByID("shop") as Agent;
                gustavShop.AddSystem(shopSystem);
                s.PlayerAgent.AddMeterValue(MeterType.Money, 400);
            };
            ch.Add(backToTheShop);

            Mission buyChip = MissionFactory.MissionQuickStart("smMainBuyChip");            
            buyChip.Objective = new AcquireItemObjective("Chip", tutorialGoal: new TutorialGoal("Chip", false, false));
            ///Add time objective
            buyChip.OnMissionCompletion += (Mission mission, Scene scene) =>
            {
                scene.PlayerAgent.Inventory.RemoveItem("Chip");
            };
            ch.Add(buyChip);
        
            //smMainMeetDrifter
            var driffter = AddGameObject("DrifterShip", new Vector2(-30440, 73011) * 0.9f, 90, FactionType.TradingGuild) as Agent;           
            driffter.Name = "Drifter";
            Mission smMainMeetDrifter = MissionFactory.MissionQuickStart("smMainMeetDrifter");
            smMainMeetDrifter.Objective = new GoToTargetObjective(driffter, 1400);
            smMainMeetDrifter.Data = driffter;
            smMainMeetDrifter.OnMissionCompletion += (m, s) => { 
                var agent = m.Data as Agent; 
                agent.FactionType = FactionType.Player;
                var dummyTarget = new DummyObject(m.Agent.Position + Vector2.UnitX * 8000);
                agent.SetTarget(dummyTarget, TargetType.Goal);
                GetPlayerFaction().AddHull("SmallShip2");
            };
            smMainMeetDrifter.Reward = new Reward(100);
            smMainMeetDrifter.Reward.Items.Add(new Tuple<string, int>("RepairKit3", 2));            
            ch.AddGroup(true,smMainMeetDrifter, MakeSideMissions());

            //smMainDrifterFleetCommands
            Mission smMainDrifterFleetCommands = MissionFactory.MissionQuickStart("smMainDrifterFleetCommands");
            smMainDrifterFleetCommands.Objective = new ObjectiveGroup();
            smMainDrifterFleetCommands.AddObjective(new PlayerCommandObjective(PlayerCommand.CallHelp));
            //smMainDrifterFleetCommands.AddObjective(new DestroyTargetObjective(gustavFleet[0]), true);
            ch.Add(smMainDrifterFleetCommands);


            Vector2 offset = new Vector2(8000, 10000);
            PlaceHolderObjective placeHolderObjective = new PlaceHolderObjective();
            SpwanEnemysProcess enemyProcess = new SpwanEnemysProcess("GustavFleet1,GustavFleet2,GustavFleet2", new ReferencePositionProvider(offset, AgentType.Player), placeHolderObjective);

            //ClearMissions();
            //ch.LastMissionAdded = null;//!!!!!!!!!!!!!!!!

            Mission killFleet = MissionFactory.MissionQuickStart("smMainDestroyGustavFleet");
            killFleet.Color = Color.Red;
            killFleet.EmitterOnStart = enemyProcess;
            killFleet.Objective = placeHolderObjective;
            killFleet.Data = driffter;
            killFleet.OnMissionStart += (mission, scene) => {
                Agent localDriffter = mission.Data as Agent;                   
                scene.GetPlayerFaction().Mothership.GetSystem<FleetSystem>().AddShipToFreeSlot(localDriffter as Agent);             
            };
            killFleet.Reward = new Reward(500);
            killFleet.Reward.Items.Add(new Tuple<string, int>("HomeBeaconKit", 3));
            ch.Add(killFleet);


            Mission delay1 = new Mission();
            delay1.IsHidden = false;
            delay1.ID = "D1";
            delay1.Objective = new MinimumTimeObjective(1);
            ch.Add(delay1);         

            Mission smMainIntroduceShipSwapZeroDeaths = MissionFactory.MissionQuickStart("smMainIntroduceShipSwapZeroDeaths");
            smMainIntroduceShipSwapZeroDeaths.OnMissionStart += (m, s) => { IsShipSwitchable = true; };
            smMainIntroduceShipSwapZeroDeaths.Objective = new ControlMothershipObjective();
            smMainIntroduceShipSwapZeroDeaths.OnMissionCompletion += (s,m) => { PersistenceManager.Inst.Save(); IsShipSwitchable = false; };

            ch.Add(smMainIntroduceShipSwapZeroDeaths);

            Mission delay2 = new Mission();
            delay2.IsHidden = false;
            delay2.ID = "D2";
            delay2.Objective = new MinimumTimeObjective(1);
            ch.Add(delay2);

             
            placeHolderObjective = new PlaceHolderObjective();
            enemyProcess = new SpwanEnemysProcess("GustavFleet2,GustavFleet2", new ReferencePositionProvider(FMath.ToCartesian(4000, FMath.Rand.NextAngle()), AgentType.Mothership), placeHolderObjective);
            Mission smMainFirstMothershipDefense = MissionFactory.MissionQuickStart("smMainFirstMothershipDefense");
            smMainFirstMothershipDefense.Color = Color.Red;
            smMainFirstMothershipDefense.EmitterOnStart = enemyProcess;           
            smMainFirstMothershipDefense.Objective = placeHolderObjective;
            ch.Add(smMainFirstMothershipDefense);

            Mission smMainRaceHomeDefendMothership = MissionFactory.MissionQuickStart("smMainRaceHomeDefendMothership");
            smMainRaceHomeDefendMothership.OnMissionStart += (m, s) => { IsShipSwitchable = true; };
            smMainRaceHomeDefendMothership.Objective = new ControlShipObjective(PlayerAgent);
            ch.Add(smMainRaceHomeDefendMothership);

            //smMainIntroduceHomeBeaconKit

            Mission smMainIntroduceHomeBeaconKit = MissionFactory.MissionQuickStart("smMainIntroduceHomeBeaconKit");
            smMainIntroduceHomeBeaconKit.OnMissionStart += (m, s) => 
            { 
                var tmp = s.PlayerAgent.Inventory.GetItem(0); 
                s.PlayerAgent.Inventory.SetItem(0, ContentBank.Inst.GetItem("HomeBeaconKit", true));
                s.PlayerAgent.AddItemToInventory(tmp);
            };
            smMainIntroduceHomeBeaconKit.Objective =  new ControlSignalObjective(ControlSignals.QuickUse1);   
            ch.Add(smMainIntroduceHomeBeaconKit);

            Mission delay3 = new Mission();
            delay3.IsHidden = false;
            delay3.ID = "D3";
            delay3.Objective = new MinimumTimeObjective(2);
            ch.Add(delay3);


            placeHolderObjective = new PlaceHolderObjective();
             enemyProcess = new SpwanEnemysProcess("GustavFleet1,GustavFleet2", new ReferencePositionProvider(FMath.ToCartesian(4000, FMath.Rand.NextAngle()), AgentType.Mothership), placeHolderObjective);
            Mission smMainFirstMotherShipDefenseAll = MissionFactory.MissionQuickStart("smMainFirstMotherShipDefenseAll");
            smMainFirstMotherShipDefenseAll.EmitterOnStart = enemyProcess;
            smMainFirstMotherShipDefenseAll.Color = Color.Red;
            //  smMainFirstMotherShipDefenseAll.OnMissionStart += (m,s) => { var wp = new WaveProcces(m); s.GameEngine.AddGameProcces(wp); }; //Make the wave process end the mission
            smMainFirstMotherShipDefenseAll.Objective = placeHolderObjective;
          
            ch.Add(smMainFirstMotherShipDefenseAll);

            //smMainIntroduceHangar
            Mission smMainIntroduceHangar = MissionFactory.MissionQuickStart("smMainIntroduceHangar");
            smMainIntroduceHangar.EmitterOnStart = new SceneComponentEmitter(SceneComponentType.Hangar, true);
            smMainIntroduceHangar.Objective = new OpenSceneComponentObjective(SceneComponentType.Hangar);
            ch.Add(smMainIntroduceHangar);

            

            //smMainSpaceportStarDriveHelp
            Mission smMainSpaceportStarDriveHelp = MissionFactory.MissionQuickStart("smMainSpaceportStarDriveHelp");
            smMainSpaceportStarDriveHelp.Objective = new GoToTargetObjective(starport);
            ch.Add(smMainSpaceportStarDriveHelp);

            //smMainIntroduceTacticalMap
            Mission smMainIntroduceTacticalMap = MissionFactory.MissionQuickStart("smMainIntroduceTacticalMap");
            smMainIntroduceTacticalMap.EmitterOnStart = new SceneComponentEmitter(SceneComponentType.TacticalMap, true);
            smMainIntroduceTacticalMap.Objective = new OpenSceneComponentObjective(SceneComponentType.TacticalMap);
            ch.Add(smMainIntroduceTacticalMap);

            //smMainTheTradingGuildShopForStarDrive


            //smMainTheTradingGuildShopForStarDrive
            

            Mission gotoGuildShop = MissionFactory.MissionQuickStart("smMainTheTradingGuildShopForStarDrive");
            gotoGuildShop.Objective = new GoToTargetObjective(guildShop);
            AddGameObject("CraftingStation", guildShop.Position - Vector2.UnitX * 2500);
            AddGameObject("GuideStation", guildShop.Position - Vector2.UnitX * 3000);
            ch.Add(gotoGuildShop);

            //smMainKillPierce
            Vector2 pos = new Vector2(155, 400) * 100;
            var pirateLord = AddGameObject("FirstNodeEnemy2", FactionType.Pirates1, pos) as Agent;
            pirateLord.Name = "The Great Pirate Pierce";
            pirateLord.AddItemToInventory("Shotgun1");
            // pirateLord.AddItemToInventory("MiniMissileLauncher");
            //pirateLord.AddItemToInventory("Missile1", 200);
            pirateLord.AddItemToInventory("AsteroidPullMine", 2);

            var destroyPirateLord = MissionFactory.MissionQuickStart("smMainKillPierce");
            destroyPirateLord.Color = Color.Red;
            destroyPirateLord.Objective = new DestroyTargetObjective(pirateLord);
            destroyPirateLord.OnMissionCompletion += (mission, scene) => {
                Meter moneyMeter = scene.GetPlayerFaction().GetMeter(MeterType.Money);
                moneyMeter.Value += 500;
            };
            ch.Add(destroyPirateLord);

            //smMainBackToTradingGuildShopForStarDrive
            Mission smMainBackToTradingGuildShopForStarDrive = MissionFactory.MissionQuickStart("smMainBackToTradingGuildShopForStarDrive");
            smMainBackToTradingGuildShopForStarDrive.Objective = new GoToTargetObjective(guildShop);            
            ch.Add(smMainBackToTradingGuildShopForStarDrive);

            //smMainMineCopperforStarDrive
            Mission smMainMineCopperforStarDrive = MissionFactory.MissionQuickStart("smMainMineCopperforStarDrive");
            smMainMineCopperforStarDrive.Objective = new ObjectiveGroup();
            smMainMineCopperforStarDrive.AddObjective(new AcquireItemObjective("MatA2", 10));
            smMainMineCopperforStarDrive.AddObjective(new GoToObjectTypeObjective(GameObjectType.Asteroid, level: 2), true);            
            ch.Add(smMainMineCopperforStarDrive);

            var smMainDestroyTurretsForQuantumCarburetor = MissionFactory.MissionQuickStart("smMainDestroyTurretsForQuantumCarburetor");            
            smMainDestroyTurretsForQuantumCarburetor.Objective = new ObjectiveGroup();
            smMainDestroyTurretsForQuantumCarburetor.AddObjective(new AcquireItemObjective("Cmp1", 3));
            smMainDestroyTurretsForQuantumCarburetor.AddObjective(new GoToObjectTypeObjective(GameObjectType.Turret, level: 1), true);
            ch.Add(smMainDestroyTurretsForQuantumCarburetor);

            //smMainCreateStarDrive
            Mission smMainCreateStarDrive = MissionFactory.MissionQuickStart("smMainCreateStarDrive");
            smMainCreateStarDrive.Objective = new GoToTargetObjective(GetPlayerFaction().Mothership);
            smMainCreateStarDrive.OnMissionCompletion += (m, s) => { s.HideRecycleBean = false; };
            ch.Add(smMainCreateStarDrive);

            //smMainDestroyFirstWarpInhibitor
            var inhib = FindGameObjectByID("void_apostleHull");
            var destroyInhib = MissionFactory.MissionQuickStart("smMainDestroyFirstWarpInhibitor");
            destroyInhib.Color = Color.Red;
            destroyInhib.Objective = new DestroyTargetObjective(inhib); 
            ch.Add(destroyInhib);

            //smPickUpWarpCore
            var pickupcore = MissionFactory.MissionQuickStart("smPickUpWarpCore");
            pickupcore.Objective = new ObjectiveGroup();
            pickupcore.AddObjective(new AcquireItemObjective("InhibitorCore", 1));
            pickupcore.AddObjective(new TimeObjective(60));
            ch.Add(pickupcore);

            //smMainIntroduceGalaxyMap
            var openMap = MissionFactory.MissionQuickStart("smMainIntroduceGalaxyMap");
            openMap.Objective = new OpenSceneComponentObjective(SceneComponentType.GalaxyMap);
            openMap.EmitterOnStart = new SceneComponentEmitter(SceneComponentType.GalaxyMap, true);
            ch.AddGroup(false, openMap);
        }
      

        private void AddGenericMissions()
        {

            Mission veryCloseToGustav = new Mission("");
            veryCloseToGustav.IsHidden = true;
            veryCloseToGustav.IsGoalHidden = true;
            veryCloseToGustav.Objective = new GoToTargetObjective(gustav, 300);
            AddMission(veryCloseToGustav);

            Mission activatePushOnGustav = new Mission("Push the basterd");
            activatePushOnGustav.IsDismissable = true;
            activatePushOnGustav.Objective = new ControlSignalObjective(ControlSignals.Action3);
            activatePushOnGustav.DialogOnStart = new Dialog("Use #action{Action3} to push, note the cooldown", isBlocking: false);
            veryCloseToGustav.NextMissionOnComplete = activatePushOnGustav;


            //fnMeetFederation
            Mission goToFederationBase = MissionFactory.MissionQuickStart("fnMeetFederation");
            goToFederationBase.Objective = new GoToObjectTypeObjective(GameObjectType.Mothership, FactionType.Federation);
          //  AddMissionGenerator(goToFederationBase);

            //Find resource mine
            Mission mineMission = MissionFactory.MissionQuickStart("glFindMine", true);
            mineMission.ID = GetNewMissionID();
            mineMission.IsGoalHidden = true;
            mineMission.Objective = new GoToObjectTypeObjective(GameObjectType.Mine, null, 500); //TODO: fix objective            
            AddMission(mineMission);

            //First time Player destroyed
            Mission playerDestroyed = MissionFactory.MissionQuickStart("glPlayerShipDestroyed", true);            
            playerDestroyed.IsGoalHidden = true;            
            playerDestroyed.Objective = new PlayerDestroyedObjective(MissionObjective.ObjectiveStatus.Completed);
            AddMission(playerDestroyed);

            //Control mothership - if plyaer controls mothership by mistake
            Mission controlMothership = MissionFactory.MissionQuickStart("glControlMothership", true);
            controlMothership.Objective = new ControlMothershipObjective();            
            AddMission(controlMothership);

            //tour of the galaxy - visit all node types
            Mission tourOfGalaxy = MissionFactory.MissionQuickStart("glTour", true);
            ObjectiveGroup group = new ObjectiveGroup();
            group.AddObjective(new VisitNodeTypeObjective(NodeType.AsteroidField));
            group.AddObjective(new VisitNodeTypeObjective(NodeType.BinaryStar));
        //    group.AddObjective(new VisitNodeTypeObjective(NodeType.BlueGiant));
            group.AddObjective(new VisitNodeTypeObjective(NodeType.Nebula));
            group.AddObjective(new VisitNodeTypeObjective(NodeType.RedSun));
            group.AddObjective(new VisitNodeTypeObjective(NodeType.Vile));
          //  group.AddObjective(new VisitNodeTypeObjective(NodeType.WhiteDwarf));
            tourOfGalaxy.Objective = group;
            tourOfGalaxy.Reward = new Reward();
            tourOfGalaxy.Reward.Items.Add(new Tuple<string, int>("ActiveShield", 2));
            //tourOfGalaxy.OnMissionCompletion += (mission, scene) => {
            //    scene.PlayerAgent.AddItemToInventory("ActiveShield");
            //};

            AddMissionGenerator(tourOfGalaxy);
        }

        public Mission MakeSideMissions()
        {
            MissionChainingHelper ch = new MissionChainingHelper(this);

            Mission makeCraftingStation = new Mission("Craft a Crafting Station");
            makeCraftingStation.IsDismissable = true;
            makeCraftingStation.Objective = new AcquireItemObjective("CraftingStationKit", 1, false, new TutorialGoal("CraftingStationKit", false, false, false, true));
            makeCraftingStation.DialogOnStart = new Dialog("Press #action{MissionLog} to access additional missions");
            ch.LastMissionAdded = makeCraftingStation;

            Mission craftSmallEngine = new Mission("Craft Engine I");
            craftSmallEngine.IsDismissable = true;
            craftSmallEngine.Objective = new AcquireItemObjective("SmallEngine1", 1, true, new TutorialGoal("SmallEngine1", false, true, false, true));
            ch.Add(craftSmallEngine);

            Mission craftGenerator = new Mission("Craft Generator I");
            craftGenerator.IsDismissable = true;
            craftGenerator.Objective = new AcquireItemObjective("Generator1", 1, true, new TutorialGoal("Generator1", false, true, false, true));
            ch.Add(craftGenerator);

            return makeCraftingStation;
        }


        public override void UpdateScript(InputState inputState)
        {
            if(SceneFrameCounter == 0)
            {
                fadeAlpha = 1;                
                //AddGameObject(Consts.WARPIN_EFFECT, FactionType.Neutral, PlayerAgent.Position);
                ParamEmitter emitter = new ParamEmitter();
                emitter.EmitterID = "Debris";                
                emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
                emitter.RotationRange = 0.1f; //Test it
                emitter.RotationSpeedBase = -0.1f;
                emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Random;
                emitter.PosAngleRange = 360;
                emitter.PosRadType = ParamEmitter.EmitterPosRad.Random;
                emitter.PosRadRange = 1000;
                emitter.PosRadMin = 30;
                emitter.MinNumberOfGameObjects = 200;
                emitter.RotationType = ParamEmitter.EmitterRotation.Random;
                emitter.RotationRange = 360;
                emitter.LifetimeType = ParamEmitter.EmitterLifetime.Random;
                emitter.LifetimeMin = 3000;
                emitter.LifetimeRange = 1000;
                emitter.Emit(GameEngine, null, FactionType.Neutral, startingPosition, Vector2.Zero, 0, 0);
                Camera.Zoom = 0.8f;
                CameraManager.TargetZoom = 0.8f;
            }

            if(SceneFrameCounter == 10)
            {
                GameEngine.AddGameProcces(new FadeInProcces());
            }
            if (!_initDone && PlayerAgent != null)
            {

                CameraManager.MovmentType = CameraMovmentType.OnPlayer;
                PlayerAgent.Position = startingPosition;
                PlayerAgent.CurrentHitpoints = PlayerAgent.MaxHitpoints * 0.3f;
                PlayerAgent.SetMeterValue(MeterType.StunTime, 600);
                if (this.SceneFrameCounter > 60 * 3)
                {
                    _initDone = true;                    
                    AddMissionsAndInitGame();
                }
            }
        }

        //private void AddStarport() //TODO: add with feature
        //{

        //    starportDialog.AddTextWithPotratit("I need to find the Void*", Consts.COMMUNICATION_CONSOLE_TEXTURE_ID); // yaniv: arrived here
        //    starportDialog.AddText("Anyway, if you're planning to mine some asteroids, \nwe will always be happy to buy your minerals in our shop.");
        //    starportDialog.AddText("Please enter our starport and enjoy our facilities.");
        //    starportDialog.AddTextWithPotratit("In the Mission Log (#color{255,255,0}F1#defalutColor{}), you can select a mission to see how to get there.", null);

        //    TextData text = new TextData("I'm curious, why you don't use visual communication.");
        //    TextData text2 = new TextData("Captain, should I remind you what happened last time people saw you?!", Consts.AI_HELPER_TEXTURE_ID, "RemindYou");
        //    TextData text3 = new TextData("Uhhmmm... I'm ugly.", Consts.COMMUNICATION_CONSOLE_TEXTURE_ID);
        //    TextData text4 = new TextData("Ha Ha Ha. Well, it's the inner beauty that counts.");

        //    TextData text5 = new TextData("Did you hear about the sun that exploded?");
        //    TextData text6 = new TextData("Ahhaaa... Yes.", Consts.COMMUNICATION_CONSOLE_TEXTURE_ID);
        //    TextData text7 = new TextData("It's horrible, the death count just keep rising. \nWho could have done such a cruel thing?");

        //    starportDialog.AddRandomText(new List<TextData> { text, text2, text3, text4 });
        //    starportDialog.AddRandomText(new List<TextData> { text5, text6, text7 });

        //}

        /// <summary>
        /// Adding missions that guide the players
        /// </summary>
        private void AddTutorialMissions()
        {
            MissionChainingHelper ch = new MissionChainingHelper(this);

            var mission = MissionFactory.MissionQuickStart("glVacuumModulator", true);
            mission.Objective = new AcquireItemObjective("VacuumModulator1", 1, true, new TutorialGoal("VacuumModulator1", true));
            ch.Add(mission);
            //Craft and equip Vac
            //Mine for generator and Crafting station
            //Craft and deploy crafting station //Crafting staion near mothership
            //Crafting Station
            //Generator
            //Engine

            //ch.Add()
        }



        public static Activity ActivityProvider(string parameters = "")
        {
            Scene scene = new FirstNode();
            return scene;
        }



























        public void AddGustavMissions()
        {

            //AddGameObject("GuideStation", PlayerStartingPoint - Vector2.One * 3000);

            MissionChainingHelper ch = new MissionChainingHelper(this);
            Agent shop = AddGameObject("SmallShop1", PlayerStartingPoint + new Vector2(-20000, 3000), 90) as Agent;
            shop.ID = "shop";
            shop.collideWithMask = GameObjectType.None;
            //    AddGameObject("ImbuingStation", shop.Position + Vector2.One * 1000, 90);
            AddGameObject("CraftingStation", shop.Position - Vector2.UnitX * 1500, 90);

            var starport = FindGameObjectByID("starport") as Agent;
            // starport.InteractionSystem = new AgentDialogSystem("StarportDialog1");



            // Create Gustav    
            var gustav = AddGameObject("GustavShip", FactionType.Pirates1, shop.Position + new Vector2(-1000, -8000), -90) as Agent;
            string gustavLootId = "Shield1";
            gustav.AddItemToInventory(gustavLootId);
            gustav.AddItemToInventory("AsteroidPullMine", 1);
            gustav.AddSystem(new LootSystem("GustavLoot"));
            // gustav.AddSystem(new SlotItemDropSystem(ControlSignals.OnDestroyed));//!!
            gustav.Name = "Gustav";

            // Tutorial mission to loot Gustav's corpse (KLUDGY)
            var lootGustav = MissionFactory.MissionQuickStart("fnLootGustav");
            lootGustav.Color = Color.Magenta;
            lootGustav.Objective = new ObjectiveGroup();
            lootGustav.AddObjective(new GoToByIDObjective(gustavLootId));
            lootGustav.AddObjective(new AcquireItemObjective(gustavLootId, 1));
            lootGustav.AddObjective(new TimeObjective(120));

            var equipGustavLoot = MissionFactory.MissionQuickStart("fnEquipGustavLoot");
            equipGustavLoot.Objective = new AcquireItemObjective(gustavLootId, 1, true, new TutorialGoal(gustavLootId, false, true, false));
            lootGustav.NextMissionOnComplete = equipGustavLoot;
            // Add a hidden mission to kill Gustav, which will lead to the (tutorial) mission to loot his stuff
            // This is in addition to the not-so-hidden mission to kill Gustav
            var killGustavHidden = new Mission();
            killGustavHidden.IsHidden = true;
            killGustavHidden.IsGoalHidden = true;
            killGustavHidden.Objective = new DestroyTargetObjective(gustav);
            killGustavHidden.NextMissionOnComplete = lootGustav;
           // killGustavHidden.OnMissionCompletion += KillGustavHidden_OnMissionCompletion;
            AddMission(killGustavHidden);

            Mission meetShop = MissionFactory.MissionQuickStart("fnMeetGustavShop");
            meetShop.IsGoalHidden = true;
            meetShop.Objective = new GoToTargetObjective(shop, 300);
            ch.Add(meetShop);

            Mission killGustav = MissionFactory.MissionQuickStart("fnKillGustav");
            killGustav.Objective = new DestroyTargetObjective(gustav);
            killGustav.OnMissionCompletion += (Mission mission, Scene scene) =>
            {
            };
            ch.Add(killGustav);
            //Text on meeting Gustav
            Mission meetGustav = MissionFactory.MissionQuickStart("fnMeetGustav"); //TODO: expend text, add dialog options
            meetGustav.Objective = new TargetOnScreenObjective(gustav);
            AddMission(meetGustav);

            Mission meetGustavAfterDeath = MissionFactory.MissionQuickStart("fnMeetGustavAfterDeath");
            var afterDeathObj = new ObjectiveGroup();
            afterDeathObj.AddObjective(new PlayerIsLiveObjective());
            afterDeathObj.AddObjective(new TargetOnScreenObjective(gustav));
            afterDeathObj.AddObjective(new MetterBiggerObjective(MeterType.Kills, 1, gustav));
            meetGustavAfterDeath.Objective = afterDeathObj;
            AddMission(meetGustavAfterDeath);



            Mission backToTheShop = MissionFactory.MissionQuickStart("fnBackToTheShop");
            backToTheShop.Objective = new GoToTargetObjective(shop, 300);
            backToTheShop.OnMissionCompletion += (Mission mission, Scene scene) =>
            {
                var shopSystem = new ShopSystem();
                shopSystem.shopData.Portrait = Sprite.Get("Gustav_Shop");
                shopSystem.AddItemsFromAsset("GustavShopInv");
                Agent gustavShop = scene.FindGameObjectByID("shop") as Agent;
                gustavShop.AddSystem(shopSystem);
                scene.PlayerAgent.AddMeterValue(MeterType.Money, 400);
            };

            ch.Add(backToTheShop);

            Mission buyChip = MissionFactory.MissionQuickStart("fnBuyChip");
            buyChip.Objective = new AcquireItemObjective("Chip", tutorialGoal: new TutorialGoal("Chip", false, false));
            ///Add time objective
            buyChip.OnMissionCompletion += (Mission mission, Scene scene) =>
            {
                scene.PlayerAgent.Inventory.RemoveItem("Chip");
            };
            ch.Add(buyChip);
            //Optional
            var driffter = AddGameObject("DrifterShip", new Vector2(-30440, 73011) * 0.9f, 90, FactionType.TradingGuild) as Agent;
            //GroupEmitter ge = new GroupEmitter();
            // ge.AddEmitter("AgentSlotDropEmitter");
            // ge.AddEmitter("CargoDropEmitter");
            //var basicEmitter = new BasicEmitterCallerSystem(ControlSignals.OnDestroyed, ge);
            //driffter.AddSystem(basicEmitter);
            driffter.Name = "Drifter";
            Mission meetDrifter = MissionFactory.MissionQuickStart("fnMeetDrifter");
            meetDrifter.Objective = new GoToTargetObjective(driffter, 1400);
            meetDrifter.Data = driffter;
            meetDrifter.OnMissionCompletion += (m, s) => {
                var agent = m.Data as Agent; agent.FactionType = FactionType.Player; agent.SetTarget(s.PlayerAgent, TargetType.Goal);
                MetaWorld.Inst.GetFaction(FactionType.Player).AddHull("SmallShip2");
            }; //move to next mission on start
            ch.Add(meetDrifter);
            //Add hiden, if drifter daking damage from player and his on screen say: stop shooting!
            //If drifter dies add, tell my wife I love her...
            Vector2 offset = new Vector2(8000, 10000);
            var gustavFleet = new List<GameObject> {
                ContentBank.Inst.GetGameObjectFactory("GustavFleet1").MakeGameObject(GameEngine, null, FactionType.Pirates1),
                ContentBank.Inst.GetGameObjectFactory("GustavFleet2").MakeGameObject(GameEngine, null, FactionType.Pirates1),
                ContentBank.Inst.GetGameObjectFactory("GustavFleet2").MakeGameObject(GameEngine, null, FactionType.Pirates1)
            };

            gustavFleet[0].Position = driffter.Position + offset;
            gustavFleet[1].Position = driffter.Position + offset + new Vector2(600f);
            gustavFleet[2].Position = driffter.Position + offset - new Vector2(600f);

            Mission killFleet = MissionFactory.MissionQuickStart("fnDestroyFleet");
            killFleet.Objective = new ObjectiveGroup();
            gustavFleet.Do(a => killFleet.AddObjective(new DestroyTargetObjective(a)));
            //killFleet.AddObjective(new ProtectTargetObjective(driffter));
            //killFleet.AddObjective(new PlayerIsLiveObjective());
            gustavFleet.Add(driffter);
            killFleet.Data = gustavFleet;
            killFleet.OnMissionStart += (mission, scene) => {
                var fleet = (mission.Data as List<GameObject>);
                Agent localDriffter = fleet[fleet.Count - 1] as Agent;
                fleet.RemoveAt(fleet.Count - 1);

                fleet.Do(a => scene.GameEngine.AddGameObject(a));
                localDriffter.SetTarget(fleet[0], TargetType.Enemy);
                localDriffter.SetTarget(scene.PlayerAgent, TargetType.Ally);
                //localDriffter.targetSelector.
                // localDriffter.AddSystem(new TargetSelectorSystem());

                scene.GetPlayerFaction().Mothership.GetSystem<FleetSystem>().AddShipToFreeSlot(localDriffter as Agent);
            };
            //killFleet.OnMissionCompletion += (mission, scene) => {
            //    scene.PlayerAgent.AddMeterValue(MeterType.Money, 500);
            //};
            killFleet.Reward = new Reward(500);
            killFleet.Reward.Items.Add(new Tuple<string, int>("HomeBeaconKit", 3));
            ch.Add(killFleet);

            var useHomeBecon = MissionFactory.MissionQuickStart("fnPlaceHomeKit");
            var beconObj = new PlaceItemObjective("HomeBeaconKit", 0);

            beconObj.AddTutorialGoal(new TutorialGoal("HomeBeaconKit", false, false, destIndex: 0)); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1 fix it
            useHomeBecon.Objective = beconObj;

            //  ch.Add(useHomeBecon);


            //   ch.LastMissionAdded = null;

            //   ch.LastMissionAdded = null;
            Mission backToStarport = MissionFactory.MissionQuickStart("fnBackToStarport");
            backToStarport.Objective = new GoToObjectTypeObjective(GameObjectType.Starport, null);
            ch.Add(backToStarport);



            Agent guildShop = AddGameObject("SmallShop1", PlayerStartingPoint + new Vector2(-3000, -20000), 90, FactionType.TradingGuild) as Agent;
            guildShop.Name = "GuildShop";
            guildShop.ID = "guildShop";
            guildShop.collideWithMask = GameObjectType.None;
            var guildSystem = new ShopSystem();
            guildSystem.shopData.Portrait = Sprite.Get("flen");
            guildSystem.AddItemsFromAsset("GuildShopInv");
            guildShop.AddSystem(guildSystem);
            Mission gotoGuildShop = MissionFactory.MissionQuickStart("fnVisitGuild");
            gotoGuildShop.Objective = new GoToTargetObjective(guildShop);
            AddGameObject("CraftingStation", guildShop.Position - Vector2.UnitX * 2000);
            //   ch.LastMissionAdded = null; ///Adds this mission to mission manager
            ch.Add(gotoGuildShop);



            //   ch.LastMissionAdded = null;
            Mission gotoGuildShopHidden = MissionFactory.MissionQuickStart("fnVisitGuildHidden");
            gotoGuildShopHidden.IsGoalHidden = true;
            gotoGuildShopHidden.Objective = new GoToTargetObjective(guildShop);
            ch.Add(gotoGuildShopHidden);

            var beUgly = MissionFactory.MissionQuickStart("fnBeUgly");
            beUgly.IsGoalHidden = true;
            beUgly.Objective = new GoToTargetObjective(starport);
            ch.AddGroup(false, beUgly);

            // Drifter's dying message
            //var dyingMessage = MissionFactory.MissionQuickStart("fnDrifterDead");
            //dyingMessage.IsHidden = true;
            //dyingMessage.IsGoalHidden = true;
            //var group = new ObjectiveGroup();
            //group.AddObjective(new DestroyTargetObjective(driffter));        
            //dyingMessage.Objective = group;
            //AddMission(dyingMessage);

            Vector2 pos = new Vector2(155, 400) * 100;
            this.AddObjectRandomlyInLocalCircle("Asteroid2", 5, 5000, guildShop.Position);
            this.AddObjectRandomlyInLocalCircle("SmallAsteroid2", 15, 6000, guildShop.Position);
            // this.AddObjectRandomlyInLocalCircle("LavaAsteroid1", 5, 4000, pos);

            var pirateLord = AddGameObject("FirstNodeEnemy2", FactionType.Pirates1, pos) as Agent;
            pirateLord.Name = "The Great Pirate Pierce";
            pirateLord.AddItemToInventory("Shotgun1");
            // pirateLord.AddItemToInventory("MiniMissileLauncher");
            //pirateLord.AddItemToInventory("Missile1", 200);
            pirateLord.AddItemToInventory("AsteroidPullMine", 2);

            var destroyPirateLord = MissionFactory.MissionQuickStart("fnKillPierce");
            destroyPirateLord.Color = Color.Red;
            destroyPirateLord.Objective = new DestroyTargetObjective(pirateLord);
            destroyPirateLord.OnMissionCompletion += (mission, scene) => {
                Meter moneyMeter = scene.GetPlayerFaction().GetMeter(MeterType.Money);
                moneyMeter.Value += 500;
            };
            ch.Add(destroyPirateLord);

            Mission pickupTheLoot = new Mission("Pickup The Loot");
            var objective = new ObjectiveGroup();
            objective.IsHidden = false;
            objective.AddObjective(new GoToTargetObjective(pirateLord, targetCanBeNonActive: true), true);
            objective.AddObjective(new AcquireItemObjective("Shotgun1", 2), false);
            pickupTheLoot.Objective = objective;
            // ch.Add(pickupTheLoot);

            var backToGuild = MissionFactory.MissionQuickStart("fnBackToGuild");
            backToGuild.Objective = new GoToTargetObjective(guildShop);
            backToTheShop.Reward = new Reward(1000);
            ch.Add(backToGuild);
            //  ch.LastMissionAdded = null;
            //repairDrive.Objective = new AcquireItemObjective("StarDrive");
            //ch.LastMissionAdded = null;
            // Missions to acquire Star Drive materials
            // var recipe = ContentBank.Inst.GetRecipe("StarDriveRecipe");

            var getCopper = MissionFactory.MissionQuickStart("fnMineMatA2");
            getCopper.Color = Color.Yellow;
            getCopper.Objective = new ObjectiveGroup();
            getCopper.AddObjective(new AcquireItemObjective("MatA2", 10));
            getCopper.AddObjective(new GoToObjectTypeObjective(GameObjectType.Asteroid, level: 2), true);

            var getSprockets = MissionFactory.MissionQuickStart("fnGetSprockets");
            getSprockets.Color = Color.Yellow;
            getSprockets.Objective = new ObjectiveGroup();
            getSprockets.AddObjective(new AcquireItemObjective("Cmp1", 3));
            getSprockets.AddObjective(new GoToObjectTypeObjective(GameObjectType.Turret, level: 1), true);
            ch.Add(getCopper);
            ch.Add(getSprockets);

            //var repairDrive = MissionFactory.MissionQuickStart("fnRepairDrive");
            //MissionFactory.ObtainMaterialsForItem("StarDrive", repairDrive);
            //ch.Add(repairDrive);

            //if (false) // test
            //{
            //    ch.LastMissionAdded = null;
            //    AddGameObject("Shield5", PlayerStartingPoint);
            //    AddGameObject("Blaster5", PlayerStartingPoint);
            //    AddGameObject("Generator5", PlayerStartingPoint);
            //}
            // ch.LastMissionAdded = null;
            var inhib = FindGameObjectByID("void_apostleHull");
            var destroyInhib = MissionFactory.MissionQuickStart("fnKillInhib");
            destroyInhib.Color = Color.Red;
            destroyInhib.Objective = new DestroyTargetObjective(inhib);
            ch.Add(destroyInhib);

            var pickupcore = MissionFactory.MissionQuickStart("fnPickupCore");
            pickupcore.Objective = null;
            pickupcore.AddObjective(new AcquireItemObjective("InhibitorCore", 1));
            pickupcore.AddObjective(new TimeObjective(30));
            ch.Add(pickupcore);


            var goToSol = MissionFactory.MissionQuickStart("fnGoToSol"); //
            goToSol.IsGlobal = true;
            goToSol.Objective = new GoToNodeObjective(1);
            goToSol.DestenationNode = 1;
            //ch.LastMissionAdded = null;
            ch.AddGroup(false, goToSol);

            //TODO: Add pick up inhibitor core

            var openMap = MissionFactory.MissionQuickStart("fnOpenGalaxyMap");
            openMap.Objective = new OpenSceneComponentObjective(SceneComponentType.GalaxyMap);
            openMap.EmitterOnStart = new SceneComponentEmitter(SceneComponentType.GalaxyMap, true);
            ch.AddGroup(false, openMap);
        }

        private void AddMainMissions()
        {
            // GetPlayerFaction().Mothership.AddItemToInventory("VacuumModulator1");
            MissionChainingHelper ch = new MissionChainingHelper(this);

            //Intro
            Mission intro = MissionFactory.MissionQuickStart("fnIntro");
            ch.Add(intro);


            Mission mineAsteroids = MissionFactory.MissionQuickStart("fnMineAsteroids");
            MissionFactory.ObtainMaterialsForItem("VacuumModulator1", mineAsteroids, num: 1);
            mineAsteroids.AddObjective(new GoToObjectTypeObjective(GameObjectType.Asteroid, level: 1), true);
            ch.Add(mineAsteroids);

            //Equip Vacuum 
            Mission equipItem = MissionFactory.MissionQuickStart("fnCraftVacuum"); //Add an hidden timer objective                   
            equipItem.EmitterOnStart = new SceneComponentEmitter(SceneComponentType.Inventory);
            string vacItemID = "VacuumModulator1";
            equipItem.Objective = new AcquireItemObjective(vacItemID, 1, true, new TutorialGoal(vacItemID, true, isSourceCrafting: true));

            var reward = new Reward();
            reward.Money = 100;
            equipItem.Reward = reward;
            ch.Add(equipItem);


            var gotobase = MissionFactory.MissionQuickStart("fnGoToMothership");
            gotobase.Objective = new ObjectiveGroup();
            MissionFactory.ObtainMaterialsForItem("Shield1", gotobase, null, MissionObjective.ObjectiveStatus.Failed);
            gotobase.AddObjective(new GoToTargetObjective(GetPlayerFaction().Mothership));
            //  ch.Add(gotobase);

            //Cock and craft shield
            Mission craftShieldAtMothership = MissionFactory.MissionQuickStart("fnCraftOnMothership");
            craftShieldAtMothership.Objective = new AcquireItemObjective("Shield1", 1, true, tutorialGoal: new TutorialGoal("Shield1", true, true, false));
            //craftShieldAtMothership.Objective = new ObjectiveGroup();
            //craftShieldAtMothership.AddObjective(new AcquireItemObjective("Shield1", 1, true, tutorialGoal: new TutorialGoal("Shield1", true, true, false), statusOnNotAcquired: MissionObjective.ObjectiveStatus.Ongoing));
            //  dockWithMothership.AddObjective(new OpenSceneComponentObjective(SceneComponentType.Inventory), false);
            //Add timer ??
            //    craftShieldAtMothership.OnMissionCompletion += CraftShieldAtMothership_OnMissionCompletion;
            craftShieldAtMothership.Reward = new Reward(200);
            craftShieldAtMothership.Reward.Items.Add(new Tuple<string, int>("MatA1", 5));
            craftShieldAtMothership.Reward.Items.Add(new Tuple<string, int>("MatB1", 5));
            //   ch.Add(craftShieldAtMothership);


            // this.RemoveMission(intro);
            // ch.LastMissionAdded = null;



            //     var craftingStationMission = MissionFactory.MissionQuickStart("fnCraftCraftingStation");
            //       craftingStationMission.Objective = new AcquireItemObjective("CraftingStationKit", 1, false, new TutorialGoal("CraftingStationKit", false, false, destIndex: 0));
            //   ch.Add(craftingStationMission);
            //   var deployStation = MissionFactory.MissionQuickStart("fnDeployStation");
            //   deployStation.IsHidden = false;
            //   deployStation.Reward = new Reward(500);
            //   deployStation.Reward.Items.Add(new Tuple<string, int>("MatC1", 3));
            //   deployStation.Reward.Items.Add(new Tuple<string, int>("MatA1", 5));
            //   deployStation.Objective = new ControlSignalObjective(ControlSignals.QuickUse1 | ControlSignals.QuickUse2 | ControlSignals.QuickUse3 | ControlSignals.QuickUse4);
            ////   craftingStationMission.NextMissionOnComplete = deployStation;
            //  ch.Add(deployStation);
            //AddMission(craftingStationMission);

            // ch.Add(new Mission());

            ////Dock and place iron in mothership
            //Mission dockWithMothership = MissionFactory.MissionQuickStart("fnDockWithMothership");
            ////dockWithMothership.AgentType = Mission.MissionAgentType.Mothership;
            //dockWithMothership.Objective = new ObjectiveGroup();
            //dockWithMothership.AddObjective(new AcquireItemObjective("MatA1", 12, tutorialGoal: new TutorialGoal("MatA1", true, false, true), statusOnNotAcquired: MissionObjective.ObjectiveStatus.Failed));
            //dockWithMothership.AddObjective(new OpenSceneComponentObjective(SceneComponentType.Inventory));
            //ch.Add(dockWithMothership);

            Mission gotoStarport = MissionFactory.MissionQuickStart("fnGoToStarport");
            gotobase.NextMissionOnFail = gotoStarport; //!!!!!!!!!!!!!!!!!!!!!11
            gotoStarport.Objective = new GoToObjectTypeObjective(GameObjectType.Starport, null);
            gotoStarport.OnMissionCompletion += (mission, scene) => { scene.PlayerAgent.AddItemToInventory("RepairKit1", 2); };
            ch.Add(gotoStarport);
            //  gotobase.NextMissionOnFail = gotoStarport;

            //Mission getMatriealsForShield = MissionFactory.MissionQuickStart("fnGetShieldMaterials");
            //getMatriealsForShield.Color = Color.Magenta;
            //MissionFactory.ObtainMaterialsForItem("Shield1", getMatriealsForShield);
            //var obj  = getMatriealsForShield.Objective as ObjectiveGroup;


            ////obj.AddObjective(new GoToObjectTypeObjective(GameObjectType.Asteroid, null), true);
            //Mission craftCraftingStation = MissionFactory.MissionQuickStart("fnCraftShield");
            //craftCraftingStation.IsHidden = false;
            //var carftObj = new ObjectiveGroup();
            //craftCraftingStation.Objective = carftObj;
            //carftObj.AddObjective(new AcquireItemObjective("CraftingStationKit", tutorialGoal: new TutorialGoal("CraftingStationKit", true, false)));
            //carftObj.AddObjective(new AcquireItemObjective("Shield1",1, true, tutorialGoal: new TutorialGoal("Shield1", true, true)));
            //getMatriealsForShield.NextMissionOnComplete = craftCraftingStation;
            ////ch.AddGroup(getMatriealsForShield, openLog);

            // ch.LastMissionAdded = null;
            Mission hiddenStarportMission = new Mission();
            hiddenStarportMission.IsGoalHidden = true;
            hiddenStarportMission.IsHidden = true;
            hiddenStarportMission.Objective = new GoToObjectTypeObjective(GameObjectType.Starport, null);
            ch.Add(hiddenStarportMission);

            Mission goToGustavShop = MissionFactory.MissionQuickStart("fnGoToGustavShop");
            var gustavShop = this.FindGameObjectByID("shop");
            goToGustavShop.Objective = new GoToTargetObjective(gustavShop);
            ch.Add(goToGustavShop);
            //   ch.AddGroup(goToGustavShop, openLog , getMatriealsForShield);
            /**********************************************************************************************************************/
            ////GOTO ms and craft s
            //var pirate = AddGameObject("Skill_Gen",FactionType.Pirates1,  PlayerStartingPoint + Vector2.UnitY * 8000, -90) as Agent;
            //pirate.Name = "Jiffiy";
            ////pirate.AddSystem(new ShowTargetSystem());
            //pirate.AddItemToInventory(ContentBank.Inst.GetItem("Shotgun1", true));
            //var destroyPirate = MissionFactory.MissionQuickStart("fnKillPirate");
            ////MissionFactory.DestroyTargetObjective(pirate, "Destroy Jiffiy the Pirate");
            //destroyPirate.Objective = new DestroyTargetObjective(pirate);            
            //destroyPirate.OnMissionCompletion += (mission, scene) => {
            //    Meter moneyMeter = scene.GetPlayerFaction().GetMeter(MeterType.Money);
            //    moneyMeter.Value += 500;
            //};
            //ch.Add(destroyPirate);

            //Mission craftingStation = new Mission("Mine asteroids for copper"); //TODO: change name
            //MissionFactory.ObtainMaterialsForItem("Shield2", craftingStation);
            //objective = craftingStation.Objective as ObjectiveGroup;
            //objective.AddObjective(new GoToObjectTypeObjective(GameObjectType.Asteroid, null), true);
            //craftingStation.DialogOnComplete = new Dialog("You can craft #itemname{Shield2} before you continue, you will need to craft a #itemname{CraftingStationKit} before");//  //TextBank.Inst.GetDialogNode("fnCraftEngineStart1");
            //ch.Add(craftingStation);

            //item = ContentBank.Inst.GetItem("Shield2", false); //Craft 
            //Mission equipShieldMission2 = MissionFactory.GenericMission("Equip " + item.Tag + item.IconTag);
            //equipShieldMission2.DialogOnStart = TextBank.Inst.GetDialogNode("You can craft an advanced shield by opening the inventory );            
            //equipShieldMission2.Objective = new AcquireItemObjective(item.ID, 1, true, new TutorialGoal(item.ID, false));            




            //defend from a
            //Open Inventory Objective 
            //Mine
            //Get missiles
            //fight                      
            FindGameObjectByID("FederationBase").SetTarget(FindGameObjectByID("MothershipEmpire0"), TargetType.Goal);


        }





    }
}