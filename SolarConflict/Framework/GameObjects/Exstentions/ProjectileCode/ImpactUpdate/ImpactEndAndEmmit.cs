using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;

namespace SolarConflict
{
    [Serializable]
    public class ImpacedEndAndEmmit : BaseImpactUpdate
    {
        public IEmitter emmiter;
        public float minNormalizedLifeTime = 0.1f;        
                
        public override void Update(Projectile projectile, GameObject collidingObject, GameEngine gameEngine)
        {

            if (collidingObject == projectile.Parent && projectile.Lifetime/(float)projectile.maxLifeTime >= minNormalizedLifeTime) //change
            {
                if (emmiter != null)                    
                    emmiter.Emit(gameEngine, projectile, projectile.GetFactionType(), projectile.Position, projectile.Velocity, projectile.Rotation);                            
                projectile.IsActive = false;
            }
        }
    }
}
