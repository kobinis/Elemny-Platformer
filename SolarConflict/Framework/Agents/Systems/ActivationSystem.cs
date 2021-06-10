
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using XnaUtils;
//using SolarConflict.Framework.Agents.Systems;

//namespace SolarConflict
//{
//    /// <summary>
//    /// ActivationSystem - will activate other systems when conditions are met
//    /// </summary>
//    [Serializable]
//    class ActivationSystem : AgentSystem
//    { 
//        public int CooldownTime;
//        public AgentSystem System;

//        private int cooldown;
        
//        public ActivationSystem(AgentSystem system)
//        {
//            System = system;
//        }

//        public override bool Update(Agent agent, GameEngine engine, Vector2 initPosition, float initRotation, bool tryActivate)
//        {
//            bool isActivating = tryActivate && cooldown <= 0; 
//            bool activationRes = System.Update(agent, engine, initPosition, initRotation, isActivating);
//            return activationRes;
//        }

//        public override AgentSystem GetWorkingCopy()
//        {
//            SystemHolder systemActivationClone = (SystemHolder)MemberwiseClone();
//            systemActivationClone.system = System.GetWorkingCopy();
//            return systemActivationClone;
//        }

//        public override float GetCooldown()
//        {
//            return Math.Max(cooldown, System.GetCooldown());
//        }

//        public override float GetCooldownTime()
//        {
//            return Math.Max(CooldownTime, System.GetCooldownTime());
//        }
//    }
//}

