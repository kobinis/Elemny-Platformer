//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;
//using SolarConflict.Framework;
//using SolarConflict.Framework.CameraControl.Zoom;
//using SolarConflict.Framework.Logger;
//using SolarConflict.GameContent.Agents;
//using SolarConflict.GameContent.Emitters;
//using SolarConflict.GameContent.Items;
//using SolarConflict.GameContent.NewItems;
//using SolarConflict.GameContent.NewItems.Utils;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics.Contracts;
//using XnaUtils;

//namespace SolarConflict.GameContent.Activities
//{
//    [Serializable]
//    public class PromoLevel : Scene
//    {
//        #region Constants

//        private const float SCENE_WORLD_SCALE = 3;
//        private readonly int START_PLAYER_LOCATION_X = 0;
//        private const int START_PLAYER_LOCATION_Y = 5000;
//        private const float SCALE_DISTANCE_FROM_SUN = 0.7f;
//        private readonly int FADE_EFFECT_LENGTH = 60 * 3;
//        private readonly Vector2 PLAYER_START_POSITION = Vector2.UnitY * 7060;

//        #endregion


//        #region Fields

//        private Vector2[] _shipGroupsLocationsInWorld = { Vector2.UnitY, Vector2.UnitX, Vector2.UnitX * -1, Vector2.UnitY * -1,
//                                                          Vector2.UnitX * SCALE_DISTANCE_FROM_SUN + Vector2.UnitY * SCALE_DISTANCE_FROM_SUN,
//                                                          Vector2.UnitX * -SCALE_DISTANCE_FROM_SUN + Vector2.UnitY * -SCALE_DISTANCE_FROM_SUN,
//                                                          Vector2.UnitX * SCALE_DISTANCE_FROM_SUN + Vector2.UnitY * -SCALE_DISTANCE_FROM_SUN,
//                                                          Vector2.UnitX * -SCALE_DISTANCE_FROM_SUN + Vector2.UnitY * SCALE_DISTANCE_FROM_SUN };

//        private Vector2[] _locationInShipGroup = { Vector2.UnitX, Vector2.UnitY, Vector2.UnitX * -1, Vector2.UnitY * -1, Vector2.Zero };
//        private string[] _enemyShipNamesInnerRing = { "PrologueEnemy1", "PrologueEnemy1", "PrologueEnemy1", "PrologueEnemy1", typeof(SolarMiner).Name };
//        private string[] _enemyShipNamesOuterRing = { "PrologueEnemy1", "PrologueEnemy1", "PrologueEnemy1", "PrologueEnemy1", "PrologueEnemy1" }; // kobi: add more types of federation ships 
//        private List<GameObject> _motherships;

//        private DummyObject _kemronGoal = new DummyObject();
//        private bool _isAtWar = false;
//        private int _endLevelTimer = 0;
//        private GameObject _sun;
//        private LevelStateMachine _levelStateMachine;
//        private GameObject _kemron;
//        private int _playerAnswer;
//        private bool _destroySunVoiceTextShown = false;
//        private Agent _player;
//        private bool _playerAlreadyIngnoredOnce = false;
//        private string _shipName;

//        #endregion

//        public PromoLevel(string parameters = null)
//            : base(parameters)
//        {
//            _shipName = parameters;
//        }

//        public static Activity ActivityProvider(string parameters)
//        {
//            return new PromoLevel(parameters);
//        }


//        public override void InitScript(string parameters, ActivityParameters activityParameters = null)
//        {
//            //Contract.Requires(parameters != null);            
//            _levelStateMachine = new LevelStateMachine();
//            IsConfirmQuitNeeded = true;
//            _motherships = new List<GameObject>();

//            AddGameObjects();

//            InitStateMachine();

//            GameEngine.Update();

            

//            SetCameraFromSunToShip();
//        }

//        #region Private Methods 

//        private void SetCameraFromSunToShip()
//        {
//            CameraManager.MovmentType = CameraMovmentType.ToTarget;
//            CameraManager.CameraMovmentAcceleration = 1f;
//            CameraManager.CameraMovmentFactor = 0f;
//            CameraManager.CameraMovmentSpeed = 0;
//            CameraManager.TargetPosition = PLAYER_START_POSITION;
//            CameraManager.ZoomType = CameraZoomType.ToTargetZoom;
//            CameraManager.TargetZoom = 0.08f;
//         //   ManualZoom._zoom = 0;

//            Camera.Zoom = 1f;
//        }
        

//        private void InitOuterRingShips(int groupLocationIndex, float shipRotation)
//        {
//            for (int shipNameIndex = 0; shipNameIndex < _enemyShipNamesOuterRing.Length; shipNameIndex++)
//            {
//                var shipWorldLocation = _shipGroupsLocationsInWorld[groupLocationIndex] * 6500 * SCENE_WORLD_SCALE;
//                var shipGroupLocation = _locationInShipGroup[shipNameIndex] * 200;
//                GameObject enemyShip = AddGameObject(_enemyShipNamesOuterRing[shipNameIndex], shipWorldLocation + shipGroupLocation, MathHelper.ToDegrees(shipRotation), FactionType.Federation);
//                if (groupLocationIndex == 0)
//                    enemyShip.SetTarget(_kemron, TargetType.Goal);
//            }
//        }

//        private void InitInnerRingShips(int groupLocationIndex, float shipRotation)
//        {
//            for (int shipNameIndex = 0; shipNameIndex < _enemyShipNamesInnerRing.Length; shipNameIndex++)
//            {
//                var shipWorldLocation = _shipGroupsLocationsInWorld[groupLocationIndex] * 3000 * SCENE_WORLD_SCALE;
//                var shipGroupLocation = _locationInShipGroup[shipNameIndex] * 200;

//                FactionType shipFaction = FactionType.Federation;

//                if (_enemyShipNamesInnerRing[shipNameIndex] == typeof(SolarMiner).Name)
//                {
//                    shipFaction = FactionType.Neutral; // Solar miner explosion will also damage his allies.
//                }

//                GameObject enemyShip = AddGameObject(_enemyShipNamesInnerRing[shipNameIndex], shipWorldLocation + shipGroupLocation, MathHelper.ToDegrees(shipRotation), shipFaction);
//                if (enemyShip.GetId() == "Base") //TODO: fix
//                {
//                    _motherships.Add(enemyShip);
//                }
//            }
//        }

//        #endregion


//        #region Game Objects

//        private void AddGameObjects()
//        {
//            var firstTargetLocation = new Vector2(START_PLAYER_LOCATION_X + 300, START_PLAYER_LOCATION_Y + 300) * SCENE_WORLD_SCALE;

//            _sun = AddGameObject("SunWithBackground", Vector2.Zero);
//            AddAsteroids();
//        }

//        private void AddShip()
//        {
//            _shipName = "PrologueShip1";
//            GameObject playerShip = AddGameObject(_shipName, PLAYER_START_POSITION, -90, FactionType.Player, AgentControlType.Player);
//            playerShip.AddItemToInventory((Item)ContentBank.Inst.GetGameObjectFactory(typeof(StarDestroyerItem).Name).MakeGameObject(this.GameEngine));
//            //playerShip.AddItemToInventory((Item)ContentBank.Inst.GetGameObjectFactory(typeof(VacuumModulatorItem).Name).MakeGameObject(this.GameEngine));

//            // Add PhoenixDeviceItem
//            Item item = (Item)ContentBank.Inst.GetGameObjectFactory(typeof(PhoenixDeviceItem).Name).MakeGameObject(this.GameEngine);
//            item.Stack = 15;
//            playerShip.AddItemToInventory(item);

//            // Add AutoRepairKit
//            item = (Item)ContentBank.Inst.GetGameObjectFactory(typeof(RepairKit1).Name).MakeGameObject(this.GameEngine);
//            item.Stack = 10;
//            playerShip.AddItemToInventory(item);

//           // playerShip.SetMeterValue(MeterType.StunTime, 50);

//            //AddGameObject("SunFullscreenColorFx", Vector2.Zero);
//            // HyperSpace jump effect
//            AddGameObject(typeof(HyperSpaceJumpFx).Name, playerShip.Position);
//        }

//        private void AddAsteroids()
//        {
//            //this.AddObjectRandomlyInCircle("TestAsteroid", 800, 10000 * SCENE_WORLD_SCALE, 2000 * SCENE_WORLD_SCALE);
//            this.AddObjectRandomlyInCircle("BigAsteroid", 100, 15000 , 6000 );
//            this.AddObjectRandomlyInCircle("Asteroid", 100, 15000 , 6000 );
//          //  this.AddObjectRandomlyInCircle("LavaAsteroid", 300, _sun.Size * 2.5f, _sun.Size * 1.2f);
//        }

//        #endregion      


//        #region States

//        private LevelStateMachine.StateOutput MoveCameraToPlayer_State(int time)
//        {
//            if (time == 1)
//            {
//                MusicEngine.Instance.PlaySong(MusicEngine.PROLOGUE_BEGINING_SONG);
//            }

//            if (CameraManager.IsCameraOnTarget(Camera))
//            {
//                AddShip();

//                CameraManager.MovmentType = CameraMovmentType.OnPlayer;
//                CameraManager.TargetZoom = 1;
//                CameraManager.ZoomType = CameraZoomType.Manual; //  CameraZoomType.ToTargetZoom;
//                return true;
//            }

//            return false;
//        }

//        private LevelStateMachine.StateOutput Awaken_State(int time)
//        {
//            //string id = AddTextBox(PlotTexts.Tutorial1Awaken, image: Consts.COMMUNICATION_CONSOLE_TEXTURE_ID, id: "void", soundID: "awakenAI");
//            //string id = AddTextBox("Awaken, time is of the essence", image: null, id: "void");

//            return true;
//        }

//        private LevelStateMachine.StateOutput MoveMouse_State(int time)
//        {
//            return true;
//        }

//        //private LevelStateMachine.StateOutput Zoom_State(int time)
//        //{
//        //    bool next = false;

//        //    if (time == 1)
//        //    {
//        //        AddTextBox(PlotTexts.Tutorial3Zoom, Palette.TextBoxFont, id: "ZoomText", time: 250);
//        //    }

//        //    if (!dialogManager.IsDialogUp("ZoomText"))
//        //    {
//        //        next = true;
//        //    }       

//        //    return next;
//        //}

//        private LevelStateMachine.StateOutput Speed_State(int time)
//        {

//            return true;
//        }

//        private LevelStateMachine.StateOutput MoveToSun_State(int time)
//        {
//            return true;
//        }

       

//        private LevelStateMachine.StateOutput PlayerRespond_State(int time)
//        {
//            IList<string> playerOptions = new List<string>();
//            playerOptions.Add(PlotTexts.Tutorial11PlayerRespondKemron1);
//            playerOptions.Add(PlotTexts.Tutorial12PlayerRespondKemron2);
//            playerOptions.Add(PlotTexts.Tutorial13PlayerRespondKemron3);
//            playerOptions.Add(PlotTexts.Tutorial14PlayerRespondKemron4);

//            int answer = DialogManagerOld.AddQuestionBox(null, playerOptions);

//            _playerAnswer = answer;

//            return answer != 0;
//        }

//        private LevelStateMachine.StateOutput KemronRespond_State(int time)
//        {
//            Contract.Requires(_playerAnswer >= 0 && _playerAnswer < 4);
//            string ID = string.Empty;
//            Faction fedFaction;

//            switch (_playerAnswer)
//            {
//                case 1:
//                    ID = AddTextBox(PlotTexts.Tutorial15KemronRespond1, id: "KemronRespond1ID", image: "Prologue_CaptainKemron", isBlocking: true, isSkipable: true);
//                    //ID = AddTextBox(PlotTexts.Tutorial15KemronRespond1, id: "KemronRespond1ID", image: "Prologue_CaptainKemron", soundID: "KemronAnswer1", isBlocking: true, isSkipable: true);
//                    fedFaction = base.GameEngine.GetFaction(FactionType.Federation);
//                    fedFaction.ChangeRelationToFaction(GameEngine, FactionType.Player, -0.1f);
//                    break;

//                case 2:
//                    ID = AddTextBox(PlotTexts.Tutorial16KemronRespond2, id: "KemronRespond2ID", image: "Prologue_CaptainKemron", isBlocking: true, isSkipable: true);
//                    //ID = AddTextBox(PlotTexts.Tutorial16KemronRespond2, id: "KemronRespond2ID", image: "Prologue_CaptainKemron", soundID: "KemronAnswer2", isBlocking: true, isSkipable: true);
//                    break;

//                case 3:
//                    //ID = AddTextBox(PlotTexts.Tutorial17KemronRespond3, id: "KemronRespond3ID", image: "Prologue_CaptainKemron", soundID: "KemronAnswer3", isBlocking: true, isSkipable: true);
//                    ID = AddTextBox(PlotTexts.Tutorial17KemronRespond3, id: "KemronRespond3ID", image: "Prologue_CaptainKemron", isBlocking: true, isSkipable: true);
//                    fedFaction = base.GameEngine.GetFaction(FactionType.Federation);
//                    fedFaction.ChangeRelationToFaction(GameEngine, FactionType.Player, -0.1f);
//                    break;

//                case 4:
//                    if (!_playerAlreadyIngnoredOnce)
//                    {
//                        ID = AddTextBox(PlotTexts.Tutorial18KemronRespond4, id: "KemronRespond4ID", image: "Prologue_CaptainKemron", isBlocking: true, isSkipable: true);
//                        //ID = AddTextBox(PlotTexts.Tutorial18KemronRespond4, id: "KemronRespond4ID", image: "Prologue_CaptainKemron", soundID: "KemronAnswer4", isBlocking: true, isSkipable: true);

//                        if (ID == null)
//                        {
//                            _playerAlreadyIngnoredOnce = true;
//                            _playerAnswer = 0;
//                            return "PlayerRespond_State";
//                        }
//                    }
//                    else
//                    {
//                        ID = AddTextBox(PlotTexts.Tutorial17KemronRespond3, id: "KemronRespond3ID", image: "Prologue_CaptainKemron", isBlocking: true, isSkipable: true);
//                        //ID = AddTextBox(PlotTexts.Tutorial17KemronRespond3, id: "KemronRespond3ID", image: "Prologue_CaptainKemron", soundID: "KemronAnswer3", isBlocking: true, isSkipable: true);
//                        fedFaction = base.GameEngine.GetFaction(FactionType.Federation);
//                        fedFaction.ChangeRelationToFaction(GameEngine, FactionType.Player, -0.1f);
//                    }

//                    break;
//            }

//            return ID == null;
//        }

//        private LevelStateMachine.StateOutput Stun_State(int time)
//        {
//            return true;
//        }

//        private LevelStateMachine.StateOutput DestroySunVoice_State(int time)
//        {
//            bool next = false;

//            bool killedMothership = false;
//            foreach (GameObject mothership in _motherships)
//            {
//                if (mothership.IsNotActive)
//                {
//                    killedMothership = true;
//                }
//            }

//            if (killedMothership)
//            {
//                bool isCloseToTarget = (_player.Position - _sun.Position).Length() < 8000;

//                if (isCloseToTarget && !_destroySunVoiceTextShown)
//                {
//                    AddTextBox(PlotTexts.Tutorial20DestroySunVoice, Palette.TextBoxFont, id: "DestroySunVoiceTextID", image: Consts.COMMUNICATION_CONSOLE_TEXTURE_ID, soundID: "DestroySunAI", time: 300);
//                    _destroySunVoiceTextShown = true;
//                }
//            }

//            if (_destroySunVoiceTextShown && !DialogManagerOld.IsDialogUp("DestroySunVoiceTextID"))
//            {
//                next = true;
//            }

//            return next;
//        }

//        private LevelStateMachine.StateOutput DestroySun_State(int time)
//        {
//            if (time == 1)
//            {
//                AddTextBox(PlotTexts.Tutorial21DestroySunExplanation, Palette.TextBoxFont, id: "DestroySunTextID", time: 250, image: Consts.INFORMATION_TEXTURE_ID);
//            }

//            return true;
//        }

//        #endregion

//        private void InitStateMachine()
//        {
//            // Tutorial
//            _levelStateMachine.AddState(this, MoveCameraToPlayer_State);
//            _levelStateMachine.AddState(this, Awaken_State);
//            // _levelStateMachine.AddState(this, MoveMouse_State);
//            //_levelStateMachine.AddState(this, Zoom_State);
//            //  _levelStateMachine.AddState(this, FireWeapon_State);
//            _levelStateMachine.AddState(this, Speed_State);
//            _levelStateMachine.AddState(this, MoveToSun_State);

//            ////Conversion
            

//            // TODO: ships will surround you when you first meet them.

//            // Destroy Sun
//            _levelStateMachine.AddState(this, DestroySunVoice_State);
//            _levelStateMachine.AddState(this, DestroySun_State);
//        }

//        string[] ships = { "PrologueEnemy1","MediumShip4a","SmallShip4A","SmallShip6A",
//                "SmallShip7A","SmallShip8A","SmallShip9A","PirateKing1","Miner1","SmallShip13a","SmallShip14A", "StartingShip3" };
//        bool shipsAdded = false;
//        public override void UpdateScript(InputState inpuState)
//        {
//            if (inpuState.IsKeyPressed(Keys.Z) & !shipsAdded)
//            {
//                GameObject ship = AddGameObject("SmallShip8A", FactionType.Pirates1, Vector2.UnitY * 10000, -90);
//                ship.SetTarget(PlayerAgent, TargetType.Goal);
//            }

//            if (inpuState.IsKeyPressed(Keys.Space) & !shipsAdded)
//            {
//                shipsAdded = true;
            
//                for (int r = 0; r < 4; r++)
//                {
//                    int numberOfShips = 11 + r * 2;
//                    for (int i = 0; i < numberOfShips; i++)
//                    {
//                        string loadout = ships[GameEngine.Rand.Next(ships.Length)];
//                        float angle = ((i / (float)numberOfShips) - 0.5f) * 2f - MathHelper.PiOver2;
//                        GameObject ship = AddGameObject(loadout, FactionType.Federation, -FMath.ToCartesian(10000 + r * 500, angle), angle * 360f);
//                        ship.SetTarget(PlayerAgent, TargetType.Goal);
//                    }
//                }
//            }


//            if (FindPlayer() != null && _player != FindPlayer())
//            {
//                _player = (Agent)FindPlayer();
//                MetaWorld.Inst.UpdatePlayerShip(_player);
//                // FindPlayer().SetTarget(_sun, TargetType.Goal);
//            }

//            //if (_player != null && _player.IsNotActive)
//            //{
//            //    _sun.SetMeterValue(MeterType.Hitpoints, 0);
//            //}

//            Faction fedFaction = GameEngine.GetFaction(FactionType.Federation);
//            _isAtWar = _isAtWar | (fedFaction.GetRelationToFaction(FactionType.Player) < 0);

//            if (_player != null && !_isAtWar)
//            {

                

                
//            }            

//            UpdateGoal();

//            if (_endLevelTimer == 0)
//            {
//                _levelStateMachine.ActivateState();
//            }

//            //  HandleReviveTextBox();
//            HandleEndLevel();            
//        }


//        private void UpdateGoal()
//        {
//            if (FindPlayer() != null)
//            {
//                if (GameObject.DistanceFromEdge(FindPlayer(), _sun) > 1500 && _sun.IsActive)
//                {
//                    FindPlayer().SetTarget(_sun, TargetType.Goal);
//                }
//                else
//                {
//                    FindPlayer().SetTarget(null, TargetType.Goal);
//                }
//            }
//        }

//        private void HandleEndLevel()
//        {
//            if (_sun.IsNotActive || (_player != null && _player.IsNotActive))
//            {
//                UpdateZoomToSunExplosion();

//                _endLevelTimer++;
//                if (_endLevelTimer > 1)
//                {
//                    fadeAlpha = (_endLevelTimer) / (float)FADE_EFFECT_LENGTH;
//                }
//            }

//            if (_endLevelTimer > FADE_EFFECT_LENGTH)
//            {
//                _endLevelTimer++;

//                fadeAlpha = 1;

//                if (_endLevelTimer > FADE_EFFECT_LENGTH + 60)
//                {
//                    NextLevel();
//                }
//            }
//        }

//        private void UpdateZoomToSunExplosion()
//        {
//            CameraManager.TargetZoom = 0.08f;
//            CameraManager.MovmentType = CameraMovmentType.ToTarget;
//            CameraManager.CameraMovmentAcceleration = 1f;
//            CameraManager.CameraMovmentFactor = 0f;
//            CameraManager.CameraMovmentSpeed = 0;
//            CameraManager.TargetPosition = _sun.Position;
//            CameraManager.ZoomType = CameraZoomType.ToTargetZoom;
//        }

//        private void NextLevel()
//        {
          
//            ActivityManager.SwitchActivity("StartingNode","", false);
//        }

       
//    }
//}
