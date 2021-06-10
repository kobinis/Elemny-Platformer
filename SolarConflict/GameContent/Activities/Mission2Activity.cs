//using Microsoft.Xna.Framework;
//using SolarConflict.Session.World.MissionManagment;
//using SolarConflict.Session.World.MissionManagment.Objectives;
//using System.Collections.Generic;
//using XnaUtils;

//namespace SolarConflict.GameContent.Activities
//{
//    /// <summary>
//    /// Collect all items in under 20 seconds (sample mission)
//    /// </summary>
//    class Mission2Activity : Scene
//    {
//        private List<GameObject> items;
//        private Agent player;

//        public override void InitScript(string parameters = null, ActivityParameters activityParameters = null)
//        {
//            SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.MissionLog);
//            base.background = new Background(8);  // starry galaxy
//            this.player = AddPlayer();
//            this.items = AddItems();
//            Mission mission = AddCollectMission(items);
//            base.MissionManager.AddMission(mission);
//        }

//        private Agent AddPlayer()
//        {
//            return AddGameObject("Skill", Framework.FactionType.Player, Vector2.Zero, 0, AgentControlType.Player) as Agent;
//        }

//        private GameObject AddItem(Vector2 position)
//        {
//            return AddGameObject("MatA1", Framework.FactionType.Neutral, position, 0, AgentControlType.None);
//        }

//        private List<GameObject> AddItems()
//        {
//            List<GameObject> items = new List<GameObject>();
//            Vector2 delta = new Vector2(0, -800);
//            Vector2 position = player.Position;
//            int numitems = 4;
//            for (int i = 0; i < numitems; ++i)
//            {
//                position += delta;
//                GameObject rock = AddItem(position);
//                items.Add(rock);
//            }
//            return items;
//        }

//        private Mission AddCollectMission(List<GameObject> items)
//        {
//            Mission mission = new Mission("Collect All items");
//            ObjectiveGroup group = new ObjectiveGroup();
//            foreach (GameObject rock in items)
//            {
//                group.AddObjective(new DestroyTargetObjective(rock));
//            }
//            group.AddObjective(new TimeObjective(30));
//            mission.Objective = group;
//            return mission;
//        }

//        public static Activity ActivityProvider(string parameters)
//        {
//            return new Mission2Activity();
//        }
//    }
//}
