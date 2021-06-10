using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using SolarConflict.Framework;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework.CameraControl.Zoom;
using SolarConflict.Framework.Emitters;
using SolarConflict.Framework.Emitters.SceneRelated;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.Framework.GUI;
using SolarConflict.Framework.Scenes;
using SolarConflict.Framework.Scenes.DialogEngine;
using SolarConflict.Framework.Scenes.HudEngine;
using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.Activities.SceneActivitys;
using SolarConflict.GameContent.Agents;
using SolarConflict.GameContent.Emitters;
using SolarConflict.GameContent.Items;
using SolarConflict.GameContent.NewItems;
using SolarConflict.GameContent.NewItems.Utils;
using SolarConflict.Generation;
using SolarConflict.Session;
using SolarConflict.Session.World.MissionManagment;
using SolarConflict.Session.World.MissionManagment.Objectives;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using XnaUtils;
using XnaUtils.Graphics;

//TODO: fix camera 
namespace SolarConflict.GameContent.Activities
{
    [Serializable]
    public class PrologWave : Scene
    {
        #region Constants
        private const float WORLD_SCALE = 3;
        //private const float SCALE_DISTANCE_FROM_SUN = 0.7f;        
        private readonly Vector2 PLAYER_START_POSITION = Vector2.UnitY * 11000 * WORLD_SCALE;
        private const int FADE_EFFECT_LENGTH = 60 * 3;
        private const int NUM_INNER_GROUPS = 8;
        private const int NUM_OUTER_GROUPS = 4;
        private const FactionType ENEMY_FACTION = FactionType.Federation;
        #endregion

        #region Fields        
        private List<string> _enemyShipNamesInnerRing = new List<string> { "PrologueEnemy2", "PrologueEnemy2", "PrologueEnemy2" };
        private List<string> _enemyShipNamesOuterRing = new List<string> { "PrologueEnemy2", "PrologueEnemy1", "PrologueEnemy1", "PrologueEnemy1", "PrologueEnemy1", "PrologueEnemy1" }; // kobi: add more types of federation ships 
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
        GameObject _firstEnemy;
        ActivityParameters activityParams;
        #endregion

        //public Prolog() { }

        public override void InitScript(string parameters, ActivityParameters activityParameters = null)
        {
            //HudManager.AddComponent(new AnnouncerEffects());
            activityParams = activityParameters;
            IsConfirmQuitNeeded = true;
            _playerShipName = "PrologShip2B";
            if (activityParameters.ParamDictionary.ContainsKey("loadout"))
            {
                _playerShipName = activityParameters.ParamDictionary["loadout"];
            }
            background = new Background(7);
            _enemyShips = new List<GameObject>();
            PersistenceManager.Inst.NewSession(null);
            //SceneComponentSelector.AddComponent(SceneComponentType.MissionLog);
            //SceneComponentSelector.AddComponent(SceneComponentType.Inventory);//, "InventoryActivityTutorial");
            //SceneComponentSelector.AddComponent(SceneComponentType.TacticalMap);
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
            _firstEnemy = AddGameObject("PrologFirstEnemy", PlayerStartingPoint - Vector2.UnitX * 200 - Vector2.UnitY * 6500, 90, FactionType.Pirates1);
        }

        private void AddMissions()
        {
            // First enemy - aggro range.
            _firstEnemy.SetAggroRange(650, 5000, TargetType.Enemy);

            // Will instantly show with next texts.
            DialogManager.AddDialog("PW1.1_Arrival"); // Finish PW1

            Mission moveToSunPW2Mission = MissionFactory.MissionQuickStart(null);
            moveToSunPW2Mission.DialogOnStartID = "PW2.1_FollowArrow";
            // Offset from center of object
            moveToSunPW2Mission.Objective = new GoToTargetObjective(_sun, 1300);
            moveToSunPW2Mission.Color = Color.Green;
            AddMission(moveToSunPW2Mission);

            Mission meetPatrolPW3Mission = MissionFactory.MissionQuickStart(null);
            meetPatrolPW3Mission.DialogOnCompleteID = "PW3.1_PatrolStart";
            meetPatrolPW3Mission.Objective = new GoToTargetObjective(_kemron, 1300);
            meetPatrolPW3Mission.IsGoalHidden = true;
            meetPatrolPW3Mission.OnMissionCompletion += (mission, scene) => { moveToSunPW2Mission.IsGoalHidden = true; };
            AddMission(meetPatrolPW3Mission);

            var options2 = meetPatrolPW3Mission.DialogOnComplete.GetDialogOptions();
            options2[0].OnSelect += (dialog, scene) =>
            {
                Mission mission = new Mission();
                mission.Objective = new MinimumTimeObjective(60);
                var groupMeter = new GroupEmitter();
                var metterSetter = new MeterSetterEmitter();
                metterSetter.Faction = ENEMY_FACTION;
                metterSetter.AddMeter(MeterType.StunTime, 60 * 60 * 10);
                groupMeter.AddEmitter(metterSetter);
                groupMeter.AddEmitter(ContentBank.Inst.GetEmitter("FullScreenColorFX"));
                mission.EmitterOnComplete = groupMeter;
                //scene.GameEngine.AddGameProcces(new WaveGameProcces(mission));
                scene.GameEngine.GetFaction(ENEMY_FACTION).SetRelationToFaction(scene.GameEngine, FactionType.Player, -1);    
                
            };


            Mission shotMission = MissionFactory.MissionQuickStart("pw_Shot");
            shotMission.Color = Color.Red;
            shotMission.Objective = new DestroyTargetObjective(_firstEnemy);
            moveToSunPW2Mission.NextMissionOnComplete = shotMission;
            moveToSunPW2Mission.NextMissionOnFail = shotMission;

            Mission destroyControl = MissionFactory.MissionQuickStart("pw_DestroyCommand");
            //destroyControl.DialogOnStart.IsInterupting = true;
            destroyControl.Color = Color.Red;
            destroyControl.Objective = new DestroyTargetObjective(_commandStation);
            shotMission.NextMissionOnComplete = destroyControl;

            Mission goToSunMissionNext = MissionFactory.MissionQuickStart("pw_GoToSun");
           // goToSunMissionNext.DialogOnStart.IsInterupting = true;
            goToSunMissionNext.Objective = new GoToTargetObjective(_sun, 0, 1.5f);
            destroyControl.NextMissionOnComplete = goToSunMissionNext;

            Mission equipDevice = MissionFactory.MissionQuickStart("pw_EquipDevice");
            equipDevice.EmitterOnStart = new SceneComponentEmitter(SceneComponentType.Inventory);
            equipDevice.Objective = new AcquireItemObjective("StarDestroyerItem", 1, true, new TutorialGoal("StarDestroyerItem", false));
            goToSunMissionNext.NextMissionOnComplete = equipDevice;

            Mission destroySun = MissionFactory.MissionQuickStart("pw_DestroySun");
            destroySun.Color = Color.Red;
            destroySun.Objective = new DestroyTargetObjective(_sun, 500);
            equipDevice.NextMissionOnComplete = destroySun;
            //Hidden missions
           // Mission meetKemron = MissionFactory.MissionQuickStart("proMeetKemron");
           //// meetKemron.DialogOnComplete.IsInterupting = true;
           // meetKemron.IsGoalHidden = true;
           // meetKemron.Objective = new GoToTargetObjective(_kemron, 1200);
           // meetKemron.NextMissionOnComplete = destroyControl;
           // var options = meetKemron.DialogOnComplete.GetDialogOptions();
           // options[0].OnSelect += (dialog, scene) =>
           // {
           //     scene.GameEngine.GetFaction(ENEMY_FACTION).SetRelationToFaction(scene.GameEngine, FactionType.Player, -1);
           //     scene.GameEngine.AddGameProcces(new WaveGameProcces());
           // };
            //options[2].OnSelect += (dialog, scene) => { scene.GameEngine.GetFaction(ENEMY_FACTION).SetRelationToFaction(scene.GameEngine, FactionType.Player, -1); };
            //MissionManager.AddMission(meetKemron);

            Mission attackZone = MissionFactory.MissionQuickStart("pw_AttackZone");
            attackZone.IsGoalHidden = true;
            //attackZone.IsHidden = false;
            attackZone.Color = Color.Yellow;
            var objectiveGroup = new ObjectiveGroup();
            objectiveGroup.AddObjective(new GoToPositionObjective(Vector2.Zero, PLAYER_START_POSITION.Y / 3 + 1000)); //change to offset only
            objectiveGroup.AddObjective(new AllianceObjective(ENEMY_FACTION));
            objectiveGroup.AddObjective(new DefendTargetObjective(_kemron));
            attackZone.Objective = objectiveGroup;
            attackZone.OnMissionCompletion += (mission, scene) => { scene.GameEngine.GetFaction(ENEMY_FACTION).SetRelationToFaction(scene.GameEngine, FactionType.Player, -1); };
            AddMission(attackZone);

            Mission kemronDie = MissionFactory.MissionQuickStart("pw_KemronDie");
            kemronDie.IsGoalHidden = true;
            kemronDie.Objective = new DestroyTargetObjective(_kemron);
            AddMission(kemronDie);
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

            if (_commandStation != null && _commandStation.IsNotActive && !didStun) //change to event on station death
            {
                didStun = true;
                StunAllShips();
            }

            if ((GameEngine.GetFaction(FactionType.Federation).GetMinRelationBetweenFactions(FactionType.Player) >= 0f)
                && (_kemron?.IsActive == true) && (_player?.IsActive == true))
            {
                // Point Kemron in the general direction of the player, but don't have him pursue past a certain distance from the sun
                Vector2 goal = _player.Position;
                goal.Normalize();
                float distance = Math.Min(20000, _player.Position.Length());
                goal *= distance;
                _kemronGoal.Position = goal;
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
                    AddGameObject(Consts.WARPIN_EFFECT, FactionType.Neutral, _player.Position);
                    _player.IsActive = false;
                }
            }
        }


        private void HandleEndLevel()
        {
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
            if (_sun.IsNotActive)
            {
                parameters.DataParams.Add("Scene", this);
                parameters.ParamDictionary.Add("title", "What have you done?");
                parameters.ParamDictionary.Add("text", TextBank.Inst.GetTextAsset("proEnd").Text);
                ActivityManager.Inst.SwitchActivity("SplashScreen", parameters, false);
            }
            else
            {
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
            CameraManager.TargetZoom = 0.08f;
            //ManualZoom.ManualTargetZoom = 0f;
            Camera.Zoom = 1f;
        }


        #region WorldBuilding

        private void AddPlayerShip()
        {
            if (string.IsNullOrEmpty(_playerShipName))
            {
                _playerShipName = "PrologShip2B";
            }

            _player = AddGameObject(_playerShipName, PLAYER_START_POSITION, -90, FactionType.Player, AgentControlType.Player) as Agent;
            _player.AddItemToInventory(typeof(StarDestroyerItem).Name, 1);
            _player.SetMeterValue(MeterType.StunTime, 50);

            // HyperSpace jump effect
            AddGameObject(typeof(HyperSpaceJumpFx).Name, _player.Position);
            AddGameObject("sound_SPACE_WARP", _player.Position);
        }

        private void AddGameObjects()
        {
            _sun = AddGameObject("SunWithBackground", Vector2.Zero);
            AddGameObject("SunFullscreenColorFx", Vector2.Zero);
            var planet = AddGameObject("Earth", new Vector2(-1,1) * PLAYER_START_POSITION.Y * 2 / 4);
            planet.Name = "New Erath";
            planet.Parent = _sun;
        }

        private void AddEnemyShips()
        {

            //Inner group
            _enemyShips.AddRange(AddShipsGroups(8, _sun.Size * 1.5f, _enemyShipNamesInnerRing));
            //Outer group
            _enemyShips.AddRange(AddShipsGroups(4, PLAYER_START_POSITION.Y / 2, _enemyShipNamesOuterRing));

            _commandStation = AddGameObject("SolarMiner", Vector2.UnitY * (_sun.Size * 1.5f + 2500), -MathHelper.Pi, ENEMY_FACTION);
            _commandStation.Name = "CommandStation";
            var kemronGroup = AddShipsGroups(1, PLAYER_START_POSITION.Y / 2 + 2000, _enemyKemronGroup);
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
            return new PrologWave();
        }
    }               
}
