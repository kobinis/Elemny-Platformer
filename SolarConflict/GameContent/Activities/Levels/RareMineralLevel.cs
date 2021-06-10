//using Microsoft.Xna.Framework;
//using SolarConflict.AI;
//using SolarConflict.Framework;
//using SolarConflict.GameContent;
//using SolarConflict.GameContent.Agents;
//using SolarConflict.NewContent.Projectiles;
//using System;
//using System.Collections.Generic;
//using XnaUtils;

//namespace SolarConflict.GameContent.Activities
//{
//    [Serializable]
//    class RareMineralLevel : Scene
//    {
//        private List<Vector2> groupCenters = new List<Vector2>() {new Vector2(6200, 6917), new Vector2(-6200, 6917), new Vector2(6500, -6500), new Vector2(-6800, -6654)};
//        private List<GameObject> enemyShips = new List<GameObject>();
//        private LevelStateMachine lsm;
//        private int numDiamonds;
//        private const int neededDiamonds = 4;
//        private Agent _playerShip;
//        private bool diamondsIncreased;
//        private Institute _portal;

//        public RareMineralLevel(string parameters)
//            : base(parameters)
//        {            
//        }

//        public override void InitScript(string parameters = null, ActivityParameters activityParameters = null)
//        {
//            // (this.CallingActivity as Scene) 
//            ActionOnPlayerDeath = ActionOnPlayerDeathType.Back;
            
//            numDiamonds = 0;
//            for (int i = 0; i < groupCenters.Count; i++)
//            {
//                CreateAsteroids(i);
//                //TODO: remove
//                enemyShips.Add(AddGameObject("SmallShip6A", groupCenters[i] + new Vector2(650, 0), 180f, FactionType.Pirates1));
//                enemyShips.Add(AddGameObject("SmallShip6A", groupCenters[i] + new Vector2(350, 110), 180f, FactionType.Pirates1));
//                enemyShips.Add(AddGameObject("SmallShip6A", groupCenters[i] + new Vector2(250, 250), 180f, FactionType.Pirates1));

//                enemyShips.Add(AddGameObject("MediumShip4a", groupCenters[i] + new Vector2(-650, 0), 0f, FactionType.Pirates1));
//            }

//            foreach (var enemyShip in enemyShips)
//            {
//                (enemyShip as Agent).control.SetAIControl(AIBank.Inst.GetControl(5).GetWorkingCopy()); //KOBI: add SetAI to game object
//                enemyShip.SetAggroRange(2500, TargetType.Enemy);
//            }

//            _portal = CreatePortal();

//            _playerShip = (Agent)AddGameObject("Player", FactionType.Player, Vector2.Zero, 0, AgentControlType.Player);
//            _playerShip.SetTarget(null, TargetType.Goal);

//            if (MetaWorld.Inst.MissionManager.ContainAndNotFinishedMission(MissionsTexts.StartingNode_Diamonds_Name))
//            {
//                lsm = new LevelStateMachine();
//                InitLsm();
//            }
//        }

//        protected Institute CreatePortal(string activityName = "back")
//        {
//            Institute portal = (Institute)AddGameObject(typeof(Portal).Name, 0, Vector2.Zero, 0);
//            portal.ActivityName = activityName;
//            portal.Message = "Press F to enter";
//            return portal;
//        }

//        private void CreateAsteroids(int i)
//        {
//            this.AddObjectRandomlyInLocalCircle(typeof(Asteroid).Name, 50, 2000f, groupCenters[i]);
//            this.AddObjectRandomlyInLocalCircle(typeof(BigAsteroid).Name, 50, 2000f, groupCenters[i]);
//            this.AddObjectRandomlyInLocalCircle("DiamondAsteroid", 5, 1000f, groupCenters[i]);
//        }       

//        public override ActivityParameters OnBack()
//        {
//            GameEngine.RemoveGameObject(_playerShip);
//            return base.OnBack();
//        }

//        private void InitLsm()
//        {
//            lsm.AddState(this, (int time) =>
//            {
//                string id = AddTextBox("OK, according to the blueprints, you'll need at least " + neededDiamonds.ToString() + " #color{255,255,0}diamonds#defalutColor{} for this part of the Warp Drive.\n" +
//                    "This area has been reported to have some precious gems recently, try your luck in the surrounding asteroid clumps.", null, null, null, "1", true);                
//                return  id == null;
//            });
//            lsm.AddState(this, (int time) =>
//            {
//                string id = AddTextBox("Watch out, it's a popular mining area, the competition might not be welcome.", null, null, null, "2", true);                
//                return id == null;
//            });
//            lsm.AddState(this, (int time) =>
//            {
//                if (diamondsIncreased )
//                {
//                    int left = neededDiamonds - _playerShip.GetInventory().GetItemCount("DiamondItem");
//                    if (left > 0)
//                    {
//                        string s = "Well done, you need " + left.ToString() + " more diamond" + (left > 1 ? "s" : "");
//                        AddTextBox(s, null, 60*3, null, "diamondCounting", isSkipable:false);
//                    }
//                    else
//                        return 1;
//                }
//                return 0;
//            });
//            lsm.AddState(this, (int time) =>
//            {
//                AddTextBox("Good job, you have all the diamonds you need for the Warp Drive.", null, 200, null, "end", isSkipable:false); 
//                MetaWorld.Inst.MissionManager.FinishMission(MissionsTexts.StartingNode_Diamonds_Name);
//                _playerShip.SetTarget(_portal, TargetType.Goal);

//                return 1;
//            });
//        }
        
//        public override int UpdateScript(InputState inpuState)
//        {
//            base.UpdateScript(inpuState);

//            int prevNum = numDiamonds;

//            if (_playerShip != null)
//            {
//                numDiamonds = _playerShip.GetInventory().GetItemCount("DiamondItem");

//                if (prevNum < numDiamonds)
//                    diamondsIncreased = true;
//                else
//                    diamondsIncreased = false;
//            }

//            if (MetaWorld.Inst.MissionManager.ContainMission(MissionsTexts.StartingNode_Diamonds_Name)) 
//            {
//                lsm.ActivateState();
//            }

//            return 0;
//        }

//        public static Activity ActivityProvider(string parameters)
//        {
//            return new RareMineralLevel(parameters);
//        }
//    }
//}
