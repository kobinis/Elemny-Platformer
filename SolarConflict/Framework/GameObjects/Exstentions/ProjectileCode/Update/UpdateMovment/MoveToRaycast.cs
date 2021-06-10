using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaUtils;

namespace SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Update.UpdateMovment
{
    [Serializable]
    class MoveToRaycast : BaseUpdate
    {
        public MoveToRaycast(int maxLength)
        {
            this.maxLength = maxLength;
        }

        float maxLength = 1000;
        
        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            if(projectile.Parent != null)
            {
                GameObject obj;
                projectile.Position = gameEngine.CollisionManager.RayCast(projectile.Parent.Position, FMath.ToCartesian(1, projectile.Rotation), maxLength, out obj, projectile.profile.ObjectType, projectile.GetAgentAncestor(), projectile.profile.CollideWithMask);
            }
        }
    }
}
