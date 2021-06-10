//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using XnaUtils;
//using XnaUtils.Graphics;
//using SolarConflict.Framework.Agents.Systems;

//namespace SolarConflict
//{
//    / <summary>
//    / GoalIndicatorAndChanger - displays an arrow to a spesific game object
//    / </summary>            
//    [Serializable]
//    public class GoalIndicatorChanger : AgentSystem //TODO: add option to be always on or never on
//    {
//        public GameObject Goal;
//        public Sprite arrowSprite;
//        public Color arrowColor;
//        public float sizeMult = 1.3f;
//        public float scale = 0.5f;
//        public int sceneId;
//        private bool sameScene;

//        public GoalIndicatorChanger(GameObject goal, int sceneId)
//        {
//            this.Goal = goal;
//            this.sceneId = sceneId;
//            arrowSprite =  Sprite.Get("smallFuzzyArrow");
//            arrowColor = Color.White;
//        }

//        public GoalIndicatorChanger():this(null, 0)
//        {
//        }        
//        public override bool Update(Agent agent, GameEngine engine, Vector2 initPosition, float initRotation, bool tryActivate)
//        {
//            TODO:
//            sameScene = engine.Scene != null && engine.Scene.SceneID == sceneId;
//            if (tryActivate && sameScene)
//            {
//                agent.SetTarget(Goal, TargetType.Goal);
//                return true;
//            }

//            return false;
//        }

//        public override void Draw(Camera camera, Agent agent, Vector2 initPosition, float initRotation, DrawType drawType = DrawType.Alpha) //Add GameEngibe to draw
//        {           
//            if (Goal != null && sameScene)
//            {
//                Vector2 goalRelPos = Goal.Position - agent.Position;
//                float angle =  (float)Math.Atan2(goalRelPos.Y, goalRelPos.X);
//                Vector2 pos = FMath.ToCartesian(agent.Size * sizeMult, angle);
//                camera.CameraDraw(arrowSprite, agent.Position + pos, angle, scale, arrowColor); //Is Active??
//            }
//        }

//        public override AgentSystem GetWorkingCopy()
//        {
//            GoalIndicatorChanger system = (GoalIndicatorChanger)MemberwiseClone();
//            system.sameScene = false;
//            return system;
//        }

//    }
//}
