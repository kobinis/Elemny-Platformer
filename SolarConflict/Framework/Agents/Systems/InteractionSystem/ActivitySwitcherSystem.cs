using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework.Agents;

namespace SolarConflict
{
    /// <summary>
    /// ActivitySwitcherSystem - switches activity when activated(on item), or when on interaction
    /// </summary>
    [Serializable]
    public class ActivitySwitcherSystem : AgentSystem, IInteractionSystem //TODO: refractor
    {
        /// <summary>
        /// Text thet will show when cursor is over the object
        /// </summary>
        public string InteractionText;
        
        /// <summary>
        /// ActivityProvider name
        /// </summary>
        public string ActivityName { get; private set; }
        public ActivityParameters ActivityParams;
        public bool Persistent;
        public Activity TargetActivity { get; private set; }
        public float MinRelations; //The minimum releations you need with the faction to interact

        /// <summary>
        /// Target Activity - if this is not null it will switch to the target activity ignoring ActivityName, 
        /// if Persistent is true and this is null it will set it to the created activity from ActivityName
        /// </summary>

        public int? SceneIndex; //
        //private bool _sameScene;

        public ActivitySwitcherSystem()
        {
            InteractionText = "Dock";
        }

        public ActivitySwitcherSystem(string activityName):this()
        {            
            ActivityName = activityName;
        }

        public ActivitySwitcherSystem(Activity targetActivity) : this()
        {
            TargetActivity = targetActivity;
        }

        private bool SwitchActivity(GameEngine gameEngine, Agent agent)
        {
            if (!(gameEngine.Scene != null && (SceneIndex == null || gameEngine.Scene.SceneID == SceneIndex)))
                return false;
            if (gameEngine.GetFaction(agent.GetFactionType()).GetRelationToFaction(Framework.FactionType.Player) < MinRelations)
            {

                ActivityManager.Inst.AddToast("You need to improve standings with this faction", 90); //TODO: change
                return false;
            }

            if (TargetActivity != null)
            {
                ActivityManager.Inst.SwitchActivity(TargetActivity);
            }
            else
            {
              //  if (ActivityParams == null)
              //      ActivityParams = new ActivityParameters();
               // ActivityParams.DataParams["Calling_agent"] = agent;
                var targetActivity = gameEngine.Scene.SwitchActivity(ActivityName, ActivityParams, null, agent);
                if (Persistent)
                    TargetActivity = targetActivity;
            }
            return true;
        }


        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate)
        {            
            if (tryActivate)
            {
                return SwitchActivity(gameEngine, agent);                
            }
            return false;
        }        

             
        public string GetInteractionText(Agent agent, GameEngine gameEngine, Agent playerAgent)
        {
            return InteractionText;
        }

        public bool Interact(Agent agent, GameEngine gameEngine, Agent playerAgent) 
        {            
            if (gameEngine.Scene != null)
            {
                return SwitchActivity(gameEngine, agent);
            }
            else
            {
                ActivityManager.Inst.SwitchActivity(ActivityName, ActivityParams);
            }
            return false;            
        }


        public override AgentSystem GetWorkingCopy()
        {            
            ActivitySwitcherSystem system = (ActivitySwitcherSystem)MemberwiseClone();
            if (ActivityName != null)
                system.TargetActivity = null;
            return system;
        }
    }
}

