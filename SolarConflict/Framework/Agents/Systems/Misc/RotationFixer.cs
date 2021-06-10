using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict
{
    [Serializable]
    public class RotationFixer : AgentSystem
    {
        private float rotation;

        public RotationFixer(float rotation)
        {
            this.rotation = MathHelper.ToRadians(rotation);
        }

        public override bool Update(Agent ship, GameEngine engine, Vector2 initPosition, float initRotation, bool tryActivate)
        {
            ship.gameObjectType |= GameObjectType.NonRotating;
            ship.Rotation = rotation;
            return false;
        }        

        public override AgentSystem GetWorkingCopy()
        {
            return this;
        }
    }
}
