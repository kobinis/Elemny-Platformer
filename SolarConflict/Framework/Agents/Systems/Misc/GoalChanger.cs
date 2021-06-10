using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using XnaUtils.Graphics;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict
{
    /// <summary>
    /// 
    /// </summary>            
    [Serializable]
    public class GoalChanger : AgentSystem //TODO: add option to be always on or never on
    {
       
        public GoalChanger()
        {
         
        }

        public override bool Update(Agent agent, GameEngine engine, Vector2 initPosition, float initRotation, bool tryActivate)
        {
            var goal = agent.GetTarget(engine, TargetType.Goal);
            if(goal != null && GameObject.DistanceFromEdge(goal, agent) < 200)
            {
                agent.SetTarget(null, TargetType.Goal);
            }
            return false;
        }

        public override void Draw(Camera camera, Agent agent, Vector2 initPosition, float initRotation, DrawType drawType = DrawType.Alpha) //Add GameEngibe to draw
        {
            
        }

        public override AgentSystem GetWorkingCopy()
        {
            GoalChanger system = (GoalChanger)MemberwiseClone();
            return system;
        }

    }
}
