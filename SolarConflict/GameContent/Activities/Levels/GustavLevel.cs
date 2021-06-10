//using Microsoft.Xna.Framework;
//using SolarConflict.Framework;
//using SolarConflict.GameContent;
//using SolarConflict.GameContent.Agents;
//using SolarConflict.GameContent.Items;
//using SolarConflict.Session.World.MissionManagment;
//using System;
//using System.Collections.Generic;
//using XnaUtils;

//namespace SolarConflict.GameContent.Activities
//{
//    [Serializable]
//    class GustavLevel : Scene
//    {
//        private readonly string COMMUNICATION_CHIP_ITEM = "Communication Chip";
//        private readonly int MONEY_REWARD = 500;

//        private Agent _ally;
//        private GameObject _enemy1;
//        private GameObject _enemy2;
//        private GameObject _enemy3;
//        private GameObject _playerShip;
//        private Agent _gustav;
//        private bool _levelDone = false;
//        private bool _isVisitedShop = false;
//        private Agent _shop;
//        private Agent _portal;
//        private AgentDialogSystem _shopDialog;
//        private AgentDialogSystem _allyDialog;
//        private MissionManager _mm = null;// MetaWorld.Inst.MissionManager;

//        public GustavLevel(string parameters)
//            : base(parameters, true)
//        {

//        }

      

//        public override void InitScript(string parameters, ActivityParameters activityParameters = null)
//        {
//            CreateSpeedBoosters();
//            //CreateShop();
//            CreateGustav();
//            CreateAllyShip();
//            CreateEnemies();
//         //   _portal = CreatePortal();

//            this.AddObjectRandomlyInCircle("Asteroid", 1000, 13500, 12500);
//        }

//        #region Gustav

//        private void AddGustavTexts()
//        {
//            AgentDialogSystem gustavDialog = new AgentDialogSystem();
//            gustavDialog.DefaultPotratitID = "M_13";
//            gustavDialog.Range = 1200;
//            gustavDialog.AddText("Fresh meat!!"); // yaniv: to add more text
//            gustavDialog.DialogStart += new DialogStartEventHandler(MeetGustav);
//            _gustav.AddSystem(gustavDialog);
//        }

//        private void MeetGustav()
//        {
//            _playerShip.SetTarget(null, TargetType.Goal);
//        }

//        private void CreateGustav()
//        {
//            _gustav = AddGameObject("PirateKing2", new Vector2(10000, -5000), 180, FactionType.Pirates1, AgentControlType.AI) as Agent;
//            _gustav.SetAggroRange(1400, 3000, TargetType.Enemy);
//            _gustav.AddItemToInventory(typeof(LyleDriveItem).Name);
//          //  _gustav.AddItem((Item)ContentBank.Inst.GetItem(typeof(MultiMissileLauncher).Name).MakeGameObject(this.GameEngine));
//            AddGustavTexts();
//        }

//        private void CheckIfGustavAlive()
//        {
//            if (!_gustav.IsActive && !_levelDone)
//            {
//                var miningLaser = AddGameObject("LyleDriveItem", _gustav.Position, 0, FactionType.Neutral, AgentControlType.None);
//                _playerShip.SetTarget(_shop, TargetType.Goal);
//                _shopDialog.DialogStart -= new DialogStartEventHandler(AddGustavAliveShopText);
//                _shopDialog.DialogStart += new DialogStartEventHandler(AddGustavDeadShopText);
//                _levelDone = true;
//            }
//        }

//        #endregion


//        #region Shop

//        //private void CreateShop()
//        //{
//        //    Vector2 shopLocation = new Vector2(6000, 1400);
//        //    _shop = (Institute)AddGameObject(typeof(Shop).Name, 0, shopLocation, 0, AgentControlType.AI);
//        //    _shop.range = -1000; // not accessible

//        //    ShopActivity shopActivity = new ShopActivity(this);
//        //    shopActivity.ShopName = "Spicy Space shop";
//        //    shopActivity.AddItems(new List<string>() { "AutoRepairKit", "AutoEnergyKit", "PhoenixDeviceItem", COMMUNICATION_CHIP_ITEM, "SigmaBattery",
//        //    "Generator1", "Generator3", "Generator6", "Generator2",  "Generator5", "Generator7"});

//        //    _shopDialog = new AgentDialogSystem();
//        //    _shopDialog.DefaultPotratitID = "Gustav_Shop";
//        //    _shopDialog.AddText("Hello traveler.\nYou look like you could use some upgrades to your ship.");
//        //    _shopDialog.DialogStart += new DialogStartEventHandler(AddGustavAliveShopText);

//        //    _shop.AddSystem(_shopDialog);
//        //    _shop.TargetActivity = shopActivity;
//        //    _shop.Message = "Press F to trade";
//        //}

//        private void AddGustavDeadShopText()
//        {
//            //_shop.range = 150;

//            if (_isVisitedShop)
//            {
//                _shopDialog.AddText("I've heard Gustav is...out of the picture. That's one way to convince him.\nSo, what are you looking for?"); //Please feel free to browse my wares.\n");
//                _shopDialog.AddTextWithPotratit("I need to repair my communication system. \nCan you help me?", Consts.COMMUNICATION_CONSOLE_TEXTURE_ID);
//                _shopDialog.AddText("Sure, you probably need a new #color{255,255,0}communication chip#defalutColor{} which I have in my store.\nIf it won't help let me know."); //Please feel free to browse my wares.\n");
//            }
//            else
//            {
//                _shopDialog.AddText("I can only sell to anyone Gustav the Pirate says I can sell to.");
//                _shopDialog.AddText("But I've heard Gustav is...out of the picture. You didn't have anything to do with it right?\nSo, what are you looking for?");
//                _shopDialog.AddTextWithPotratit("I need to repair my communication system. \nCan you help me?", Consts.COMMUNICATION_CONSOLE_TEXTURE_ID);
//                _shopDialog.AddText("Sure, you probably need a new #color{255,255,0}communication chip#defalutColor{} which I have in my store. If it won't help let me know."); //Please feel free to browse my wares.\n");
//            }

//            _mm.FinishMission(MissionsTexts.StartingNode_Gustav_Name);

//            if (_ally.IsActive)
//            {
//                _shopDialog.AddText("Here's a hot tip for ya - a lone spacer has been \nspotted loitering about near the asteroids\n" +
//                  "If you have time and the inclination you can go have a chat with him.\nOr, you know, blow him to smithereens.");
//                _playerShip.SetTarget(_ally, TargetType.Goal);
//            }
//        }

//        private void AddGustavAliveShopText()
//        {
//            _shopDialog.AddText("Unfortunately for you, " +
//                    "I can only sell to anyone Gustav the Pirate says I can sell to.\nAnd he didn't mention you.\nGo change his mind, then we'll talk.\n" +
//                    "I'll send you his location.");
//            _shopDialog.AddText("Return to me once you've convinced Gustav.");
//            _playerShip.SetTarget(_gustav, TargetType.Goal);
//            _isVisitedShop = true;
//        }
//        #endregion


//        #region Ally

//        private void AllyReward()
//        {
//            string killFleetText = string.Empty;

//            if (!IsShipSwitchable)
//            {
//                killFleetText = _gustav.IsActive ? "You killed his fleet. " : "You also killed his fleet? ";
//            }
//            _allyDialog.Range = 10000;
//            _allyDialog.AddText(killFleetText + "WOW! Here, take this money and buy something nice for your AI Helper.");
//            _allyDialog.AddTextWithSoundAndPotrait("At least someone care about me!", "careaboutme", Consts.AI_HELPER_TEXTURE_ID);
//            _allyDialog.AddTextWithPotratit("I don't remember everything, but I do remember what happened last time you complained.", Consts.COMMUNICATION_CONSOLE_TEXTURE_ID);
//            _allyDialog.AddTextWithSoundAndPotrait("No No, I'll mute, just don't shut me down!", "mute", Consts.AI_HELPER_TEXTURE_ID);
//            _allyDialog.AddText("I transferred the money. Hope to see you again.");
//            MetaWorld.Inst.GetFaction(FactionType.Player).GetMeter(MeterType.Money).Value += MONEY_REWARD;
//            IsShipSwitchable = false;

//            var goal = _gustav.IsActive ? _gustav : _portal;
//            _playerShip.SetTarget(goal, TargetType.Goal);
//        }

//        private void MeetAlly()
//        {
//            _allyDialog.AddText("Greetings!\nI've been drifting out here, not knowing what to do \never since Gustav's boys wiped out my whole crew.");

//            if (!_gustav.IsActive)
//            {
//                _allyDialog.AddTextWithPotratit("I destroyed him.", Consts.COMMUNICATION_CONSOLE_TEXTURE_ID);
//                _allyDialog.AddText("You killed Gustav! Thank you.");
//            }

//            if (_enemy1.IsActive || _enemy2.IsActive || _enemy3.IsActive)
//            {
//                _allyDialog.AddText("You seem to be packing a serious punch. \nI'd be extremely grateful if you could help me get some payback with his remaining fleet.\n" +
//                      "I'll even make it worth your while. What do you say?");

//                _allyDialog.AddTextWithPotratit("Let's do it!", Consts.COMMUNICATION_CONSOLE_TEXTURE_ID); // TODO: add option to choose
//                _allyDialog.AddText("Excellent, let's fight them.");
//                //_allyDialog.AddTextWithPotratit("Press Tab to switch between your ships", Consts.INFORMATION_TEXTURE_ID);
//                IsShipSwitchable = true;
//                _playerShip.SetTarget(_enemy1, TargetType.Goal);
//                _ally.SetTarget(_enemy1, TargetType.Enemy);
//                _ally.SetAggroRange(20000, 20000, TargetType.Enemy);
//            }
//            else
//            {
//                AllyReward();
//            }
//        }

//        private void CreateAllyShip()
//        {
//            _ally = (Agent)AddGameObject("AllyShip1", FactionType.Player, new Vector2(500, 8000), 0);
//            (_ally as Agent).isControllable = false;

//            _allyDialog = new AgentDialogSystem();
//            _allyDialog.DefaultPotratitID = "Gustav_Ally";

//            _allyDialog.DialogStart += new DialogStartEventHandler(MeetAlly);
//            _ally.AddSystem(_allyDialog);
//        }

//        #endregion

//        private void CreateEnemies()
//        {
//            _enemy1 = AddGameObject("PirateLord1", new Vector2(-6600, 3700), 0, FactionType.Pirates1);
//            _enemy1.SetAggroRange(2000, TargetType.Enemy);
//            _enemy1.AddItemToInventory((Item)ContentBank.Inst.GetItem(typeof(LyleDriveItem).Name).MakeGameObject(this.GameEngine));

//            _enemy2 = AddGameObject("SmallShip4A", new Vector2(-6700, 3700), 0, FactionType.Pirates1);
//            _enemy2.SetAggroRange(2000, TargetType.Enemy);
//            _enemy2.AddItemToInventory((Item)ContentBank.Inst.GetItem(typeof(LyleDriveItem).Name).MakeGameObject(this.GameEngine));

//            _enemy3 = AddGameObject("SmallShip4A", new Vector2(-6700, 3800), 0, FactionType.Pirates1);
//            _enemy3.SetAggroRange(2000, TargetType.Enemy);
//           // _enemy3.AddItem((Item)ContentBank.Inst.GetItem(typeof(KineticMineLauncher).Name).MakeGameObject(this.GameEngine));
//        }

//        private void CreateSpeedBoosters()
//        {
//            AddGameObject("SpeedBooster", 0, FMath.ToCartesian(300, 0.0f), MathHelper.ToDegrees(0));
//            AddGameObject("SpeedBooster", 0, new Vector2(1300, -150), -25);
//            AddGameObject("SpeedBooster", 0, new Vector2(2300, -270), 10);
//            AddGameObject("SpeedBooster", 0, new Vector2(3300, -180), 25);
//            AddGameObject("SpeedBooster", 0, new Vector2(4300, 250), 80);
//            AddGameObject("SpeedBooster", 0, new Vector2(4600, 1400), 0);
//        }

//        public override int UpdateScript(InputState inpuState)
//        {
//            base.UpdateScript(inpuState);

//            CheckCommunicationMission();
//            CheckIfGustavAlive();

//            if (_enemy1.IsNotActive && _enemy2.IsNotActive && _enemy3.IsNotActive && _ally.IsActive && IsShipSwitchable)
//            {
//                AllyReward();
//            }

//            if (_ally.IsActive && IsShipSwitchable && _ally.GetControlType() == AgentControlType.None)
//            {
//                _ally.SetControlType(AgentControlType.AI);
//            }


//            return 0;
//        }

//        private void CheckCommunicationMission()
//        {
//            if (IsFinishCoomunicationMission())
//            {
//                AgentDialogSystem communicationFixedDialog = new AgentDialogSystem();
//                communicationFixedDialog.DefaultPotratitID = Consts.AI_HELPER_TEXTURE_ID;
//                _shop.AddSystem(communicationFixedDialog);
//                communicationFixedDialog.AddTextWithSound("Excellent! I can fix the communication system with this chip.", "FixChip");
//                communicationFixedDialog.AddTextWithSound("Decrypting message from the Void.", "Decrypt");
//                communicationFixedDialog.AddTextWithPotratit("The Zigma sun was destroyed using our new prototype. \nIt will change the balance of power. \nAll ships, report immediately at the rendezvous solar system.", Consts.COMMUNICATION_CONSOLE_TEXTURE_ID);
//                communicationFixedDialog.AddTextWithSound("You heard that! Your test changed the balance of power. \nMaybe you will even get promoted! \nJust don't forget who served you loyally, when you get to the top.", "heardthat");
//                communicationFixedDialog.AddTextWithSound("But we need to fix our #color{255,255,0}star drive#defalutColor{} to get there.", "fixdrive");

//                _mm.FinishMission(MissionsTexts.StartingNode_FixCommunication_Name);
//                PlayerAgent.Inventory.RemoveItem(COMMUNICATION_CHIP_ITEM);
//                if (!PlayerAgent.Inventory.CheckForItem("WarpDrive", 1))
//                {
//              //      _mm.AddMission(new Mission(MissionsTexts.StartingNode_FixWarpDrive_Name, MissionsTexts.StartingNode_FixWarpDrive_Description, SceneID));
//                }
//            }
//        }

//        private bool IsFinishCoomunicationMission()
//        {
//            throw new NotImplementedException();
//            /*
//            return PlayerAgent != null && PlayerAgent.Inventory.CheckForItem(COMMUNICATION_CHIP_ITEM, 1) &&
//                   _mm.ContainAndNotFinishedMission(MissionsTexts.StartingNode_FixCommunication_Name);*/
//        }

//        public override ActivityParameters OnLeave()
//        {
//            ActivityParameters res = new ActivityParameters();
//            res.ParamDictionary.Add("gustavdone", _levelDone.ToString());
//            return res;
//        }

//        public override void OnEnter(ActivityParameters parameters)
//        {
//            base.OnEnter(parameters);
//            ActionOnPlayerDeath = ActionOnPlayerDeathType.Back;
//            _playerShip = AddGameObject("Player", FactionType.Player, Vector2.Zero, 0, AgentControlType.Player);

//            SetGoal();
//        }

//        private void SetGoal()
//        {
//            //GameObject goal = null;

//            //if (_isVisitedShop)
//            //{
//            //    if (_gustav.IsActive)
//            //    {
//            //        goal = _gustav;
//            //    }
//            //    else if (_mm.ContainAndNotFinishedMission(MissionsTexts.StartingNode_FixCommunication_Name))
//            //    {
//            //        goal = _shop;
//            //    }
//            //    else
//            //    {
//            //        goal = _portal;
//            //    }
//            //}
//            //else
//            //{
//            //    goal = _shop;
//            //}

//            //_playerShip.SetTarget(goal, TargetType.Goal);
//        }

//        public override ActivityParameters OnBack()
//        {
//            GameEngine.RemoveGameObject(_playerShip);
//            _playerShip.SetTarget(null, TargetType.Goal);
//            return base.OnBack();
//        }

//        public static Activity ActivityProvider(string parameters)
//        {
//            return new GustavLevel(parameters);
//        }
//    }
//}
