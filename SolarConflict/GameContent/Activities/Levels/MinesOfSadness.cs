//using Microsoft.Xna.Framework;
//using SolarConflict.Framework;
//using SolarConflict.GameContent;
//using SolarConflict.GameContent.Agents;
//using SolarConflict.GameContent.Items;
//using SolarConflict.GameContent.NewItems.Utils;
//using System;
//using XnaUtils;

//namespace SolarConflict.GameContent.Activities
//{
//    [Serializable]
//    class MinesOfSadness : Scene
//    {
//        private int _respwanTime = 60*60; 
//        private LevelStateMachine _lsm;
//        private GameObject _boss;
//        private GameObject _playerShip;
//        private int _bossRespawnTimer = 0;
//        //private Institute _portal; 

//        public MinesOfSadness(string parameters)
//            : base(parameters)
//        {
            
//        }

//        public override void InitScript(string parameters, ActivityParameters activityParameters = null)
//        {
//            CreateBoss();
//            CreatePortal();
//            this.ActionOnPlayerDeath = ActionOnPlayerDeathType.Back;
//            this.AddObjectRandomlyInCircle("Asteroid", 500, 10000);
//            this.AddObjectRandomlyInCircle("BigAsteroid", 500, 10000);
//            _lsm = new LevelStateMachine();

//            InitStateMachine();
//        }

        


//        protected Agent CreatePortal(string activityName = "back") //Move to GameEngine Utils
//        {
//            //Institute portal = (Institute)AddGameObject(typeof(Portal).Name, 0, Vector2.Zero, 0);
//            //portal.ActivityName = activityName;
//            //portal.Message = "Press F to enter";
//            throw new NotImplementedException();
//           // return portal;
//        }

//        private void CreateBoss()
//        {
//            _boss = AddGameObject("LargeShip1A", Vector2.UnitX * -3000 + Vector2.UnitY * - 4000, 0, FactionType.Pirates1, AgentControlType.AI);            
//            //_boss.AddItemToInventory((Item)ContentBank.Inst.GetGameObjectFactory(typeof(VacuumModulatorItem).Name));            
//        }

//        public override void OnEnter(ActivityParameters parameters)
//        {

//            base.OnEnter(parameters);
//            _playerShip = AddGameObject("Player", FactionType.Player, Vector2.Zero, 0, AgentControlType.Player);
//            //_playerShip.SetTarget(_portal, TargetType.Goal);

//            if (_boss.IsActive)
//            {
//                _playerShip.SetTarget(_boss, TargetType.Goal);
//            }
//        }

//        public override ActivityParameters OnBack()
//        {
//            _playerShip.SetTarget(null, TargetType.Goal);
//            GameEngine.RemoveGameObject(_playerShip);
//            return base.OnBack();
//        }

//        private void InitStateMachine()
//        {
//            _lsm.AddState(this, (int time) =>
//                {
//                    if (_boss.IsNotActive)
//                        return 1;
//                    return 0;
//                });
//            _lsm.AddState(this, (int time) =>
//                {
//                    if (time == 1)
//                    {
//                        AddTextBox("YES! You destroyed the Ancient Miner! Make sure you pick up the mining laser.", null, null, Consts.AI_HELPER_TEXTURE_ID, "KilledHim", soundID: "destoryanicnetminder");
//                        var miningLaser = AddGameObject("MiningLaserItem1", _boss.Position, 0, FactionType.Neutral, AgentControlType.None);
//                        _playerShip.SetTarget(miningLaser, TargetType.Goal);
//                    }

//                    if (PlayerAgent.GetInventory().CheckForItem("MiningLaserItem1", 1))
//                    {
//                        return 1;
//                    }                   

//                    return 0;
//                });

//            _lsm.AddState(this, (int time) =>
//            {
//             //   MetaWorld.Inst.MissionManager.FinishMission(MissionsTexts.StartingNode_MinesOfSadness_Name);
//                //_playerShip.SetTarget(_portal, TargetType.Goal);
//                return 1;
//            });
//        }

        
//        public override void UpdateScript(InputState inpuState)
//        {
//            base.UpdateScript(inpuState);

//            _lsm.ActivateState();
//            if(_boss.IsNotActive)
//            {
//                _bossRespawnTimer++;
//                if (_bossRespawnTimer > _respwanTime)
//                {
//                    float angle = (float)GameEngine.Rand.NextDouble() * MathHelper.TwoPi;
//                    _boss = AddGameObject("LargeShip1A", FMath.ToCartesian(9000, angle), angle, FactionType.Pirates1, AgentControlType.AI);
//                    _bossRespawnTimer = 0;
//                }
//            }            
//        }

//        public static Activity ActivityProvider(string parameters)
//        {
//            return new MinesOfSadness(parameters);
//        }
//    }
//}
