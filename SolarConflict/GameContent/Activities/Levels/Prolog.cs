using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using SolarConflict.Framework;
using SolarConflict.Framework.CameraControl.Zoom;
using SolarConflict.Framework.Emitters.SceneRelated;
using SolarConflict.Framework.GUI;
using SolarConflict.Framework.Scenes;
using SolarConflict.Framework.Scenes.DialogEngine;
using SolarConflict.Framework.Scenes.HudEngine.Components;
using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.Activities.SceneActivitys;
using SolarConflict.GameContent.Agents;
using SolarConflict.GameContent.Emitters;
using SolarConflict.GameContent.Items;
using SolarConflict.GameContent.NewItems;
using SolarConflict.GameContent.NewItems.Utils;
using SolarConflict.Session;
using SolarConflict.Session.World.MissionManagment;
using SolarConflict.Session.World.MissionManagment.GlobalObjectives;
using SolarConflict.Session.World.MissionManagment.Objectives;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using XnaUtils;
using XnaUtils.Graphics;
using static SolarConflict.Framework.HudManager;

//TODO: fix camera 
namespace SolarConflict.GameContent.Activities
{
    [Serializable]
    public class Prolog : Scene
    {
        #region Constants
        private const float WORLD_SCALE = 3;
        //private const float SCALE_DISTANCE_FROM_SUN = 0.7f;        
        private readonly Vector2 PLAYER_START_POSITION = Vector2.UnitY * 11200 * WORLD_SCALE;
        private const int FADE_EFFECT_LENGTH = 60 * 3;        
        private const int NUM_INNER_GROUPS = 8;
        private const int NUM_OUTER_GROUPS = 4;
        private const FactionType ENEMY_FACTION = FactionType.Federation;
        #endregion

        #region Fields        
        private List<string> _enemyShipNamesInnerRing = new List<string> { "PrologueEnemy2", "PrologueEnemy2", "PrologueEnemy2" };
        private List<string> _enemyShipNamesOuterRing = new List<string> { "PrologueEnemy2", "PrologueEnemy1", "PrologueEnemy1", "PrologueEnemy1", "PrologueEnemy1" , "PrologueEnemy1" }; // kobi: add more types of federation ships 
        private List<string> _enemyKemronGroup = new List<string> { "Kemron", "PrologueEnemy1", "PrologueEnemy1" };
        private DummyObject _kemronGoal = new DummyObject();
        private int _endLevelTimer = 0;
        private GameObject _sun;
        private LevelStateMachine _levelStateMachine;        
        private GameObject _kemron;
        private Agent _player;
        private string _playerShipName;            
        private List<GameObject> _enemyShips;
        private GameObject _commandStation;
        //private Mission _startingMission;
        private GameObject _firstEnemy;
        ActivityParameters activityParams;
        #endregion

        //public Prolog() { }
        string shipIndexSelected;

        public override void InitScript(string parameters, ActivityParameters activityParameters = null)
        {
            SkipInteraction = true;
            activityParams = activityParameters;
            IsConfirmQuitNeeded = true;
            _playerShipName = "PrologShip3";
            if(activityParameters.ParamDictionary.ContainsKey("loadout"))
            {
                _playerShipName = activityParameters.ParamDictionary["loadout"];
            }
            shipIndexSelected = activityParameters.GetParam("index");
            background = new Background(1, isRandom:false);
            _enemyShips = new List<GameObject>();
            var ships = GlobalData.Inst.GetData("starting_ships").Split(',');
            string startingShip = ships[int.Parse(shipIndexSelected)];
            PersistenceManager.Inst.NewSession(startingShip);
            //  SceneComponentSelector.AddComponent(SceneComponentType.MissionLog);
            //  SceneComponentSelector.AddComponent(SceneComponentType.Inventory);//, "InventoryActivityTutorial");
            //  SceneComponentSelector.AddComponent(SceneComponentType.TacticalMap);
            HudManager.AddComponent(new PlayerGoalsIndicator(), PositionType.TLtoBL);

            PlayerStartingPoint = PLAYER_START_POSITION;
            GameEngine.AmbientColor = Vector3.One * 0.3f;
            _levelStateMachine = new LevelStateMachine();
            IsConfirmQuitNeeded = true;
            AddGameObjects();
            InitStateMachine();
            AddEnemyShips();
            SetCameraFromSunToShip();
            GameEngine.Update(InputState.EmptyState);
            GetPlayerFaction().ReflectRelations = true;
            GameEngine.GetFaction(ENEMY_FACTION).SetRelationToFaction(GameEngine, FactionType.Player, 0);
            _firstEnemy = AddGameObject("PrologFirstEnemy", PlayerStartingPoint - Vector2.UnitX * 200 - Vector2.UnitY * 7000, 90, FactionType.Pirates1);
            AddGameObject("Asteroid1", PlayerStartingPoint + Vector2.UnitX * 200 - Vector2.UnitY * 1000);
            AddGameObject("Asteroid1", PlayerStartingPoint - Vector2.UnitX * 150 - Vector2.UnitY * 1100);

        }

        private void AddMissions()
        {

            MissionChainingHelper ch = new MissionChainingHelper(this);
            // DialogManager.AddDialog("proIntro1");       
            

            _firstEnemy.SetAggroRange(400, 5000, TargetType.Enemy);

            Mission intro = MissionFactory.MissionQuickStart("smProDestroyStation");
            intro.Objective = new MinimumTimeObjective(0.1f);
            ch.Add(intro);

            //Mission evadeMission = MissionFactory.MissionQuickStart("proEvade");
            //// moveMission.Color = Color.Green;
            //evadeMission.Objective = new ControlSignalObjective(ControlSignals.Left | ControlSignals.Right);
            //ch.Add(evadeMission);


            Mission shotMainMission = MissionFactory.MissionQuickStart("smProPrimaryFire");
            // moveMission.Color = Color.Green;
            shotMainMission.Objective = new ControlSignalObjective(ControlSignals.Action1);
            ch.Add(shotMainMission);

            Mission shotSecMission = MissionFactory.MissionQuickStart("smProSecondaryFire");
            // moveMission.Color = Color.Green;
            shotSecMission.Objective = new ControlSignalObjective(ControlSignals.Action2);
            ch.Add(shotSecMission);

           

            Mission moveMission = MissionFactory.MissionQuickStart("smMove");
            // moveMission.Color = Color.Green;
            moveMission.Objective = new GoToTargetObjective(_firstEnemy, 1300);
            ch.Add(moveMission);

            //Mission openLog = MissionFactory.MissionQuickStart("fnOpenMissionLog"); //Move Mission Log to Prolog
            //openLog.Objective = new OpenSceneComponentObjective(SceneComponentType.MissionLog);
            //openLog.EmitterOnStart = new SceneComponentEmitter(SceneComponentType.MissionLog);
            //ch.AddGroup(openLog);

            //Destroy Enemy
            Mission shotMission = MissionFactory.MissionQuickStart("smProFirstFight");
            shotMission.Color = Color.Red;
            shotMission.MissionGiver = _firstEnemy;
            shotMission.Objective = new DestroyTargetObjective(_firstEnemy);
            shotMission.OnMissionStart += (m, s) => { m.MissionGiver.SetAggroRange(4000,7000, TargetType.Enemy); };
            ch.Add(shotMission);

            Mission destroyControl = MissionFactory.MissionQuickStart("smProDestroyCommandStation");
            destroyControl.DialogOnStart.IsInterupting = true;
            destroyControl.Color = Color.Red;
            destroyControl.Objective = new DestroyTargetObjective(_commandStation);
            destroyControl.OnMissionStart += (m, s) => { s.SceneComponentSelector.AddComponent(SceneComponentType.MissionLog); };


            Mission delay = new Mission();
            delay.IsHidden = true;
            delay.Objective = new MinimumTimeObjective(0.1f);
            

            Mission openMissionLog = MissionFactory.MissionQuickStart("smMissionLog");
            openMissionLog.Objective = new OpenSceneComponentObjective(SceneComponentType.MissionLog);
            openMissionLog.EmitterOnStart = new SceneComponentEmitter(SceneComponentType.MissionLog, true);
            delay.NextMissionOnComplete = openMissionLog;

            ch.AddGroup(true, destroyControl, delay);

            Mission goToSunMissionNext = MissionFactory.MissionQuickStart("smProGoToTheStar");
            goToSunMissionNext.DialogOnStart.IsInterupting = true;
            goToSunMissionNext.Objective = new GoToTargetObjective(_sun, 0,1.5f);
            ch.Add(goToSunMissionNext);

            Mission equipDevice = MissionFactory.MissionQuickStart("smProInventoryEquipment");
            equipDevice.EmitterOnStart = new SceneComponentEmitter(SceneComponentType.Inventory, true);
            equipDevice.Objective = new AcquireItemObjective("StarDestroyerItem", 1, true, new TutorialGoal("StarDestroyerItem", false));
            equipDevice.OnMissionStart += (m, s) => { SceneComponentSelector.AddComponent(SceneComponentType.Inventory); SceneComponentSelector.GetProviderControl(SceneComponentType.Inventory).IsGlowing = true;
                var inventoryProvider = SceneComponentSelector.GetProviderControl(SceneComponentType.Inventory);
                inventoryProvider.ActivityParams = new ActivityParameters();
                inventoryProvider.ActivityParams.ParamDictionary.Add("hide_crafting", string.Empty);
            };
            ch.Add(equipDevice);

            Mission destroySun = MissionFactory.MissionQuickStart("smProDestroyStar");
         //   destroySun.Color = Color.Red;
            destroySun.Objective = new DestroyTargetObjective(_sun, 500);
            ch.Add(destroySun);

            //Hidden missions
            Mission meetKemron = MissionFactory.MissionQuickStart("smProMeetKemron");
            meetKemron.DialogOnComplete.IsInterupting = true;
            meetKemron.IsGoalHidden = true;
            meetKemron.Objective = new GoToTargetObjective(_kemron, 1200);
            meetKemron.NextMissionOnComplete = destroyControl;
            var options = meetKemron.DialogOnComplete.GetDialogOptions();            
         //   options[0].OnSelect += (dialog, scene) => { scene.GameEngine.GetFaction(ENEMY_FACTION).SetRelationToFaction(scene.GameEngine, FactionType.Player, -1); };
           // options[2].OnSelect += (dialog, scene) => { scene.GameEngine.GetFaction(ENEMY_FACTION).SetRelationToFaction(scene.GameEngine, FactionType.Player, -1); };
            AddMission(meetKemron);

            Mission attackZone = MissionFactory.MissionQuickStart("smProProvokeKemron");
            attackZone.IsGoalHidden = true;
            //attackZone.IsHidden = false;
            attackZone.Color = Color.Yellow;
            var objectiveGroup = new ObjectiveGroup();
            objectiveGroup.AddObjective(new GoToPositionObjective(Vector2.Zero, PLAYER_START_POSITION.Y/3 + 1000)); //change to offset only
            objectiveGroup.AddObjective(new AllianceObjective(ENEMY_FACTION));
            objectiveGroup.AddObjective(new DefendTargetObjective(_kemron));
            attackZone.Objective = objectiveGroup;
            attackZone.OnMissionCompletion += (mission, scene) => { scene.GameEngine.GetFaction(ENEMY_FACTION).SetRelationToFaction(scene.GameEngine, FactionType.Player, -1); };
            AddMission(attackZone);

            //Mission kemronDie = MissionFactory.MissionQuickStart("proKemronDie");
            //kemronDie.IsGoalHidden = true;
            //kemronDie.Objective = new DestroyTargetObjective(_kemron);
            //AddMission(kemronDie);
        }
        
        #region States

        private LevelStateMachine.StateOutput MoveCameraToPlayer_State(int time)
        {
            if (time == 1)
            {
                MusicEngine.Instance.PlaySong(MusicEngine.PROLOGUE_BEGINING_SONG);
            }

            if (CameraManager.IsCameraOnTarget(Camera))
            {
                           
                AddPlayerShip();
                
                //AddMission("Destroy The Sun", "The year is 2525. Mankind is still alive, but for how long?", Color.Yellow, _sun);
                CameraManager.MovmentType = CameraMovmentType.OnPlayer;
                CameraManager.TargetZoom = 0.5f;
                // CameraManager.ZoomType = CameraZoomType.Auto; //  CameraZoomType.ToTargetZoom;

                return true;
            }

            return false;
        }

        private LevelStateMachine.StateOutput Awaken_State(int time)
        {
            this.CameraManager.ZoomType = CameraZoomType.Manual;
            if (time > 60)
            {
               // DialogManager.AddDialog("proIntro1"); //TODO: move to diffrent state
                AddMissions();
                return true;
            }
            return false;
        }
        


        private LevelStateMachine.StateOutput Missions_state(int time)
        {
            
            return true;
        }




        #endregion

        bool didStun = false;
        public override void UpdateScript(InputState inpuState)
        {
            if(MetaWorld.Inst.CheckBlackboard("KemAttack"))
            {
                GameEngine.GetFaction(ENEMY_FACTION).SetRelationToFaction(GameEngine, FactionType.Player, -1);
                //ActivityManager.Inst.AddToast("Attaakk!", 10);
            }

            if(_commandStation != null && _commandStation.IsNotActive && !didStun) //change to event on station death
            {
                didStun = true;
                StunAllShips();
            }

            Vector2 targetPos = PLAYER_START_POSITION;
            if (_player?.IsActive == true)
                targetPos = _player.Position;
                                    
            if ((GameEngine.GetFaction(FactionType.Federation).GetMinRelationBetweenFactions(FactionType.Player) >= 0f)
                && _kemron?.IsActive == true)
            {
                // Point Kemron in the general direction of the player, but don't have him pursue past a certain distance from the sun
                float distance = Math.Min(20000, targetPos.Length());
                targetPos.Normalize();
                targetPos *= distance;
                _kemronGoal.Position = targetPos;
                _kemron.SetTarget(_kemronGoal, TargetType.Goal);
            }            

            if (_endLevelTimer == 0)
            {
                _levelStateMachine.ActivateState();
            }

            //  HandleReviveTextBox();
            HandleEndLevel();

            if (_sun.IsNotActive && _player != null && _player.IsActive)
            {
                if ((_player.ControlSignal & ControlSignals.OnDamageToShield) > 0 || (_player.ControlSignal & ControlSignals.OnDamageToHull) > 0)
                {
                   // AddGameObject(Consts.WARPIN_EFFECT, FactionType.Neutral, _player.Position);
                  //  _player.IsActive = false;
                }
            }
        }

      
        private void HandleEndLevel()
        {
            MetaWorld.Inst.CodexManager._missions.Clear();
            if (_sun.IsNotActive || (_player != null && _player.IsNotActive))
            {
                // Player and/or sun are dead; everyone is either happy or dead
                UpdateZoomToSunExplosion();
                _endLevelTimer++;
                if (_endLevelTimer > 1)
                {
                    fadeAlpha = (_endLevelTimer) / (float)FADE_EFFECT_LENGTH;
                }
            }

            if (_endLevelTimer > FADE_EFFECT_LENGTH)
            {
                _endLevelTimer++;

                fadeAlpha = 1;

                if (_endLevelTimer > FADE_EFFECT_LENGTH + 60)
                {
                    NextLevel();
                }
            }
        }

        private void UpdateZoomToSunExplosion()
        {
            CameraManager.TargetZoom = 0.08f;
            CameraManager.MovmentType = CameraMovmentType.ToTarget;
            CameraManager.CameraMovmentAcceleration = 1f;
            CameraManager.CameraMovmentFactor = 0f;
            CameraManager.CameraMovmentSpeed = 0;
            CameraManager.TargetPosition = _sun.Position;
            CameraManager.ZoomType = CameraZoomType.ToTargetZoom;
        }

        private void NextLevel()
        {
            var parameters = new ActivityParameters();
            var ships = GlobalData.Inst.GetData("starting_ships").Split(',');
            string startingShip = ships[int.Parse(shipIndexSelected)];
            if (_sun.IsNotActive)
            {
                //shipIndexSelected

                //parameters.ParamDictionary.Add("loa", "StartingShibB");
                parameters.ParamDictionary.Add("loadout", startingShip);
                parameters.ParamDictionary.Add("title", "Your Journey Begins");
                parameters.DataParams.Add("Scene", this);                
                
               // parameters.ParamDictio nary.Add("title", "What have you done?");
                parameters.ParamDictionary.Add("text", TextBank.Inst.GetTextAsset("smProEnd").Text);
                ActivityManager.Inst.SwitchActivity("SplashScreen", parameters, false);
              //  GameSession.Inst.Continue();
            }
            else
            {
                GetPlayerFaction().MothershipHanger.AddShipCopyToSlot(0, startingShip);
                ActivityManager.Inst.SwitchActivity("Prolog", activityParams, false);
            }
        }



        private void InitStateMachine()
        {
            // Tutorial
            _levelStateMachine.AddState(this, MoveCameraToPlayer_State);
            _levelStateMachine.AddState(this, Awaken_State);
            _levelStateMachine.AddState(this, Missions_state);
        }

        private void SetCameraFromSunToShip()
        {
            CameraManager.MovmentType = CameraMovmentType.ToTarget;
            CameraManager.CameraMovmentAcceleration = 1f;
            CameraManager.CameraMovmentFactor = 0f;
            CameraManager.CameraMovmentSpeed = 0;
            CameraManager.TargetPosition = PLAYER_START_POSITION;
            CameraManager.ZoomType = CameraZoomType.ToTargetZoom;
            CameraManager.TargetZoom = 0.1f;
            //ManualZoom.ManualTargetZoom = 0f;
            Camera.Zoom = 1f;
        }
        

        #region WorldBuilding

        private void AddPlayerShip()
        {
            if (string.IsNullOrEmpty(_playerShipName))
            {
                _playerShipName = "PrologueShip3";
            }

            _player = AddGameObject(_playerShipName, PLAYER_START_POSITION, -90, FactionType.Player, AgentControlType.Player) as Agent;
            _player.AddItemToInventory(typeof(StarDestroyerItem).Name, 1);
            _player.SetMeterValue(MeterType.StunTime, 20);
            // _player.AddItemToInventory("EmpRecoveryKit1", 40);
            _player.SetAggroRange(5000, 10000, TargetType.Enemy);

            // HyperSpace jump effect
            AddGameObject(typeof(HyperSpaceJumpFx).Name, _player.Position);
            _player.AddItemToInventory("VacuumModulator5");
            //AddGameObject("sound_SPACE_WARP", _player.Position);            
        }

        private void AddGameObjects()
        {
            _sun = AddGameObject("SunWithBackground", Vector2.Zero);
            AddGameObject("SunFullscreenColorFx", Vector2.Zero);
            var planet = AddGameObject("Earth", PLAYER_START_POSITION * 0.7f);
            planet.Name = "";
            planet.Parent = _sun;
        }

        private void AddEnemyShips()
        {
                       
            //Inner group
            _enemyShips.AddRange(AddShipsGroups(8, _sun.Size * 1.5f, _enemyShipNamesInnerRing));
            //Outer group
            _enemyShips.AddRange(AddShipsGroups(4, PLAYER_START_POSITION.Y / 2.6f, _enemyShipNamesOuterRing));

            _commandStation = AddGameObject("SolarMiner", Vector2.UnitY* (_sun.Size * 1.5f+ 2500), -MathHelper.Pi, ENEMY_FACTION);
            _commandStation.Name = "Command Station";
            var kemronGroup = AddShipsGroups(1, PLAYER_START_POSITION.Y / 2 - 5000, _enemyKemronGroup);
            _kemron = kemronGroup[0];
            _kemron.Name = "Kemron";
            _enemyShips.AddRange(kemronGroup);
        }

        private List<GameObject> AddShipsGroups(int numOfGroups, float rad, List<string> ships)
        {
            List<GameObject> gameObjects = new List<GameObject>();
            for (int i = 0; i < numOfGroups; i++)
            {
                float rotation = i / (float)numOfGroups * MathHelper.TwoPi + MathHelper.PiOver2;
                Vector2 pos = FMath.ToCartesian(rad, rotation);
                var groupObjects = GameEngine.AddObjectsInFormation(ships, ENEMY_FACTION, pos, MathHelper.ToDegrees(rotation), 50);
                gameObjects.AddRange(groupObjects);
            }
            return gameObjects;
        }

        private void StunAllShips() //Stuns all ships for 20 min
        {
            foreach (var ship in _enemyShips)
            {
                ship.SetMeterValue(MeterType.StunTime, 60 * 60 * 20);
            }
        }

        #endregion
        public static Activity ActivityProvider(string parameters)
        {
            return new Prolog();
        }


    }
}
