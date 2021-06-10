

//            //_mineMessage = new InstituteSystem();
//            //_mineMessage.Text = "Asteroid mines have rare materials, but you will need #color{255,255,0}mining laser#defalutColor{} to mine it.";
//            //_mineMessage.Range = 220;
//            //_mineMessage = new AgentDialogSystem();
//            //_mineMessage.IsBlocking = false;
//            //// _mineMessage.DefaultPotratitID = Consts.INFORMATION_TEXTURE_ID;
//            //_mineMessage.AddText("Asteroid mines have rare materials, but you will need mining laser to mine it.");
//        }

//        #region Add Game Objects          

//        private void AddCraftingStaion(Vector2 position)
//{
//    Agent craftingStaion = (Agent)AddGameObject("CraftingStation", FactionType.Player, position + Vector2.One * 400);

//    InstituteSystem system = new InstituteSystem();
//    system.Text = "Press I to store items";
//    system.Range = 150;
//    craftingStaion.AddSystem(system);
//}

//private void AddEmpireBases(GameObject moon)
//{
//    var empireBase = AddAgent("EmpireBase", moon.Position + Vector2.UnitY * -200, 0, FactionType.Empire);
//    empireBase.SetTarget(moon, TargetType.Goal);
//    empireBase.AddSystem(CreateArena(FactionType.Empire));
//    _empireBases.Add(empireBase);

//    empireBase = AddAgent("EmpireBase", PLAYER_STARTING_POSITION + new Vector2(12000, -10000), 0, FactionType.Empire);
//    empireBase.SetTarget(_federationBase, TargetType.Goal);
//    //  var mine = AddAgent("Mine1", empireBase.Position - Vector2.UnitX * 5000 + Vector2.UnitY * -8000, 0);
//    //  mine.AddSystem(_mineMessage);

//    empireBase.AddSystem(CreateArena(FactionType.Empire));
//    _empireBases.Add(empireBase);

//    empireBase = AddAgent("EmpireBase", PLAYER_STARTING_POSITION + new Vector2(-40000, 9000), 0, FactionType.Empire);
//    empireBase.SetTarget(_federationBase, TargetType.Goal);
//    //var mine2 = AddAgent("Mine1", empireBase.Position - Vector2.UnitX * -3000 + Vector2.UnitY * 8000, 0);
//    //mine2.AddSystem(_mineMessage);

//    empireBase.AddSystem(CreateArena(FactionType.Empire));
//    _empireBases.Add(empireBase);

//    _pirateBase = AddAgent("PirateBase", PLAYER_STARTING_POSITION + new Vector2(0, -45000), 0, FactionType.Pirates1);
//    //    AddAgent("Mine1", _pirateBase.Position - Vector2.UnitX * -3000 + Vector2.UnitY * 8000, 0);

//    var pirateBase = AddAgent("PirateBase", PLAYER_STARTING_POSITION + new Vector2(24500, 0), 0, FactionType.Pirates1);
//    //   AddAgent("Mine1", pirateBase.Position - Vector2.UnitX * -3000 + Vector2.UnitY * 8000, 0);
//}

//private InstituteSystem CreateArena(FactionType factionType)
//{
//    InstituteSystem arena = new InstituteSystem();
//    arena.ActivityName = "FactionChallenges";
//    arena.ActivityParams = ((int)factionType).ToString();
//    arena.Text = "Press F to enter the arena";
//    arena.Persistent = false;
//    return arena;
//}

//private void AddFederationBase()
//{
//    _federationBase = AddAgent("FederationBase", new Vector2(-19000, -2500), 0, FactionType.Federation);
//    InstituteSystem arena = new InstituteSystem();
//    _federationBase.AddSystem(CreateArena(FactionType.Federation));

//    var mine = AddAgent("ResourceMine1", _federationBase.Position - Vector2.UnitX * -2500 + Vector2.UnitY * -7000, 0);
//    mine.AddSystem(_mineMessage);

//    _federationBaseDialog = new AgentDialogSystem();
//    _federationBaseDialog.DefaultPotratitID = "F_11";
//    _federationBaseDialog.Range = 1000;
//    _federationBaseDialog.AddText("Greetings Traveler. \nI'm admiral Kelor from the Galactic Federation. \n" +
//                                    "We've just established our first base in the sector \nto open trading routes with the trading guild. \n" +
//                                    "However, the empire is accusing us of invading their sector,\nand it's sending their ships to attack us.");
//    _federationBaseDialog.AddText("Greetings Traveler. \nI'm admiral Kelor from the Galactic Federation. \n" +
//                                    "We've just established our first base in the sector \nand the Nova Empire sending their ships to attack us.");
//    _federationBaseDialog.AddText("We want to hire you to help us defend against them.\n" +
//                                   "If you are able to #color{255,255,0}destroy one of their bases#defalutColor{},\n" +
//                                   "we will be able to repel the attacks from the remaining bases.");
//    _federationBaseDialog.AddText("After you've destroyed their base, come back and we will reward you \nwith a special upgrade for your ship.");
//    _federationBaseDialog.DialogStartChangeText += new DialogStartChangeTextEventHandler(MeetingFederationBase);

//    _federationBase.AddSystem(_federationBaseDialog);
//}

//private void AddEmpirePlanetDialog()
//{
//    // kobi: don't play battle music
//    _empireBaseDialog = new AgentDialogSystem();
//    _empireBaseDialog.DefaultPotratitID = "M_09";
//    _empireBaseDialog.Range = 1000;

//    _empireBaseDialog.AddText("Hello, this is general Travis from the Nova Empire.\n");
//    // +"I'm the Representative of the Nova empire in this solar system \nand I am responsible of protecting all the empire citizens in this sector.\n");

//    _empireBaseDialog.AddText("I heard you met admiral Kelor from the Galactic Federation.\n" +
//        "I don't know what she told you, but my intelligence confirms their plans \nto destroy our settlement on the planet Zendar.\n" +
//        "They built a military base as a base of operations \nfor a final attack on our settlement.");

//    _empireBaseDialog.AddText("I heard you met admiral Kelor from the Galactic Federation.\n" +
//         "Our intelligence confirms their plans to destroy our \nsettlement on the planet Zendar.\n" +
//         "They built a military base as a base of operations \nfor a final attack on our settlement.");

//    _empireBaseDialog.AddText("I keep sending my forces to destroy their base, but they have a strong line of defense\n" +
//        "and they keep building ships using the minerals they mine.\n" +
//        "I need your help to remove their threat to our people's lives.");
//    _empireBaseDialog.AddText("If you would help us #color{255,255,0}defeat their base#defalutColor{} we will award you with our latest weapon.\n" +
//        "I suggest you coordinate your attack with our fleets.");

//    _empireBaseDialog.DialogStartChangeText += new DialogStartChangeTextEventHandler(MeetingEmpireBase);

//    _empireBases[0].AddSystem(_empireBaseDialog);
//}

//private void MeetingEmpireBase(AgentDialogSystem agentDialogSystem)
//{
//    //_mm.RemoveMission(MissionsTexts.StartingNode_VisitEmpire_Name);
//    // _mm.AddMission(Missions[MissionsTexts.StartingNode_DestroyFederation_Name]);

//    float relation = GameEngine.GetFaction(FactionType.Empire).GetRelationToFaction(FactionType.Player);

//    if (relation < 0)
//    {
//        agentDialogSystem.AddText("As a token of good faith, I will order all of the empire ships not to engage you in battle as long as you won't shoot first.");
//        GameEngine.GetFaction(FactionType.Empire).ChangeRelationToFaction(GameEngine, FactionType.Player, 2);
//    }

//    agentDialogSystem.AddTextWithSoundAndPotrait("Captain, Admiral Kelor is hailing us.", "admiralKelor", Consts.AI_HELPER_TEXTURE_ID);
//    agentDialogSystem.AddTextWithPotratit("Captain, I know you talked with general Travis. \nDon't believe his lies!\n" +
//        "He probably told you we came to kill them all. \nWe just came here to trade and mine minerals in this rich sector.\n" +
//        "I hope you will help me, keeping my crew safe.", "F_11");

//    agentDialogSystem.AddTextWithPotratit("Captain, don't believe general Travis' lies!\n" +
//       "He probably told you we came to kill them all. \nWe just came here to trade and mine minerals in this rich sector.\n" +
//       "I hope you will help me, keeping my crew safe.", "F_11");

//    agentDialogSystem.AddTextWithSoundAndPotrait("It's a Tough choice! \nWho do you want to help in this battle between the federation and the empire? \n" +
//          "There's always the option not to interfere in their struggle.\n", "thoughChoice", Consts.AI_HELPER_TEXTURE_ID); // yaniv: update sound
//                                                                                                                           // "Your choice, Captain. \nI'm with you whatever you choose, as long you don't kill us in battle."
//}

//private void MeetingFederationBase(AgentDialogSystem agentDialogSystem)
//{
//    AddEmpirePlanetDialog();
//    float relation = GameEngine.GetFaction(FactionType.Federation).GetRelationToFaction(FactionType.Player);

//    if (relation < 0)
//    {
//        agentDialogSystem.AddText("I know you've had some conflicts with our ships in the past.\n" +
//                                    "I'm sure it was a misunderstanding. \nThey won't attack you again unless provoked.");
//    }

//    GameEngine.GetFaction(FactionType.Federation).ChangeRelationToFaction(GameEngine, FactionType.Player, 1);

//    agentDialogSystem.AddTextWithSoundAndPotrait("I think we should talk with the empire before \nwe decide if we want to start a war with them.", "TalkFirst", Consts.AI_HELPER_TEXTURE_ID);
//}

//private void AddShips()
//{
//    _pirateLord = AddAgent("SmallShip6A", _tradingGuildBase.Position + Vector2.UnitX * 16000 - Vector2.UnitY * 9000, 0, FactionType.Pirates1);
//    _pirateLord = AddAgent("SmallShip6A", _tradingGuildBase.Position + Vector2.UnitX * 8000 - Vector2.UnitY * 9000, 0, FactionType.Pirates1);

//    //var miner = AddAgent("Miner1", _federationBase.Position / AddGameObjectScale - Vector2.UnitX + Vector2.UnitY * 1000, 0, Factions.Federation);
//    //miner.Parent = _federationBase;

//    for (int j = 0; j < 5; j++)
//    {
//        var supporter = AddAgent("Enemy1", _federationBase.Position + Vector2.UnitX * +(j * 100) + Vector2.UnitY * 1000, 0, FactionType.Federation);
//        supporter.SetTarget(_federationBase, TargetType.Goal);
//    }
//}

//private void AddAsteroids()
//{

//    var mine = AddAgent("ResourceMine1", PLAYER_STARTING_POSITION + new Vector2(-5000, 5000), 0);
//    mine.AddSystem(_mineMessage);

//    //this.AddObjectRandomlyInCircle(typeof(BigAsteroid).Name, 300, PLAYER_STARTING_POSITION.Y * 0.5f, PLAYER_STARTING_POSITION.Y * 2.55f, 143443);
//    //this.AddObjectRandomlyInCircle(typeof(Asteroid).Name, 1500, PLAYER_STARTING_POSITION .Y* 0.45f, PLAYER_STARTING_POSITION.Y * 2.6f, 4123);
//    this.AddObjectRandomlyInCircle("Asteroid1", 300, PLAYER_STARTING_POSITION.Y * 0.5f, PLAYER_STARTING_POSITION.Y * 2.55f, 143443);
//    this.AddObjectRandomlyInCircle("SmallAsteroid1", 1500, PLAYER_STARTING_POSITION.Y * 0.45f, PLAYER_STARTING_POSITION.Y * 2.6f, 4123);
//    this.AddObjectRandomlyInCircle("CrateA", 300, PLAYER_STARTING_POSITION.Y * 0.5f, PLAYER_STARTING_POSITION.Y * 2.55f, 1423441);

//    Random rand = new Random(97433);
//    for (int i = 1; i < 5; i++)
//    {
//        float minRad = 6000;
//        float maxRad = PLAYER_STARTING_POSITION.Y * 2.45f;
//        for (int j = 0; j < 60; j++)
//        {
//            Vector2 centerPoint = FMath.ToCartesian(FMath.TransformToRadius((float)rand.NextDouble(), maxRad, minRad), (float)rand.NextDouble() * MathHelper.TwoPi);
//            this.AddObjectRandomlyInLocalCircle("Asteroid" + i.ToString(), 10, rand.Next(1000, 2000), centerPoint);
//        }
//    }


//    //for (int i = 0; i < GemGeneration.Names.Count; i++)
//    //{
//    //    float minRad = 6000;
//    //    float maxRad = PLAYER_STARTING_POSITION.Y * 2.45f;
//    //    for (int j = 0; j < 60; j++)
//    //    {
//    //        Vector2 centerPoint = FMath.ToCartesian(FMath.TransformToRadius((float)rand.NextDouble(), maxRad, minRad), (float)rand.NextDouble() * MathHelper.TwoPi);
//    //        this.AddObjectRandomlyInLocalCircle("Big" + GemGeneration.Names[i] + "Asteroid", 1, rand.Next(1000, 2000), centerPoint);
//    //        //this.AddObjectRandomlyInCircle("Big" + GemGeneration.names[i] + "Asteroid", 800, , 5300, 12341 + (i*31+j) * 11); //Change 
//    //    }
//    //}

//    //this.AddObjectRandomlyInCircle(typeof(LavaAsteroid).Name, 200, PLAYER_STARTING_POSITION.Y * 0.7f, 6300, 323234);

//    GameEngine.Update();
//}

//private GameObject AddSun()
//{
//    GameObject sunBackground = AddGameObject("SunBackground", 0, Vector2.Zero);
//    var sun = AddGameObject("Sun", 0, Vector2.Zero, 0);
//    sunBackground.Size = sun.Size;
//    AddGameObject("SunFullscreenColorFx", Vector2.Zero);
//    return sun;
//}

//private GameObject AddEmpirePlanet(GameObject sun)
//{
//    var earth = AddGameObject("Earth", 0, Vector2.UnitX * sun.Size * -7.5f);
//    earth.Parent = sun;

//    var moon = AddGameObject("Moon", 0, Vector2.UnitX * sun.Size * -8.58f);
//    moon.Parent = earth;
//    return moon;
//}

//private void AddPortals()
//{
//    _gustavPortal = (Agent)AddGameObject(typeof(Portal).Name, 0, PLAYER_STARTING_POSITION - Vector2.UnitX * 11000, 0);
//    //_gustavPortal.ActivityName = typeof(GustavLevel).Name;
//    //_gustavPortal.ActivityParams = SceneID.ToString();
//    //_gustavPortal.Message = "Press F to enter wormhole"; //meet Gustav!"; //Change

//    Random rand = new Random(23443);
//    for (int i = 0; i < 5; i++)
//    {

//        Vector2 pos = _gustavPortal.Position + FMath.ToCartesian((float)(rand.NextDouble() * 6000 + 1500), (float)rand.NextDouble() * MathHelper.TwoPi);
//        //    this.AddObjectRandomlyInLocalCircle(typeof(BigAsteroid).Name, 10, 4000, pos, 50, i * 291 + 1231);
//        this.AddObjectRandomlyInLocalCircle(typeof(Asteroid).Name, 20, 5000, pos, 50, i * 431 + 2432);
//    }

//    //_epsilonMinesPortal = (Institute)AddGameObject(typeof(Portal).Name, 0, PLAYER_STARTING_POSITION + new Vector2(5000, 7200));
//    //_epsilonMinesPortal.ActivityName = typeof(RareMineralLevel).Name;
//    //_epsilonMinesPortal.ActivityParams = SceneID.ToString();
//    //_epsilonMinesPortal.Message = "Press F to enter wormhole";//the Epsilon mines";

//    //_minesOfSadnessPortal = (Institute)AddGameObject(typeof(Portal).Name, 0, PLAYER_STARTING_POSITION + new Vector2(-10500, 7200), 0);
//    //_minesOfSadnessPortal.ActivityName = typeof(MinesOfSadness).Name;
//    //_minesOfSadnessPortal.ActivityParams = SceneID.ToString();
//    //_minesOfSadnessPortal.Message = "Press F to enter wormhole"; //"to enter the Mines of Sadness";
//}

//#endregion


//#region StarPort

//private void AddStarPort()
//{
//    AddMissionsMenu();

//    _starPort = (Agent)AddGameObject(typeof(Starport).Name, 0, (PLAYER_STARTING_POSITION - Vector2.UnitX * 5000), 90);
//    _starportShopActivity = AddStarPortItemsShop();
//    //Menu starportMenu = AddStartPortMenu(shopActivity);
//    //_starPort.TargetActivity = _starportShopActivity;
//    //_starPort.Message = $"Press {PlayerMouseAndKeys.commandBindings[PlayerCommand.Use]} to dock";
//    //TODO: update regularly, to keep up if the player rebinds keys.A rich text tag which looks up key/ command bindings would be ideal

//    AddStarPortText(_starPort);
//}

//private void AddStarPortText(Agent starPort)
//{
//    AgentDialogSystem starportDialog = new AgentDialogSystem();
//    starportDialog.DefaultPotratitID = "F_05";
//    starportDialog.AddText("Welcome to Solar Conflict starport. \nMy name is Laura. \nHow can I help you?");
//    starportDialog.DialogStartChangeText += new DialogStartChangeTextEventHandler(AddStarportMissions);
//    starPort.AddSystem(starportDialog);
//}

//private void AddStarportMissions(AgentDialogSystem starportDialog)
//{
//    PlayerAgent.SetTarget(null, TargetType.Goal);

//    //if (Missions.ContainsKey(MissionsTexts.StartingNode_Gustav_Name))
//    //{
//    //    _mm.AddMission(Missions[MissionsTexts.StartingNode_Gustav_Name]);
//    //    starportDialog.AddTextWithPotratit("I need to #color{255,255,0}repair my communication system#defalutColor{}. \nCan you assist me?", Consts.COMMUNICATION_CONSOLE_TEXTURE_ID); // yaniv: arrived here
//    //    starportDialog.AddText("Unfortunately we don't have spare parts for fixing your communication system.\n" +
//    //                                                                                          "But I am sure Gustav will have some. Maybe you can get them from him.\nAssuming you can convince him to deal with you instead of shooting you.\n" +
//    //                             "But I am sure Gustav will have some.\n" +
//    //                             "I'm sending you his coordinates. \nGood Luck! Anything else?");
//    //}

//    //if (Missions.ContainsKey(MissionsTexts.StartingNode_VisitTradingGuild_Name))
//    //{
//    //    _mm.AddMission(Missions[MissionsTexts.StartingNode_VisitTradingGuild_Name]);
//    //    starportDialog.AddTextWithPotratit("Do you have a #color{255,255,0}Star drive#defalutColor{}?", Consts.COMMUNICATION_CONSOLE_TEXTURE_ID);

//    //    starportDialog.AddText("No, but you should go and #color{255,255,0}visit the trading guild starbase#defalutColor{}, \nthey should have blueprints for a warp drive.\n");
//    //}
//    starportDialog.AddText("Anyway, if you're planning to mine some asteroids, \nwe will always be happy to buy your minerals in our shop.");
//    starportDialog.AddText("Please enter our starport and enjoy our facilities.");
//    starportDialog.AddTextWithPotratit("In the Mission Log (#color{255,255,0}L#defalutColor{}), you can select a mission to see how to get there.", null);

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

//private ShopActivity AddStarPortItemsShop()
//{
//    ShopActivity shopActivity = new ShopActivity();
//    shopActivity.AddItems(new List<string>() { "AutoRepairKit", "AutoEnergyKit", "FastEngineItem", "VacuumModulatorItem", "AsteroidGrinderItem", "ChainGunTurret", "HeavyGun",
//                                                      "EchoSprintItem", "KineticMineLauncher",
//                                                       "PlasmaGun1", "KineticShotgun", "EnergyNetLauncher", "DeployableTurretItem",
//                                                       "ShieldA0", "ShieldB0",
//                                                       "GeneratorA0", "GeneratorB0", "MatA0", "MatB0" ,"MatC1"
//            });
//    shopActivity.AddItems(new List<string>() { "AutoRepairKit", "AutoEnergyKit", "VacuumModulatorItem", "AsteroidGrinderItem", "ChaingunTurretItem", "HeavyChainGunItem",
//                                                       "PlasmaGun1", "EpicShotgunItem", "FusionBlasterTurretItem", "StunGunItem", "HarpoonGun", "EnergyNetLauncher", "DeployableTurretItem" });
//    shopActivity.AddItems(new List<string>() { "AutoRepairKit", "AutoEnergyKit", "EpicShotgunItem", "HeavyChainGunItem", "FastEngineItem", "VacuumModulatorItem", "AsteroidGrinderItem", "EchoSprintItem", "KineticMineLauncher", "AllyShip1KitItem" });
//    shopActivity.ShopName = "Starport Traders";

//    return shopActivity;
//}

//private Menu AddStartPortMenu(ShopActivity shopActivity)
//{
//    MenuData menuData = new MenuData("Starport: Solar Conflict");


//    MenuEntry entery = new MenuEntry();
//    entery.DisplayText = "Starport Shop";
//    entery.activity = shopActivity;
//    menuData.MenuEntryList.Add(entery);



//    entery = new MenuEntry();
//    entery.DisplayText = "Starport Bar";
//    entery.TooltipText = "Enter to get missions and information\nYou can see your missions on the mission log screen (press #color{255,255,0}L#defalutColor{}).\nWhen you #color{255,255,0}select a mission#defalutColor{} an arrow will be set to the mission location.";
//    entery.activity = _missionsMenu;
//    menuData.MenuEntryList.Add(entery);


//    entery = new MenuEntry();
//    entery.ActivityName = "FactionChallenges";
//    entery.ActivityParams = ((int)FactionType.TradingGuild).ToString();
//    entery.DisplayText = "Challenges";
//    entery.TooltipText = "Complete challenges of varying difficulty";
//    menuData.MenuEntryList.Add(entery);

//    entery = new MenuEntry();
//    entery.DisplayText = "Undock";
//    entery.TooltipText = "Exit to the solar system";
//    entery.ActivityName = "back";
//    menuData.MenuEntryList.Add(entery);

//    return new Menu(menuData);
//}

//#endregion


//#region StarPort Missions

//private void AddMissionsMenu()
//{
//    MenuData menuData = new MenuData("Ten Forward bar");
//    menuData.MenuEntryList.Add(CreateMenuEntry(MissionsTexts.StartingNode_VisitFederation_Name, MissionsTexts.StartingNode_VisitFederation_Description));
//    menuData.MenuEntryList.Add(CreateMenuEntry(MissionsTexts.StartingNode_ArenaFight_Name, MissionsTexts.StartingNode_ArenaFight_Description));
//    menuData.MenuEntryList.Add(CreateMenuEntry("Back", null));
//    _missionsMenu = new Menu(menuData);
//}

//private MenuEntry CreateMenuEntry(string displayName, string tooltip)
//{
//    MenuEntry entery = new MenuEntry();
//    entery.ActivityName = "back";
//    entery.Action = MissionSelectHandler;
//    entery.DisplayText = displayName;
//    entery.TooltipText = tooltip;
//    entery.Data = displayName;

//    return entery;
//}

//private void MissionSelectHandler(GuiControl source, CursorInfo cursorLocation) //TODO: temp for demo
//{
//    string missionID = source.Data.ToString();

//    if (missionID != "Back")
//    {
//        ActivityManager.Inst.AddToast("You accepted the mission", 60 * 3);
//        //_mm.AddMission(Missions[missionID]);
//        _missionsMenu.RemoveMenuEntry(missionID);
//    }
//}

//#endregion


//#region Trading Guild Base

//private void AddTradingGuildBase()
//{
//    _tradingGuildBase = (Agent)AddGameObject(typeof(SmallShop1).Name, 0, PLAYER_STARTING_POSITION + new Vector2(10500, -500), 90);

//    ShopActivity shopActivity = new ShopActivity();
//    shopActivity.ShopName = "Trading Guild starbase Destiny";
//    shopActivity.AddItems(new List<string>() { "FluxCapacitorItem", WARP_DRIVE_BLUEPRINTS_ITEM, "AutoEnergyKit", "MatA0", "MatB0", "MatC0" });
//    /*"EchoSprintItem", "Shield3", "Shield6", "Shield4", "Shield5", "Shield7",
//    "MissileLauncherItem", "MissileItem", "BlastMissileItem", "Goo DronesMissileItem", "EMPMissileItem", "Energy DrainMissileItem",   "Gravity WellMissileItem",
//    "Blast WaveWarheadItem",   "EMP WaveWarheadItem",  "Goo DronesWarheadItem",   "Energy Drain WaveWarheadItem",   "Gravity WellWarheadItem",
//    "FusionBlasterTurretItem", "StunGunItem", "HarpoonGun"});*/

//    AddTradingGuildFirstEncounterText();
//    //_tradingGuildBase.TargetActivity = shopActivity;
//    //_tradingGuildBase.Message = "Press F to dock"; // to Trading Guild starbase Destiny";
//}

//private void AddTradingGuildFirstEncounterText()
//{
//    _tradingGuildDialog = new AgentDialogSystem();
//    _tradingGuildDialog.DefaultPotratitID = "M_03";
//    _tradingGuildDialog.AddText("Greetings from the Trading Guild starbase Destiny. \nMy name is Flen. \nDo you wish to trade?");

//    _tradingGuildDialog.DialogStart += new DialogStartEventHandler(AddTradingPostMission);
//    _tradingGuildBase.AddSystem(_tradingGuildDialog);
//}

//private void AddTradingPostMission(Scene scene)
//{
//    //_mm.FinishMission(MissionsTexts.StartingNode_VisitTradingGuild_Name);
//    // Missions.Remove(MissionsTexts.StartingNode_VisitTradingGuild_Name);
//    _tradingGuildDialog.AddTextWithPotratit("I need to repair my warp drive.", Consts.COMMUNICATION_CONSOLE_TEXTURE_ID);

//    if (_pirateLord.IsActive)
//    {
//        //  _mm.AddMission(Missions[MissionsTexts.StartingNode_DestoryPirateLord_Name], true);
//        _tradingGuildDialog.AddText("We can provide you with blueprints for a new #color{255,255,0}warp drive#defalutColor{}. \nBut first, you will need to help us.");
//        _tradingGuildDialog.AddText("The Pirate lord Pierce has been targeting our shipping routes for a long time. \nIf you can help us \"solve\" this problem, we will give you the blueprints.");
//        _tradingGuildDialog.AddTextWithPotratit("Consider Pierce history.", Consts.COMMUNICATION_CONSOLE_TEXTURE_ID);
//        _tradingGuildDialog.AddText("Great! Let us know when you take care of him. \nIn the meantime would you like to purchase something at our store?");
//    }
//    else
//    {
//        //Missions.Remove(MissionsTexts.StartingNode_DestoryPirateLord_Name);
//        //_tradingGuildDialog.AddText("No problem, we have warp drive blueprints in our stock.");
//        //_mm.AddMission(Missions[MissionsTexts.StartingNode_GetWarpDriveBluePrints_Name], true);
//        //((ShopActivity)_tradingGuildBase.TargetActivity).AddItem("WarpDriveBlueprints");
//    }

//    TextData text = new TextData("I would like to tell you about our new pre-order sale. \nIf you order our sub atomic nuclear toaster today you can save up to 2.36% ! \nYou won't find a better deal than this.");
//    _tradingGuildDialog.AddRandomText(new List<TextData> { text });
//}

//private void GetBluePrints()
//{
//    _tradingGuildDialog.AddText("In order to build the #color{255,255,0}warp drive#defalutColor{} you will need several items. \n#color{255,255,0}Diamonds#defalutColor{} that you can find in the Epsilon mines.\n" +
//                                  "You'll also need some #color{255,255,0}Germanium#defalutColor{}, which you can mine from huge asteroids, \nbut you will need a mining laser.\n" +
//                                  "The Ancient Miner in the Mines of Sadness has a mining laser, maybe he can mine it for you.\n" +
//                                  "I'm sure you can buy the rest of the items, or find them on your own.");
//    //_mm.FinishMission(MissionsTexts.StartingNode_GetWarpDriveBluePrints_Name);

//    //if (!PlayerAgent.GetInventory().CheckForItem("DiamondItem", 4))
//    //{
//    //    _mm.AddMission(Missions[MissionsTexts.StartingNode_Diamonds_Name]);
//    //}
//    //if (!PlayerAgent.GetInventory().CheckForItem("MiningLaserItem1", 1))
//    //{
//    //    _mm.AddMission(Missions[MissionsTexts.StartingNode_MinesOfSadness_Name]);
//    //}
//}

//private void AddTradingPostReward()
//{
//    FindPlayer().GetInventory().AddItem("WarpDriveBlueprints");
//}

//#endregion

//private void AddStartingAiHelperText()
//{
//    _startingDialogueInitialized = true;

//    AgentDialogSystem startingDialog = new AgentDialogSystem();
//    startingDialog.DefaultPotratitID = Consts.AI_HELPER_TEXTURE_ID;

//    startingDialog.AddTextWithSound("Finally you're back! \nBut you don't look like yourself. \nWait. I am scanning.", "StartingNode_1");
//    startingDialog.AddTextWithSound("Yes, I was right. \nYour reconstruction didn't go well. \nYou probably don't remember everything, right?!\n", "StartingNode_2");
//    startingDialog.AddTextWithSound("But as your loyal AI helper I will share what I know.\n" +
//            "Whenever your ship is destroyed it will reconstruct here, near the mothership.\n" +
//            "You will lose all your inventory, but at least you'll keep all of your equipped items.\n" +
//            "You can also use the mothership cargo hold to store items.", "StartingNode_3");
//    startingDialog.AddTextWithSound("While you were gone, we were attacked. \nIt was very scary, " +
//            "however I was able to scare them away.\n", "StartingNode_4");
//    startingDialog.AddTextWithSound("I am afraid to admit it, \nbut they damaged the mother ship.\n" +
//           "I fixed what I could. \nBut the star drive and the communication system were damaged.\n" +
//           "Please don't erase me, I will do better next time, I promise.\n", "StartingNode_5");




//    startingDialog.AddTextWithSound("Anyway, we have to #color{255,255,0}fix the communication system#defalutColor{} to decrypt \na message we got from the Void.", "StartingNode_6a");
//    startingDialog.AddTextWithPotratit("The Void?", Consts.COMMUNICATION_CONSOLE_TEXTURE_ID);
//    startingDialog.AddTextWithSound("Who's the Void?! WOW, you really don't remember? It's your boss.\n", "StartingNode_6b");
//    startingDialog.AddTextWithSound("I found the location of a nearby starport. \nI suggest you go there and see if they can fix our communication system.",
//       "StartingNode_7");

//    startingDialog.AddTextWithPotratit("You got a new mission, check the mission log (F2).", Consts.INFORMATION_TEXTURE_ID);
//    startingDialog.AddTextWithPotratit("Press Tab to switch between your ships", Consts.INFORMATION_TEXTURE_ID);
//    _mothership.AddSystem(startingDialog);
//}

//private void InitMissions()
//{
//    //_mm.AddMission(new Mission(MissionsTexts.StartingNode_FixCommunication_Name, MissionsTexts.StartingNode_FixCommunication_Description, SceneID));

//    //Missions.Add(MissionsTexts.StartingNode_Gustav_Name, new Mission(MissionsTexts.StartingNode_Gustav_Name, MissionsTexts.StartingNode_Gustav_Description, SceneID, _gustavPortal));
//    //Missions.Add(MissionsTexts.StartingNode_VisitTradingGuild_Name, new Mission(MissionsTexts.StartingNode_VisitTradingGuild_Name, MissionsTexts.StartingNode_VisitTradingGuild_Description, SceneID, _tradingGuildBase));
//    //Missions.Add(MissionsTexts.StartingNode_Diamonds_Name, new Mission(MissionsTexts.StartingNode_Diamonds_Name, MissionsTexts.StartingNode_Diamonds_Description, SceneID, _epsilonMinesPortal));
//    //Missions.Add(MissionsTexts.StartingNode_MinesOfSadness_Name, new Mission(MissionsTexts.StartingNode_MinesOfSadness_Name, MissionsTexts.StartingNode_MinesOfSadness_Description, SceneID, _minesOfSadnessPortal));
//    //Missions.Add(MissionsTexts.StartingNode_DestoryPirateLord_Name, new Mission(MissionsTexts.StartingNode_DestoryPirateLord_Name, MissionsTexts.StartingNode_DestoryPirateLord_Description, SceneID, _pirateLord));
//    //Missions.Add(MissionsTexts.StartingNode_VisitFederation_Name, new Mission(MissionsTexts.StartingNode_VisitFederation_Name, MissionsTexts.StartingNode_VisitFederation_Description, SceneID, _federationBase));
//    //Missions.Add(MissionsTexts.StartingNode_VisitEmpire_Name, new Mission(MissionsTexts.StartingNode_VisitEmpire_Name, MissionsTexts.StartingNode_VisitEmpire_Description, SceneID, _empireBases[0]));
//    //Missions.Add(MissionsTexts.StartingNode_DestroyEmpire_Name, new Mission(MissionsTexts.StartingNode_DestroyEmpire_Name, MissionsTexts.StartingNode_DestroyEmpire_Description, SceneID, _empireBases[0]));
//    //Missions.Add(MissionsTexts.StartingNode_DestroyFederation_Name, new Mission(MissionsTexts.StartingNode_DestroyFederation_Name, MissionsTexts.StartingNode_DestroyFederation_Description, SceneID, _federationBase));
//    //Missions.Add(MissionsTexts.StartingNode_GetWarpDriveBluePrints_Name, new Mission(MissionsTexts.StartingNode_GetWarpDriveBluePrints_Name, MissionsTexts.StartingNode_GetWarpDriveBluePrints_Description, SceneID, _tradingGuildBase));
//    //Missions.Add(MissionsTexts.StartingNode_ArenaFight_Name, new Mission(MissionsTexts.StartingNode_ArenaFight_Name, MissionsTexts.StartingNode_ArenaFight_Description, SceneID, _empireBases[0]));
//    //Missions.Add(MissionsTexts.StartingNode_ClaimKillFederationReward_Name, new Mission(MissionsTexts.StartingNode_ClaimKillFederationReward_Name, MissionsTexts.StartingNode_ClaimKillFederationReward_Description, SceneID, _empireBases[0]));
//    //Missions.Add(MissionsTexts.StartingNode_ClaimKillEmpireReward_Name, new Mission(MissionsTexts.StartingNode_ClaimKillEmpireReward_Name, MissionsTexts.StartingNode_ClaimKillEmpireReward_Description, SceneID, _federationBase));
//}

//public override void OnEnter(ActivityParameters parameters)
//{
//    base.OnEnter(parameters);

//    //PlayerAgent = GameEngine.GetFaction(FactionType.Player).FleetShips.First() as Agent; // TEMP
//    //_mothership = GameEngine.GetFaction(FactionType.Player).MotherShip as Agent;

//    //PlayerAgent.control.ControlType = AgentControlType.Player; // TEMP. Why is this necessary?            

//    //if (!_startingDialogueInitialized)
//    //    AddStartingAiHelperText(); // doesn't work if we stick it in the LSM, even though _mothership should be correct before the first Update() call
//    ////How come?
//}

//public override void UpdateScript(InputState inpuState)
//{
//    _pirateBase.SetTarget(_mothership, TargetType.Enemy);
//    //yaniv: do we need this code ?
//    //if (PlayerAgent.Inventory.CheckForItem("Communication Chip", 1) && _mm.ContainMission(MissionsTexts.StartingNode_FixCommunication_Name))
//    //{
//    //    string id = AddTextBox("Your Communication is fixed", time: 120, image: Consts.AI_HELPER_TEXTURE_ID, id: "CommunicationFixed");
//    //    _mm.FinishMission(MissionsTexts.StartingNode_FixCommunication_Name);
//    //    PlayerAgent.Inventory.RemoveItem("Communication Chip");
//    //}


//    //base.UpdateScript(inpuState);
//    //if (GameEngine.FrameCounter == (int)(60 * 60 * 2.5f))
//    //{
//    //    DialogManager.AddDialogBox("New missions available, Dock to the Starport", boxID: "newMissions", isFixedSize: false, maxLifetime: 300);
//    //    _starPort.TargetActivity = AddStartPortMenu(_starportShopActivity);
//    //}

//    CheckDestroyPirateLordMission();
//    CheckIfFederationBaseDestroyed();
//    CheckIfEmpireBaseDestroyed();
//    CheckIfLowOnFunds();
//    CheckEmpireRelation();
//    CheckFederationRelation();
//    CheckBluePrintsMission();

//    ShowCraftingMessage();
//    CheckArenaMission();
//    SetDeathLocation();

//    FadeIn();

//    _lsm.ActivateState();
//}

//private void SetDeathLocation()
//{
//    if (LastPlayerDeathPostion != Vector2.Zero)
//    {
//        var dummy = new DummyObject();
//        dummy.Position = PlayerAgent.Position;

//        if (GameObject.DistanceFromEdge(PlayerAgent, dummy) < 100)
//        {
//            LastPlayerDeathPostion = Vector2.Zero;
//            PlayerAgent.SetTarget(null, TargetType.Goal);
//        }
//        else
//        {
//            PlayerAgent.SetTarget(dummy, TargetType.Goal);
//        }
//    }
//}

//private void CheckArenaMission()
//{
//    //if (_mm.ContainAndNotFinishedMission(MissionsTexts.StartingNode_ArenaFight_Name))
//    //{
//    //    if (MetaWorld.Inst.GlobalMessages.ContainsKey("arena"))
//    //    {
//    //        _mm.FinishMission(MissionsTexts.StartingNode_ArenaFight_Name);
//    //    }
//    //}
//}

//private void CheckBluePrintsMission()
//{
//    if (IsFinishBluePrintsMission())
//    {
//        GetBluePrints();
//    }
//}

//private bool IsFinishBluePrintsMission()
//{
//    //return PlayerAgent != null && PlayerAgent.Inventory.CheckForItem(WARP_DRIVE_BLUEPRINTS_ITEM, 1) &&
//    //       _mm.ContainAndNotFinishedMission(MissionsTexts.StartingNode_GetWarpDriveBluePrints_Name);
//    return false;
//}

//private void CheckFederationRelation()
//{
//    float relation = GameEngine.GetFaction(FactionType.Federation).GetRelationToFaction(FactionType.Player);

//    if (_isGoodRelatioWithFederation && relation < 0)
//    {
//        _isGoodRelatioWithFederation = false;
//        _federationBase.RemoveSystem(_arena);

//    }

//    if (!_isGoodRelatioWithFederation && relation >= 0)
//    {
//        _isGoodRelatioWithFederation = true;
//        _federationBase.AddSystem(_arena);
//    }
//}

//private void CheckEmpireRelation()
//{
//    float relation = GameEngine.GetFaction(FactionType.Empire).GetRelationToFaction(FactionType.Player);

//    if (_isGoodRelatioWithEmpire && relation < 0)
//    {
//        _isGoodRelatioWithEmpire = false;

//        foreach (var empireBase in _empireBases)
//        {
//            empireBase.RemoveSystem(_arena);
//        }
//    }

//    if (!_isGoodRelatioWithEmpire && relation >= 0)
//    {
//        _isGoodRelatioWithEmpire = true;

//        foreach (var empireBase in _empireBases)
//        {
//            empireBase.AddSystem(_arena);
//        }
//    }
//}

//private void FadeIn()
//{
//    if (_fadeInTimer > 0)
//    {
//        --_fadeInTimer;
//        fadeAlpha = (_fadeInTimer) / (float)FADE_EFFECT_LENGTH;
//    }
//}

//private void CheckIfLowOnFunds()
//{
//    if (!_isLowMoneyMessageShown && GetPlayerFaction().GetMeter(MeterType.Money).Value < 300)
//    {
//        AddTextBox("If you are low on funds, you may try mining some minerals from the surrounding asteroids.\nShop keepers will buy them from you.",
//            null, null, Consts.INFORMATION_TEXTURE_ID, "lowfunds", true);

//        _isLowMoneyMessageShown = true;
//    }
//}

//private void ShowCraftingMessage()
//{
//    if (!_isCraftingMessageShown && GameEngine.FrameCounter % 60 == 0 && FindPlayer() != null && FindPlayer().GetInventory() != null
//        && FindPlayer().GetInventory().GetItemCount("Hydrogen") > 0)
//    {
//        AddTextBox("You can craft your first item. Open the Crafting menu (C).", null, null, Consts.INFORMATION_TEXTURE_ID, "crafting", true);
//        _isCraftingMessageShown = true;
//    }
//}

//private void GetEmpireReward()
//{
//    _empireBaseDialog.AddText("Captain, we did it. We destroyed the Federation base. \nMy people are safe, thanks to your help.");
//    _empireBaseDialog.AddText("Please take this weapon as a token of our gratitude.");
//    FindPlayer().GetInventory().AddItem("BlackHoleGun");
//    PlayerAgent.SetTarget(null, TargetType.Goal);
//    //_mm.FinishMission(MissionsTexts.StartingNode_ClaimKillFederationReward_Name);
//    GameEngine.GetFaction(FactionType.Empire).ChangeRelationToFaction(GameEngine, FactionType.Player, 2);
//}

//private void GetFederationReward()
//{
//    _federationBaseDialog.AddText("Thank you captain. Now we can defend against the empire attacks.");
//    _federationBaseDialog.AddText("I would like to give you our unique shield, it will push your enemies back when your shield is low.");
//    _federationBaseDialog.AddText("I won't forget your help.");
//    PlayerAgent.SetTarget(null, TargetType.Goal);
//    FindPlayer().GetInventory().AddItem("PushShield1");
//    // _mm.FinishMission(MissionsTexts.StartingNode_ClaimKillEmpireReward_Name);
//    GameEngine.GetFaction(FactionType.Federation).ChangeRelationToFaction(GameEngine, FactionType.Player, 1);
//}

//private void CheckIfEmpireBaseDestroyed()
//{
//    //if (_empireBases.Any(empireBase => empireBase.IsNotActive) && Missions.ContainsKey(MissionsTexts.StartingNode_DestroyEmpire_Name))
//    //{
//    //    if (_mm.ContainMission(MissionsTexts.StartingNode_DestroyEmpire_Name))
//    //    {
//    //        _mm.FinishMission(MissionsTexts.StartingNode_DestroyEmpire_Name);
//    //        _mm.RemoveMission(MissionsTexts.StartingNode_DestroyFederation_Name);
//    //        _mm.AddMission(Missions[MissionsTexts.StartingNode_ClaimKillEmpireReward_Name], true);

//    //        _generalDialog.DefaultPotratitID = Consts.AI_HELPER_TEXTURE_ID;
//    //        _generalDialog.AddTextWithSound("Captain, General Travis is hailing us.", "GeneralTravis");
//    //        _generalDialog.AddTextWithPotratit("That was a mistake, you shouldn't underestimate the might of the Nova empire.", "M_09");
//    //        _generalDialog.AddTextWithSound("Captain, you destroyed an empire base. go back to Admiral Kelor to claim your reward.", "DestroyEmpireBase");

//    //        PlayerAgent.SetTarget(_federationBase, TargetType.Goal);
//    //        _federationBaseDialog.DialogStart += new DialogStartEventHandler(GetFederationReward);
//    //        OneEmpireBaseAttackPlayer();
//    //    }
//    //    else // Player didn't take destroy Empire base.
//    //    {
//    //        _federationBase.RemoveSystem(_federationBaseDialog);
//    //        _mm.RemoveMission(MissionsTexts.StartingNode_VisitFederation_Name);
//    //        _mm.RemoveMission(MissionsTexts.StartingNode_VisitEmpire_Name);
//    //    }

//    //    if (_mm.ContainAndNotFinishedMission(MissionsTexts.StartingNode_VisitEmpire_Name))
//    //    {
//    //        _mm.RemoveMission(MissionsTexts.StartingNode_VisitEmpire_Name);
//    //    }

//    //    if (_mm.ContainAndNotFinishedMission(MissionsTexts.StartingNode_ArenaFight_Name))
//    //    {
//    //        _mm.RemoveMission(MissionsTexts.StartingNode_ArenaFight_Name);
//    //    }

//    //    Missions.Remove(MissionsTexts.StartingNode_DestroyEmpire_Name);
//    //    Missions.Remove(MissionsTexts.StartingNode_DestroyFederation_Name);
//    //}
//}

//private void OneEmpireBaseAttackPlayer()
//{
//    foreach (var empireBase in _empireBases)
//    {
//        if (empireBase != null && empireBase.IsActive && empireBase.GetTarget(GameEngine, TargetType.Goal) == (_federationBase as GameObject))
//        {
//            empireBase.SetTarget(FindPlayer(), TargetType.Goal);
//            break;
//        }
//    }
//}

//private void CheckIfFederationBaseDestroyed()
//{
//    //if (_federationBase.IsNotActive && Missions.ContainsKey(MissionsTexts.StartingNode_DestroyFederation_Name))
//    //{
//    //    if (_mm.ContainMission(MissionsTexts.StartingNode_DestroyFederation_Name))
//    //    {
//    //        _mm.FinishMission(MissionsTexts.StartingNode_DestroyFederation_Name);
//    //        _mm.RemoveMission(MissionsTexts.StartingNode_DestroyEmpire_Name);
//    //        _mm.AddMission(Missions[MissionsTexts.StartingNode_ClaimKillFederationReward_Name], true);

//    //        _generalDialog.DefaultPotratitID = Consts.AI_HELPER_TEXTURE_ID;
//    //        _generalDialog.AddTextWithSound("Captain, Admiral Kelor is hailing us.", "admiralKelor");
//    //        _generalDialog.AddTextWithPotratit("BASTARD, you killed my people! \nI will hunt you down for this.", "F_11");
//    //        _generalDialog.AddTextWithSound("Captain, you destroyed the federation base. go back to General Travis to claim your reward.", "Destroyfederationbase");
//    //        PlayerAgent.SetTarget(_empireBases[0], TargetType.Goal);

//    //        _empireBaseDialog.DialogStart += new DialogStartEventHandler(GetEmpireReward);
//    //    }
//    //    else  // Player didn't take destroy federation base.
//    //    {
//    //        _empireBases[0].RemoveSystem(_empireBaseDialog);
//    //        _mm.RemoveMission(MissionsTexts.StartingNode_VisitFederation_Name);
//    //        _mm.RemoveMission(MissionsTexts.StartingNode_VisitEmpire_Name);
//    //    }

//    //    Missions.Remove(MissionsTexts.StartingNode_DestroyEmpire_Name);
//    //    Missions.Remove(MissionsTexts.StartingNode_DestroyFederation_Name);
//    //}
//}

//private void CheckDestroyPirateLordMission()
//{
//    //if (!_pirateLord.IsActive && _mm.ContainAndNotFinishedMission(MissionsTexts.StartingNode_DestoryPirateLord_Name))
//    //{
//    //    AddTextBox("Master, you destroyed the mighty Pirate Lord! HURRAY! \nNow let's get back to the #color{255,255,0}trading guild#defalutColor{} and get the star drive blueprints.", isBlocking: true, image: Consts.AI_HELPER_TEXTURE_ID, id: "PirateLordDead", soundID: "DestoryPirateLord");
//    //    _mm.FinishMission(MissionsTexts.StartingNode_DestoryPirateLord_Name);
//    //    _mm.AddMission(Missions[MissionsTexts.StartingNode_GetWarpDriveBluePrints_Name], true);

//    //    _tradingGuildDialog.AddText("Great news everyone! Pierce is out of business. \nThank you for your help, here are the blueprints.");
//    //    _tradingGuildDialog.DialogStart += new DialogStartEventHandler(AddTradingPostReward);
//    //    //TODO: maybe add sale sign next to the shop
//    //}
//}

//        //private void InitStateMachine()
//        //{
//        //    //add opening states, camera moving

//        //    //_lsm.AddState(this,
//        //    //  (int time) =>
//        //    //  {
//        //    //      SetText("Press Space to continue");
//        //    //      var res = AddTextBox("AI Matrix reconstruction completed with errors.", null, null, Consts.COMMUNICATION_CONSOLE_TEXTURE_ID, "LoadingComplete", true);
//        //    //      if (time == 1)
//        //    //      {
//        //    //          MusicEngine.Instance.PlaySong(MusicEngine.STARTING_NODE_BEGINING_SONG);
//        //    //      }
//        //    //      return res == null;
//        //    //  });

//        //    /*_lsm.AddState(this,
//        //    (int time) =>
//        //    {
//        //        SetText("");
//        //        AddStartingAiHelperText();
//        //        PlayerAgent.SetTarget(_starPort, TargetType.Goal);
//        //        return true;
//        //    });*/
//        //  //   ^ moved to OnEnter

//        //    _lsm.AddState(this,
//        //        (int time) =>
//        //        {
//        //            bool hasLyle = PlayerAgent.GetInventory().CheckForItem(typeof(LyleDriveItem).Name, 1);
//        //            bool hasFlux = PlayerAgent.GetInventory().CheckForItem(typeof(FluxCapacitorItem).Name, 1);
//        //            bool hasDiamonds = PlayerAgent.GetInventory().CheckForItem("DiamondItem", 4);
//        //            bool hasCore = PlayerAgent.GetInventory().CheckForItem("Germanium", 1);
//        //            bool hasBlueprint = PlayerAgent.GetInventory().CheckForItem("WarpDriveBlueprints", 1);

//        //            if (hasLyle & hasCore & hasDiamonds & hasDiamonds & hasFlux & hasBlueprint)
//        //                return 1;
//        //            return 0;
//        //        });
//        //    _lsm.AddState(this,
//        //        (int time) =>
//        //        {
//        //            if (time == 1)
//        //            {
//        //                AgentDialogSystem endingDialog = new AgentDialogSystem();
//        //                endingDialog.DefaultPotratitID = Consts.AI_HELPER_TEXTURE_ID;
//        //                endingDialog.Range = 100000;
//        //                endingDialog.AddTextWithSound("Well done! You have everything you need for the Warp Drive", "BuildWarpdrive");
//        //                endingDialog.AddTextWithPotratit("Enter the #color{255,255,0}crafting menu#defalutColor{} and create the warp drive.", Consts.INFORMATION_TEXTURE_ID);
//        //                _mothership.AddSystem(endingDialog);
//        //            }

//        //            return true;
//        //        });
//        //    _lsm.AddState(this,
//        //        (int time) =>
//        //        {
//        //            if (PlayerAgent.GetInventory().CheckForItem("WarpDrive", 1))
//        //                return 1;
//        //            return 0;
//        //        });
//        //    _lsm.AddState(this,
//        //        (int time) =>
//        //        {
//        //            if (time == 1)
//        //            {
//        //                _generalDialog.DefaultPotratitID = Consts.INFORMATION_TEXTURE_ID;
//        //                _generalDialog.AddTextWithSoundAndPotrait("Congratulations! You built the warp drive!", "BuiledWarpDrive", Consts.AI_HELPER_TEXTURE_ID);
//        //                _generalDialog.AddText("The warp drive will allow you to travel to new solar systems in the galaxy, \n" +
//        //                "but unfortunately they aren't part of the demo.\n" +
//        //                "You may roam the solar system freely.\n" +
//        //                "To be continued...");
//        //                _generalDialog.AddText("Credits:\nProgramming - Kobi Nistel, Yaniv Kahana & Ron Lange\nArt - Silviu Ploisteanu\n" +
//        //                                       "Additional programming: Yochai Gani\n" +
//        //                                       "Additional help: Sally Halon & Richard Halon & Nadav Etinzon & Avihay Baratz & Eran Rothfeld\n" +
//        //                                       "Music: Chill Carrier from the album \"Back In The Days\"");
//        //                _mm.FinishMission(MissionsTexts.StartingNode_FixWarpDrive_Name);
//        //            }

//        //            AddTextBox("Congratulations, you built the warp drive!\n"
//        //                , null, null, Consts.AI_HELPER_TEXTURE_ID, "BuildWarpDrive", true);

//        //            var res = AddTextBox("The warp drive will allow you to travel to new solar systems in the galaxy, \n" +
//        //                "but unfortunately they aren't part of the demo.\n" +
//        //                "You may roam the solar system freely. \n" +
//        //                "To be continued..."
//        //                , null, null, Consts.INFORMATION_TEXTURE_ID, "useit", true);

//        //            if (res == null)
//        //            {
//        //                AddTextBox("Credits:\nProgramming - Kobi Nistel, Ron Lange & Yaniv Kahana\nArt - Silviu Ploisteanu", null, null, Consts.INFORMATION_TEXTURE_ID, "credits", true); // kobi: why they appear togather?
//        //            }


//        //            // yaniv: should we remove it
//        //            //var res = AddTextBox("You're not a very good tech, are you? You managed to make a broken Warp Drive.\n" +
//        //            //    "It seems there's only 1 destination this will take you to. Luckily for you - it's the Delphi star system!\n" +
//        //            //    "Click F2 to access your inventory, put the Warp Drive in the first slot and press Escape.\n" +
//        //            //    "Then click \"1\" to activate your first quickslot item.", null, null, Consts.AI_HELPER_TEXTURE_ID, "useit", true);
//        //            return res == null;
//        //            return 1;
//        //        });
//        //}
//    }
//}
