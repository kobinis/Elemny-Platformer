//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using SolarConflict.Framework;
//using SolarConflict.Framework.Agents.Systems;
//using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc;
//using SolarConflict.Framework.InGameEventSystem.Content;
//using SolarConflict.Framework.Scenes;
//using SolarConflict.Framework.Scenes.DialogEngine;
//using SolarConflict.GameContent.Agents;
//using SolarConflict.GameWorld;
//using SolarConflict.Generation;
//using SolarConflict.Session.World.Generation.Content;
//using SolarConflict.Session.World.MissionManagment;
//using System;
//using System.Collections.Generic;
//using XnaUtils;

//namespace SolarConflict.GameContent.Activities
//{
//    //TODO: make the missions trigger the converstions
//    /// <summary>
//    /// The first node the player starts in, to replace StartingNode
//    /// </summary>
//    [Serializable]
//    class FirstNodeOld : Scene
//    {
//        //public MessageBox MessageBox { get; private set; }

//        public override void InitScript(string parameters, ActivityParameters activityParameters = null)
//        {
//            SaveOnExit = true;
//            IsConfirmQuitNeeded = true;
//            //MessageBox = new MessageBox();
//            SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.MissionLog);
//            SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.Inventory);
//            SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.Hanger);
//            SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.TacticalMap);
//            SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.GalaxyMap);
//            this.IsShipSwitchable = true;
//            this.SaveOnExit = true;
//            this.ActionOnPlayerDeath = ActionOnPlayerDeathType.Respawn;
//            SceneGenerator generator = FirstNodeGenerationInfo.Make(); //Generate spanners at different places
//            //generator.SetRandomForFeatures(24312);
//            generator.GenerateScene(this);
//            GameEngine.AddGameProcces(SpawnMaker.MakeCargoShipSpawner());
//            GameEngine.AddGameProcces(SpawnMaker.MakeCargoShipSpawner());
//            GameEngine.AddGameProcces(SpawnMaker.MakePirateRaidSpawner());
//            //GameEngine.AddGameProcces(SpawnMaker.MakeBlobSpawner());
//            GameEngine.AddGameProcces(SpawnMaker.MakeBlobSpawner()); //TODO: add more events 
//                                                                     //GameEngine.AddGameProcces(SpawnMaker.MakeGuild());

//            AddStarport();
//            AddStartingDialog();
//            AddTradingGuildShop();
//            AddEmpireBase();
//            AddFederationBase();
//            AddPirateBoss();
//            AddShipyard();
//            AddPortals();
//        }




//        public static Activity ActivityProvider(string parameters = "")
//        {
//            Scene scene = new FirstNodeOld();
//            return scene;
//        }

//        public override int UpdateScript(InputState inpuState)
//        {

//            return 0;
//        }

//        public override void DrawScript(SpriteBatch sb)
//        {

//        }


//        private void AddPortals()
//        {
//            GenerationUtils.AddPortal(this.GameEngine, Vector2.UnitY * 100, "GustavLevel", false);
//        }

//        private void AddStartingDialog()
//        {
//            //TODO: replace with a dialog proccess
//            AgentDialogSystem startingDialog = new AgentDialogSystem();
//            startingDialog.DefaultPotratitID = Consts.AI_HELPER_TEXTURE_ID;

//            startingDialog.AddTextWithSound("Finally you're back! \nBut you don't look like yourself. \nWait. I am scanning.", "StartingNode_1");
//            startingDialog.AddTextWithSound("Yes, I was right. \nYour reconstruction didn't go well. \nYou probably don't remember everything, right?!\n", "StartingNode_2");
//            startingDialog.AddTextWithSound("But as your loyal AI helper I will share what I know.\n" +
//                    "Whenever your ship is destroyed it will reconstruct here, near the mothership.\n" +
//                    "You will lose all your inventory, but at least you'll keep all of your equipped items.\n" +
//                    "You can also use the mothership cargo hold to store items.", "StartingNode_3");
//            startingDialog.AddTextWithSound("While you were gone, we were attacked. \nIt was very scary, " +
//                    "however I was able to scare them away.\n", "StartingNode_4");
//            startingDialog.AddTextWithSound("I am afraid to admit it, \nbut they damaged the mother ship.\n" +
//                   "I fixed what I could. \nBut the star drive and the communication system were damaged.\n" +
//                   "Please don't erase me, I will do better next time, I promise.\n", "StartingNode_5");
//            startingDialog.AddTextWithSound("Anyway, we have to #color{255,255,0}fix the communication system#defalutColor{} to decrypt \na message we got from the Void.", "StartingNode_6a");
//            startingDialog.AddTextWithPotratit("The Void?", Consts.COMMUNICATION_CONSOLE_TEXTURE_ID);
//            startingDialog.AddTextWithSound("Who's the Void?! WOW, you really don't remember? It's your boss.\n", "StartingNode_6b");
//            startingDialog.AddTextWithSound("I found the location of a nearby starport. \nI suggest you go there and see if they can fix our communication system.",
//               "StartingNode_7");
//            startingDialog.AddTextWithPotratit("You got a new mission, check the mission log (F2).", Consts.INFORMATION_TEXTURE_ID);
//            startingDialog.AddTextWithPotratit("Press Tab to switch between your ships", Consts.INFORMATION_TEXTURE_ID);

//            startingDialog.DialogStart += delegate ()
//            {
//                var go = FindGameObjectByID("Starport");
//                var mission = MissionFactory.GoToTargetMission(go, "Go to the Nova-1 Starport", "Go to the near Starport to get information");
//                MissionManager.AddMission(mission);
//            };

//            GetPlayerFaction().Mothership.AddSystem(startingDialog);
//        }

//        private void AddStarport()
//        {
//            Agent starport = AddGameObject("Starport", FactionType.Neutral, this.PlayerStartingPoint - Vector2.UnitX * 3000) as Agent;
//            var shop = new ShopSystem();
//            shop.AddItem("MiningLaser1", 1);
//            shop.AddItem("MediumRotationEngine2", 1);
//            shop.AddItem("VacuumModulatorItem", 1);
//            shop.AddItem("MiningLaser1", 1);
//            starport.AddSystem(shop);

//            AgentDialogSystem starportDialog = new AgentDialogSystem();
//            starportDialog.DefaultPotratitID = "F_05";
//            starportDialog.AddText("Welcome to Solar Conflict starport. \nMy name is Laura. \nHow can I help you?");
//            //  starportDialog.DialogStartChangeText += new DialogStartChangeTextEventHandler(AddStarportMissions);            
//            starportDialog.DialogStart += delegate ()
//            {
//                var go = FindGameObjectByID("GuildShop");
//                var mission = MissionFactory.GoToTargetMission(go, "Trading Guild Shop", "Find The Trading Guild Shop");
//                MissionManager.AddMission(mission);
//            };
//            starport.AddSystem(starportDialog);

//            starportDialog.AddTextWithPotratit("I need to #color{255,255,0}repair my communication system#defalutColor{}. \nCan you assist me?", Consts.COMMUNICATION_CONSOLE_TEXTURE_ID); // yaniv: arrived here
//            //    starportDialog.AddText("Unfortunately we don't have spare parts for fixing your communication system.\n" +
//            //                                                                                          "But I am sure Gustav will have some. Maybe you can get them from him.\nAssuming you can convince him to deal with you instead of shooting you.\n" +
//            //                             "But I am sure Gustav will have some.\n" +
//            //                             "I'm sending you his coordinates. \nGood Luck! Anything else?");
//            //}

//            //if (Missions.ContainsKey(MissionsTexts.StartingNode_VisitTradingGuild_Name))
//            //{
//            //    _mm.AddMission(Missions[MissionsTexts.StartingNode_VisitTradingGuild_Name]);
//            //    starportDialog.AddTextWithPotratit("Do you have a #color{255,255,0}warp drive#defalutColor{}?", Consts.COMMUNICATION_CONSOLE_TEXTURE_ID);

//            //    starportDialog.AddText("No, but you should go and #color{255,255,0}visit the trading guild starbase#defalutColor{}, \nthey should have blueprints for a warp drive.\n");
//            //}
//            starportDialog.AddText("Anyway, if you're planning to mine some asteroids, \nwe will always be happy to buy your minerals in our shop.");
//            starportDialog.AddText("Please enter our starport and enjoy our facilities.");
//            starportDialog.AddTextWithPotratit("In the Mission Log (#color{255,255,0}L#defalutColor{}), you can select a mission to see how to get there.", null);

//            TextData text = new TextData("I'm curious, why you don't use visual communication.");
//            TextData text2 = new TextData("Captain, should I remind you what happened last time people saw you?!", Consts.AI_HELPER_TEXTURE_ID, "RemindYou");
//            TextData text3 = new TextData("Uhhmmm... I'm ugly.", Consts.COMMUNICATION_CONSOLE_TEXTURE_ID);
//            TextData text4 = new TextData("Ha Ha Ha. Well, it's the inner beauty that counts.");

//            TextData text5 = new TextData("Did you hear about the sun that exploded?");
//            TextData text6 = new TextData("Ahhaaa... Yes.", Consts.COMMUNICATION_CONSOLE_TEXTURE_ID);
//            TextData text7 = new TextData("It's horrible, the death count just keep rising. \nWho could have done such a cruel thing?");

//            starportDialog.AddRandomText(new List<TextData> { text, text2, text3, text4 });
//            starportDialog.AddRandomText(new List<TextData> { text5, text6, text7 });

//        }

//        private void AddTradingGuildShop()
//        {
//            var tradingGuildBase = (Agent)AddGameObject(typeof(SmallShop1).Name, 0, PlayerStartingPoint + new Vector2(10500, -500), 90);
//            tradingGuildBase.ID = "GuildShop";
//            tradingGuildBase.Name = "#ctext{255,255,0,'Trading Guild Magic Shop'}";
//            var _tradingGuildDialog = new AgentDialogSystem();
//            _tradingGuildDialog.DefaultPotratitID = "M_03";
//            _tradingGuildDialog.AddText("Greetings from the Trading Guild starbase Destiny. \nMy name is Flen. \nDo you wish to trade?");
//            tradingGuildBase.AddSystem(_tradingGuildDialog);
//            _tradingGuildDialog.AddTextWithPotratit("I need to repair my warp drive.", Consts.COMMUNICATION_CONSOLE_TEXTURE_ID);
//            //  _mm.AddMission(Missions[MissionsTexts.StartingNode_DestoryPirateLord_Name], true);
//            _tradingGuildDialog.AddText("We can provide you with blueprints for a new #color{255,255,0}warp drive#defalutColor{}. \nBut first, you will need to help us.");
//            _tradingGuildDialog.AddText("The Pirate lord Pierce has been targeting our shipping routes for a long time. \nIf you can help us \"solve\" this problem, we will give you the blueprints.");
//            _tradingGuildDialog.AddTextWithPotratit("Consider Pierce history.", Consts.COMMUNICATION_CONSOLE_TEXTURE_ID);
//            _tradingGuildDialog.AddText("Great! Let us know when you take care of him. \nIn the meantime would you like to purchase something at our store?");
//            _tradingGuildDialog.AddTextWithPotratit("We got some gear I suggest that you equip it before our next fight.", Consts.AI_HELPER_TEXTURE_ID);
//            _tradingGuildDialog.AddTextWithPotratit("Press #ctext{255,255,0,'I'} to open your inventory.", Consts.AI_HELPER_TEXTURE_ID);

//            ShopActivity shopActivity = new ShopActivity();
//            shopActivity.ShopName = "Trading Guild starbase Destiny";


//            _tradingGuildDialog.DialogStart += delegate ()
//            {
//                //Add two FireTrailSprint

//                var go = FindGameObjectByID("PirateLord");
//                //Mission mission = new Mission("Trading Guild Shop", "Find The Trading Guild Shop", 0, go);
//                //MissionManager.AddMission(mission);
//                var player = FindPlayer();
//                player.GetInventory().AddItem("FireTrailSprint");
//                player.GetInventory().AddItem("FireTrailSprint");
//                this.SceneComponentSelector.GetProviderControl(SceneComponentType.Inventory).IsGlowing = true;

//                var mission = MissionFactory.DestroyTargetObjective(go, "Destroy the Pirate Lord", "The Pirate Lord Roberts reakes havok on the local traders.\nTake care of him!");
//                MissionManager.AddMission(mission);
//            };
//        }

//        private void AddPirateBoss()
//        {
//            var pirate = (Agent)AddGameObject("PirateLord", FactionType.Pirates1, PlayerStartingPoint + new Vector2(10500, -500) + Vector2.One * 5000, 90);
//            pirate.ID = "PirateLord";
//            pirate.Name = "ctext{255,0,0,'Pirate lord Pierce'}";

//            var dialog = new AgentDialogSystem();
//            dialog.IsBlocking = false;
//            dialog.DefaultPotratitID = "M_13";
//            dialog.AddText("Arrr, You dirty sea cucumber!!");
//            pirate.AddSystem(dialog);
//        }



//        private void AddEmpireBase()
//        {
//            var _empireBase = AddAgent("EmpireBaseA", PlayerStartingPoint + new Vector2(12000, -10000), 0, FactionType.Empire);
//            //var mine = AddAgent("Mine1", empireBase.Position - Vector2.UnitX * 5000 + Vector2.UnitY * -8000, 0);     
//            //kobi: don't play battle music
//            var empireBaseDialog = new AgentDialogSystem();
//            empireBaseDialog.DefaultPotratitID = "M_09";
//            empireBaseDialog.Range = 1000;

//            empireBaseDialog.AddText("Hello, this is general Travis from the Nova Empire.\n");
//            //+"I'm the Representative of the Nova empire in this solar system \nand I am responsible of protecting all the empire citizens in this sector.\n");

//            empireBaseDialog.AddText("I heard you met admiral Kelor from the Galactic Federation.\n" +
//                "I don't know what she told you, but my intelligence confirms their plans \nto destroy our settlement on the planet Zendar.\n" +
//                "They built a military base as a base of operations \nfor a final attack on our settlement.");

//            empireBaseDialog.AddText("I heard you met admiral Kelor from the Galactic Federation.\n" +
//                 "Our intelligence confirms their plans to destroy our \nsettlement on the planet Zendar.\n" +
//                 "They built a military base as a base of operations \nfor a final attack on our settlement.");

//            empireBaseDialog.AddText("I keep sending my forces to destroy their base, but they have a strong line of defense\n" +
//                "and they keep building ships using the minerals they mine.\n" +
//                "I need your help to remove their threat to our people's lives.");
//            empireBaseDialog.AddText("If you would help us #color{255,255,0}defeat their base#defalutColor{} we will award you with our latest weapon.\n" +
//                "I suggest you coordinate your attack with our fleets.");

//            // empireBaseDialog.DialogStartChangeText += new DialogStartChangeTextEventHandler(MeetingEmpireBase);

//            _empireBase.AddSystem(empireBaseDialog);
//        }

//        private void AddFederationBase()
//        {
//            var _federationBase = AddAgent("FederationBaseA", PlayerStartingPoint + new Vector2(-3000, -3000), 0, FactionType.Federation);

//            var fleetSystem = new MothershipFleetSystem();
//            fleetSystem.FleetSlots.Add(new MothershipFleetSystem.FleetSlot(SizeType.Medium));
//            fleetSystem.FleetSlots.Add(new MothershipFleetSystem.FleetSlot(SizeType.Gigantic));
//            fleetSystem.AddShipToSlot(0, "SmallShip14A");

//            var ship1 = AddGameObject("SmallShip14A", FactionType.Federation, _federationBase.Position + Vector2.UnitY * 500); //Temp
//            //ship1.IsActive = false;
//            ship1.SetTarget(_federationBase, TargetType.Goal);


//            _federationBase.AddSystem(fleetSystem);


//            var _federationBaseDialog = new AgentDialogSystem();
//            _federationBaseDialog.DefaultPotratitID = "F_11";
//            _federationBaseDialog.Range = 1000;
//            _federationBaseDialog.AddText("Greetings Traveler. \nI'm admiral Kelor from the Galactic Federation. \n" +
//                                            "We've just established our first base in the sector \nto open trading routes with the trading guild. \n" +
//                                            "However, the empire is accusing us of invading their sector,\nand it's sending their ships to attack us.");
//            _federationBaseDialog.AddText("Greetings Traveler. \nI'm admiral Kelor from the Galactic Federation. \n" +
//                                            "We've just established our first base in the sector \nand the Nova Empire sending their ships to attack us.");
//            _federationBaseDialog.AddText("We want to hire you to help us defend against them.\n" +
//                                           "If you are able to #color{255,255,0}destroy one of their bases#defalutColor{},\n" +
//                                           "we will be able to repel the attacks from the remaining bases.");
//            _federationBaseDialog.AddText("After you've destroyed their base, come back and we will reward you \nwith a special upgrade for your ship.");

//            //_federationBaseDialog.DialogStartChangeText += new DialogStartChangeTextEventHandler(MeetingFederationBase);

//            _federationBase.AddSystem(_federationBaseDialog);
//        }

//        public void AddShipyard()
//        {
//            var shipyard = AddGameObject("ScrapYard", FactionType.Neutral, this.PlayerStartingPoint + Vector2.UnitY * 3000) as Agent;
//            var shop = new ShopSystem();
//            shop.AddItem("SmallShip14AKitItem", 1);
//            shop.AddItem("StartingShip1KitItem", 1);
//            shop.AddItem("Enemy1KitItem", 0.5f);
//            shipyard.AddSystem(shop);
//        }



//        private void AddMoreContent()
//        {
//            var robber = AddGameObject("Robber1", FactionType.Pirates1, this.PlayerStartingPoint - Vector2.One * 8000) as Agent;
//            robber.ID = "Robber";
//            var robberDialog = new AgentDialogSystem();
//            robberDialog.AddTextWithPotratit("If you want the warp-drive you got to go throw me!", "M_13");
//            robber.Inventory.AddItem("WarpCell");
//            robber.Inventory.AddItem("ChainGun2");


//            Agent starport = AddGameObject("Starport", FactionType.Neutral, this.PlayerStartingPoint - Vector2.UnitX * 3000) as Agent;
//            var shop = new ShopSystem();
//            shop.AddItem("WarpCell", 1);
//            shop.AddItem("MediumRotationEngine2", 1);
//            shop.AddItem("VacuumModulatorItem", 1);
//            starport.AddSystem(shop);

//            var starportDialog = new AgentDialogSystem();
//            starportDialog.AddTextWithPotratit("Hello traveler, what are you looking for?", "F_11");
//            starportDialog.AddTextWithPotratit("I need to fix my ship,", Consts.AI_HELPER_TEXTURE_ID);
//            starportDialog.AddTextWithPotratit("You can fix it here,", "F_11");
//            starportDialog.AddTextWithPotratit("Also need a warpdrive,", Consts.AI_HELPER_TEXTURE_ID);
//            starportDialog.AddTextWithPotratit("You can get it at the #ctext{255,0,255,'burgers place'},", "F_11");
//            starportDialog.DialogStart += delegate ()
//            {
//                var go = FindGameObjectByID("SpaceBurger");
//                FindPlayer().SetTarget(go, TargetType.Goal);
//            };
//            starport.AddSystem(starportDialog);



//            Agent burger = AddGameObject("SpaceBurger", FactionType.Neutral, this.PlayerStartingPoint - Vector2.UnitY * 3000) as Agent;
//            shop = new ShopSystem();
//            shop.AddItem("MatA2", 1);
//            burger.AddSystem(shop);
//            var burgerDialog = new AgentDialogSystem();
//            burgerDialog.AddTextWithPotratit("You can buy a tasty Sunday, The oracle loves Sundays", "Gustav_Shop");
//            burgerDialog.AddTextWithPotratit("Also if you kill the Great Pirate Robber it will help me a lot!", "Gustav_Shop");
//            burgerDialog.DialogStart += delegate ()
//            {
//                var go = FindGameObjectByID("Robber") as Agent;
//                FindPlayer().SetTarget(go, TargetType.Goal);
//            };
//            burger.AddSystem(burgerDialog);

//            var shipyard = AddGameObject("ScrapYard", FactionType.Neutral, this.PlayerStartingPoint + Vector2.UnitY * 3000) as Agent;
//            shop = new ShopSystem();
//            shop.AddItem("SmallShip14AKitItem", 1);
//            shop.AddItem("StartingShip1KitItem", 1);
//            shop.AddItem("Enemy1KitItem", 0.5f);
//            shipyard.AddSystem(shop);

//            //Dialog dialog = new Dialog("Your ship is damaged, go to the #ctext{255,255,0,'Starport'} ");
//            //DialogSystem dialogSystem = new DialogSystem();
//            //dialogSystem.Dialog = dialog;            

//            var dialogSystem = new AgentDialogSystem();
//            dialogSystem.AddTextWithPotratit("Your ship is damaged, go to the #ctext{255,255,0,'Starport'} ", Consts.AI_HELPER_TEXTURE_ID);
//            dialogSystem.DialogStart += delegate ()
//            {
//                var go = FindGameObjectByID("Starport");
//                FindPlayer().SetTarget(go, TargetType.Goal);
//            };

//            GetPlayerFaction().Mothership.AddSystem(dialogSystem);
//        }
//    }
//}