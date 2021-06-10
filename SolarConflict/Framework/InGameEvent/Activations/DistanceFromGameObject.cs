using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.InGameEvent.Activations
{
    /// <summary>
    /// Checks if agent is further gameobject
    /// </summary>
    [Serializable]
    public class DistanceFromGameObject : IEventActivationCheck
    {
        public GameObject TargetGameObject;
        public float Distance { set { _distanceSquared = value * value; } }
        /// <summary>
        /// If true check if object is closer then distance, if false checks if farther
        /// </summary>
        public bool Closer = true;
        private float _distanceSquared;


        public bool CheckActivation(Agent agent, GameEngine gameEngine)
        {
            if (agent != null && TargetGameObject != null && TargetGameObject.IsActive)
            {
                float distanceFromPointSqr = (agent.Position - TargetGameObject.Position).LengthSquared();
                if ((Closer && distanceFromPointSqr < _distanceSquared) ||
                    (!Closer && distanceFromPointSqr >= _distanceSquared))
                {
                    return true;
                }
            }
            return false;
        }

        public IEventActivationCheck GetWorkingCopy() {
            return MemberwiseClone() as DistanceFromGameObject;
        }
    }
}
