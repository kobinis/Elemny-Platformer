//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Input;
//using SolarConflict.Framework;
//using SolarConflict.Framework.CameraControl.Zoom;
//using SolarConflict.Framework.Scenes;
//using SolarConflict.Framework.Scenes.DialogEngine;
//using SolarConflict.Framework.Utils;
//using SolarConflict.GameContent.Agents;
//using SolarConflict.GameContent.Emitters;
//using SolarConflict.GameContent.Items;
//using SolarConflict.GameContent.NewItems;
//using SolarConflict.GameContent.NewItems.Utils;
//using SolarConflict.Session;
//using SolarConflict.Session.World.MissionManagment;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics.Contracts;
//using System.Linq;
//using XnaUtils;
//using XnaUtils.Graphics;

////TODO: fix camera 
//namespace SolarConflict.GameContent.Activities
//{
//    [Serializable]
//    public class Prologue : Scene
//    {
//        #region Constants
//        private const float SCENE_WORLD_SCALE = 5;
//        private const float SCALE_DISTANCE_FROM_SUN = 0.7f;
//        private const int FADE_EFFECT_LENGTH = 60 * 3;
//        private readonly Vector2 PLAYER_START_POSITION = Vector2.UnitY * 11000 * SCENE_WORLD_SCALE;
//        #endregion

//        #region Fields
//        private Vector2[] _shipGroupsLocationsInWorld = { Vector2.UnitY, Vector2.UnitX, Vector2.UnitX * -1, Vector2.UnitY * -1,
//                                                          Vector2.UnitX * SCALE_DISTANCE_FROM_SUN + Vector2.UnitY * SCALE_DISTANCE_FROM_SUN,
//                                                          Vector2.UnitX * -SCALE_DISTANCE_FROM_SUN + Vector2.UnitY * -SCALE_DISTANCE_FROM_SUN,
//                                                          Vector2.UnitX * SCALE_DISTANCE_FROM_SUN + Vector2.UnitY * -SCALE_DISTANCE_FROM_SUN,
//                                                          Vector2.UnitX * -SCALE_DISTANCE_FROM_SUN + Vector2.UnitY * SCALE_DISTANCE_FROM_SUN };

//        private readonly string[] _startingShips = { "PrologueEnemy1", "PrologueEnemy1", "PrologueEnemy1", "PrologueEnemy1", "SolarMiner" };

//        private Vector2[] _locationInShipGroup = { Vector2.UnitX, Vector2.UnitY, Vector2.UnitX * -1, Vector2.UnitY * -1, Vector2.Zero };
//        private string[] _enemyShipNamesInnerRing = { "PrologueEnemy2", "PrologueEnemy2", "PrologueEnemy2", "PrologueEnemy2", "SolarMiner" };
//        private string[] _enemyShipNamesOuterRing = { "PrologueEnemy1", "PrologueEnemy1", "PrologueEnemy1", "PrologueEnemy1", "PrologueEnemy1" }; // kobi: add more types of federation ships 
//        private List<GameObject> _motherships;
//        private DummyObject _kemronGoal = new DummyObject();
//        private int _endLevelTimer = 0;
//        private GameObject _sun;
//        private LevelStateMachine _levelStateMachine;
//        private AgentDialogSystem _kemronDialogueSystem;
//        private GameObject _kemron;
//        private bool _destroySunVoiceTextShown = false;
//        private Agent _player;
//        private string _playerShipName;
//        private float _tutorialStartingZoom;
//        private bool _tutorialPlayerUnderstandsZoom = false;
//        private string _dismissMessageSnippet = "\n \n#color{255,255,255}Press space to dismiss";
//        private bool _isControlledByAI = true;
//        #endregion

//        public Prologue(string parameters = null)
//            : base(parameters)
//        {
//            _playerShipName = parameters;            
//        }

//        public override void InitScript(string parameters, ActivityParameters activityParameters = null)
//        {

//            PersistenceManager.Inst.NewSession();
//            SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.Inventory, "InventoryActivityTutorial");
//            SceneComponentSelector.AddComponent(SceneComponentType.MissionLog);
//            _levelStateMachine = new LevelStateMachine();
//            IsConfirmQuitNeeded = true;
//            _motherships = new List<GameObject>();
//            AddGameObjects();
//            InitStateMachine();
//            AddEnemyShips();
//            AddDialogues();            
//            SetCameraFromSunToShip();
//            GameEngine.Update();
//            GameEngine.AmbientColor = Vector3.One * 0.6f;

//        }

//        public override void OnResume(ActivityParameters parameters = null)
//        {
//            GameSession.Inst.Continue();
//        }

//        #region States

//        private LevelStateMachine.StateOutput MoveCameraToPlayer_State(int time)
//        {
//            if (time == 1)
//            {
//                MusicEngine.Instance.PlaySong(MusicEngine.PROLOGUE_BEGINING_SONG);
//            }

//            if (CameraManager.IsCameraOnTarget(Camera))
//            {
//                AddPlayerShip();
//                //AddMission("Destroy The Sun", "The year is 2525. Mankind is still alive, but for how long?", Color.Yellow, _sun);
//                CameraManager.MovmentType = CameraMovmentType.OnPlayer;
//                CameraManager.TargetZoom = 0.4f;
//                // CameraManager.ZoomType = CameraZoomType.Auto; //  CameraZoomType.ToTargetZoom;

//                return true;
//            }

//            return false;
//        }

//        private LevelStateMachine.StateOutput Awaken_State(int time)
//        {
//            //string id = AddTextBox(PlotTexts.Tutorial1Awaken, image: Consts.COMMUNICATION_CONSOLE_TEXTURE_ID, id: "void", soundID: "awakenAI");
//            string id = AddTextBox("Awaken, servant of the apocalypse", id: "void", soundID: "awaken");

//            return id == null;
//        }

//        private LevelStateMachine.StateOutput MouseHelp_State(int time)
//        {
//            if (time == 1)
//            {
//                AddTextBox("Use the #ctext{255,255,0,\"mouse\"} to aim and the #ctext{255,255,0,\"mouse wheel\"} to zoom in and out.\n",
//                    Palette.TextBoxFont, id: "MouseHelpText", image: Consts.INFORMATION_TEXTURE_ID);

//                _tutorialStartingZoom = Camera.Zoom;
//            }

//            // Has the player figured out zooming?
//            var ratio = Camera.Zoom / _tutorialStartingZoom;
//            if (ratio > 1.1f || ratio < 0.9f)
//                _tutorialPlayerUnderstandsZoom = true;

//            if ((_tutorialPlayerUnderstandsZoom && time > Utility.Frames(2f)) || !IsTextBoxUp("MouseHelpText"))
//            {
//                DialogManagerOld.RemoveDialogBox("MouseHelpText");
//                return true;
//            }

//            return false;
//        }

//        private LevelStateMachine.StateOutput MoveHelp_State(int time)
//        {
//            if (time == 1)
//            {
//                AddTextBox("Use #ctext{255,255,0,\"WASD\"} to move.", Palette.TextBoxFont, id: "MoveHelpText", image: Consts.INFORMATION_TEXTURE_ID);
//            }
//            // Has the player held down movement keys
//            if ((_player?.ControlSignal & (ControlSignals.Up | ControlSignals.Down | ControlSignals.Left | ControlSignals.Right)) > 0)
//            {
//                DialogManagerOld.RemoveDialogBox("MoveHelpText");
//                return true;
//            }
//            return false;
//        }
        
//        private LevelStateMachine.StateOutput EquipItemHelp_State(int time)
//        {
//            if (time == 1)
//                AddTextBox("#color{255,0,0}PHD DEVICE CHARGED#defalutColor{}\n \nPress #color{255,255,0}I#defalutColor{} to open your inventory.", id: "EquipItem", isSkipable: false);

//            // Has the player equipped the item?
//            if (_player.ItemSlotsContainer.Any(s => s?.Item?.ID == typeof(StarDestroyerItem).Name))
//            {
//                DialogManagerOld.RemoveDialogBox("EquipItem");
//                return true;
//            }

//            return false;
//        }

//        private LevelStateMachine.StateOutput DeployDevice_State(int time)
//        {
//            if (time == 1)
//                AddTextBox("Press #color{255,255,0}Right Mouse Button#defalutColor{} while targeting the sun.", id: "EquipItem", isSkipable: false);
//            if (_isControlledByAI)
//            {
//                _player.SetTarget(_sun, TargetType.Enemy);
//            }


//            return _sun.IsActive;
//        }

//        //private LevelStateMachine.StateOutput Speed_State(int time)
//        //{

//        //    string ID = AddTextBox(PlotTexts.Tutorial6Speed, Palette.TextBoxFont, id: "SpeedTextID", image: Consts.INFORMATION_TEXTURE_ID);

//        //    bool next = (_player.ControlSignal & (ControlSignals.Action4)) > 0 || ID == null; //TODO: change according to key binding  //(_player.ControlSignal & (ControlSignals.Action3)) > 0;

//        //    if (next)
//        //    {
//        //        DialogManager.RemoveDialogBox("SpeedTextID");
//        //    }

//        //    return next;
//        //}

//        private LevelStateMachine.StateOutput MoveToSun_State(int time)
//        {
//            if (time == 1)
//            {
//                AddTextBox("Follow the " + Color.Yellow.ToTag("arrow") + " until you reach the " + Color.Yellow.ToTag("sun") + "#image{SunBackground}", Palette.TextBoxFont, id: "MoveToSunText", image: Consts.INFORMATION_TEXTURE_ID);

//                //"#image{TmpGoalArrowIcon}
//            }

//            if (_player != null && _sun.IsActive && GameObject.DistanceFromEdge(_player, _sun) < 4000)
//            {
//                // We have arrived
//                DialogManagerOld.RemoveDialogBox("MoveToSunText");
//                return true;
//            }

//            return false;
//        }

//        // TODO: replace the below with a dialogue
//        /*private LevelStateMachine.StateOutput KemronHailing_State(int time)
//        {
//            string id = string.Empty;
//            float distance = (_player.Position - _kemron.Position).Length();
//            //if (isCloseToTarget)
//            //{
//            //    id = AddTextBox(PlotTexts.Tutorial10KemronHailing, id: "KemronHailingID", image: "Prologue_CaptainKemron", isBlocking: true, isSkipable: true);
//            //    //  id = AddTextBox(PlotTexts.Tutorial10KemronHailing, id: "KemronHailingID", image: "Prologue_CaptainKemron", soundID: "KemronHailing", isBlocking: true, isSkipable: true);
//            //}
//            return distance < 800;
//        }

//        private LevelStateMachine.StateOutput PlayerRespond_State(int time)
//        {           
//            IList<string> playerOptions = playerOptions = new List<string>();
//            playerOptions.Add(PlotTexts.Tutorial11PlayerRespondKemron1);
//            playerOptions.Add(PlotTexts.Tutorial12PlayerRespondKemron2);
//            playerOptions.Add(PlotTexts.Tutorial13PlayerRespondKemron3);      
//            _playerAnswer = DialogManager.AddQuestionBox(PlotTexts.Tutorial10KemronHailing, playerOptions, "Prologue_CaptainKemron", "KemronHailing");            
//            return _playerAnswer != 0;
//        }

//        private LevelStateMachine.StateOutput KemronRespond_State(int time)
//        {
//            Contract.Requires(_playerAnswer >= 0 && _playerAnswer < 4);

//            string ID = string.Empty;
//            Faction fedFaction;

//            switch (_playerAnswer)
//            {
//                case 1:
//                    //ID = AddTextBox(PlotTexts.Tutorial15KemronRespond1, id: "KemronRespond1ID", image: "Prologue_CaptainKemron", isBlocking: true, isSkipable: true);
//                    ID = AddTextBox(PlotTexts.Tutorial15KemronRespond1, id: "KemronRespond1ID", image: "Prologue_CaptainKemron", soundID: "KemronAnswer1", isBlocking: true, isSkipable: true);
//                    fedFaction = base.GameEngine.GetFaction(FactionType.Federation);
//                    fedFaction.ChangeRelationToFaction(GameEngine, FactionType.Player, -0.1f);
//                    break;

//                case 2:
//                    //ID = AddTextBox(PlotTexts.Tutorial16KemronRespond2, id: "KemronRespond2ID", image: "Prologue_CaptainKemron", isBlocking: true, isSkipable: true);
//                    ID = AddTextBox(PlotTexts.Tutorial16KemronRespond2, id: "KemronRespond2ID", image: "Prologue_CaptainKemron", soundID: "KemronAnswer2", isBlocking: true, isSkipable: true);
//                    break;

//                case 3:
//                    //ID = AddTextBox(PlotTexts.Tutorial17KemronRespond3, id: "KemronRespond3ID", image: "Prologue_CaptainKemron", isBlocking: true, isSkipable: true);
//                    ID = AddTextBox(PlotTexts.Tutorial17KemronRespond3, id: "KemronRespond3ID", image: "Prologue_CaptainKemron", soundID: "KemronAnswer3", isBlocking: true, isSkipable: true);                    
//                    fedFaction = base.GameEngine.GetFaction(FactionType.Federation);
//                    fedFaction.ChangeRelationToFaction(GameEngine, FactionType.Player, -0.1f);
//                    break;                  
//            }

//            return ID == null;
//        }*/

//        //private LevelStateMachine.StateOutput Stun_State(int time)
//        //{
//        //    bool next = false;

//        //    if (time == 1)
//        //    {
//        //        AddTextBox(PlotTexts.Tutorial19Stun, Palette.TextBoxFont, id: "StunTextID", image: Consts.INFORMATION_TEXTURE_ID);
//        //    }

//        //    if (InputState.IsKeyPressed(Keys.Q)) 
//        //    {
//        //        this.DialogManager.RemoveDialogBox("StunTextID");
//        //        next = true;
//        //    }

//        //    if (!DialogManager.IsDialogUp("StunTextID"))
//        //    {
//        //        next = true;
//        //    }

//        //    return next;
//        //}

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

//        public override void UpdateScript(InputState inpuState)
//        {
//            if (_isControlledByAI)
//            {
//                _player?.SetTarget(_sun, TargetType.Goal);
//            }
//            // Is the player fighting the Federation?
//            if (GameEngine.GetFaction(FactionType.Federation).GetRelationToFaction(FactionType.Player) < 0f)
//                // Yes. So much for Kemron's diplomacy
//                (_kemron as Agent)?.RemoveSystem(_kemronDialogueSystem);
//            // ^ This really shouldn't be necessary, dialogue systems need better triggers
//            if (_isControlledByAI)
//                _player?.SetControlType(AgentControlType.AI);
//            if (FindPlayer() != null && _player != FindPlayer())
//            {
//                _player = (Agent)FindPlayer();
//                MetaWorld.Inst.UpdatePlayerShip(_player);
//                // FindPlayer().SetTarget(_sun, TargetType.Goal);
//            }

//            if ((GameEngine.GetFaction(FactionType.Federation).GetMinRelationBetweenFactions(FactionType.Player) >= 0f)
//                && (_kemron?.IsActive == true) && (_player?.IsActive == true))
//            {
//                // Point Kemron in the general direction of the player, but don't have him pursue past a certain distance from the sun
//                Vector2 goal = _player.Position;
//                goal.Normalize();
//                float distance = Math.Min(30000, _player.Position.Length());
//                goal *= distance;
//                _kemronGoal.Position = goal;
//                _kemron.SetTarget(_kemronGoal, TargetType.Goal);
//            }


//            UpdateGoal();

//            if (_endLevelTimer == 0)
//            {
//                _levelStateMachine.ActivateState();
//            }

//            //  HandleReviveTextBox();
//            HandleEndLevel();

//            if (_sun.IsNotActive && _player != null && _player.IsActive)
//            {
//                if ((_player.ControlSignal & ControlSignals.OnDamageToShield) > 0 || (_player.ControlSignal & ControlSignals.OnDamageToHull) > 0)
//                {
//                    AddGameObject(Consts.WARPIN_EFFECT, FactionType.Neutral, _player.Position);
//                    _player.IsActive = false;
//                }
//            }
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
//                // Player and/or sun are dead; everyone is either happy or dead



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
//            var parameters = new ActivityParameters();
//            parameters.DataParams.Add("Scene", this);
//            parameters.ParamDictionary.Add("title", "What have you done?");
//            ActivityManager.SwitchActivity("SplashScreen", parameters, false);
//        }

//        #region Private Methods 

//        private void AddDialogues()
//        {
//            _kemronDialogueSystem = new AgentDialogSystem();
//            _kemronDialogueSystem.Range = 1200;

//            var kemronDialogue1 = new Dialogue(PlotTexts.Tutorial10KemronHailing, soundID: "KemronHailing");
//            var kemronDialogue2 = new Dialogue(PlotTexts.Tutorial15KemronRespond1, soundID: "KemronAnswer1");
//            var kemronDialogue3 = new Dialogue(PlotTexts.Tutorial16KemronRespond2, soundID: "KemronAnswer2");
//            var kemronDialogue4 = new Dialogue(PlotTexts.Tutorial17KemronRespond3, soundID: "KemronAnswer3");
//            var kemronDebugDialogue = new Dialogue("No, for real, though, listen...", kemronDialogue1);

//            Func<Dialogue> kemronResponseCallback1 = () =>
//            {
//                GameEngine.GetFaction(FactionType.Federation).SetRelationToFaction(GameEngine, FactionType.Player, -1f);
//                return kemronDialogue2;
//            };
//            Func<Dialogue> kemronResponseCallback2 = () =>
//            {
//                GameEngine.GetFaction(FactionType.Federation).SetRelationToFaction(GameEngine, FactionType.Player, -1f);
//                return kemronDialogue4;
//            };


//            kemronDialogue1.Options = new DialogueOption[] {
//                new DialogueOption(PlotTexts.Tutorial11PlayerRespondKemron1, kemronResponseCallback1),
//                new DialogueOption(PlotTexts.Tutorial12PlayerRespondKemron2, kemronDialogue3),
//                new DialogueOption(PlotTexts.Tutorial13PlayerRespondKemron3, kemronResponseCallback2) };

//            if (DebugUtils.IsDebug)
//                kemronDialogue1.Options = kemronDialogue1.Options.Concat(new DialogueOption[]
//                    {new DialogueOption("This is also Captain Kemron of the Federation starship Vortex. Carry on.", kemronDebugDialogue)}).ToArray();

//            kemronDialogue1.DefaultPortraitID = "Prologue_CaptainKemron";

//            //_kemronDialogueSystem.SetDialogue(kemronDialogue1);

//            (_kemron as Agent).AddSystem(_kemronDialogueSystem);
//        }

//        //private void AddPointsOfInterest() {
//        //    PointsOfInterest = new IPointOfInterest[] {
//        //        new SimplePointOfInterest(_kemron, "Kemron", "I never liked this guy"),
//        //        new SimplePointOfInterest(_sun, "A sun", "Some sort of incandescent orb", true),
//        //        new SimplePointOfInterest(new DummyObject(_kemron.Position + new Vector2(1000, 1000)), "Scenic outlook",
//        //        "That's funny, the damage doesn't look as bad from out here", true)
//        //    };
//        //}


//        private void InitStateMachine()
//        {
//            // Tutorial
//            _levelStateMachine.AddState(this, MoveCameraToPlayer_State);
//            _levelStateMachine.AddState(this, Awaken_State);
//            _levelStateMachine.AddState(this, MouseHelp_State);
//            _levelStateMachine.AddState(this, MoveHelp_State);
//            //_levelStateMachine.AddState(this, BrakeHelp_State);

//            // _levelStateMachine.AddState(this, Zoom_State);
//            //_levelStateMachine.AddState(this, FireWeapon_State);
//            //  _levelStateMachine.AddState(this, Speed_State); 
//            _levelStateMachine.AddState(this, MoveToSun_State);
//            _levelStateMachine.AddState(this, EquipItemHelp_State);
//            _levelStateMachine.AddState(this, DeployDevice_State);

//            ////Conversion
//            /*_levelStateMachine.AddState(this, KemronHailing_State);
//            _levelStateMachine.AddState(this, PlayerRespond_State, "PlayerRespond_State");
//            _levelStateMachine.AddState(this, KemronRespond_State, "KemronRespond_State");*/
//            //  _levelStateMachine.AddState(this, Stun_State);

//            // TODO: ships will surround you when you first meet them.

//            // Destroy Sun
//            _levelStateMachine.AddState(this, DestroySunVoice_State);
//            _levelStateMachine.AddState(this, DestroySun_State);
//        }

//        private void SetCameraFromSunToShip()
//        {
//            CameraManager.MovmentType = CameraMovmentType.ToTarget;
//            CameraManager.CameraMovmentAcceleration = 1f;
//            CameraManager.CameraMovmentFactor = 0f;
//            CameraManager.CameraMovmentSpeed = 0;
//            CameraManager.TargetPosition = PLAYER_START_POSITION;
//            CameraManager.ZoomType = CameraZoomType.ToTargetZoom;
//            CameraManager.TargetZoom = 0.08f;
//            //ManualZoom.ManualTargetZoom = 0f;
//            Camera.Zoom = 1f;
//        }
//        #endregion

//        #region WorldBuilding

//        private void AddPlayerShip()
//        {
//            if (string.IsNullOrEmpty(_playerShipName))
//            {
//                _playerShipName = "PrologueShip3";
//            }

//            _player = AddGameObject(_playerShipName, PLAYER_START_POSITION, -90, FactionType.Player, AgentControlType.Player) as Agent;
//            //_player.AddItemToInventory((Item)ContentBank.Inst.GetGameObjectFactory(typeof(VacuumModulatorItem).Name).MakeGameObject(this.GameEngine));
//            if (_isControlledByAI)
//                _player.SetControlType(AgentControlType.AI);
//            // Add PhoenixDeviceItem
//            Item item = (Item)ContentBank.Inst.GetGameObjectFactory(typeof(PhoenixDeviceItem).Name).MakeGameObject(this.GameEngine);
//            item.Stack = 15;
//            _player.AddItemToInventory(item);

//            // Add AutoRepairKit
//            item = (Item)ContentBank.Inst.GetGameObjectFactory(typeof(RepairKit1).Name).MakeGameObject(this.GameEngine);
//            item.Stack = 10;
//            _player.AddItemToInventory(item);

//            _player.SetMeterValue(MeterType.StunTime, 50);

//            AddGameObject("SunFullscreenColorFx", Vector2.Zero);
//            // HyperSpace jump effect
//            AddGameObject(typeof(HyperSpaceJumpFx).Name, _player.Position);
//            SoundEffect soundEffect = SoundBank.Inst.GetSound("SPACE_WARP");
//            SoundEffectInstance soundEffectInstance = soundEffect.CreateInstance();
//            soundEffectInstance.Play();
//        }

//        private void AddGameObjects()
//        {
//            _sun = AddGameObject("SunWithBackground", Vector2.Zero);

//            this.AddObjectRandomlyInCircle("Asteroid0", 150, 10000 * SCENE_WORLD_SCALE, 2000 * SCENE_WORLD_SCALE);
//            this.AddObjectRandomlyInCircle("SmallAsteroid0", 300, 10000 * SCENE_WORLD_SCALE, 2000 * SCENE_WORLD_SCALE);
//            // this.AddObjectRandomlyInCircle("LavaAsteroid", 300, _sun.Size * 2.5f, _sun.Size * 1.2f);
//        }

//        private void AddEnemyShips()
//        {
//            _kemron = AddGameObject("Kemron", Vector2.UnitY * PLAYER_START_POSITION.Y / 2, 0, FactionType.Federation);

//            for (int groupLocationIndex = 0; groupLocationIndex < _shipGroupsLocationsInWorld.Length; groupLocationIndex++)
//            {
//                float shipRotation = (float)Math.Atan2(_shipGroupsLocationsInWorld[groupLocationIndex].Y, _shipGroupsLocationsInWorld[groupLocationIndex].X);

//                InitInnerRingShips(groupLocationIndex, shipRotation);
//                InitOuterRingShips(groupLocationIndex, shipRotation);
//            }
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
//        public static Activity ActivityProvider(string parameters)
//        {
//            return new Prologue(parameters);
//        }


//    }
//}
