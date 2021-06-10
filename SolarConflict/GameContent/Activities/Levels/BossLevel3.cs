//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using XnaUtils;
//using Microsoft.Xna.Framework;
//using SolarConflict.GameContent.Items;
//using SolarConflict.NewContent.Levels;
//using SolarConflict.NewContent.Emitters;
//using SolarConflict.Framework;

//namespace SolarConflict.GameContent.Activities
//{
//    [Serializable]
//    class BossLevel3 : Scene
//    {
       


//        public BossLevel3(string parameters)
//            : base(parameters)
//        {
//        }
//        Agent playerShip, boss;
//        uint time;
//        LevelStateMachine lsm;
//        IEmitter devastationEmitter;
//        bool gameover;

//        public override void InitScript(string parameters, ActivityParameters activityParameters = null)
//        {
//            lsm = new LevelStateMachine();
//            gameover = false;
//            devastationEmitter = ContentBank.Inst.GetEmitter(typeof(DevastationEmitter).Name);
//            time = 0;
//          //  playerShipPosition = PersistentWorld.World.FindPlayer().Position; ////Add this functinalty to scene

//            boss = (Agent)AddGameObject("PlayerClone", FactionType.Federation, Vector2.Zero, 0, AgentControlType.AI);
//            boss.Inventory.Clear();
//            boss.AddItem((Item)ContentBank.Inst.GetGameObjectFactory(typeof(AutoEnergyKit).Name));
//            Item item = (Item)ContentBank.Inst.GetGameObjectFactory(typeof(AutoRepairKit).Name);
//            item.Stack = 5;
//            boss.AddItem(item);
//            boss.AddItem((Item)ContentBank.Inst.GetGameObjectFactory(typeof(VacuumModulatorItem).Name));


//            //save ship pos
//            //store all players inventory

//            playerShip = (Agent)AddGameObject("PlayerClone", FactionType.Player, Vector2.UnitY * 20000, 0, AgentControlType.Player);
//            playerShip.SetTarget(boss, TargetType.Goal);
//            this.AddObjectRandomlyInCircle("Asteroid", 500, 10000);
//            this.AddObjectRandomlyInCircle("BigAsteroid", 500, 10000);
//            InitStateMachine();
//        }

//        private void InitStateMachine()
//        {
//            lsm.AddState(this, (int time) =>
//                {
//                    var res = AddTextBox("This is the Oracle's domain. She has two lines of defense that you need to survive if you want to talk to her.", null, null, Consts.AI_HELPER_TEXTURE_ID, "intro", true);
//                    return (res==null);
//                });
//            lsm.AddState(this, (int time) =>
//                {
//                    AddTextBox("Avoid the rings of fire!", null, null, Consts.AI_HELPER_TEXTURE_ID, "avoid", false);
//                    if (GameEngine.FrameCounter % 120 == 0)
//                    {
//                        devastationEmitter.Emit(GameEngine, null, FactionType.Federation, PlayerAgent.Position + Vector2.UnitY * 800, Vector2.Zero, 0f);
//                    }
//                    if (Institute.DistanceFromEdge(boss, PlayerAgent) < 6000)
//                        return 1;
//                    return 0;
//                });
//            lsm.AddState(this, (int time) =>
//                {
//                    var res = AddTextBox("You've passed through the first wave of the Oracle's defenses.\n" +
//                        "Now you'll have to defeat her guardian.", null, null, Consts.AI_HELPER_TEXTURE_ID, "guardian", true);
//                    return res == null;
//                });
//            lsm.AddState(this, (int time) =>
//                {
//                    if (boss.IsNotActive)
//                        return 1;
//                    return 0;
//                });
//            lsm.AddState(this, (int time) =>
//                {
//                    AddGameObject("SmallShip1a", 0, boss.Position, boss.Rotation, AgentControlType.None);
//                    var res = AddTextBox("Welcome, Traveler.\nYou must have a great many questions, but our time is short.", null, null, "person1", "1", true);
//                    return res==null;
//                });
//            lsm.AddState(this, (int time) =>
//                {
//                    var res = AddTextBox("A great evil is threatening the galaxy. The Void is out to consume every living being. He is on his way to Sol right now.\n" +
//                        "You were once a servant of The Void, but on your latest mission, something went wrong.\n" +
//                        "After destroying a star, you were caught in its nova, and that disrupted The Void's hold on you for long enough for you to regain your free thought.",
//                        null, null, "person1", "2", true);
//                    return res == null;
//                });
//            lsm.AddState(this, (int time) =>
//            {
//                var res = AddTextBox("The Void is on its way to Sol right now, if left unchecked it will destroy the sun and end humanity.\n" +
//                    "Should you wish to alter humanity's fate, you must hurry. I can send you back to Sol, the rest is up to you.",
//                    null, null, "person1", "3", true);
//                return res==null;
//            });
//            lsm.AddState(this, (int time) =>
//                {
//                    gameover = true;
//                    ActivityManager.Back();
//                    return 0;
//                });
//        }

//        public override int UpdateScript(InputState inpuState)
//        {
//            if (playerShip.IsNotActive)
//            {
//                time++;
//                if (time > 60 * 3)
//                {
//                    ActivityManager.Back();
//                }
//            }
//            lsm.ActivateState();
//            return 0;
//        }

//        public override ActivityParameters OnBack()
//        {
//            ActivityParameters par = new ActivityParameters();
//            par.ParamDictionary.Add("gameover", gameover.ToString());
//            return par;
//        }



//        public static Activity ActivityProvider(string parameters)
//        {
//            //MethodBase.GetCurrentMethod().DeclaringType.GetConstructor(System.Type.EmptyTypes).Invoke()
//            return new BossLevel3(parameters);
//        }
//    }
//}
