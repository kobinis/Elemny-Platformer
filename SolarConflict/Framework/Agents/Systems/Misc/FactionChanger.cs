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
//    /// FactionChanger - changes faction to last object to colide
//    /// </summary>
//    [Serializable]
//    public class FactionChanger : AgentSystem //TODO: add option to change nearest object or a to a spesific faction, add option to work one or keep working
//    {
//        public ActivationCheck activationCheck;
//        private bool changed;

//        public FactionChanger()
//        {
//            activationCheck = new ActivationCheck(ControlSignals.OnColision);            
//        }

//        public override bool Update(Agent agent, GameEngine engine, Vector2 initPosition, float initRotation, bool tryActivate)
//        {
//            bool wasActive = false;
//            if (!changed && activationCheck.Check(agent, tryActivate) && agent.lastDamagingObjectToCollide != null
//                && agent.lastDamagingObjectToCollide.GetFactionType() != 0)
//            {
//                wasActive = true;
//                changed = true;
//                agent.FactionType = agent.lastDamagingObjectToCollide.GetFactionType();
//                agent.SetControlType(AgentControlType.AI);
//            }

//            return wasActive;
//        }        

//        public override AgentSystem GetWorkingCopy()
//        {
//            FactionChanger cargoEmitterCopy = (FactionChanger)MemberwiseClone();
//            cargoEmitterCopy.activationCheck = activationCheck.GetWorkingCopy();
//            return cargoEmitterCopy;
//        }


//    }
//}
