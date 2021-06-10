//using Microsoft.Xna.Framework;
//using SolarConflict.Session.World.MissionManagment;
//using SolarConflict.Session.World.MissionManagment.Objectives;
//using XnaUtils;

//namespace SolarConflict.GameContent.Activities
//{
//    /// <summary>
//    /// Destory enemy (sample mission)
//    /// </summary>
//    class Mission3Activity : Scene
//    {
//        private Agent player;
//        private GameObject enemy;

//        public override void InitScript(string parameters = null, ActivityParameters activityParameters = null)
//        {
//            SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.MissionLog);
//            base.background = new Background(8);  // starry galaxy
//            this.player = AddPlayer();
//            this.enemy = AddEnemy();
//            Mission mission = AddDestroyMission(enemy);
//            base.MissionManager.AddMission(mission);
//        }

//        private Agent AddPlayer()
//        {
//            return AddGameObject("Skill", Framework.FactionType.Player, Vector2.Zero, 0, AgentControlType.Player) as Agent;
//        }

//        private GameObject AddEnemy()
//        {
//            Vector2 position = this.player.Position + new Vector2(0, -1500);
//            return AddGameObject("HelperGun", Framework.FactionType.Pirates1, position, 0, AgentControlType.AI);
//        }

//        private Mission AddDestroyMission(GameObject enemy)
//        {
//            Mission mission = new Mission("Destroy enemy");
//            mission.Objective = new DestroyTargetObjective(enemy);
//            return mission;
//        }

//        public static Activity ActivityProvider(string parameters)
//        {
//            return new Mission3Activity();
//        }
//    }
//}