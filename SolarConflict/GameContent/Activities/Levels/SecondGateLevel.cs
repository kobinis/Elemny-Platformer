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

namespace SolarConflict.GameContent.Activities
{



    [Serializable]
    public class DetonatorProcess : GameProcess, IEmitter
    {
        private int VerticalGoal;


        public DummyObject parent;
        public Vector2 PlayerOffset = Vector2.UnitY * 2000;
        public Vector2 RandomRange;
        public IEmitter Emitter;


        public Vector2 _initPos;

        public IEmitter AsteroidEmitter;

        public string ID { get; set; }

        public DetonatorProcess(int verticalGoal)
        {
            VerticalGoal = verticalGoal;
            RandomRange = new Vector2(00, 300);
            Emitter = ContentBank.Get("DevastationLineEmitter");
            _initPos = new Vector2(0, 2000);

            AsteroidEmitter = ContentBank.Get("SmallAsteroid1");

            parent = new DummyObject();
            parent.Faction = FactionType.Void;
        }


        public override void Update(GameEngine gameEngine)
        {

         //   ActivityManager.Inst.AddToast(_initPos.ToString() + "  " + VerticalGoal.ToString() + "  " + gameEngine.PlayerAgent.Position.ToString(), 10000);
            if (_initPos.Y < VerticalGoal)
            {
                this.Finished = true;
               
               // ActivityManager.Inst.AddToast("Done!!!!!!!!!", 10000);
                return;
            }
            if (gameEngine.PlayerAgent == null)
                return;

           

            if (gameEngine.FrameCounter % 100 == 0)
            {

                Vector2 position = gameEngine.PlayerAgent.Position - Vector2.UnitY * 3000   + new Vector2((gameEngine.Rand.NextFloat() * 2 - 1) * 2000,0);
                AsteroidEmitter.Emit(gameEngine, null, FactionType.Void, position, -15 * Vector2.UnitY, 0, 0,0, gameEngine.Rand.Next(100,150));
            }

            int maxdistance = 2000;
            float speed = 30;

            if (Math.Abs(gameEngine.PlayerAgent.Position.Y - _initPos.Y) > maxdistance)
                _initPos -= 4*Vector2.UnitY * speed;

            _initPos -= Vector2.UnitY * speed;

            _initPos.Y = Math.Max(gameEngine.PlayerAgent.Position.Y, _initPos.Y);
            

            if (gameEngine.FrameCounter % 2  == 0)
            {
                
                Vector2 position = _initPos + PlayerOffset + new Vector2(0, (gameEngine.Rand.NextFloat() *2 -1 ) * 500) ;

                position.X = gameEngine.PlayerAgent.Position.X;

             
              


                Emitter.Emit(gameEngine, parent, FactionType.Void, position, -(speed+5) * Vector2.UnitY, 0 , 0);
            }
        }

        public override GameProcess GetWorkingCopy()
        {
            return (GameProcess)MemberwiseClone();
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            var process = GetWorkingCopy();
            gameEngine.AddGameProcces(process);
            return null;
        }
    }

    /// <summary>
    /// The first node the player starts in, to replace StartingNode
    /// </summary>
    [Serializable]
    class SecondGateLevel : Scene
    {
        private int verticalGoal = -90000;

        public SecondGateLevel() : base()
        {
            
        }
        bool Won = false;

        


        public override void InitScript(string parameters, ActivityParameters activityParameters = null)
        {
            MetaWorld = null;
            SetBackground(6);

            IsConfirmQuitNeeded = true;
            this.IsShipSwitchable = false;
            this.SaveOnExit = false;
            //this.IsPopup = true;
            UpdatePlayerShip = false;    
            
            
            background = new Background(0); //TOTO: change to sim background
            
            SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.MissionLog);
            if (DebugUtils.Mode == ModeType.Debug || DebugUtils.Mode == ModeType.Test)
            {
                SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.Inventory);
                SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.TacticalMap);
            }

            CameraManager.TargetZoom = Consts.CAMERA_MAX_ZOOM;

            //this.AddObjectRandomlyInLocalCircle("Asteroid1", 6, 1500, PlayerStartingPoint - Vector2.UnitX * 1500 - Vector2.UnitY * 3000);
            //this.AddObjectRandomlyInLocalCircle("SmallAsteroid1", 6, 1600, PlayerStartingPoint - Vector2.UnitX * 1500 - Vector2.UnitY * 3000);

            // this.AddMission(MissionFactory.EnteringInterStllerSpace(50000)); //proccess

            GameEngine.Level = 1;

            CameraManager.TargetZoom = Consts.CAMERA_MIN_ZOOM;


            GameEngine.Update(InputState.EmptyState);

            // string playerShip = "Federation2";
            string playerShip = "PlayerClone";//"PirateLord1";

            var enemyClone = AddGameObject(playerShip, (verticalGoal - 21000) * Vector2.UnitY, 180, FactionType.Void, AgentControlType.AI);
            enemyClone.GetInventory().Clear();

            this.AddObjectRandomlyInLocalCircle("WormAsteroid1", 7, 2000, enemyClone.Position, 1000); //TODO

            MissionChainingHelper ch = new MissionChainingHelper(this);

           // this.AddObjectInLine("SpeedBooster", -Vector2.UnitY * 2000, -Vector2.UnitY * 6000, spacingMultiplier: 9);

            int boosterSize = 300;
            float mult = 1f;
            int n = (int)(Math.Abs(verticalGoal) /boosterSize / mult);
            int offset = 1000;
            int amp = 1700;
            for (int i = 0; i <  n; i++)
            {
                int yPos = -(int)(i * boosterSize * mult);
                int xPos = (int)( Math.Sin(yPos / 2050f) * amp + Math.Sin(yPos / 1505f) * amp * 0.5f);
                Vector2 pos = new Vector2(xPos, yPos - offset);
                yPos = -(int)((i+1) * boosterSize * mult);
                xPos = (int)(Math.Sin(yPos / 2050f) * amp + Math.Sin(yPos / 1505f) * amp * 0.5f);
                Vector2 pos1 = new Vector2(xPos, yPos - offset);
                this.AddObjectInLine("SpeedBooster", pos,  pos1 - Vector2.UnitY *10, spacingMultiplier: 5);
            }

            //FirstGate

            this.AddGameObject("FirstGateRight", -Vector2.UnitX* 2000, 0);
            this.AddGameObject("FirstGateLeft", Vector2.UnitX * 2000, 0);
            var oracle = this.AddGameObject("Oracle", (verticalGoal - 33000) * Vector2.UnitY, 0);

            int num = 6;
            for (int i = 0; i < num; i++)
            {
                var fly = AddGameObject("Fireflys", oracle.Position + FMath.ToCartesian(300,i /(float)num * MathHelper.TwoPi), 0);
                fly.Parent = oracle;
            }
            

            var playerClone = AddGameObject(playerShip, Vector2.UnitY * 5000, 180, FactionType.Player, AgentControlType.Player);
            playerClone.GetInventory().Clear(); //Clear clone inventroty //Don't clear ammo!!!

            var gateMission = MissionFactory.MissionQuickStart("glGate");
            gateMission.Objective =  new GoToPositionObjective(Vector2.Zero, 1000);
            ch.Add(gateMission);
           
            gateMission.OnMissionCompletion += (Mission mission, Scene scene) => {  };
            //GameEngine.AddGameProcces(new DetonatorProcess());

            var startFlameMission = new Mission();
            startFlameMission.IsHidden = true;
            startFlameMission.IsGoalHidden = true;
            startFlameMission.Objective = new VerticalGoalObjective(-offset); // 
            startFlameMission.EmitterOnComplete = new DetonatorProcess(verticalGoal + 10500);
            AddMission(startFlameMission);

            var finishGate = MissionFactory.MissionQuickStart("glFirstGate");
            finishGate.Objective = new GoToPositionObjective(Vector2.UnitY * (verticalGoal  - 15500), 6000);
            ch.Add(finishGate);
            //finishGate.EmitterOnComplete = new DetonatorProcess(verticalGoal);

            var destroyClone = MissionFactory.MissionQuickStart("glGateClone");
            destroyClone.Objective = new DestroyTargetObjective(enemyClone);
            destroyClone.Color = Color.Red;
            ch.Add(destroyClone);
      
            var gotoOracle = MissionFactory.MissionQuickStart("glOracle");
            gotoOracle.Objective = new GoToTargetObjective(oracle);
            gotoOracle.Color = Color.Yellow;
            ch.Add(gotoOracle);

            var endMission = new Mission();
            endMission.OnMissionCompletion += EndMission_OnMissionCompletion;
            endMission.Objective = new MinimumTimeObjective(2);
            ch.Add(endMission);
        }

        private void EndMission_OnMissionCompletion(Mission mission, Scene scene)
        {
            Won = true;
           // Session.GameSession.Inst.MetaWorld
            Session.GameSession.Inst.MetaWorld.Blackboard["oracle"] = "won";
        }

        int cooldown = 0;
        
        public override void Update(InputState inputState)
        {
        
            base.Update(inputState);
            if(!this.PauseGameEngine && (  !this.IsPlayerInScene || Won))
            {
                this.fadeAlpha = Math.Min(fadeAlpha + 0.01f, 1);
                cooldown++;
            }
            if (cooldown == 100)
                ActivityManager.Inst.Back();

            //if (this.PlayerAgent != null && this.PlayerAgent.Position.Y < 100)
            //{
            //    CameraManager.TargetZoom = Consts.CAMERA_MIN_ZOOM;
            //}

        }

        public override void DrawScript(SpriteBatch sb)
        {
            //sb.Begin();
            //sb.DrawString(Game1.menuFont, PlayerAgent.Position.ToString(), Vector2.One * 200, Color.Purple);
            //sb.End();
        }

        

        public static Activity ActivityProvider(string parameters = "")
        {
            return new SecondGateLevel();
        }

    }
}