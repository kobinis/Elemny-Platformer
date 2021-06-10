//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Input;
//using SolarConflict.Framework;
//using SolarConflict.Framework.CameraControl.Zoom;
//using SolarConflict.Framework.Emitters.SceneRelated;
//using SolarConflict.Framework.Scenes;
//using SolarConflict.Framework.Scenes.DialogEngine;
//using SolarConflict.Framework.Utils;
//using SolarConflict.GameContent.Activities.SceneActivitys;
//using SolarConflict.GameContent.Agents;
//using SolarConflict.GameContent.Emitters;
//using SolarConflict.GameContent.Items;
//using SolarConflict.GameContent.NewItems;
//using SolarConflict.GameContent.NewItems.Utils;
//using SolarConflict.Session;
//using SolarConflict.Session.World.MissionManagment;
//using SolarConflict.Session.World.MissionManagment.Objectives;
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
//    public class PrologB : Scene
//    {

//        #region Constants
//        private const float WORLD_SCALE = 3;
//        //private const float SCALE_DISTANCE_FROM_SUN = 0.7f;        
//        private readonly Vector2 PLAYER_START_POSITION = Vector2.UnitY * 11000 * WORLD_SCALE;
//        private const int FADE_EFFECT_LENGTH = 60 * 3;
//        private const int NUM_INNER_GROUPS = 8;
//        private const int NUM_OUTER_GROUPS = 4;
//        private const FactionType ENEMY_FACTION = FactionType.Federation;
//        #endregion

//        #region Fields        
//        private List<string> _enemyShipNamesInnerRing = new List<string> { "PrologueEnemy2", "PrologueEnemy2", "PrologueEnemy2" };
//        private List<string> _enemyShipNamesOuterRing = new List<string> { "PrologueEnemy2", "PrologueEnemy1", "PrologueEnemy1", "PrologueEnemy1", "PrologueEnemy1", "PrologueEnemy1" }; // kobi: add more types of federation ships 
//        private List<string> _enemyKemronGroup = new List<string> { "Kemron", "PrologueEnemy1", "PrologueEnemy1" };
//        private DummyObject _kemronGoal = new DummyObject();
//        private int _endLevelTimer = 0;
//        private GameObject _sun;
//        private LevelStateMachine _levelStateMachine;
//        private GameObject _kemron;
//        private Agent _player;
//        private string _playerShipName;
//        private List<GameObject> _enemyShips;
//        private GameObject _commandStation;
//        private Mission _startingMission;
//        GameObject enemy;
//        #endregion

//        public PrologB(string parameters = null)
//            : base(parameters)
//        {
//            _playerShipName = parameters;
//        }

//        public override void InitScript(string parameters, ActivityParameters activityParameters = null)
//        {
//            background = new Background(7);
//            _enemyShips = new List<GameObject>();
//            PersistenceManager.Inst.NewSession(null);
//            //SceneComponentSelector.AddComponent(SceneComponentType.MissionLog);
//            //SceneComponentSelector.AddComponent(SceneComponentType.Inventory);//, "InventoryActivityTutorial");
//            //SceneComponentSelector.AddComponent(SceneComponentType.TacticalMap);
//            PlayerStartingPoint = PLAYER_START_POSITION;
//            GameEngine.AmbientColor = Vector3.One * 0.3f;
//            _levelStateMachine = new LevelStateMachine();
//            IsConfirmQuitNeeded = true;
//            AddGameObjects();
//            InitStateMachine();
//            AddEnemyShips();
//            SetCameraFromSunToShip();
//            GameEngine.Update();
//            GetPlayerFaction().ReflectRelations = true;
//            GameEngine.GetFaction(ENEMY_FACTION).SetRelationToFaction(GameEngine, FactionType.Player, 0);
//            enemy = AddGameObject("PrologFirstEnemy", PlayerStartingPoint - Vector2.UnitX * 200 - Vector2.UnitY * 6500, 90, FactionType.Pirates1);
//        }

//        private void AddMissions()
//        {
//            SceneComponentSelector.AddComponent(SceneComponentType.MissionLog);
//            DialogManager.AddDialog("p_intro1");

//            enemy.SetAggroRange(600, 5000, TargetType.Enemy);
//            Mission equipRotation = MissionFactory.MissionQuickStart("p_rotation");
//            equipRotation.IsHidden = false;
//            equipRotation.EmitterOnStart = new SceneComponentEmitter(SceneComponentType.Inventory);
//            equipRotation.Objective = new AcquireItemObjective("SmallRotationEngine3", 1, true, new TutorialGoal("SmallRotationEngine3", false));
//            MissionManager.AddMission(equipRotation);

//            Mission gotoPoint = MissionFactory.MissionQuickStart("p_gotoPoint");
//            gotoPoint.Color = Color.Green;
//            gotoPoint.Objective = new GoToTargetObjective(enemy, 1500);  //new DestroyTargetObjective(enemy);
//            equipRotation.NextMissionOnComplete = gotoPoint;
//            //equipRotation.NextMissionOnFail = gotoPoint;



//            Mission destroyEnemy = MissionFactory.MissionQuickStart("p_destroyShip");
//            destroyEnemy.Color = Color.Red;
//            destroyEnemy.Objective = new DestroyTargetObjective(enemy);
//            gotoPoint.NextMissionOnComplete = destroyEnemy;

//            Mission destroyStation = MissionFactory.MissionQuickStart("p_destroyStation");
//            destroyStation.Color = Color.Red;
//            destroyStation.Objective = new DestroyTargetObjective(_commandStation);
//            destroyEnemy.NextMissionOnComplete = destroyStation;

           

//            Mission equipDevice = MissionFactory.MissionQuickStart("p_equipDevice");
//            equipDevice.EmitterOnStart = new SceneComponentEmitter(SceneComponentType.Inventory);
//            equipDevice.Objective = new AcquireItemObjective("StarDestroyerItem", 1, true, new TutorialGoal("StarDestroyerItem", false));
//            //equipDevice.DialogOnComplete.IsInterupting = true;
//            destroyStation.NextMissionOnComplete = equipDevice;

//            Mission destroySun = MissionFactory.MissionQuickStart("p_destroySun");
//            destroySun.Objective = new DestroyTargetObjective(_sun);
//            equipDevice.NextMissionOnComplete = destroySun;
//            //Hidden missions
//            //Mission meetKemron = MissionFactory.MissionQuickStart("proMeetKemron");
//            //meetKemron.IsGoalHidden = true;
//            //meetKemron.Objective = new GoToTargetObjective(_kemron, 1500);
//            //meetKemron.NextMissionOnComplete = destroyControl;
//            //var options = meetKemron.DialogOnComplete.GetDialogOptions();
//            //options[0].OnSelect += (dialog, scene) => { scene.GameEngine.GetFaction(ENEMY_FACTION).SetRelationToFaction(scene.GameEngine, FactionType.Player, -1); };
//            //options[2].OnSelect += (dialog, scene) => { scene.GameEngine.GetFaction(ENEMY_FACTION).SetRelationToFaction(scene.GameEngine, FactionType.Player, -1); };
//            //MissionManager.AddMission(meetKemron);

//            Mission attackZone = MissionFactory.MissionQuickStart("proAttackZone");
//            attackZone.IsGoalHidden = true;
//            attackZone.Color = Color.Red;
//            var objectiveGroup = new ObjectiveGroup();
//            objectiveGroup.AddObjective(new GoToTargetObjective(_sun, 200, 3f)); //change to offset only
//            objectiveGroup.AddObjective(new AllianceObjective(ENEMY_FACTION));
//            objectiveGroup.AddObjective(new DefendTargetObjective(_kemron));
//            attackZone.Objective = objectiveGroup;
//            attackZone.OnMissionCompletion += (mission, scene) => { scene.GameEngine.GetFaction(ENEMY_FACTION).SetRelationToFaction(scene.GameEngine, FactionType.Player, -1); };
//            MissionManager.AddMission(attackZone);

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
//            this.CameraManager.ZoomType = CameraZoomType.Manual;
//            if (time > 60)
//            {
//                AddMissions();
//                return true;
//            }
//            return false;
//        }



//        private LevelStateMachine.StateOutput Missions_state(int time)
//        {

//            return true;
//        }




//        #endregion

//        bool didStun = false;
//        public override void UpdateScript(InputState inpuState)
//        {

//            if (_commandStation != null && _commandStation.IsNotActive && !didStun) //change to event on station death
//            {
//                didStun = true;
//                StunAllShips();
//            }

//            if ((GameEngine.GetFaction(FactionType.Federation).GetMinRelationBetweenFactions(FactionType.Player) >= 0f)
//                && (_kemron?.IsActive == true) && (_player?.IsActive == true))
//            {
//                // Point Kemron in the general direction of the player, but don't have him pursue past a certain distance from the sun
//                Vector2 goal = _player.Position;
//                goal.Normalize();
//                float distance = Math.Min(20000, _player.Position.Length());
//                goal *= distance;
//                _kemronGoal.Position = goal;
//                _kemron.SetTarget(_kemronGoal, TargetType.Goal);
//            }

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
//                    //AddGameObject(Consts.WARPIN_EFFECT, FactionType.Neutral, _player.Position);
//                    _player.IsActive = false;
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
//            parameters.ParamDictionary.Add("text", TextBank.Inst.GetTextAsset("proEnd").Text);
//            ActivityManager.SwitchActivity("SplashScreen", parameters, false);
//        }



//        private void InitStateMachine()
//        {
//            // Tutorial
//            _levelStateMachine.AddState(this, MoveCameraToPlayer_State);
//            _levelStateMachine.AddState(this, Awaken_State);
//            _levelStateMachine.AddState(this, Missions_state);
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


//        #region WorldBuilding

//        private void AddPlayerShip()
//        {
//            if (string.IsNullOrEmpty(_playerShipName))
//            {
//                _playerShipName = "PrologueShip3B";
//            }

//            _player = AddGameObject(_playerShipName, PLAYER_START_POSITION, -90, FactionType.Player, AgentControlType.Player) as Agent;
//            //  _player.AddItemToInventory(typeof(RepairKit1).Name, 10);
//            _player.SetMeterValue(MeterType.StunTime, 50);

//            // HyperSpace jump effect
//            AddGameObject(typeof(HyperSpaceJumpFx).Name, _player.Position);
//            AddGameObject("sound_SPACE_WARP", _player.Position);
//        }

//        private void AddGameObjects()
//        {
//            _sun = AddGameObject("SunWithBackground", Vector2.Zero);
//            AddGameObject("SunFullscreenColorFx", Vector2.Zero);
//            var planet = AddGameObject("Earth", Vector2.UnitY * PLAYER_START_POSITION.Y * 2 / 3);
//            planet.Name = "New Erath";
//            planet.Parent = _sun;
//        }

//        private void AddEnemyShips()
//        {

//            //Inner group
//            _enemyShips.AddRange(AddShipsGroups(8, _sun.Size * 1.5f, _enemyShipNamesInnerRing));
//            //Outer group
//            _enemyShips.AddRange(AddShipsGroups(4, PLAYER_START_POSITION.Y / 2, _enemyShipNamesOuterRing));

//            _commandStation = AddGameObject("SolarMiner", Vector2.UnitY * (_sun.Size * 1.5f + 2500), -MathHelper.Pi, ENEMY_FACTION);
//            _commandStation.Name = "CommandStation";
//            var kemronGroup = AddShipsGroups(1, PLAYER_START_POSITION.Y / 2 + 2000, _enemyKemronGroup);
//            _kemron = kemronGroup[0];
//            _kemron.Name = "Kemron";
//            _enemyShips.AddRange(kemronGroup);
//        }

//        private List<GameObject> AddShipsGroups(int numOfGroups, float rad, List<string> ships)
//        {
//            List<GameObject> gameObjects = new List<GameObject>();
//            for (int i = 0; i < numOfGroups; i++)
//            {
//                float rotation = i / (float)numOfGroups * MathHelper.TwoPi + MathHelper.PiOver2;
//                Vector2 pos = FMath.ToCartesian(rad, rotation);
//                var groupObjects = GameEngine.AddObjectsInFormation(ships, ENEMY_FACTION, pos, MathHelper.ToDegrees(rotation), 50);
//                gameObjects.AddRange(groupObjects);
//            }
//            return gameObjects;
//        }

//        private void StunAllShips() //Stuns all ships for 20 min
//        {
//            foreach (var ship in _enemyShips)
//            {
//                ship.SetMeterValue(MeterType.StunTime, 60 * 60 * 20);
//            }
//        }

//        #endregion
//        public static Activity ActivityProvider(string parameters)
//        {
//            return new PrologB(parameters);
//        }


//    }
//}
