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
    public class DistanceFromPoint : IEventActivationCheck
    {
        public Vector2 Point = Vector2.Zero;        
        public float Distance { set { _distanceSquared = value * value; } }
        /// <summary>
        /// If true check if object is closer then distance, if false checks if farther
        /// </summary>
        public bool Closer = true;
        private float _distanceSquared;

        public DistanceFromPoint() { }
        public DistanceFromPoint(float distance, Vector2 point = default(Vector2), bool closer = true)
        {
            Distance = distance;
            Point = point;
            Closer = closer;
        }

        public bool CheckActivation(Agent agent, GameEngine gameEngine)
        {
            if (agent != null)
            {
                float distanceFromPointSqr = (agent.Position - Point).LengthSquared();
                if ((Closer && distanceFromPointSqr < _distanceSquared) ||
                    (!Closer && distanceFromPointSqr >= _distanceSquared))
                {
                    return true;
                }
            }
            return false;
        }

        public IEventActivationCheck GetWorkingCopy() {
            return MemberwiseClone() as DistanceFromPoint;
        }
    }
}
